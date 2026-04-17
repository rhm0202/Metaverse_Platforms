using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 움직임과 카메라 제어를 담당하는 클래스
public class PlayerController : MonoBehaviour {

    // 플레이어의 이동 속도 관련 변수들
    [SerializeField]  // Unity 인스펙터에서 수정 가능하도록 설정
    private float walkSpeed;    // 기본 걷기 속도
    [SerializeField]
    private float runSpeed;     // 달리기 속도
    [SerializeField]
    private float crouchSpeed;  // 앉기 속도

    private float applySpeed;   // 현재 적용되는 실제 이동 속도

    [SerializeField]
    private float jumpForce;    // 점프 힘

    // 플레이어의 현재 상태를 나타내는 불리언 변수들
    private bool isRun = false;     // 달리기 상태
    private bool isCrouch = false;  // 앉기 상태
    private bool isGround = true;   // 지면 접촉 상태

    // 카메라 높이 조절 관련 변수들
    [SerializeField]
    private float crouchPosY;       // 앉았을 때의 카메라 Y좌표
    private float originPosY;       // 기본 카메라 Y좌표
    private float applyCrouchPosY;  // 현재 적용되는 카메라 Y좌표

    // 지면 감지를 위한 콜라이더
    private CapsuleCollider capsuleCollider;

    // 마우스 조작 관련 변수들
    [SerializeField]
    private float lookSensitivity;  // 마우스 감도
    [SerializeField]
    private float cameraRotationLimit;  // 카메라 상하 회전 제한 각도
    private float currentCameraRotationX = 0;  // 현재 카메라 X축 회전값

    // 필요한 컴포넌트 참조
    [SerializeField]
    private Camera theCamera;   // 메인 카메라
    private Rigidbody myRigid;  // 물리 처리를 위한 리지드바디
    private Crosshair theCrosshair;

    // 게임 시작 시 초기화
    void Start () {
        // 필요한 컴포넌트 가져오기
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        theCrosshair = FindObjectOfType<Crosshair>();
        applySpeed = walkSpeed;  // 초기 이동 속도 설정

        // 카메라 초기 위치 설정
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    // 매 프레임마다 실행되는 업데이트 함수
    void Update () {
        // 플레이어 상태 체크 및 동작 실행
        IsGround();          // 지면 체크
        TryJump();           // 점프 시도
        TryRun();            // 달리기 시도
        TryCrouch();         // 앉기 시도
        Move();              // 이동
        CameraRotation();    // 카메라 회전
        CharacterRotation(); // 캐릭터 회전
    }

    // 앉기 키 입력 감지
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))  // 왼쪽 Ctrl 키를 누르면
        {
            Crouch();  // 앉기 동작 실행
        }
    }

    // 앉기 동작 처리
    private void Crouch()
    {
        isCrouch = !isCrouch;  // 앉기 상태 토글
        if (theCrosshair != null)
            theCrosshair.CrouchingAnimation(isCrouch);

        if (isCrouch)
        {
            applySpeed = crouchSpeed;  // 앉은 상태의 이동 속도 적용
            applyCrouchPosY = crouchPosY;  // 앉은 상태의 카메라 높이 적용
            if (theCrosshair != null)
                theCrosshair.RunningAnimation(false);
        }
        else
        {
            applySpeed = walkSpeed;  // 기본 이동 속도로 복귀
            applyCrouchPosY = originPosY;  // 기본 카메라 높이로 복귀
        }

        StartCoroutine(CrouchCoroutine());  // 부드러운 앉기 동작 실행
    }

    // 부드러운 앉기 동작을 위한 코루틴
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        // 카메라 높이를 부드럽게 조절
        while(_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);  // 선형 보간으로 부드러운 이동
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)  // 무한 루프 방지
                break;
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);
    }

    // 지면 접촉 여부 확인
    private void IsGround()
    {
        // 레이캐스트를 사용하여 지면 감지
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    // 점프 키 입력 감지
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)  // 스페이스바를 누르고 지면에 있을 때
        {
            Jump();  // 점프 실행
        }
    }

    // 점프 동작 처리
    private void Jump()
    {
        if (isCrouch)  // 앉은 상태에서 점프하면 앉기 해제
            Crouch();

        myRigid.linearVelocity = transform.up * jumpForce;  // 위쪽 방향으로 힘을 가함
    }

    // 달리기 키 입력 감지
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))  // 왼쪽 Shift 키를 누르고 있을 때
        {
            Running();  // 달리기 시작
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))  // Shift 키를 놓으면
        {
            RunningCancel();  // 달리기 종료
        }
    }

    // 달리기 시작
    private void Running()
    {
        if (isCrouch)  // 앉은 상태에서 달리면 앉기 해제
            Crouch();

        isRun = true;
        applySpeed = runSpeed;  // 달리기 속도 적용
        if (theCrosshair != null)
            theCrosshair.RunningAnimation(true);
    }

    // 달리기 종료
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;  // 기본 걷기 속도로 복귀
        if (theCrosshair != null)
            theCrosshair.RunningAnimation(false);
    }

    // 플레이어 이동 처리
    private void Move()
    {
        // 입력 받기
        float _moveDirX = Input.GetAxisRaw("Horizontal");  // 좌우 이동
        float _moveDirZ = Input.GetAxisRaw("Vertical");    // 전후 이동
        bool isWalking = (_moveDirX != 0 || _moveDirZ != 0) && !isRun && !isCrouch;
        if (theCrosshair != null)
            theCrosshair.WalkingAnimation(isWalking);

        // 이동 방향 계산
        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        // 최종 이동 속도 계산 및 적용
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;
        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    // 캐릭터 좌우 회전 처리
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");  // 마우스 좌우 이동
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    // 카메라 상하 회전 처리
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");  // 마우스 상하 이동
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        // 카메라 회전 각도 제한
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
}
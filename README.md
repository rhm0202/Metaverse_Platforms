# Metaverse_Platforms

> 3D FPS 생존 크래프팅 | Unity 3D | 수업 프로젝트

수업 과제로 진행한 3D FPS 생존 크래프팅 게임입니다.
강의 기반으로 제작하였으며, 발생한 버그를 직접 분석하고 수정하였습니다.

---

## 게임 개요

| 항목 | 내용 |
|---|---|
| 장르 | 3D FPS 생존 크래프팅 |
| 엔진 | Unity (URP 3D) |
| 수업 | 메타버스 플랫폼 |
| 기간 | 2026.03 ~ 진행 중 |
| 플랫폼 | PC / Mobile (렌더 파이프라인 분리 구성) |

---

## 핵심 게임플레이

- **자원 수집**: 도끼(나무), 곡괭이(돌)로 자원 채취
- **크래프팅**: Alchemy Table에서 아이템 제작
- **전투**: SubMachineGun으로 적(Pig) 사냥
- **생존**: 산소, 체력 관리
- **건설**: Wood Wall, Pillar, Floor 등 구조물 배치

---

## 조작법

| 키 | 동작 |
|---|---|
| `W / A / S / D` | 이동 |
| `Left Shift` | 달리기 |
| `Left Ctrl` | 앉기 |
| `Space` | 점프 |
| `마우스` | 시점 조작 |
| `좌클릭` | 공격 / 채집 |
| `우클릭` | 정조준 |
| `1` | 맨손 (도끼/곡괭이) |
| `2` | 총기 |
| `R` | 재장전 |

---

## 구현 시스템

### 플레이어 이동
- 걷기 / 달리기 / 앉기 / 점프
- Rigidbody 기반 물리 이동
- `Mathf.Lerp` 코루틴으로 앉기 시 카메라 부드럽게 전환
- 마우스 상하 회전 각도 제한 (Clamp)

### 무기 시스템
- `Dictionary<string, Gun/Hand>` 로 무기 O(1) 접근
- 코루틴 기반 무기 전환 딜레이
- 전환 중 중복 입력 방지 플래그
- WeaponSway: 이동 시 무기 흔들림 효과

### 렌더링
- PC / Mobile 렌더 파이프라인 분리 (`PC_RPAsset`, `Mobile_RPAsset`)
- 커스텀 Water Shader 적용

---

## 기술 스택

- **Engine**: Unity, URP 3D
- **Language**: C#
- **Rendering**: Universal Render Pipeline (PC / Mobile 분리)

---

## 프로젝트 구조

```
Assets/
├── script/          # 플레이어, 무기, HUD 스크립트
├── 3D Model/        # FBX 모델 (무기, 동물, 환경 오브젝트)
├── Animation/       # 무기·손 애니메이션 컨트롤러
├── Materials/       # 재질
├── Prefab/          # 프리팹
├── Scenes/          # SampleScene
├── Settings/        # PC / Mobile 렌더 파이프라인 설정
├── SkyBox/          # 스카이박스
├── Sound/           # 효과음
├── Textures/        # 텍스처
└── Water Shader/    # 커스텀 워터 셰이더
```

---

## GitHub

[https://github.com/rhm0202/Metaverse_Platforms](https://github.com/rhm0202/Metaverse_Platforms)


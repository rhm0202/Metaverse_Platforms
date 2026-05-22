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

- **자원 수집**: 도끼(나무), 곡괭이(돌)로 자원 채취 → 아이템 드롭
- **인벤토리**: 아이템 획득·보관·드래그&드롭 슬롯 이동
- **전투**: SubMachineGun으로 적(Pig) 사냥
- **회복**: 소비 아이템 우클릭으로 HP/SP/DP 회복
- **생존**: 체력·스태미나·배고픔·갈증 관리

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
| `우클릭` | 정조준 / 아이템 사용 (인벤토리) |
| `E` | 아이템 줍기 |
| `I` | 인벤토리 열기/닫기 |
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
- 인벤토리 열린 동안 카메라 회전 정지

### 무기 시스템
- 근접무기 추상 클래스(`CloseWeaponController`) 기반 도끼·곡괭이·맨손
- 코루틴 기반 무기 전환 딜레이 및 중복 입력 방지
- WeaponSway: 이동 시 무기 흔들림 효과
- 총기: 발사·재장전·정조준·반동 구현

### 아이템 & 인벤토리
- ScriptableObject 기반 아이템 데이터 (`Item`)
- E키 레이캐스트로 월드 아이템 획득 (`ActionController`)
- 슬롯 드래그&드롭으로 아이템 위치 이동 (`DragSlot`)
- 슬롯 우클릭으로 장비 장착 / 소비 아이템 사용
- 마우스 오버 시 툴팁 표시 (이름·설명·사용법)
- `ItemEffectDatabase`로 아이템별 HP·DP·배고픔·갈증 효과 관리

### 동물 AI
- `Pig`: 대기·풀뜯기·두리번·걷기 랜덤 행동
- 피격 시 공격자 방향 반대로 도주
- 체력 0 시 사망 애니메이션

### 환경
- `Rock`: 곡괭이로 채굴 시 돌 아이템 랜덤 개수 드롭
- SoundManager 싱글톤으로 효과음 관리

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
├── Script/
│   ├── Player/          # PlayerController
│   ├── Weapon/          # Gun, GunController, CloseWeapon, HandController, AxeController, PickaxeController, WeaponManager, WeaponSway
│   ├── UI/              # Crosshair, HUD, Inventory, Slot, DragSlot, SlotToolTip, StatusController
│   ├── Item/            # Item, ItemPickUp, ActionController
│   ├── Environment/     # Rock, Pig
│   └── Manager/         # SoundManager, ItemEffectDatabase
├── 3D Model/            # FBX 모델 (무기, 동물, 환경 오브젝트)
├── Animation/           # 애니메이션 컨트롤러
├── Materials/           # 재질
├── Prefab/              # 프리팹
├── Scenes/              # SampleScene
├── Settings/            # PC / Mobile 렌더 파이프라인 설정
├── SkyBox/              # 스카이박스
├── Sound/               # 효과음
├── Textures/            # 텍스처
└── Water Shader/        # 커스텀 워터 셰이더
```

---

## GitHub

[https://github.com/rhm0202/Metaverse_Platforms](https://github.com/rhm0202/Metaverse_Platforms)

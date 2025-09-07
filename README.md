# 살놈살 (SalNomSal)

Unity 2D 턴 기반 RPG 게임 프로젝트입니다.

## 🎥 게임 플레이 영상

[![게임 플레이 영상](https://img.youtube.com/vi/FJbo1RMgxhc/0.jpg)](https://www.youtube.com/watch?v=FJbo1RMgxhc)

## 프로젝트 개요

**살놈살**은 다키스트 던전 스타일의 턴제 RPG 게임입니다. Unity Universal Render Pipeline (URP)을 사용한 2D 게임으로, 플레이어는 캐릭터들로 구성된 팀을 이끌고 맵을 탐험하며 몬스터와 전투를 펼칩니다. 

게임의 독특한 특징은 **캐릭터 사망 직전 미니게임을 통해 부활할 수 있는 시스템**입니다. 이를 통해 전략적인 턴제 전투에 긴장감 넘치는 액션 요소를 더했습니다.

## 주요 특징

### 🎮 게임 시스템
- **턴 기반 전투**: 전략적인 턴제 전투 시스템
- **부활 미니게임**: 캐릭터 사망 직전 미니게임을 통한 부활 시스템
- **미니게임 시스템**: 다양한 미니게임 (회피, 패링, 매칭 게임)
- **맵 생성 시스템**: 절차적 맵 생성
- **장비 시스템**: 캐릭터 장비 및 스탯 관리
- **스킬 시스템**: 다양한 스킬과 버프/디버프 효과

### 🏗️ 기술적 특징
- **Unity 2D URP**: Universal Render Pipeline 사용
- **Additive Scene Loading**: 전투 중 상태 저장 없이 미니게임으로 씬 전환 구현
- **DOTween**: 부드러운 애니메이션 효과
- **ProCamera2D**: 고급 카메라 시스템
- **Nice Vibrations**: 햅틱 피드백
- **포스트 프로세싱**: 시각적 효과 향상

## 프로젝트 구조

```
Assets/
├── 00.Settings/           # 프로젝트 설정 파일
├── 01.Scenes/            # 게임 씬 파일들
├── 02.Scripts/           # C# 스크립트 파일들
│   ├── Combat/           # 전투 관련 스크립트
│   ├── Equipments/       # 장비 시스템
│   ├── Manager/          # 게임 매니저들
│   ├── MiniGame/         # 미니게임 관련
│   ├── Monster/          # 적 캐릭터 시스템
│   ├── PlayableCharacters/ # 플레이 가능한 캐릭터
│   ├── Skills/           # 스킬 시스템
│   ├── UI/              # 사용자 인터페이스
│   ├── Village/         # 마을 관련 시스템
│   └── Utils/           # 유틸리티 스크립트
├── 03.Prefabs/          # 프리팹 파일들
├── 04.Images/           # 이미지 리소스
├── 05.Models/           # 3D 모델 (빈 폴더)
├── 06.Sounds/           # 사운드 파일들
├── 07.Animations/       # 애니메이션 파일들
├── 08.Materials/        # 머티리얼 파일들
└── 09.Font/            # 폰트 파일들
```

## 주요 씬

- **SalnomSalTitleMenu**: 타이틀 메뉴
- **Village**: 메인 마을 허브
- **BattleScene**: 메인 전투 씬
- **MapCreate**: 맵 생성 씬
- **AvoidScene**: 회피 미니게임
- **ParryingScene**: 패링 미니게임
- **MagicScene**: 매직 미니게임

## 핵심 시스템

### 전투 시스템
- `CombatManager`: 전투 흐름 관리
- 턴 기반 액션 시스템
- 스킬 및 타겟팅 시스템

### 캐릭터 시스템
- `GameManager`: 팀 구성 및 캐릭터 상태 관리
- `PlayableCharacter`: 플레이어 캐릭터
- `EnemyCharacter`: 적 캐릭터

### 장비 시스템
- `EquipmentManager`: 장비 관리
- `InventoryRepository`: 인벤토리 시스템
- 스탯 수정자 및 패시브 효과

## 사용된 외부 에셋

- **ProCamera2D**: 고급 2D 카메라 시스템
- **Nice Vibrations (Lofelt)**: 햅틱 피드백
- **DOTween**: 트위닝 애니메이션
- **Feel (More Mountains)**: 게임 피드백 시스템
- **다양한 UI 및 그래픽 에셋**

## 🎨 그래픽 및 아트워크

게임의 **모든 그래픽 및 아트워크는 AI를 활용하여 제작**되었습니다. AI 도구를 통해 일관된 아트 스타일과 다양한 캐릭터, 배경, UI 요소들을 효율적으로 생성했습니다.

## 개발 환경

- **Unity**: 6000.0.44f1 (URP 템플릿)
- **언어**: C#
- **렌더 파이프라인**: Universal Render Pipeline (URP)
- **플랫폼**: PC 지원

## 게임 플레이

1. **마을**: 캐릭터 관리, 장비 교체, 팀 구성
2. **맵 탐험**: 절차적으로 생성된 맵에서 노드 선택
3. **전투**: 턴 기반 전투와 미니게임 결합
4. **성장**: 캐릭터 스탯 향상 및 새로운 장비 획득

이 게임은 전통적인 턴 기반 RPG에 액션 요소를 더한 하이브리드 게임플레이를 제공합니다.

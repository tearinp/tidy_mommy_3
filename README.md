# 💗엄마의 삼단정리💗
> 추억의 게임 &lt;액션 퍼즐 패밀리>의 미니 게임 중 하나인 &lt;엄마의 삼당정리>를 모작한 안드로이드 모바일 게임입니다. 

- [서론](#-서론)
  - [현재까지 구현된 모습](#-현재까지-구현된-모습)
  - [앞으로의 구현 계획](#-앞으로의-구현-계획)
- [게임 로직](#-게임-로직)
- [최적화 & 기술](#-최적화와-기술)


![image](https://user-images.githubusercontent.com/42318591/117399805-4fffe100-af3c-11eb-8b68-f712c2314d69.png)

---

## 📌 서론

> 아직 미완성 상태입니다. 

- 제작 기간 : 5/2 ~ 5/6, 5/10 ~
- 제작 인원 : 1 인 

### 🖍 현재까지 구현된 모습

<https://youtu.be/qMaP38rR1Bo>

### 🖍 앞으로의 구현 계획

- 게임 로직
  - 콤보 시스템 추가 (현재 레벨 이하의 횟수로 움직일 떄 콤보 1 증가, 초과하면 콤보 초기화)
    - 👉 추가했다.
  - 피버, 슈퍼 피버 게임 상태 추가 
  - 대화 시스템 추가
  - 씬 전환 및 구분 (메뉴 씬, 게임 씬, 게임 종료 씬) 
  - 점수 저장 (json 혹은 playerprefs)
- 외적인 부분
  - 최적화를 염두에 두고 파티클 시스템 추가
  - 애니메이션 추가 (게임 상태와 입력 위치에 따른 해, 달 애니메이션과 텍스트 애니메이션)
  - 시스템 메뉴 UI 추가
- 사운드 
  - 현재까지 구현된 상태에서 BGM 재생시 중간에 뚝뚝 끊기고 늘어지는 현상이 있는데 해결 필요 (아직 해결하지 못 했습니다 ㅠㅠ)
    - 👉 Project Settings - Audio - DSP buffer size 를 best latency 에서 best performance 로 설정을 바꾸었더니 해결 되었다!! 
  - 효과음 추가
- 안드로이드 환경에서 디버깅 
- 코드를 좀 더 구조화하여 정리하고자 합니다. 

---

## 📌 게임 로직 

> 3 개의 열 중 하나를 터치하여 이동할 블록을 선택하고, 이 상태에서 다른 열을 선택하여 블록을 이동시킬 수 있습니다.

![image](https://user-images.githubusercontent.com/42318591/117399678-0b744580-af3c-11eb-9271-6a67674dd886.png)

![image](https://user-images.githubusercontent.com/42318591/117399365-62c5e600-af3b-11eb-90c2-addd35e3c19e.png)

![image](https://user-images.githubusercontent.com/42318591/117399388-6a858a80-af3b-11eb-9aae-5d83225bab83.png)

![image](https://user-images.githubusercontent.com/42318591/117399833-5db56680-af3c-11eb-822c-d6b13866ba25.png)

---

## 📌 최적화와 기술

### 🔥 오브젝트 풀링

- 보드 내에 위치할 수 있는 블록은 최대 3 x 10 = 30 개이기 때문에 런타임으로 매 순간 블록 오브젝트를 생성해내는 것이 아닌, sort layer 를 적용해준 기본적인 블록 프리팹을 통하여 오브젝트 30 개만 게임 시작시 생성해 놓고 활성/비활성화로 재사용하는 오브젝트 풀링 패턴을 사용하였습니다.  
- 풀에서 꺼내와 게임 보드에 새롭게 생성시킨 블록에 스프라이트를 세팅하고 블록 데이터를 등록해주는 등등의 초기화 작업을 해주고 사용합니다. 게임 보드에서 제거되는 블록은 비활성화하여 다시 큐에 삽입합니다.

### 🔥 드로우콜 줄이기

- 드로우콜을 줄이고자 여러 개의 텍스처를 하나의 텍스처로 묶는 스프라이트 아틀라스를 사용하였습니다. 따라서 게임에서 새롭게 그려야할 블록이 계속 생성됨에도 불구하고 배치 수가 일정하게 유지되게 하였습니다.
- 다채로운 Material 이 요구되는 게임은 아니기 때문에 Sprite 들의 Material 을 통일했습니다.

### 🔥 Update 함수 지양하고 코루틴 사용

이 게임은 시간을 계속 업데이트 해야 하며 고정적인 시간마다 블록을 생성하는 게임입니다. 이러한 작업을 Update 함수 안에서 하지 않고 코루틴을 사용하여 구현했습니다. 매 프레임마다 어떤 작업을 수행시키는 것은 부담스러운 일이기 떄문에 Update 함수는 전혀 사용하지 않았습니다.

### 🔥 ScriptableObject 사용

블록의 종류마다 점수, 스프라이트 등등 정보가 다르기 때문에 풀에서 꺼내올 때 이를 런타임 중에 매번 일일이 모든 정보를 기입해주는 작업을 해주기보단, 블록의 데이터를 저장하는 ScriptableObject 에셋을 만들고 블록 종류 별로 데이터를 기입하여 미리 생성해놓고 이를 참조하도록 하였습니다.

### 🔥 싱글톤

게임 매니저, 오브젝트 풀 매니저, 오디오 매니저 등등 여러 매니저 프로젝트들을 싱글톤으로 📜Managers.cs 한 곳에 모아 관리하였습니다. 

### 🔥 가비지를 줄이기 위한 캐싱

- 코루틴을 많이 사용하기 때문에 매번 생성되는 new WaitSeconds 객체 가비지를 막고자 시간 별 YieldInstruction 객체를 미리 캐싱해두고 사용했습니다.
- gameObject, transform 또한 캐싱해두고 사용했습니다. 

### 🔥 UI 관련

- 상호작용이 필요 없는 모든 UI 컴포넌트들의 Raycast Target 를 꺼서 충돌 검사를 하지 않게 하였습니다.
- 캔버스는 하위 요소 하나만 변경되도 캔버스 내의 전체 하위 요소들을 전부 다시 그리기 때문에 정적, 동적 성격에 따라 캔버스들을 분할하였습니다.



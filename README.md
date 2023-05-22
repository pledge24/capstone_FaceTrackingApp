# capstone_FaceTrackingApp

현실 세계의 사람의 얼굴 표정을 ARKit를 통해 인식 및 추출하고, 해당 데이터와 ARKit에서 제공하는 52개의 BlendShape들을 이용해 expression cloning하는 virtual character를 출력하는 application을 만드는 프로젝트입니다아.

## Unity Version - 2021.3.24f1 (2023/5/21 기준)

깃 공부 중입니다. 해당 레포지토리의 변화에 대해선 무시해도 됩니다. 주인장 맘임.

## Version 1.0 세팅완료

### 1. MiraiKomachi 모델 관절 값 변경
-> 머리와 목의 회전값을 0으로 세팅했습니다. 너무 고개를 내리고 있어 정면을 바라보게 했습니다.

### 2. ARFaceBlendShapeVisualizer 코드 추가
-> faceTracking을 하기 위한 가장 기본적인 코드가 담겨져 있습니다. 연결한 BlendShape은 눈 깜빡임, 눈동자 회전, 입 O모양으로 벌리기로 총 3개입니다.

+) model의 face와 연결되어 있습니다.

### 3. AR Face Manager 컴포넌트 추가
-> AR Face Manager가 AR Session Origin에 추가되었습니다.

+) Face prefab은 AR Character 오브젝트에 연결되어 있습니다.

### 4. AR Character 오브젝트 추가
-> AR Character라는 오브젝트가 추가되었습니다. 이는 얼굴의 tracking 위치를 모델의 root 값인 발바닥으로 매칭시키는 문제를 해결하기 위함입니다.
해당 오브젝트를 얼굴의 중점으로 세팅하고, 하위 계층에 속한 model을 이동 및 회전하여 얼굴위치에 캐릭터의 얼굴이 오도록 합니다.
세팅한 이동 값은 (0, -1.45, 0), 회전 값은 (0, 180, 0)입니다.

### !! 주의 사항 !!
- AR Camera 범위 안에 오브젝트를 배치하지 마세요. 앱 화면 상에 나옵니다. 조심조심
- 해당 버전은 Assets/Secnes/This_Is_Scene 씬에 구현되어 있습니다. Assets/Secnes/WindowDeskTop Scene 씬은 Temp용(실험용)이니 무시해도 됩니다.

==============================================================================================================================================

## Version 1.0 확인해야 할 사항
 - project setting: XR Plug-in Management에 ARkit가 체크되어 있는지 확인, Player목록에 ARkit support어쩌구 체크 확인.
 - package확인: 화면의 상단 탭에 Window -> Package Manager 들가서 ARKit 관련 패키지 3개 체크되어 있는지 확인.
 - history확인: version 1.0이라는 제목으로 적은 내용(위의 Version 1.0 세팅완료 내용과 동일) 읽고, 다 적용되어 있는지 확인. 안되어있으면 읽고 적용해주세요.  

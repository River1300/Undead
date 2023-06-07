/*
1. 뱀서라이크 - 2D 오브젝트 만들기

    #1. 오브젝트 만들기
        [a]. 아틀라스( 스프라이트 시트 )를 잘라서 사용해야 한다.
            Farmer 0 을 선택한다.
                PixelsPerUnit : 한 칸에 픽셀이 몇개가 들어가는지 ( 18 개로 지정 )
                FilterMode : Point no Filter
                Compression : 색상의 압축 방싱 ( None으로 지정 )
                Sprite Mode : Multiple로 지정
                SpriteEdit에 들어가서 GridByCellSize로 18 / 20으로 Padding은 1 / 1로 지정하여 자른다.
                잘린 스프라이트의 이름을 바꿔준다.
        [b]. 아틀라스에서 자른 스프라이트를 확인하고 Stand 0 스프라이트를 씬에 배치한다.
        [c]. Stand 0을 Player로 명명 한다.
        [d]. 위치 값을 초기화 한다.
        [e]. 기즈모 버튼에서 3D Icons의 크기를 작게 줄여 준다.

    #2. 컴포넌트 추가하기
        [a]. 플레이어는 물리적인 이동을 할 예정이다.
        [b]. 리지드바디 컴포넌트를 추가한다.
            상하좌우로 움직일 예정이므로 중력 값을 0으로 지정한다.
        [c]. 캡슐 콜라이더 컴포넌트를 추가한다.
            Size : 0.6 / 0.9
        [d]. 아틀라스 중 프롭스 아틀라스를 찾아서 그림자 스프라이트를 Player 자식으로 추가한다.
            위치 y축 - 0.45로 지정
            OrderInLayer를 Player는 5, Shadow는 0
        [e]. Main 카메라의 배경 색을 회색으로 수정
*/

/*
2. 뱀서라이크 - 플레이어 이동 구현하기

    #1. C# 스크립트 만들기
        [a]. 스크립트를 모아둘 폴더를 만든다.
        [b]. Player 스크립트를 만든다.

    #2. 키보드 입력 받기
        [a]. 움직이기 위한 입력을 받아야 한다.
            받기 위한 변수가 속성으로 필요 하다.
                public Vector2 inputVec;
        [b]. Update() 함수에서 입력을 받는다.
            inputVec.x 가 수평값을 받는다. ( GetAxis )
            inputVec.y 가 수직값을 받는다.
        [c]. Player 스크립트를 Player 객체에 부착한다.

    #3. 물리 이동 방법
        [a]. 이동을 하기 위해 리지드바디를 사용해야 한다.
            Rigidbody2D rigid;
        [b]. 리지드 바디를 Awake()에서 초기화 한다.
        [c]. 물리에 대해서는 FixedUpdate()에서 로직을 작성하도록 한다.
        [d]. 힘을 준다.
            rigid.AddForce()
        [e]. 속도 제어
            rigid.velocity = 
        [f]. 위치 이동
            rigid.MovePosition()

    #4. 물리 이동 구현
        [a]. 위치 이동을 활용하여 플레이어를 이동 시키도록 한다.
            rigid.MovePosition(rigid.position + inputVec);
        [b]. 컴퓨터 성능과 관계없이 캐릭터 속력을 동일하게 맞추어 준다.
            Vector2 nextVec 이라는 지역 변수를 만들어서 방향 값의 크기를 정규화시킨다.
                = inputVec.normalized * speed * Time.fixedDeltaTime;
            public float speed 속성을 새로 갖는다.
        [c]. rigid.MovePosition(rigid.position + nextVec);
        [d]. 미끄러지듯 움직이는 것을 방지하기 위해 입력값을 받는 로직을 GetAxisRaw로 수정한다.
        [e]. 씬으로 나가서 speed 속성 값을 3으로 지정
*/

/*
3. 뱀서라이크 - 인풋 시스템 적용하기

    #1. 패키지 설치
        [a]. Window -> PackageManager
        [b]. Unity Registry
        [c]. Input System -> Install
        [d]. 새로운 Input System으로 프로젝트를 세팅해야 하므로 재시작을 해야 한다.

    #2. 인풋 액션 설정
        [a]. Player에게 PlayerInput 컴포넌트 추가
        [b]. CreateActions -> Undead 폴더에 Player이름의 새 Actions 생성
        [c]. Move Action을 사용할 예정이다.
            Move를 펼치면 디바이스 별 키 입력이 들어 있다.
        [d]. Processors에서 Normalize Vector2를 추가한다.
        [e]. Save Asset

    #3. 스크립트 적용
        [a]. 네임스페이스로 UnityEngine.InputSystem을 추가 한다.
        [b]. Update() 함수를 제거한다.
        [c]. 씬에서 Player에 부착한 PlayerInput 컴포넌트를 보면 Behavior 아래에 사용 가능한 함수들이 나열되어 있다.
            OnMove()를 사용할 예정이다.
        [d]. 함수 void OnMove()를 만든다.
            매개 변수로 InputValue value를 받는다.
        [e]. inputVec 속성에 값을 저장하도록 한다.
            inputVec = value.Get<Vector2>();
        [f]. 이미 액션 설정에서 Normalize를 추가 하였으므로 FixedUpdate()에서 작성해 두었던 normalized는 필요 없다.
*/

/*
4. 뱀서라이크 - 2D셀 애니메이션 제작하기

    #1. 방향 바라보기
        [a]. 스프라이트 랜더러에 보면 Flip이 있는데 이를 통해 이미지를 반전 시킬 수 있다.
        [b]. Player 스크립트에서 Update()를 통해 입력 값을 받고 있다.
        [c]. LateUpdate() 함수를 만든다.
            만약에 inputVec.x 값이 0이 아닐 경우 스프라이트를 반전 시키도록 한다.
        [d]. 속성으로 스프라이트 랜더러를 받는다.
        [e]. if(inputVec.x != 0) spriter.flipX = inputVec.x < 0;

    #2. 셀 애니메이션
        [a]. 셀 애니메이션이란 여러 장의 이미지를 순차적으로 보여주는 방식을 뜻한다.
        [b]. Run 0 ~ Run 5를 모두 선택한 상태로 Player객체에 넣어 준다.
        [c]. Animations 폴더에 Run_Player0 anim이라는 이름을 만든다.
        [d]. Stand 0 ~ Stand 3 모두 선택한 상태로 Player객체에 넣어 준다.
        [e]. Animations 폴더에 Stand_Player0 anim이라는 이름으로 만든다.
        [f]. Dead 0 ~ Dead 1도 마찬가지...
        [g]. 애니메이션 컨트롤러의 이름을 AcPlayer로 명명

    #3. 애니메이터 설정
        [a]. Default 상태를 Stand_Player0으로 지정한다.
        [b]. 애니메이터에 있는 상태 이름을 Stand, Run, Dead로 명명
        [c]. Dead는 AniState 위로 옮기고 연결한다.
            Dead에서 Exit는 연결하지 않는다.
        [d]. Stand와 Run을 서로 연결한다.
        [e]. 파라미터를 추가한다.
            Float타입의 Speed
            Trigger타입의 Dead
        [f]. Stand <=> Run 에서 Speed를 Greater 0.01, Less 0.01로 지정한다.
        [g]. Dead에 Trigger
        [h]. 셀 애니메이션 이므로 Transition Duration은 0.01로 모두 줄이고 Has Exit Time도 모두 해제
        [i]. Dead 애니메이션은 반복할 필요가 없다.
            Dead 애니메이션의 loop Time을 해제한다.

    #4. 코드 작성하기
        [a]. 애니메이터를 속성으로 받고 초기화를 진행한다.
        [b]. LateUpdate()에서 애니메이션 파라미터를 전달한다.
            Float로 벡터의 크기를 전달한다.
                anim.SetFloat("Speed", inputVec.magnitude);

    #5. 애니메이터 재활용
        [a]. 플레이어 종류가 다양하게 있는데 만들어둔 애니메이션을 재활용 하여 다른 플레이어에게 적용 시키자.
        [b]. 애니메이션 스프라이트는 각기 다르기 때문에 애니메이션 클립으로 각각 저장해 주어야 한다.
            이전 마찬가지로 Animations 폴더에 Player0 ~ 3으로 각각 저장한다.
        [c]. 애니메이터에 추가된 새로운 클립들은 지운다.
        [d]. 기존에 만들어 두었던 AcPlayer가 있다.
        [e]. Animator Override Controller을 만들어서 AcPlayer1로 명명 한다.
            Controller에 AcPlayer을 넣어 준다.
            Override에 해당 애니메이션 클립을 부착한다.
        [f]. 테스트를 위해 Player객체를 복사한다.
            스프라이트는 수정할 필요가 없고 애니메이터 컨트롤러만 바꿔준다.
*/

/*
5. 뱀서라이크 - 무한 맵 이동

    #1. 타일 그리기
        [a]. 스프라이트 폴더에서 Tiles 아틀라스를 통해 맵을 그릴 예정이다.
        [b]. Window -> Tile Palette를 연다.
        [c]. Tile 폴더에서 2D -> Rule Tile을 만든다.
            Ran Tile로 명명
        [d]. Number of Tiling Rules 를 1로
        [e]. Output을 Random으로
        [f]. Size를 10으로 지정한다.
        [g]. Tile 0 ~ 2 각각 두 개씩 배치 3 ~ 5는 하나 씩 배치
        [h]. 완성된 타일 에셋을 팔레트에 드래그 드랍하여 덮어 씌운다.
        [i]. 하이어라키 창에서 2D Object -> TileMap -> Rectangular을 만든다.
        [j]. 타일 팔레트를 선택하면 아래에 Default Brush가 있다.
            Line Brush로 선택한다.
                그릴 시작 지점 클릭 -> 10칸 오른쪽으로 그린다.
                반대쪽도 마찬가지로 총 중심을 기준으로 20칸을 그린다.
                아래로도 10칸 위로도 10칸 그린다.
            Default Brush로 바꾼뒤 채우기 아이콘으로 빈칸을 채운다.
        [k]. Ran Tile에서 Nois 수치를 조절하면서 원하는 형태의 타일맵 제작

    #2. 재배치 이벤트 준비
        [a]. 앞으로 타일맵을 4개로 복사하고 플레이어와의 거리가 멀어진 맵을 가까워진 구역으로 재배치할 예정이다.
        [b]. 재배치를 하기 위해서 맵( 타일 맵 )에 타일맵 콜라이더 2D 컴포넌트를 부착한다.
        [c]. 컴포짓 콜라이더2D를 부착한다.
            타일맵 콜라이더에서 used By Composit을 체크한다.
            컴포짓 콜라이더는 트리거 체크
        [d]. 리지드 바디 Body Type은 Static으로 바꾼다.
        [e]. Tag를 추가한다. Ground, Area
        [f]. TileMap은 Ground 태그를 부착
        [g]. Player 객체에서 자식으로 빈 오브젝트를 추가한다.
            Area로 명명
            Box 콜라이더 추가
                Size 20 / 20
                트리거 체크
                Area 태그 부착

    #3. 재배치 스크립트 준비
        [a]. 타일맵 이동을 위한 스크립트를 만든다. Reposition
        [b]. 게임 매니저 스크립트도 만든다.
            빈 오브젝트를 만들어서 게임 매니저 스크립트를 부착한다.
            게임 매니저를 통해서 각각의 타일에 플레이어 정보를 전달할 예정이다.
        [c]. 게임 매니저는 Player 스크립트 변수를 속성으로 갖는다.
            게임 매니저 자체를 메모리에 얹기 위해 정적으로 게임 매니저 타입의 변수를 속성으로 같는다.
                public static GameManager instance;
                Awake() 함수에서 초기화 한다. instance = this;
                게임 매니저가 필요한 다른 클래스에서 구지 속성으로 게임 매니저를 받지 않아도 instance를 통해 게임 매니저에 접근할 수 있다.

    #4. 재배치 로직
        [a]. Reposition 스크립트에 OnTriggerExit2D 함수를 만든다.
            Player의 자식인 Area와 Exit 상태가 되었을 때 이동하도록 한다.
                if(!other.CompareTag("Area")) return;
        [b]. 타일맵과 플레이어간의 거리를 x, y 별로 비교할 필요가 있다.
            이를 위해 Vector3 변수로 플레이어 위치와 맵의 위치를 저장한다.
                Vector3 playerPos = GameManager.instance.player.transform.position;
                Vector3 myPos = transform.position;
        [c]. x 축 따로 y축 따로 거리를 변수로 저장한다.
            float diffX = Mathf.Abs(playerPos.x - myPos.x);
            float diffY = Mathf.Abs(playerPos.y - myPos.y);
        [d]. 플레이어가 어느 방향으로 가고 있는지 변수로 저장한다.
            Vector3 playerDir = GameManager.instance.player.inputVec;
            float dirX = playerDir.x < 0 ? -1 : 1;
            float dirY = playerDir.y < 0 ? -1 : 1;
        [e]. 적들 또한 재배치가 필요하므로 switch 문으로 자신의 transform.tag를 통해 다른 로직을 실행한다.
            Ground일 경우, Enemy일 경우
            만약에 x축의 거리가 y축의 거리 차이보다 더 클 경우 타일맵의 위치를 지정된 값 만큼 현재 위치에서 이동한다.
                if(diffX > diffY) transform.Translate(Vector3.right * dirX * 40);
            y축의 거리 차이가 더 클 경우
        [f]. 씬으로 돌아가서 Tile맵을 배치하도록 한다.
            씬 상단에 Snap Increment을 선택하여 Move를 10으로 지정한다.
            씬의 기즈모 화살표를 ctrl 누른 상태에서 이동 시킨다.
            타일맵을 총 4개로 만들어 준다.

    #5. 카메라 설정
        [a]. 배경의 픽셀이 움직일때 깨지는 현상이 발생하는데 픽셀 퍼펙트를 사용해야 한다.
        [b]. 카메라에 픽셀 퍼팩트 카메라 컴포넌트를 부착한다. URP 버전
            PixelsPerUnit은 18로 지정
            픽셀 퍼팩트 카메라는 해상도가 짝수여야만 한다.
            레퍼런스 레솔루션 : 카메라 크기 계산을 위한 참고 해상도 ( 135 / 270 )
        [c]. 씬 상단에 있는 Game을 Simulate로 바꾼다.
            기기마다 출력되는 화면을 미리 볼 수 있다.
        [d]. Window -> PackageManager에서 Unity Registry에서 Cinemachine을 설치한다.
        [e]. 하이어라키 창에서 씨네머신 -> 버츄얼 카메라를 생성한다.
            Virtual Camera로 명명
        [f]. 버츄얼 카메라에서 Follow에 Player객체를 넣는다.
        [g]. 플레이어 이동이 매끄럽지가 않은데 Update의 타임 때문이다.
            메인 카메라 컴포넌트인 씨네머신 브레인에서 Update Method를 FixedUpdate로 바꾼다.
        [h]. 플레이어 그림자를 보이기 위해서 TileMap의 Order Layer을 -1로 지정한다.
*/
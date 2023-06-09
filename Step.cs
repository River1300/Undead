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

/*
6. 뱀서라이크 - 몬스터 만들기

    #1. 오브젝트 만들기
        [a]. 스프라이트 폴더에 5가지의 몬스터 아틀라스가 준비되어 있다.
            오브젝트 형태를 만든다.
            Enemy 1의 Run 0을 하이어라키 창에 놓는다.
            Enemy로 명명
            OrderInLayer는 2로 지정한다.
            플레이어와 마찬가지로 그림자 스프라이트를 자식으로 등록한다.
                y축만 -0.45
        [b]. 애니메이터 컴포넌트 추가 및 컨트롤러 연결
            AcEnemy 1을 컨트롤러에 연결
        [c]. 리지드바디를 부착한다.
            중력 0, FreezeRotation z축 체크
        [d]. 캡슐 콜라이더를 부착한다.
            Size 0.7 / 0.9
        [e]. Enemy 객체를 복사한다.
            Enemy2 의 스프라이트를 넣어 준다.
            AcEnemy 2를 컨트롤러에 연결
        [f]. 플레이어도 리지드바디 FreezeRotation z 축 체크
        [g]. 플레이어의 리지드바디 Mass를 5로 지정한다.

    #2. 플레이어 추적 로직
        [a]. Enemy 스크립트를 만들고 부착한다.
        [b]. 이동을 위한 속도를 속성으로 갖는다. speed;
            따라갈 목표의 리지드바디를 속성으로 갖는다. public Rigidbody2D target;
            현재 몬스터가 살아있는지 죽어 있는지 판별할 플래그를 속성으로 갖는다. bool isLive;
            본인이 물리적인 움직임을 해야 하므로 리지드바디 속성이 필요하다.
            x축을 뒤집기 위해 스프라이트 렌더러도 속성으로 필요하다. SpriteRenderer spriter;
        [c]. 리지드바디와 스프라이트렌더러를 초기화 한다.
        [d]. 물리적인 이동을 위해 FixedUpdate() 함수를 만든다.
            몬스터가 플레이어를 향해 이동하도록 할 예정이다.
        [e]. 방향을 변수로 저장한다.
            Vector2 dirVec = target.position - rigid.position;
        [f]. 앞으로 가야할 방향, 크기를 변수로 저장한다.
            Vector2 nextVec = dirVec.normalized * speed * Time.FixedDeltaTime;
        [g]. 지정된 방향으로 이동한다.
            rigid.MovePosition(rigid.position + nextVec);
        [h]. 몬스터가 로직으로 이동할 때 플레이어와의 충돌로 물리적인 움직임이 발생하지 않도록 속도를 0으로 지정한다.
            rigid.velocity = Vector2.zero;
        [i]. 씬으로 돌아가 Target 변수에 Player를 넣어 준다.
            속도는 2.5정도로 한다.
        [j]. 몬스터 이동 방향에 따라 스프라이트 x축을 반전시킬 필요가 있다.
        [k]. LateUpdate() 함수를 만든다.
            스프라이트의 x축에 값을 배정하는데 목표의 x축 값이 자신의 x축 값보다 작은지를 배정한다.
                spriter.flipX = target.position.x < rigid.position.x;
        [l]. 몬스터가 움직이기 전에 이 몬스터가 살아있는지 체크한다.
            if(!isLive) return;
                이를 반전하기 전에도 똑같이 적용한다.
        [m]. 테스트를 위해 bool 변수를 선언할 때 true를 초기화 해준다.

    #3. 몬스터 재배치
        [a]. 플레이어가 한 방향으로 계속 가면 기존에 스폰된 적들이 플레이어와 너무 멀어지게 된다.
            맵을 재배치 했었듯이 몬스터도 재배치 한다.
        [b]. Reposition 스크립트에 switch문으로 미리 Enemy 태그를 준비해 두었다.
            그 전에 몬스터가 죽었는지 살았는지를 먼저 체크한다.
                if(coll.enabled)
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f)));
        [c]. 몬스터가 죽을 때 캡슐 콜라이더를 비활성화 할 예정이다. 
            Collider2D ( 모든 Collider2D 종류를 아우르는 클래스 ) coll; 속성을 갖는다.
                Awake() 함수에서 초기화 한다.
        [d]. 몬스터에게 Reposition 스크립트를 부착한다.
        [e]. 몬스터에게 Enemy 태그를 부착한다.
*/

/*
7. 뱀서라이크 - 오브젝트 풀링으로 소환하기

    #1. 프리팹 만들기
        [a]. 플레이어 주변에 몬스터가 무한하게 나타나도록 몬스터를 프리팹화 한다.
        [b]. 스켈레톤은 EnemyA, 좀비는 EnemyB로 명명
        [c]. 프리팹을 담을 폴더를 만든다.
            객체를 폴더로 드래그드랍한다.
                프리팹의 위치를 초기화 한다.
        [d]. 프리팹 스케일 조정자에 있는 체인 이미지를 통해 값의 균일화가 가능하다.

    #2. 오브젝트 풀 만들기
        [a]. 씬에 배치해 두었던 Enemy 객체를 모두 지운다.
        [b]. 오브젝트들을 관리한 PoolManager를 만든다.
            빈 오브젝트를 만들고 스크립트를 만들고 부착한다.
        [c]. 프리팹들을 보관할 변수와 프리팹으로 부터 만들어진 객체를 담을 '풀' 담당을 하는 리스트들이 속성으로 필요하다.
            public GameObect[] prefabs; List<GameObject>[] pools;
        [d]. 씬으로 돌아가서 속성에 프리팹을 담아준다.
        [e]. List를 초기화 한다.
            pools = new List<GameObject>[prefabs.Length];
            for(int index = 0; index < prefabs.Length; index++)
                pools[index] = new List<GameObject>();

    #3. 풀링 함수 작성
        [a]. 어디에서나 풀을 사용하기 위해 public 함수를 만든다.
            public GameObject Get(int index)
        [b]. 만들어진 인스턴스를 저장할 게임 오브젝트 변수를 만든다.
            GameObject select = null; reutrn select;
        [c]. 선택한 풀의 놀고 있는 게임 오브젝트에 접근
            발견하면 select 변수에 할당
                foreach(GameObject item in pools[index]) 로 리스트의 해당 배열을 순회
                    if(!item.activeSelf) 비 활성화 된 게임 오브젝트를 찾는다.
                        select = item; select.SetActive(true); break;
        [d]. 풀 안의 모든 게임 오브젝트가 다 씬에 배치된 상태라면 새롭게 인스턴스를 생성하여 select에 배정
            if(!select)
                select = Instantiate(prefabs[index], transform);
                pools[index].Add(select); 만들어진 객체는 리스트에 저장한다.
                select.SetActive(true);

    #4. 풀링 사용해 보기
        [a]. 만들어둔 Get() 함수를 사용하기 위해 Player의 자식으로 빈 오브젝트를 만든다.
            Spawner로 명명하고 스크립트를 만들고 부착 Spawner
        [b]. PoolManager를 여러 클래스에서 사용할 예정이므로 GameManager가 속성으로 갖도록 한다.
            public PoolManager pool;
            씬으로 돌아가서 풀 매니저를 담아준다.
        [c]. 이제 Spawner 클래스에서 게임 매니저 instance를 받아와서 사용하면 된다.
        [d]. Update() 함수에서 소환하도록 한다.
            테스트를 위해 Jump버튼이 눌렸을 때 몬스터를 소환하도록 한다.
                if(Input.GetButtonDown("Jump"))
                    GameManager.instance.pool.Get(Random.Range(0, 2));
        [e]. 이렇게 생성을 해보면 에러가 발생하는데 이는 프리팹이 플레이어의 리지드바디를 속성으로 받지 못하여서 그렇다.
            Enemy가 활성화 될 때 플레이어의 리지드바디 속성을 받기로 하자.
                OnEnable() 함수를 만든다.
                게임 매니저의 instance를 통해서 리지드바디 속성을 받아온다.
                    target = GameManager.instance.player.GetComponent<Rigidbody2D>();

    #5. 주변에 생성하기
        [a]. 이제 점프 키를 통한 소환이 아닌 재대로된 소환을 해보자.
        [b]. Player객체의 자식인 Spawner에게 다시 자식으로 빈 오브젝트를 만든다.
            Point로 명명
            Point의 좌표를 플레이어 카메라 밖으로 위치시켜서 여러 개로 복사한다.
            그러면 Player의 주위에 스폰 포인트가 만들어 진다.
        [c]. Spawner 클래스가 자신의 자식의 위치를 랜덤하게 받아와서 적을 소환하도록 한다.
            public Transform[] spawnPoint;
        [d]. 소환 타이머를 위한 변수도 속성으로 받는다.
            float timer;
        [e]. Transform[]을 Awake()에서 초기화 하도록 한다.
            spawnPoint = GetComponentsInChildren<Transform>();
        [f]. Update() 함수에서 소환 타이머를 계속 증가 시킨다.
            timer += Time.deltaTime;
        [g]. 만약 0.2초가 되었다면 소환한다.
            if(timer > 0.2f)
                그리고 timer는 초기화 한다.
        [h]. 소환을 진행할 함수를 만든다.
            void Spawn()
                GameObject enemy = GameManager.instance.pool.Get(Random.Range(0, 2));
        [i]. 받은 몬스터 객체의 위치를 스폰 포인트로 지정한다.
            마찬가지로 랜덤하게 위치를 지정하는데 현재 Transform[]에 0은 자기 자신의 위치가 저장되어 있으므로 1부터 시작한다.
            enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
*/

/*
8. 뱀서라이크 - 소환 레벨 적용하기

    #1. 시간에 따른 난이도
        [a]. 게임 매니저가 컨트롤 하기 위해 게임 시간과 최대 게임 시간을 속성으로 갖는다.
            public float gameTime; public float maxGameTime = 2 * 10f;
        [b]. Update() 함수에서 시간을 계속해서 증가 시킨다.
            gameTime += Time.deltaTime;
        [c]. 게임 시간이 최대 게임 시간보다 커질 경우 게임 시간에 최대 게임 시간을 배정한다.
        [d]. 이 게임 시간을 Spawner 클래스에서 활용하도록 한다.
            레벨을 담당할 변수를 속성으로 갖는다.
                int level;
        [e]. Update() 함수에서 레벨에 게임 매니저에서 게임 시간을 10으로 나눈 값을 배정한다.
            level = Mathf.FloorToInt(GameManager.instance.gameTime / 10f);
            기존에는 timer가 0.2초보다 클 떄 소환을 하였는데 이제는 레벨을 이용하여 소환 타임을 지정하도록 한다.
                if(timer > (level == 0 ? 0.5f : 0.2f))
        [f]. Spawn() 함수로 가서 레벨에 따라 몬스터 인스턴스를 다르게 만든다.
            GameObject enemy = GameManager.instance.pool.Get(level);

    #2. 소환 데이터
        [a]. Spawner 스크립트에서 소환 데이터를 저장한다.
        [b]. Spawner 스크립트에 새로운 클래스를 만든다.
            public class SpawnData
        [c]. 속성으로 몬스터 타입( 정수형 )과 소환 시칸, 몬스터마다의 체력, 몬스터 이동 속도
            public int spriteType; public float spawnTime; public int health; public float speed;
        [d]. Spawner 클래스 안에 만든 클래스를 속성으로 받는다. 이떄 레벨마다 각각의 소환 데이터가 필요하기 때문에 배열로 만든다.
            public SpawnData[] spawnData;
        [e]. 직렬화 : 개체를 저장 혹은 전송하기 위해 변환
            SpawnData 클래스 윗 줄에 [System.Serializable]
        [f]. 씬으로 돌아가면 Spawner 객체에 SpawnData 클래스 정보가 출력되어 있다.
            배열 크기를 2개로 만들고 속성에 값을 배정한다.
            0 0.7 10 1.5 | 1 0.2 15 2.4

    #3. 몬스터 다듬기
        [a]. 프리팹 폴더로 가서 스켈레톤 프리팹을 제거한다.
        [b]. EnemyB의 이름을 Enemy로 명명
            Speed를 0으로 지정
        [c]. Enemy 스크립트에서 스폰 데이터를 받을 수 있는 구조로 수정한다.
            애니메이터를 받을 수 있는 속성 : RunTimeAnimatorController[]를 public으로 받는다.
                animCon;
            체력 : public float health; public float maxHealth;
        [d]. 씬으로 나가서 애니메이션 컨트롤러 변수에 2 배정
            애니매이터 컨트롤러 1, 2를 배정
        [e]. void OnEnable() 함수로 돌아가서 이 몬스터가 살아있는지 죽어 있는지 작성한다.
            isLive = true; health = maxHealth;
        [f]. SpawnData를 받기 위한 함수를 만든다.
            public void Init(SpawnData data)
        [g]. 전달받은 매개변수의 속성을 현재 활성화된 몬스터 속성으로 배정한다.
            anim.runtimeAnimatorController = animCon[data.spriteType];
                Type은 몬스터의 애니메이션 인덱스로 사용한다. 그런데 Enemy 스크립트에 현재 없으므로 속성으로 애니메이터를 추가한다.
            speed = data.speed; maxHealth = data.health; health = data.health;

    #4. 소환 적용하기
        [a]. 풀매니저 배열을 1개로 수정한다.
        [b]. Spawner 스크립트로 가서 Spawn함수의 인스턴스화를 0으로 바꾼다.
            GameObejct enemy = GameManager.instance.pool.Get(0);
        [c]. SpawnData에서 만들어둔 spawnTime 속성을 가지고 소환하는 시간을 지정할 것이다.
            레벨을 SpawnData배열의 인덱스로 활용하면 레벨에 따른 각기 다른 소환 시간을 지정할 수 있다.
                if(timer > spawnData[level].spawnTime)
        [d]. Spawn()함수에서 인스턴스를 할 때 적의 스크립트에 접근하여 SpawnData를 초기화 해준다.
            enemy.GetComponent<Enemy>().Init(spawnData[level]);
        [e]. 이대로 실행할 경우 level이 spawnData배열을 넘는 값이 채워지면서 에러가 발생하므로 level값을 배정하는 구문에 수정이 필요하다.
            level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);
*/

/*
9. 뱀서라이크 - 회전하는 근접무기 구현

    #1. 프리팹 만들기
        [a]. 스프라이트 폴더에 가면 Props 아틀라스를 찾을 수 있다.
            샆 모양의 스프라이트를 플레이어 머리위에 위치 시켜보자.
        [b]. Bullet 이라는 태그를 추가하고 샆에 지정한다.
        [c]. Bullet 스크립트를 만들고 부착한다.
        [d]. 데미지와 관통타입을 속성으로 갖는다.
            public float damage; public int per;
        [e]. 속성을 초기화할 함수를 만든다.
            public void Init(float damage, int per)
                this.damage = damage; this.per = pre;
        [f]. 씬으로 돌아가서 샆 객체에 박스콜라이더를 추가한다.
            넉백은 없으므로 트리거 체크
        [g]. 프리팹화 한다.
            위치 초기화

    #2. 충돌 로직 작성
        [a]. Enemy가 플레이어 무기에 피격되었을 때의 반응을 작성한다.
            Enemy 스크립트로 가서 충돌 이벤트 함수를 만든다.
            OnTriggerEnter2D
        [b]. 충돌한 콜라이더의 태그가 총알일 때만 반응하도록 한다.
            if(!other.CompareTag("Bullet")) return;
        [c]. 총알과 충돌하였다면 해당 총알의 스크립트에 접근하여 데미지를 가져온다.
            health -= other.GetComponent<Bullet>().damage;
        [d]. 이제 만약 체력이 0보다 클때와 체력이 0보다 작을 때의 로직을 작성한다.
        [e]. 죽음 함수를 만든다.
            Dead();
                gameObject.SetActive(false);
        [f]. 테스트를 위해 플레이어 자식으로 샆을 넣고 데미지를 지정해 준다.

    #3. 근접 무기 배치1
        [a]. 샆 객체를 모두 삭제 한다.
        [b]. 풀 매니저 속성에 샆 프리팹을 등록해 준다.
        [c]. 샆을 배치할 때 기준점이 되어줄 부모 오브젝트를 만든다.
            Player의 자식으로 빈 오브젝트를 만든다.
                Weapon0으로 명명
                Weapon 스크립트를 만들고 부착한다.
        [d]. Weapon 클래스는 풀 매니저에서 받아온 무기를 관리하는 역활을 갖는다.
        [e]. 무기의 ID, 프리팹 ID, 데미지, 개수, 속도를 public 속성으로 갖는다.
            int id; int prefabId; float damage; int count; float speed;
        [f]. 초기화를 위한 함수를 만든다.
            public void Init()
                id에 따라 각기 다른 초기화가 이루어지므로 switch문으로 작성한다.
                    case 0: 일때 (샆) 회전 속도를 -150으로 초기화 한다.
        [g]. 근접 무기를 배치하기 위한 함수를 만든다.
            void Batch()
                반복문을 돌면서 count마다 풀 매니저에 등록한 프리팹을 가져올 예정이다.
                게임 매니저의 인스턴스화 함수를 호출하여 무기를 인스턴스화 한다.
                    Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
                총알을 만들면서 동시에 해당 무기의 위치를 변수로 저장하였다.
                Transform.에는 parent 속성이 있는데 이 속성에 자신의 위치를 배정한다.
                    bullet.parent = transform;
                총알 스크립트에 접근하여 초기화 함수를 호출하여 데미지와 관통 여부를 전달한다.
                    bullet.GetComponent<Bullet>().Init(damage, -1); // -1 is Infinity Per.
        [h]. 만든 초기화 함수는 Start() 함수에서 호출하도록 한다.
        [i]. 씬으로 돌아가서 Weapon 속성을 임시로 작성해 준다.
            0 1 11 1 0
        [j]. 샆을 z축으로 회전 시키기 위해 Update() 함수에서 무기 id에 따라 근접 무기일 경우 회전 시키도록 한다.
        [k]. transform.Rotate(Vector3.forward * speed * Time.deltaTime);
        [l]. 임시로 샆 객체의 y축을 1로 지정한다.
        [m]. 샆의 오더인레이어를 3으로 지정한다.

    #4. 근접 무기 배치2
        [a]. 샆 프리팹을 Weapon 0의 자식으로 등록하고 y 축을 1.5
            복사한뒤 z축 회전을 180, 위치 y축을 -1.5f로 지정, 두 개 더 복사 하여 9시와 3시
            12시 -> 9시 -> 6시 -> 3시 순으로 배치
        [b]. Weapon 스크립트의 Batch() 함수로 가서 샆의 위치를 지정해 준다.
            회전을 위해 회전 벡터를 지역 변수로 만든다. 360도를 기준으로 갯수에 따라 각도가 정해진다.
                Vector3 rotVec = Vector3.forward * 360 * index / count;
            각도를 총알에 반영한다.
                bullet.Rotate(rotVec);
            자신의 위치에서 살짝 위로 위치를 지정한다. 그리고 이동하는 방향은 World를 기준으로 잡는다.
                bullet.Translate(bullet.up * 1.5f, Space.World);
        [c]. 샆 객체를 모두 지운다.

    #5. 레벨에 따른 배치
        [a]. Weapon 스크립트로 가서 레벨에 따라 샆의 개수가 늘어나게 할 예정이다.
        [b]. 레벨업 함수를 만든다.
            public void LevelUp(float damage, int count)
            레벨업을 하면 데미지와 count가 증가하므로 매개변수를 받는다.
                this.damage = damage; this.count += count;
        [c]. 만약 id가 0일 경우에 Batch() 함수를 호출한다.
        [d]. 테스트를 위해 Update() 함수에 점프 버튼이 눌렸을 때 레벨업 함수를 호출하도록 한다.
        [e]. Warld의 위치에서 샆을 생성 하였기 때문에 Batch() 함수에서 샆을 만들기 전에 local 위치로 초기화를 먼저 진행 한다.
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;
        [f]. 기존에 오브젝트 풀링에서만 가져와서 샆을 만들었다면, 이미 만들어진 샆이 있을 때는 해당 샆을 사용하기로 한다.
            자신이 가지고 있는 자식 오브젝트의 갯수를 세서 만들어야할 무기 인덱스 값을 비교한다.
                if(index < transform.childCount)
*/
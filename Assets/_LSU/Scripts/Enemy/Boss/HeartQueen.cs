using UnityEngine;

public class HeartQueen : MonoBehaviour
{
    TakeDamage takeDamage;

    [SerializeField]
    GameObject player; //플레이어

    Rigidbody2D rb; //리지드바디2D
    SpriteUpdater spriteUpdater;

    [SerializeField]
    Bounds moveBounds; //바운드 > 캐릭터의 범위 이동을 사용하기 위함
    Vector2 targetPos; //목표지점
    float moveTime = 0.0f; //이동 쿨타임
    float moveTimer = 0.0f; //이동 타이머

    int attackType; //공격패턴

    float scaffoldCooltime; //단두대 쿨타임
    float soliderCooltime; //병사 소환 쿨타임
    float gavelCooltime; //의사봉 쿨타임
    float roseCooltime; //장미 쿨타임

    float attackTimer = 0.0f;
    float lastUpdateTime = 0.0f;
    bool canMove = true; //이동가능 여부

    public float maxHp; //최대체력
    public float hp; //현재체력
    public float addHp; //추가체력
    public float damage; //데미지
    public float offense; //공격력
    public float defense; //방어력
    public float speed; //이동속도
    public float knockbackDefense; //넉백저항력

    private void Awake()
    {
        takeDamage = GetComponent<TakeDamage>();
        rb = GetComponent<Rigidbody2D>();
        spriteUpdater = GetComponent<SpriteUpdater>();

        SetTargetPosition(); //목표지점 설정
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V)) //임시로 v키 누를 시 >>> 보스 등장 시로 변경
            SetBoundsCenter(); //센터값 변경

        moveTimer += Time.deltaTime; //이동 타이머 값 증가

        if(moveTimer >= moveTime) //이동 쿨타임 완료
        {
            moveTimer = 0.0f; //이동 타이머 값 초기화
            SetTargetPosition(); //타겟 위치 설정
        }

        if (canMove) //추격 범위와의 비교를 통해 이동 여부
        {
            Move(); //이동 함수 호출
        }

        if (Time.time - lastUpdateTime < 0.1f) //0.1초마다 레이어값 변경 함수를 실행하게
            return;

        lastUpdateTime = Time.time;
        spriteUpdater.ChangeLayer(); //레이어값 변경
    }

    //Bounds 센터 값 설정 > 이동가능 영역 중심을 플레이어 위치로 변경 / 보스 등장 시 맵과 이동 제한 두기
    void SetBoundsCenter()
    {
        moveBounds.center = player.transform.position; //바운즈의 센터값을 플레이어의 위치값으로 변경
    }

    //보스 몬스터의 이동 위치 설정
    void SetTargetPosition()
    {
        float x = Random.Range(moveBounds.min.x, moveBounds.max.x); //바운즈의 x 랜덤 값
        float y = Random.Range(moveBounds.min.y, moveBounds.max.y); //바운즈의 y 랜덤 값
        targetPos = new Vector2(x, y); //랜덤값 위치 저장

        //현재 방향과 다를 때만 플립하게
        bool isFlip = (transform.position.x < x) != spriteUpdater.spriteRenderer.flipX;
        if(isFlip)
            spriteUpdater.SpriteFlip(transform.position.x < x); //스프라이트 렌더러 플립

        moveTime = Random.Range(2.0f, 4.0f); //2~4초 랜덤 이동 쿨타임
    }

    //이동
    void Move()
    {
        Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPos, speed * Time.fixedDeltaTime); //현재 위치에서 목표 위치로 일정 속도로 이동
        rb.MovePosition(newPosition); //리지드바디 이동

        if (Vector2.Distance(rb.position, targetPos) < 0.1f) //목표지점 도착 시 멈춤 방지
        {
            SetTargetPosition(); //새로운 목표지점 설정
        }
    }

    //공격 패턴
    void Attack()
    {
        attackType = Random.Range(0, 4);

        canMove = false;

        switch (attackType)
        {
            case 0: //길로틴or단두대
                //플레이어 위치에 일정시간 후 떨어짐
                //카메라 팔로우와 비슷하게 해도 괜찮을듯
                //그림자로 위험 표시 > 자식 오브젝트: 그림자, 무기
                //
                break;

            case 1: //병사 소환
                //패턴 별로 x,y축마다 병사를 3~5마리 소환
                //플레이어 방향으로 돌진, 순차적으로 돌진or한번에 돌진
                //넉백x
                break;

            case 2: //판사봉
                //판사봉 위치를 플레이어 위치로 이동 및 활성화
                //플레이어가 이동을 하면 충분히 피할 수 있는 공격 딜레이
                //판사봉 기본 3번, 강화(광폭) 시 4~5번 실행
                //넉백o
                break;

            case 3: //장미 꽃 변환
                //장미 꽃에 플레이어 접촉 시 광기 게이지 상승
                //광기 게이지 상승 시 광폭화 진행
                break;
        }
    }
}

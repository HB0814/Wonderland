using UnityEngine;
using System.Collections;

public class HeartQueen : Enemy
{
    HitEffect takeDamage;

    [SerializeField]
    Bounds moveBounds; //이동 범위 > 이동 범위를 설정하기 위함
    Vector2 targetPos; //목표위치
    float moveTime = 0.0f; //이동 시간
    float moveTimer = 0.0f; //이동 타이머

    int attackType; //공격타입

    float scaffoldCooltime; //발판 쿨타임
    float soliderCooltime; //병사 소환 쿨타임
    float gavelCooltime; //망치 쿨타임
    float roseCooltime; //장미 쿨타임

    float attackTimer = 0.0f;
    float lastUpdateTime = 0.0f;
    bool canMove = true; //이동가능 여부

    [Header("보스 특수 속성")]
    public float phaseChangeHealth = 0.5f;  // 페이즈 전환 체력 비율
    public float summonCooldown = 5f;      // 소환 쿨다운
    public float specialAttackCooldown = 10f; // 특수 공격 쿨다운
    public GameObject minionPrefab;         // 소환할 미니언 프리팹

    private bool isPhase2 = false;
    private float nextSummonTime;
    private float nextSpecialAttackTime;

    protected override void Start()
    {
        base.Start();
        takeDamage = GetComponent<HitEffect>();

        SetTargetPosition(); //목표위치 설정
        nextSummonTime = Time.time + summonCooldown;
        nextSpecialAttackTime = Time.time + specialAttackCooldown;
    }

    private new void Update()
    {
        if(Input.GetKeyDown(KeyCode.V)) //테스트용 v키 입력 시 >>> 보스 위치 초기화
            SetBoundsCenter(); //위치가 초기화, 바운즈 센터 값 설정

        moveTimer += Time.deltaTime; //이동 타이머 증가

        if (canMove) //이동 가능할 때
        {
            MoveToTarget();
        }

        // 페이즈 체크
        if (!isPhase2 && currentHealth <= maxHealth * phaseChangeHealth)
        {
            EnterPhase2();
        }

        // 특수 공격
        if (Time.time >= nextSpecialAttackTime)
        {
            SpecialAttack();
            nextSpecialAttackTime = Time.time + specialAttackCooldown;
        }

        // 미니언 소환
        if (isPhase2 && Time.time >= nextSummonTime)
        {
            SummonMinions();
            nextSummonTime = Time.time + summonCooldown;
        }
    }

    void SetBoundsCenter()
    {
        //보스의 위치를 플레이어 위에서 나타나게 설정
        transform.position = new Vector3(player.transform.position.x,
                                        player.transform.position.y + 4.0f,
                                        transform.position.z);

        //바운즈의 센터를 플레이어 위치로 설정
        moveBounds.center = new Vector3(player.transform.position.x, 
                                        player.transform.position.y,
                                        player.transform.position.z);
    }

    void SetTargetPosition()
    {
        //목표 위치를 랜덤하게 설정
        targetPos = new Vector2(
            Random.Range(moveBounds.min.x, moveBounds.max.x),
            Random.Range(moveBounds.min.y, moveBounds.max.y)
        );
    }

    void MoveToTarget()
    {
        if (!canMove) return;

        Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPos, moveSpeed * Time.fixedDeltaTime); //현재 위치에서 목표 위치로 속도에 따라 이동
        rb.MovePosition(newPosition); //리지드바디 이동

        if (Vector2.Distance(rb.position, targetPos) < 0.1f || moveTimer > moveTime) //목표 위치에 도달하면 새로운 목표 설정
        {
            moveTimer = 0.0f;
            moveTime = Random.Range(2.0f, 5.0f);
            SetTargetPosition(); //새로운 목표위치 설정
            //목표 위치 x값 비교하여 플립 여부
            //스프라이트 렌더러 레이어 순서 바꾸기 추가
        }
    }

    //protected override void Attack()
    //{
    //    base.Attack();

    //    // 보스 특수 공격 패턴
    //    if (isPhase2)
    //    {
    //        // 원형 공격 패턴
    //        for (int i = 0; i < 8; i++)
    //        {
    //            float angle = i * 45f;
    //            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;
    //            Vector2 attackPosition = (Vector2)transform.position + direction * attackRange;

    //            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPosition, 1f);
    //            foreach (Collider2D hitCollider in hitColliders)
    //            {
    //                if (hitCollider.CompareTag("Player"))
    //                {
    //                    Player player = hitCollider.GetComponent<Player>();
    //                    if (player != null)
    //                    {
    //                        player.TakeDamage(attackDamage * 0.5f);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    private void EnterPhase2()
    {
        isPhase2 = true;
        // 페이즈 2 전환 시 특수 효과나 애니메이션 추가 가능
    }

    private void SpecialAttack()
    {
        // 특수 공격 패턴 구현
    }

    private void SummonMinions()
    {
        if (minionPrefab != null)
        {
            // 미니언 소환 로직 구현
            for (int i = 0; i < 3; i++)
            {
                Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * 2f;
                Instantiate(minionPrefab, spawnPos, Quaternion.identity);
            }
        }
    }
}

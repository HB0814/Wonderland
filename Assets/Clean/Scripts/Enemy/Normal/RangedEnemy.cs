using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("원거리 공격 설정")]
    public float ranged_attackRange = 6.0f; // 공격 범위
    public float ranged_attackCooldown = 2.0f; // 공격 쿨다운
    public float projectileSpeed = 10.0f; // 투사체 속도
    public string projectilePoolTag = "EnemyProjectile"; // ObjectPool에서 사용할 태그
    bool canAttack = true; //공격 가능 여부
    [SerializeField] private EnemyProjectile[] projectiles; //투사체 스크립트 배열
    [SerializeField] private GameObject point; //공격이 시작되는 지점

    protected float minAttackRange = 4.0f; // 최소 공격 거리
    float retreatRange = 3.0f; //후퇴 시 거리

    protected override void Start()
    {
        base.Start(); //상속한  Enemy의 Start함수 호출
        point = transform.GetChild(0).gameObject; //자식의 게임오브젝트 0번을 가져오기

        for(int i = 0; i < projectiles.Length; i++) //투사체 참조
        {
            projectiles[i] = point.transform.GetChild(i).GetComponent<EnemyProjectile>();
            //자식에 있는 EnemyProjectile 스크립트 가져오기
        }
        nextAttackTime = 0.0f; //다음 공격 주기 초기화
    }

    private new void Update() //Enemy의 Update를 사용하지 않아 private new로 새롭게 선언
    {
        float dis = (transform.position - player.transform.position).sqrMagnitude; //플레이어와의 거리 계산

        if (isKnockbacked) //넉백 상태 체크
        {
            return;
        }
        else //넉백 시 이동x
        {
            if (dis > ranged_attackRange * ranged_attackRange) //공격 사거리 초과 시 이동
            {
                MoveTowardsPlayer(); //플레이어 방향으로 이동
                canAttack= false; //공격 여부 거짓
            }
            else if (dis < retreatRange * retreatRange) // 후퇴 가능한 거리 미만 시 후퇴
            {
                Retreat(); //후퇴
                canAttack= false; //공격 여부 거짓
            }
            else
            {
                rb.linearVelocity = Vector2.zero; //이동 정지
                canAttack = true; //공격 가능
            }
        }

        UpdateSprite(); //스프라이트 업데이트 << 상속한 Enemy 스크립트에 있는 함수

        // 공격이 가능하며, 공격 주기가 완료 시
        if (canAttack && Time.time >= nextAttackTime)
        {
            RangedAttack(); //원거리 공격 함수 호출
        }
    }

    //후퇴
    private void Retreat()
    {
        if (player != null && !isKnockbacked)
        {
            Vector2 dir = (transform.position - player.transform.position).normalized; //플레이어와의 반대 방향
            rb.linearVelocity = dir * (moveSpeed * 0.85f); //해당 방향으로 감소된 이동 속도로 이동
        }
    }

    //공격
    private void RangedAttack()
    {
        nextAttackTime = Time.time + ranged_attackCooldown; //공격 주기 초기화

        foreach (var projectile in projectiles) //투사체 반복문
        {
            if(!projectile.gameObject.activeInHierarchy) //투사체가 활성화 상태 시
            {
                projectile.transform.position = point.transform.position; //투사체의 위치를 공격 포인트로 이동
                projectile.gameObject.SetActive(true); //투사체 활성화
                break;
            }
        }
    }
}
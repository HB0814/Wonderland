using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("원거리 공격 설정")]
    public float ranged_attackRange = 6.0f; // 공격 범위
    public float ranged_attackCooldown = 2.0f; // 공격 쿨다운
    public float projectileSpeed = 10.0f; // 투사체 속도
    public string projectilePoolTag = "EnemyProjectile"; // ObjectPool에서 사용할 태그
    bool canAttack = true;
    [SerializeField] private EnemyProjectile[] projectiles; //투사체 배열
    [SerializeField] private GameObject point; //공격이 시작되는 지점
                                              //스트라이트 플립에 맞춰서 해당 지점의 position 값도 변경해야할듯?

    protected float minAttackRange = 4.0f; // 최소 공격 거리
    float retreatRange = 3.0f; //후퇴 시 거리

    protected override void Start()
    {
        base.Start();
        point = transform.GetChild(0).gameObject;
        for(int i = 0; i < projectiles.Length; i++)
        {
            projectiles[i] = point.transform.GetChild(i).GetComponent<EnemyProjectile>();
        }
        nextAttackTime = 0.0f;
    }

    private new void Update()
    {
        float dis = (transform.position - player.transform.position).sqrMagnitude; //플레이어와의 거리 계산

        if (isKnockback)
        {
            knockbackRecoveryTimer += Time.deltaTime;
            if (knockbackRecoveryTimer >= 0.2f)
            {
                isKnockback = false;
                knockbackRecoveryTimer = 0f;
            }
        }
        else //넉백 시 이동x
        {
            if (dis > ranged_attackRange * ranged_attackRange) //거리 5초과 시 이동
            {
                MoveTowardsPlayer();
                canAttack= false;
            }
            else if (dis < retreatRange * retreatRange) // 거리 3 미만 시 후퇴
            {
                Retreat();
                canAttack= false;
            }
            else
            {
                rb.linearVelocity = Vector2.zero; //이동 정지
                canAttack = true;
            }
        }

        UpdateSprite();

        // 공격 범위 안에 있고 최소 공격 거리보다 멀리 있을 때만 공격
        if (canAttack && Time.time >= nextAttackTime)
        {
            RangedAttack();
        }
    }

    //후퇴
    void Retreat()
    {
        if (player != null && !isKnockback)
        {
            Vector2 dir = (transform.position - player.transform.position).normalized;
            rb.linearVelocity = dir * (moveSpeed * 0.85f); //후퇴 시 이동 속도 감소
        }
    }

    //공격
    void RangedAttack()
    {
        nextAttackTime = Time.time + ranged_attackCooldown;

        foreach (var projectile in projectiles)
        {
            if(!projectile.gameObject.activeInHierarchy)
            {
                projectile.transform.position = point.transform.position;
                projectile.gameObject.SetActive(true);
                break;
            }
        }
    }
}
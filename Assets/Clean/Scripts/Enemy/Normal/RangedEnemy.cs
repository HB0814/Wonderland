using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("원거리 공격 설정")]
    public float ranged_attackRange = 6.0f; // 공격 범위
    public float ranged_attackCooldown = 2.0f; // 공격 쿨다운
    public float projectileSpeed = 10.0f; // 투사체 속도
    public string projectilePoolTag = "EnemyProjectile"; // ObjectPool에서 사용할 태그

    protected float minAttackRange = 4.0f; // 최소 공격 거리
    float retreatRange = 3.0f; //후퇴 시 거리

    protected override void Start()
    {
        base.Start();
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
            }
            else if (dis < retreatRange * retreatRange) // 거리 3 미만 시 후퇴
            {
                Retreat();
            }
            else
            {
                rb.linearVelocity = Vector2.zero; //이동 정지
            }
        }

        UpdateSprite();

        // 공격 범위 안에 있고 최소 공격 거리보다 멀리 있을 때만 공격
        if (dis <= ranged_attackRange * ranged_attackRange && dis >= minAttackRange * minAttackRange && Time.time >= nextAttackTime)
        {
            //RangedAttack();
            nextAttackTime = Time.time + ranged_attackCooldown;
        }
    }

    //후퇴
    void Retreat()
    {
        if (player != null && !isKnockback)
        {
            Vector2 dir = (transform.position - player.transform.position).normalized;
            rb.linearVelocity = dir * (moveSpeed * 0.8f); //후퇴 시 이동 속도 감소
        }
    }

    //공격
    void RangedAttack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // 플레이어 방향으로 투사체 발사
        //Vector2 direction = (player.transform.position - transform.position).normalized;
        //GameObject projectile = WeaponManager.Instance.SpawnProjectile(projectilePoolTag, transform.position, Quaternion.identity);

        //if (projectile != null)
        //{
        //    Projectile projectileComponent = projectile.GetComponent<Projectile>();
        //    if (projectileComponent != null)
        //    {
        //        projectileComponent.damage = attackDamage;
        //        projectileComponent.speed = projectileSpeed;
        //        projectileComponent.isEnemyProjectile = true;
        //        projectileComponent.SetDirection(direction);
        //    }
        //}
    }
}
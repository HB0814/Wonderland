using UnityEngine;

public class RangedEnemy : NormalEnemyBase
{
    [Header("원거리 공격 설정")]
    public float attackRange = 8f; // 공격 범위
    public float attackCooldown = 2f; // 공격 쿨다운
    public float projectileSpeed = 10f; // 투사체 속도
    public string projectilePoolTag = "EnemyProjectile"; // ObjectPool에서 사용할 태그

    protected float nextAttackTime;
    protected float minAttackRange = 3f; // 최소 공격 거리

    protected override void Start()
    {
        base.Start();
        nextAttackTime = 0f;
    }

    protected override void Update()
    {
        base.Update();

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        
        // 공격 범위 안에 있고 최소 공격 거리보다 멀리 있을 때만 공격
        if (distanceToPlayer <= attackRange && distanceToPlayer >= minAttackRange && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    protected override void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // 플레이어 방향으로 투사체 발사
        Vector2 direction = (player.transform.position - transform.position).normalized;
        GameObject projectile = WeaponManager.Instance.SpawnProjectile(projectilePoolTag, transform.position, Quaternion.identity);
        
        if (projectile != null)
        {
            Projectile projectileComponent = projectile.GetComponent<Projectile>();
            if (projectileComponent != null)
            {
                projectileComponent.damage = attackDamage;
                projectileComponent.speed = projectileSpeed;
                projectileComponent.isEnemyProjectile = true;
                projectileComponent.SetDirection(direction);
            }
        }
    }
} 
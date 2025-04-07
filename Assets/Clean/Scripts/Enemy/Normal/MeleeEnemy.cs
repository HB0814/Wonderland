using UnityEngine;

public class MeleeEnemy : NormalEnemyBase
{
    [Header("근접 공격 설정")]
    public float attackRange = 1.5f; // 공격 범위
    public float attackCooldown = 1f; // 공격 쿨다운
    public float attackAngle = 90f; // 공격 각도

    protected float nextAttackTime;

    protected override void Start()
    {
        base.Start();
        nextAttackTime = 0f;
    }

    protected override void Update()
    {
        base.Update();

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        
        // 공격 범위 안에 있을 때만 공격
        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
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

        // 공격 범위 내의 플레이어 검출
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                // 공격 방향과 플레이어 방향 사이의 각도 계산
                Vector2 attackDirection = (player.transform.position - transform.position).normalized;
                Vector2 playerDirection = (hitCollider.transform.position - transform.position).normalized;
                float angle = Vector2.Angle(attackDirection, playerDirection);

                // 공격 각도 내에 있는 경우에만 데미지 적용
                if (angle <= attackAngle * 0.5f)
                {
                    Player player = hitCollider.GetComponent<Player>();
                    if (player != null)
                    {
                        player.TakeDamage(attackDamage);
                    }
                }
            }
        }
    }
} 
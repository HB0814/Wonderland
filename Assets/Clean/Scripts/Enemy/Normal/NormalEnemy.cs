using UnityEngine;

public class NormalEnemy : Enemy
{
    TakeDamage takeDamage;

    [Header("일반 몬스터 특수 속성")]
    protected float detectionRange = 10f;  // 플레이어 감지 범위
    protected float attackAngle = 90f;     // 공격 각도

    protected float maxHp; //최대 체력
    protected float currentHp; //현재 체력
    protected float addHp; //증가 체력
    protected float offense; //공격 속도
    protected float defense; //방어력
    protected float speed; //이동 속도
    protected float knockbackDefense; //밀려나가는 저항력

    protected override void Start()
    {
        base.Start();
        takeDamage = GetComponent<TakeDamage>();
        
        // 초기 속성 설정
        maxHealth = maxHp;
        currentHealth=currentHp;
        moveSpeed = speed;
        attackDamage = offense;
        knockbackResistance = knockbackDefense;
    }

    protected override void Update()
    {
        base.Update();

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        
        // 플레이어가 감지 범위 안에 있을 때만 공격
        if (distanceToPlayer <= attackRange)
        {
            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }

        // 플레이어가 감지 범위를 벗어나면 이동 중지
        if (distanceToPlayer >= detectionRange)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    protected virtual void Attack()
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

    protected override void MoveTowardsPlayer()
    {
        if (player != null && !isKnockedBack)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            
            // 어디에 있든 플레이어를 따라감
            if (distanceToPlayer <= detectionRange)
            {
                Vector2 direction = (player.transform.position - transform.position).normalized;
                rb.linearVelocity = direction * moveSpeed;

                // 스프라이트 방향 전환
                if (spriteRenderer != null)
                {
                    spriteRenderer.flipX = direction.x < 0;
                }
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}

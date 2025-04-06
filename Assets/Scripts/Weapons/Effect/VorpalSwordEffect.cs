using UnityEngine;

public class VorpalSwordEffect : Effect
{
    [Header("보팔 검 특수 속성")]
    public float knockbackForce = 5f; // 넉백 힘
    public float effectLength = 3f; // 공격 범위 길이
    private bool hasDealtDamage = false;
    private float spawnTime;

    private void Start()
    {
        // 콜라이더 설정
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
    }

    public override void Initialize(float damage, float duration, float radius)
    {
        base.Initialize(damage, duration, radius);
        hasDealtDamage = false;
        isActive = true; // 이펙트 활성화
        spawnTime = Time.time; // 생성 시간 기록
    }

    protected override void Update()
    {
        base.Update();

        // 이펙트가 생성된 직후 약간의 딜레이 후 데미지 적용
        if (!hasDealtDamage && Time.time - spawnTime >= 0.05f)
        {
            ApplyDamageToEnemiesInRange();
            hasDealtDamage = true;
        }
    }

    private void ApplyDamageToEnemiesInRange()
    {
        // 범위 내의 모든 적 찾기
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, effectLength, targetLayer);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    // 데미지 적용
                    enemy.TakeDamage(damage);

                    // 넉백 적용
                    Vector2 knockbackDirection = (hitCollider.transform.position - transform.position).normalized;
                    Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
                    if (enemyRb != null)
                    {
                        enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                    }
                    SpawnHitEffect();
                }
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;

        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                // 데미지 적용
                enemy.TakeDamage(damage);

                // 넉백 적용
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                }
                SpawnHitEffect();
            }
        }
    }
} 
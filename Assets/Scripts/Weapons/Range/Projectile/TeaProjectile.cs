using UnityEngine;

public class TeaProjectile : Projectile
{
    [Header("홍차 투사체 특수 속성")]
    public float knockbackForce = 5f;
    public float splashRadius = 1f; // 스플래시 범위

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        poolTag = "TeaProjectile";
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                // 스플래시 데미지 적용
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, splashRadius);
                foreach (Collider2D hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Enemy"))
                    {
                        Enemy splashEnemy = hitCollider.GetComponent<Enemy>();
                        if (splashEnemy != null)
                        {
                            splashEnemy.TakeDamage(damage);
                            // 넉백 적용
                            Vector2 knockbackDirection = (hitCollider.transform.position - transform.position).normalized;
                            splashEnemy.ApplyKnockback(knockbackDirection, knockbackForce);
                        }
                    }
                }
            }
        }
    }
} 
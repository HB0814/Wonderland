using UnityEngine;

public class CheshireCatEffect : Effect
{
    private float effectRadius;
    private bool hasAppliedDamage = false;

    public override void Initialize(float damage, float duration, float radius)
    {
        base.Initialize(damage, duration, radius);
        effectRadius = radius;
        hasAppliedDamage = false;
    }

    protected override void Update()
    {
        if (!isActive) return;

        // 이펙트가 생성된 후 한 번만 데미지를 적용
        if (!hasAppliedDamage)
        {
            ApplyDamage();
            hasAppliedDamage = true;
        }

        currentDuration -= Time.deltaTime;
        if (currentDuration <= 0)
        {
            Deactivate();
        }
    }

    private void ApplyDamage()
    {
        // 범위 내의 모든 적 찾기
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, effectRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    SpawnHitEffect();
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, effectRadius);
    }
} 
using UnityEngine;

public class PocketWatchEffect : Effect
{
    private Transform targetEnemy;
    private float effectDuration;
    private float currentTime;
    private float effectRadius = 2f;
    private bool hasAttacked = false;

    public override void Initialize(float damage, float duration, float radius)
    {
        base.Initialize(damage, duration, radius);
        isActive = true;
        hasAttacked = false;

        // 가장 가까운 적 찾기
        FindNearestEnemy();
        
        // 즉시 공격 실행
        ApplyDamage();
        hasAttacked = true;
    }

    protected override void Update()
    {
        base.Update();

    }

    private void FindNearestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, effectRadius, targetLayer);
        float nearestDistance = float.MaxValue;
        Transform nearestEnemy = null;

        foreach (Collider2D collider in colliders)
        {
            float distance = Vector2.Distance(transform.position, collider.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = collider.transform;
            }
        }

        targetEnemy = nearestEnemy;
    }

    private void ApplyDamage()
    {
        if (targetEnemy != null)
        {
            Enemy enemy = targetEnemy.GetComponent<Enemy>();
            if (enemy != null)
            {
                //enemy.TakeDamage(damage);
            }
        }
    }
} 
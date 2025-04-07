using UnityEngine;

public class PocketWatchWeapon : WeaponBase
{
    [Header("회중시계 설정")]
    public string watchEffectPoolTag = "PocketWatchEffect";
    public float effectRadius = 5f;
    public float effectDuration = 3f;
    public PocketWatchWeaponStats weaponStats;

    private void Start()
    {
        if (weaponStats != null && weaponStats.levelStats.Length > 0)
        {
            UpdateStats();
        }
    }

    public override void LevelUp()
    {
        if (weaponStats != null && currentLevel < weaponStats.levelStats.Length)
        {
            currentLevel++;
            UpdateStats();
        }
    }

    private void UpdateStats()
    {
        if (weaponStats != null && currentLevel <= weaponStats.levelStats.Length)
        {
            PocketWatchWeaponStats.LevelStats stats = weaponStats.levelStats[currentLevel - 1];
            currentLevel = stats.currentLevel;
            baseDamage = stats.damage;
            attackCooldown = stats.attackCooldown;
            effectRadius = stats.effectRadius;
            effectDuration = stats.effectDuration;
        }
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            LevelUp();
        }
        base.Update();
    }

    protected override void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // 가장 가까운 적 찾기
        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            // 가장 가까운 적 주변의 모든 적 찾기
            Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(nearestEnemy.transform.position, effectRadius);
            
            // 회중시계 이펙트 생성
            GameObject effectObj = ObjectPool.Instance.SpawnFromPool(watchEffectPoolTag, nearestEnemy.transform.position, Quaternion.identity);
            if (effectObj != null)
            {
                PocketWatchEffect effect = effectObj.GetComponent<PocketWatchEffect>();
                if (effect != null)
                {
                    effect.Initialize(baseDamage, effectDuration, effectRadius);
                    
                    // 주변의 모든 적에게 데미지 적용
                    foreach (Collider2D enemyCollider in nearbyEnemies)
                    {
                        if (enemyCollider.CompareTag("Enemy"))
                        {
                            Enemy enemy = enemyCollider.GetComponent<Enemy>();
                            if (enemy != null)
                            {
                                enemy.TakeDamage(baseDamage);
                            }
                        }
                    }
                }
            }
            nextAttackTime = 0f;
        }
    }

    private GameObject FindNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        // 주변의 모든 적 찾기
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = collider.gameObject;
                }
            }
        }

        return nearestEnemy;
    }

    private void OnDrawGizmos()
    {
        if (FindNearestEnemy() != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(FindNearestEnemy().transform.position, effectRadius);
        }
    }
} 
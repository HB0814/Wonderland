using UnityEngine;

public class FirecrackerWeapon : WeaponBase
{
    [Header("축하 폭죽 설정")]
    public string firecrackerEffectPoolTag = "FirecrackerEffect";

    private void Awake()
    {
        weaponData = WeaponDataManager.Instance.GetWeaponData(WeaponType.Firecracker);
        WeaponType = WeaponType.Firecracker;
    }
    private void Start()
    {
        base.Start();
    }

    private void LevelUpLogic()
    {
        base.LevelUpLogic();
    }

    private void UpdateStats()
    {
        base.UpdateStats();
    }



    protected override void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // 범위 내의 적 찾기
        GameObject targetEnemy = FindEnemyInRange();
        if (targetEnemy != null)
        {
            // 적 주변에 폭죽 이펙트 생성
            for (int i = 0; i < count; i++)
            {
                Vector2 randomOffset = Random.insideUnitCircle * detectionRange;
                Vector3 spawnPosition = targetEnemy.transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
                
                GameObject effectObj = ObjectPool.Instance.SpawnFromPool(firecrackerEffectPoolTag, spawnPosition, Quaternion.identity);
                if (effectObj != null)
                {
                    Effect effect = effectObj.GetComponent<Effect>();
                    if (effect != null)
                    {
                        effect.BaseInitialize(damage, size, lifeTime);
                        effect.DebuffInitialize(knockbackForce, slowForce, slowDuration);
                    }
                }
            }
        }
    }

    private GameObject FindEnemyInRange()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        if (colliders.Length == 0) return null;

        Collider2D[] enemyColliders = System.Array.FindAll(colliders, collider => collider.CompareTag("Enemy"));
        if (enemyColliders.Length == 0) return null;

        // 가장 가까운 적 선택
        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (var enemyCollider in enemyColliders)
        {
            float distance = Vector2.Distance(transform.position, enemyCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemyCollider.gameObject;
            }
        }

        return closestEnemy;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
} 
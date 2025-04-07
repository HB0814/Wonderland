using UnityEngine;

public class CheshireCatWeapon : WeaponBase
{
    [Header("채셔캣 설정")]
    public string catEffectPoolTag = "CheshireCatEffect";
    public float effectRadius = 3f;
    public float effectDuration = 1f;
    public float detectionRange = 10f;
    public CheshireCatWeaponStats weaponStats;

    private void Start()
    {
        if (weaponStats != null && weaponStats.levelStats.Length > 0)
        {
            UpdateStats();
        }
        attackCooldown = 3f; // 기본 쿨다운 3초
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
            CheshireCatWeaponStats.LevelStats stats = weaponStats.levelStats[currentLevel - 1];
            currentLevel = stats.currentLevel;
            baseDamage = stats.damage;
            effectRadius = stats.effectRadius;
            effectDuration = stats.effectDuration;
            detectionRange = stats.detectionRange;
        }
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
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

        // 무작위로 한 명의 적 선택
        GameObject targetEnemy = FindRandomEnemyInRange();
        if (targetEnemy != null)
        {
            // 선택된 적의 위치에 이펙트 생성
            GameObject effectObj = ObjectPool.Instance.SpawnFromPool(catEffectPoolTag, targetEnemy.transform.position, Quaternion.identity);
            if (effectObj != null)
            {
                CheshireCatEffect effect = effectObj.GetComponent<CheshireCatEffect>();
                if (effect != null)
                {
                    effect.Initialize(baseDamage, effectDuration, effectRadius);
                }
            }
        }
    }

    private GameObject FindRandomEnemyInRange()
    {
        // 범위 내의 모든 적 찾기
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        if (colliders.Length == 0) return null;

        // 적들 중에서 랜덤하게 선택
        Collider2D[] enemyColliders = System.Array.FindAll(colliders, collider => collider.CompareTag("Enemy"));
        if (enemyColliders.Length == 0) return null;

        int randomIndex = Random.Range(0, enemyColliders.Length);
        return enemyColliders[randomIndex].gameObject;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
} 
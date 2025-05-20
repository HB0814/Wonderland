using UnityEngine;

public class CheshireCatWeapon : WeaponBase
{
    [Header("채셔캣 설정")]
    public string catEffectPoolTag = "CheshireCatEffect";

    private void Awake()
    {
        weaponData = WeaponDataManager.Instance.GetWeaponData(WeaponType.Cat);
        WeaponType = WeaponType.Cat;
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

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            LevelUpLogic();
        }
        base.Update();
    }

    protected override void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        TargetEnemyAttack();
        //RandomAttack();
    }
    private void TargetEnemyAttack()
    {
        // 무작위로 한 명의 적 선택
        GameObject targetEnemy = FindRandomEnemyInRange();
        if (targetEnemy != null)
        {
            // 선택된 적의 위치에 이펙트 생성
            GameObject effectObj = ObjectPool.Instance.SpawnFromPool(catEffectPoolTag, targetEnemy.transform.position, Quaternion.identity);
            if (effectObj != null)
            {
                Effect effect = effectObj.GetComponent<Effect>();
                if (effect != null)
                {
                    effect.BaseInitialize(damage, size, lifeTime);
                    effect.DebuffInitialize(knockbackForce, slowForce, slowDuration);
                }
            }
            nextAttackTime = 0f;
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

    // 범위 안에 랜덤한 좌표를 공격
    private void RandomAttack()
    {
        // 범위 안에 랜덤한 좌표 생성
        Vector3 randomPosition = new Vector3
        (
            Random.Range(-detectionRange, detectionRange),
            Random.Range(-detectionRange, detectionRange),
            0
        );
        // 이펙트 생성
        GameObject effectObj = ObjectPool.Instance.SpawnFromPool(catEffectPoolTag, randomPosition, Quaternion.identity);
        if (effectObj != null)
        {
            Effect effect = effectObj.GetComponent<Effect>();
            effect.BaseInitialize(damage, size, lifeTime);
            effect.DebuffInitialize(knockbackForce, slowForce, slowDuration);
        }
    }
} 
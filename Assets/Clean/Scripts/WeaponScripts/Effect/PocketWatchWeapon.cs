using UnityEngine;

public class PocketWatchWeapon : WeaponBase
{
    [Header("회중시계 설정")]
    public string watchEffectPoolTag = "PocketWatchEffect";

    private void Awake()
    {
        weaponData = WeaponDataManager.Instance.GetWeaponData(WeaponType.Watch);
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
        if (Input.GetKeyDown(KeyCode.RightBracket))
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

        // 가장 가까운 적 찾기
        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            // 회중시계 이펙트 생성
            GameObject effectObj = ObjectPool.Instance.SpawnFromPool(watchEffectPoolTag, nearestEnemy.transform.position, Quaternion.identity);
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

    private GameObject FindNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        // 주변의 모든 적 찾기
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);
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
} 
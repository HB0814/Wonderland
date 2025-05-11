using UnityEngine;

public class BookThrowWeapon : WeaponBase
{
    [Header("책 던지기 설정")]
    public string bookProjectilePoolTag = "BookProjectile";

    private void Awake()
    {
        weaponData = WeaponDataManager.Instance.GetWeaponData(WeaponType.Book);
        WeaponType = WeaponType.Book;
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
        if (nextAttackTime >= attackCooldown)
        {
            SpawnBook();
            //nextAttackTime = 0f;
        }
    }
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            LevelUpLogic();
        }
        base.Update();
    }

    private void SpawnBook()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // 가장 가까운 적 찾기
        Vector3 nearestEnemyDirection = GetNearestEnemyDirection(transform.position);
        if (nearestEnemyDirection != Vector3.zero)
        {
            // 책 투사체 생성
            GameObject projectileObj = ObjectPool.Instance.SpawnFromPool(bookProjectilePoolTag, transform.position, Quaternion.identity);
            if (projectileObj != null)
            {
                Projectile projectile = projectileObj.GetComponent<Projectile>();
                if (projectile != null)
                {
                    projectile.BaseInitialize(damage, size, lifeTime, speed);
                    projectile.DebuffInitialize(knockbackForce, slowForce, slowDuration);
                    projectile.PierceInitialize(weaponData.levelStats.pierceCount[currentLevel - 1]);
                    projectile.SetDirection(nearestEnemyDirection);
                }
            }
            nextAttackTime = 0f;
        }
    }

    private Vector3 GetNearestEnemyDirection(Vector3 spawnPosition)
    {
        // 범위 내의 모든 콜라이더 검색
        Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPosition, detectionRange);
        float nearestDistance = float.MaxValue;
        Vector3 nearestEnemyPosition = Vector3.zero;

        // 가장 가까운 적 찾기
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(spawnPosition, collider.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemyPosition = collider.transform.position;
                }
            }
        }

        if (nearestEnemyPosition != Vector3.zero)
        {
            return (nearestEnemyPosition - spawnPosition).normalized;
        }
        return Vector3.zero;
    }
}
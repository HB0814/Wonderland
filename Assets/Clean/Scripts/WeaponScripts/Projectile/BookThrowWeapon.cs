using UnityEngine;

public class BookThrowWeapon : WeaponBase
{
    [Header("책 던지기 설정")]
    public string bookProjectilePoolTag = "BookProjectile";

    private void Awake()
    {
        weaponData = WeaponDataManager.Instance.GetWeaponData(WeaponType.Book);
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
        SpawnBook();
    }
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            LevelUpLogic();
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

        // 적이 발견되면 그 방향으로, 아니면 랜덤 방향으로
        if (nearestDistance < float.MaxValue)
        {
            return (nearestEnemyPosition - spawnPosition).normalized;
        }
        else
        {
            // 랜덤한 각도 생성 (0~360도)
            float randomAngle = Random.Range(0f, 360f);
            float radian = randomAngle * Mathf.Deg2Rad;
            return new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0).normalized;
        }
    }

    private void SpawnBook()
    {
        nextAttackTime = 0f;
        // 랜덤 시작 위치 설정
        Vector3 spawnPosition = transform.position;
        
        // 책 생성
        GameObject book = WeaponManager.Instance.SpawnProjectile(bookProjectilePoolTag, spawnPosition, Quaternion.identity);
        if (book != null)
        {
            Projectile projectile = book.GetComponent<Projectile>();
            if (projectile != null)
            {
                // 가장 가까운 적을 향한 방향 설정
                Vector3 direction = GetNearestEnemyDirection(spawnPosition);
                projectile.SetDirection(direction);
                
                // 무기 속성 설정
                projectile.BaseInitialize(damage, size, lifeTime, speed);
                projectile.DebuffInitialize(knockbackForce, slowForce, slowDuration);
            }
        }
    }
} 

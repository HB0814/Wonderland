using UnityEngine;

public class TeaSplashWeapon : WeaponBase
{
    [Header("홍차 뿌리기 특수 속성")]
    public string teaPoolTag = "TeaProjectile";
    public float teaSpeed = 12f;
    public float spreadAngle = 30f; // 부채꼴 모양의 각도
    public int projectileCount = 3; // 발사되는 투사체 개수
    public float knockbackForce = 5f; // 넉백 힘
    public TeaSplashStats weaponStats;

    private void Awake()
    {
        weaponData = WeaponDataManager.Instance.GetWeaponData(WeaponType.Tea);
        WeaponType = WeaponType.Tea;
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
        if (Input.GetKeyDown(KeyCode.P))
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
            // 적의 방향 계산
            Vector2 baseDirection = (nearestEnemy.transform.position - transform.position).normalized;
            float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;

            // 투사체 발사
            for (int i = 0; i < projectileCount; i++)
            {
                // 각 투사체의 각도 계산
                float angleStep = spreadAngle / (projectileCount - 1);
                float currentAngle = baseAngle - (spreadAngle / 2) + (angleStep * i);
                Vector2 direction = Quaternion.Euler(0, 0, currentAngle) * Vector2.right;

                // 홍차 투사체 생성 및 발사
                GameObject tea = WeaponManager.Instance.SpawnProjectile(teaPoolTag, transform.position, Quaternion.Euler(0, 0, currentAngle));
                if (tea != null)
                {
                    Rigidbody2D rb = tea.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.linearVelocity = direction * teaSpeed;
                    }

                    // 투사체에 데미지와 넉백 설정
                    Projectile teaProjectile = tea.GetComponent<Projectile>();
                    if (teaProjectile != null)
                    {
                        teaProjectile.BaseInitialize(damage, size, lifeTime, speed);
                        teaProjectile.DebuffInitialize(knockbackForce, slowForce, slowDuration);
                    }
                }
            }
            nextAttackTime = 0f;
            SoundManager.Instance?.PlayWeaponSound(weaponData.weaponType);
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
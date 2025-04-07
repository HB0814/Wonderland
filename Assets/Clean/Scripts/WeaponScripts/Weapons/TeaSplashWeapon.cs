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
            TeaSplashStats.LevelStats stats = weaponStats.levelStats[currentLevel - 1];
            currentLevel = stats.currentLevel;
            baseDamage = stats.damage;
            attackCooldown = stats.attackCooldown;
            teaSpeed = stats.teaSpeed;
            spreadAngle = stats.spreadAngle;
            projectileCount = stats.projectileCount;
            knockbackForce = stats.knockbackForce;
        }
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
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
                    TeaProjectile teaProjectile = tea.GetComponent<TeaProjectile>();
                    if (teaProjectile != null)
                    {
                        teaProjectile.damage = baseDamage;
                        teaProjectile.knockbackForce = knockbackForce;
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
} 
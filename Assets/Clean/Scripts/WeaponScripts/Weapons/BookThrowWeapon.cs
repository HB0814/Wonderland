using UnityEngine;
using System.Collections.Generic;

public class BookThrowWeapon : WeaponBase
{
    [Header("책 던지기 특수 속성")]
    public string bookPoolTag = "BookProjectile"; // ObjectPool에서 사용할 태그
    public float bookSpeed = 15f;
    public float detectionRange = 10f; // 적 탐지 범위
    public LayerMask enemyLayer; // 적 레이어
    public BookThrowStats weaponStats;

    private void Start()
    {
        if (weaponStats != null && weaponStats.levelStats.Length > 0)
        {
            UpdateStats();
        }
    }

    public void LevelUp()
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
            BookThrowStats.LevelStats stats = weaponStats.levelStats[currentLevel - 1];
            currentLevel = stats.currentLevel;
            baseDamage = stats.damage;
            detectionRange = stats.bookDetectionRange;
            attackCooldown = stats.attackCooldown;
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
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
            Vector2 direction = (nearestEnemy.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 책 생성 및 발사
            GameObject book = WeaponManager.Instance.SpawnProjectile(bookPoolTag, transform.position, Quaternion.Euler(0, 0, angle));
            if (book != null)
            {
                Rigidbody2D rb = book.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = direction * bookSpeed;
                }

                // 책에 데미지 설정 (기본 데미지의 2배)
                BookProjectile bookProjectile = book.GetComponent<BookProjectile>();
                if (bookProjectile != null)
                {
                    bookProjectile.damage = baseDamage * 2f;
                }
                
                nextAttackTime = 0f;
            }
        }
    }

    private GameObject FindNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        // 주변의 모든 적 찾기
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange, enemyLayer);
        foreach (Collider2D collider in colliders)
        {
            // 적과의 거리 계산
            float distance = Vector2.Distance(transform.position, collider.transform.position);
            
            // 더 가까운 적을 찾으면 업데이트
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = collider.gameObject;
            }
        }

        return nearestEnemy;
    }
} 
using UnityEngine;
using System.Collections.Generic;

public class HatBoomerangWeapon : WeaponBase
{
    [Header("모자 부메랑 특수 속성")]
    public string hatPoolTag = "HatProjectile";
    public LayerMask enemyLayer;
    public HatBoomerangStats weaponStats;
    public float hatSpeed = 15f;
    public float maxDistance = 10f; // 최대 사정거리
    public float returnSpeed = 20f; // 돌아오는 속도
    public float cooldownReduction = 0.2f; // 모자를 잡았을 때 쿨다운 감소 비율

    private bool isHatReturning = false;
    private GameObject currentHat = null;
    private Vector2 throwDirection;
    private float currentDistance = 0f;

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
            HatBoomerangStats.LevelStats stats = weaponStats.levelStats[currentLevel - 1];
            currentLevel = stats.currentLevel;
            baseDamage = stats.damage;
            attackCooldown = stats.attackCooldown;
            hatSpeed = stats.hatSpeed;
            maxDistance = stats.maxDistance;
            returnSpeed = stats.returnSpeed;
            cooldownReduction = stats.cooldownReduction;
        }
    }

    protected override void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // 이미 모자가 날아가고 있다면 공격하지 않음
        if (currentHat != null) return;

        // 가장 가까운 적 찾기
        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            // 적의 방향 계산
            throwDirection = (nearestEnemy.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(throwDirection.y, throwDirection.x) * Mathf.Rad2Deg;

            // 모자 생성 및 발사
            GameObject hat = WeaponManager.Instance.SpawnProjectile(hatPoolTag, transform.position, Quaternion.Euler(0, 0, angle));
            if (hat != null)
            {
                currentHat = hat;
            }
            if (currentHat != null)
            {
                Rigidbody2D rb = currentHat.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = throwDirection * hatSpeed;
                }

                // 모자에 데미지 설정
                HatProjectile hatProjectile = currentHat.GetComponent<HatProjectile>();
                if (hatProjectile != null)
                {
                    hatProjectile.damage = baseDamage;
                }

                isHatReturning = false;
                currentDistance = 0f;
                nextAttackTime = 0f;
            }
        }
    }

    protected override void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LevelUp();
        }

        base.Update();

        if (currentHat != null)
        {
            if (!isHatReturning)
            {
                // 앞으로 날아가는 중
                currentDistance = Vector2.Distance(transform.position, currentHat.transform.position);
                if (currentDistance >= maxDistance)
                {
                    isHatReturning = true;
                }
            }
            else
            {
                // 돌아오는 중
                Vector2 returnDirection = (transform.position - currentHat.transform.position).normalized;
                Rigidbody2D rb = currentHat.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = returnDirection * returnSpeed;
                }

                // 플레이어와 충돌 체크
                float distanceToPlayer = Vector2.Distance(transform.position, currentHat.transform.position);
                if (distanceToPlayer < 1f)
                {
                    // 모자를 잡았을 때 쿨다운 감소
                    nextAttackTime += attackCooldown * cooldownReduction;
                    
                    // 모자 제거
                    ObjectPool.Instance.ReturnToPool(hatPoolTag, currentHat);
                    currentHat = null;
                    isHatReturning = false;
                }
            }
        }
    }

    private GameObject FindNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, maxDistance, enemyLayer);
        foreach (Collider2D collider in colliders)
        {
            float distance = Vector2.Distance(transform.position, collider.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = collider.gameObject;
            }
        }

        return nearestEnemy;
    }
} 
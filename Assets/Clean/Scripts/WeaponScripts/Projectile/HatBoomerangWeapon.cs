using UnityEngine;
using System.Collections.Generic;

public class HatBoomerangWeapon : WeaponBase
{
    [Header("모자 부메랑 속성")]
    public string hatPoolTag = "HatProjectile";
    private float cooldownReduction = 0.2f;

    private void Awake()
    {
        weaponData = WeaponDataManager.Instance.GetWeaponData(WeaponType.Hat);
        WeaponType = WeaponType.Hat;
    }
    private void Start()
    {
        base.Start();
    }

    private void LevelUpLogic()
    {
        base.LevelUpLogic();
        cooldownReduction+=0.1f;
    }

    private void UpdateStats()
    {
        base.UpdateStats();
    }
    protected override void Attack()
    {
        SpawnHat();
    }
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LevelUpLogic();
        }
        base.Update();
    }
    private void SpawnHat()
    {
        nextAttackTime = 0f;
        GameObject hat = WeaponManager.Instance.SpawnProjectile(hatPoolTag, transform.position, Quaternion.identity);
        Projectile projectile = hat.GetComponent<Projectile>();
        
        if (projectile != null)
        {
            projectile.weaponType = WeaponType.Hat;
            projectile.BaseInitialize(damage, size, lifeTime, speed);
            projectile.DebuffInitialize(knockbackForce, slowForce, slowDuration);
            
            // 가장 가까운 적을 향해 발사
            Vector3 direction = GetNearestEnemyDirection();
            projectile.SetDirection(direction);
        }
    }

    private Vector3 GetNearestEnemyDirection()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return transform.right; // 적이 없으면 기본 방향으로 발사

        GameObject nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            return (nearestEnemy.transform.position - transform.position).normalized;
        }

        return transform.right;
    }

    public void ReduceCooldown()
    {
        var cooldown=(attackCooldown * cooldownReduction);
        nextAttackTime += cooldown;
        Debug.Log("cooldown: " + cooldown);
    }
} 
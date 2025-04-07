using UnityEngine;
using System.Collections.Generic;

public class FireworkWeapon : WeaponBase
{
    [Header("축하 폭죽 설정")]
    public string fireworkPoolTag = "FireworkEffect";
    public float effectRadius = 1.5f;
    public float effectScale = 1f;
    public float damageInterval = 1f;
    public float effectDuration = 5f;
    public float spawnRange = 3f;
    public FireworkWeaponStats weaponStats;

    private List<FireworkEffect> activeEffects = new List<FireworkEffect>();

    protected override void Start()
    {
        base.Start();
        if (weaponStats != null)
        {
            UpdateStats();
        }
    }

    public override void LevelUp()
    {
        if (weaponStats != null)
        {
            weaponStats.LevelUp();
            UpdateStats();
        }
    }

    private void UpdateStats()
    {
        FireworkWeaponStats.LevelStats stats = weaponStats.GetCurrentLevelStats();
        currentLevel = stats.currentLevel;
        baseDamage = stats.damage;
        effectRadius = stats.effectRadius;
        effectScale = stats.effectScale;
        damageInterval = stats.damageInterval;
        effectDuration = stats.effectDuration;
        spawnRange = stats.spawnRange;
    }

    protected override void Update()
    {
        base.Update();
        
        // P키를 눌러 레벨업
        if (Input.GetKeyDown(KeyCode.P))
        {
            LevelUp();
        }

        // 비활성화된 이펙트 제거
        activeEffects.RemoveAll(effect => effect == null || !effect.gameObject.activeSelf);
    }

    protected override void Attack()
    {
        // 공격 애니메이션 재생
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // 현재 레벨에 따른 고정된 수의 이펙트 생성
        int effectCount = weaponStats.GetCurrentLevelStats().effectCount;
        for (int i = 0; i < effectCount; i++)
        {
            CreateFireworkEffect();
        }
    }

    private void CreateFireworkEffect()
    {
        nextAttackTime=0f;
        // 플레이어 주변 랜덤 위치 계산
        Vector2 randomDirection = Random.insideUnitCircle;
        Vector2 spawnPosition = (Vector2)transform.position + randomDirection * spawnRange;

        // 오브젝트 풀에서 이펙트 가져오기
        GameObject effectObj = WeaponManager.Instance.SpawnProjectile(fireworkPoolTag, spawnPosition, Quaternion.identity);
        if (effectObj != null)
        {
            effectObj.transform.position = spawnPosition;
            effectObj.SetActive(true);

            FireworkEffect effect = effectObj.GetComponent<FireworkEffect>();
            if (effect != null)
            {
                effect.Initialize(baseDamage, effectDuration, effectRadius);
                effect.damageInterval = damageInterval;
                effect.effectScale = effectScale;
                activeEffects.Add(effect);
            }
        }
    }
} 
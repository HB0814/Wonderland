using UnityEngine;

public class PipeWeapon : WeaponBase
{
    [Header("담배 파이프 특수 속성")]
    public string smokePoolTag = "PipeSmoke";
    public float smokeRadius = 1f;
    public float smokeExpandSpeed = 2f;
    public float smokeMaxRadius = 3f;
    public float smokeDamageInterval = 0.5f;
    public PipeWeaponStats weaponStats;

    private GameObject currentSmokeEffect;
    private PipeSmokeEffect smokeEffectComponent;

    private void Start()
    {
        if (weaponStats != null && weaponStats.levelStats.Length > 0)
        {
            UpdateStats();
        }
        // 시작할 때 연기 이펙트 생성
        CreateSmokeEffect();
    }

    public override void LevelUp()
    {
        if (weaponStats != null && currentLevel < weaponStats.levelStats.Length)
        {
            currentLevel++;
            UpdateStats();
            // 레벨업 시 연기 이펙트 업데이트
            UpdateSmokeEffect();
        }
    }

    private void UpdateStats()
    {
        if (weaponStats != null && currentLevel <= weaponStats.levelStats.Length)
        {
            PipeWeaponStats.LevelStats stats = weaponStats.levelStats[currentLevel - 1];
            currentLevel = stats.currentLevel;
            baseDamage = stats.damage;
            smokeRadius = stats.smokeRadius;
            smokeExpandSpeed = stats.smokeExpandSpeed;
            smokeMaxRadius = stats.smokeMaxRadius;
            smokeDamageInterval = stats.smokeDamageInterval;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            LevelUp();
        }

        // 연기 이펙트가 없으면 생성
        if (currentSmokeEffect == null || !currentSmokeEffect.activeSelf)
        {
            CreateSmokeEffect();
        }
        else
        {
            // 연기 이펙트 위치 업데이트
            currentSmokeEffect.transform.position = transform.position;
        }
    }

    protected override void Attack()
    {
        // 이 무기는 Attack 메서드가 필요 없지만, 추상 메서드이므로 구현해야 합니다.
        // 실제 공격은 Update에서 연기 이펙트를 통해 처리됩니다.
    }

    private void CreateSmokeEffect()
    {
        // 연기 이펙트 생성
        currentSmokeEffect = WeaponManager.Instance.SpawnProjectile(smokePoolTag, transform.position, Quaternion.identity);
        if (currentSmokeEffect != null)
        {
            smokeEffectComponent = currentSmokeEffect.GetComponent<PipeSmokeEffect>();
            if (smokeEffectComponent != null)
            {
                smokeEffectComponent.Initialize(baseDamage, float.MaxValue, smokeRadius); // duration을 무한대로 설정
                smokeEffectComponent.expandSpeed = smokeExpandSpeed;
                smokeEffectComponent.maxRadius = smokeMaxRadius;
                smokeEffectComponent.damageInterval = smokeDamageInterval;
            }
        }
    }

    private void UpdateSmokeEffect()
    {
        if (smokeEffectComponent != null)
        {
            smokeEffectComponent.damage = baseDamage;
            smokeEffectComponent.expandSpeed = smokeExpandSpeed;
            smokeEffectComponent.maxRadius = smokeMaxRadius;
            smokeEffectComponent.damageInterval = smokeDamageInterval;
        }
    }
} 
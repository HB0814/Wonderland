using UnityEngine;

public class PipeWeapon : WeaponBase
{
    [Header("담배 파이프 특수 속성")]
    public string smokePoolTag = "PipeSmoke";

    private GameObject currentSmokeEffect;

    private void Awake()
    {
        weaponData = WeaponDataManager.Instance.GetWeaponData(WeaponType.Pipe);
        WeaponType = WeaponType.Pipe;
    }
    private void Start()
    {
        base.Start();
        CreateSmokeEffect();
    }

    private void LevelUpLogic()
    {
        base.LevelUpLogic();
    }

    private void UpdateStats()
    {
        base.UpdateStats();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            LevelUpLogic();
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
            Effect effect = currentSmokeEffect.GetComponent<Effect>();
            if (effect != null)
            {
                effect.BaseInitialize(damage, size, float.MaxValue); // duration을 무한대로 설정
                effect.DebuffInitialize(knockbackForce, slowForce, slowDuration);
            }
        }
    }
} 
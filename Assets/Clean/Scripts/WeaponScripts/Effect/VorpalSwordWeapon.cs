using UnityEngine;

public class VorpalSwordWeapon : WeaponBase
{
    [Header("보팔 검 설정")]
    public string swordEffectPoolTag = "VorpalSwordEffect";
    public float effectOffset = 0.5f; // 이펙트 위치 오프셋

    private Rigidbody2D playerRb;
    private Vector2 lastMoveDirection = Vector2.right; // 마지막 이동 방향 저장

    private void Awake()
    {
        weaponData = WeaponDataManager.Instance.GetWeaponData(WeaponType.Sword);
        WeaponType = WeaponType.Sword;
    }
    private void Start()
    {
        base.Start();
        playerRb = GetComponentInParent<Rigidbody2D>();
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
        // 플레이어의 이동 방향 업데이트
        if (playerRb.linearVelocity != Vector2.zero)
        {
            lastMoveDirection = playerRb.linearVelocity.normalized;
        }

        // 공격 쿨다운 체크
        nextAttackTime += Time.deltaTime;
        if (nextAttackTime >= attackCooldown)
        {
            Attack();
            nextAttackTime = 0f;
        }
        // 레벨업 테스트 '[' 키로 레벨업
        if(Input.GetKeyDown(KeyCode.LeftBracket))
        {
            LevelUpLogic();
        }
    }

    protected override void Attack()
    {
        // 공격 애니메이션 재생
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // 플레이어가 바라보는 방향에 따라 이펙트 생성
        CreateSwordEffect();
    }

    private void CreateSwordEffect()
    {
        Debug.Log("CreateSwordEffect");
        // 플레이어의 위치를 기준으로 이펙트 생성
        Vector3 spawnPosition = transform.position;

        // 이동 방향에 따라 이펙트 위치 조정
        spawnPosition += (Vector3)lastMoveDirection * effectOffset;

        // 이동 방향에 따른 회전 각도 계산
        float angle = Mathf.Atan2(lastMoveDirection.y, lastMoveDirection.x) * Mathf.Rad2Deg;
        Quaternion spawnRotation = Quaternion.Euler(0, 0, angle);

        // 오브젝트 풀에서 이펙트 가져오기
        GameObject effectObj = ObjectPool.Instance.SpawnFromPool(swordEffectPoolTag, spawnPosition, spawnRotation);
        if (effectObj != null)
        {
            Effect effect = effectObj.GetComponent<Effect>();
            if (effect != null)
            {
                effect.BaseInitialize(damage, size, lifeTime);
                effect.DebuffInitialize(knockbackForce, slowForce, slowDuration);
            }
        }
        SoundManager.Instance?.PlayWeaponSound(weaponData.weaponType);
    }
} 
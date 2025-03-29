using UnityEngine;
using System.Collections;

/// <summary>
/// 담배 파이프 무기 클래스
/// 플레이어 주변에 연기를 생성하여 지속적으로 데미지를 주는 근접 무기
/// </summary>
public class TobaccoPipe : MeleeWeaponBase
{
    [Header("담배 파이프 특수 효과")]
    public GameObject smokeEffectPrefab;   // 연기 이펙트 프리팹
    public float effectDuration = 1.0f;    // 이펙트 지속 시간
    public float aoeDamageInterval = 1.0f; // 데미지 간격 (초)
    public float aoeDamage = 8f;          // 초당 데미지

    [Header("슬로우 효과")]
    public bool enableSlowEffect = false;  // 슬로우 효과 활성화 여부
    public float slowAmount = 0.5f;       // 슬로우 효과 강도 (0.5 = 50% 감소)
    public float slowDuration = 2f;       // 슬로우 지속 시간

    private float nextAoeDamageTime;       // 다음 데미지 시간
    private GameObject currentSmokeEffect;  // 현재 활성화된 연기 이펙트
    private bool isEffectCreated = false;   // 이펙트 생성 여부

    protected override void Awake()
    {
        // 기본 속성 설정
        damage = 0f;                    // 직접 공격은 없음
        attackRange = 2.5f;            // 공격 범위는 AoE 범위와 동일
        detectionRange = 2.5f;         // 감지 범위도 AoE 범위와 동일
        attackSpeed = 1f;              // 공격 속도는 AoE 데미지 간격과 동일
        
        // 부모 클래스 초기화
        base.Awake();
        
        // 다음 데미지 시간 초기화
        nextAoeDamageTime = Time.time;
        
        // 이펙트 생성 시도
        StartCoroutine(TryCreateSmokeEffect());
    }

    private System.Collections.IEnumerator TryCreateSmokeEffect()
    {
        // ObjectPool이 준비될 때까지 대기
        while (ObjectPool.Instance == null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        // 이펙트 생성
        CreateSmokeEffect();
        isEffectCreated = true;
    }

    protected override void Update()
    {
        // 기존 Update 로직은 사용하지 않음
        if (Time.time >= nextAoeDamageTime)
        {
            ApplyAoeDamage();
            nextAoeDamageTime = Time.time + aoeDamageInterval;
        }

        // 이펙트가 생성되지 않았다면 다시 시도
        if (!isEffectCreated && ObjectPool.Instance != null)
        {
            CreateSmokeEffect();
            isEffectCreated = true;
        }
    }

    /// <summary>
    /// 연기 이펙트 생성
    /// </summary>
    private void CreateSmokeEffect()
    {
        if (ObjectPool.Instance == null) return;

        // 이전 이펙트 제거
        if (currentSmokeEffect != null)
        {
            Destroy(currentSmokeEffect);
        }

        // 새로운 이펙트 생성
        if (smokeEffectPrefab != null)
        {
            currentSmokeEffect = ObjectPool.Instance.SpawnFromPool("SmokeEffect", playerTransform.position, Quaternion.identity);
            if (currentSmokeEffect != null)
            {
                // 플레이어의 회전을 유지하면서 부모 설정
                currentSmokeEffect.transform.SetParent(playerTransform, true);
            }
        }
    }

    /// <summary>
    /// 범위 내 적들에게 데미지 적용
    /// </summary>
    private void ApplyAoeDamage()
    {
        if (playerTransform == null) return;

        // 플레이어 주변의 모든 적 검출
        int hitCount = Physics2D.OverlapCircleNonAlloc(
            playerTransform.position,
            attackRange,
            enemyCache,
            enemyLayer
        );
        
        for (int i = 0; i < hitCount; i++)
        {
            Enemy enemyComponent = enemyCache[i].GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                // 데미지 적용
                enemyComponent.TakeDamage(aoeDamage);

                // 슬로우 효과 적용 (활성화된 경우에만)
                if (enableSlowEffect)
                {
                    ApplySlowEffect(enemyComponent);
                }
            }
        }
    }

    /// <summary>
    /// 슬로우 효과 적용
    /// </summary>
    private void ApplySlowEffect(Enemy enemy)
    {
        enemy.ApplySlow(slowAmount, slowDuration);   
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        if (playerTransform != null)
        {
            // 피해 범위 시각화
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(playerTransform.position, attackRange);
        }
    }

    private void OnDestroy()
    {
        // 오브젝트가 파괴될 때 이펙트도 함께 제거
        if (currentSmokeEffect != null)
        {
            Destroy(currentSmokeEffect);
        }
    }
} 
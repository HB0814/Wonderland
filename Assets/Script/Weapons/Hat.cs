using UnityEngine;

/// <summary>
/// 모자 무기 클래스
/// 적을 향해 모자를 던지고 다시 돌아오는 원거리 무기
/// </summary>
public class Hat : MonoBehaviour
{
    [Header("모자 무기 속성")]
    public float damage = 10f;          // 모자의 공격력
    public float attackSpeed = 1f;      // 공격 속도 (초당 공격 횟수)
    public float throwRange = 5f;       // 모자 투척 최대 거리
    public float throwSpeed = 12f;      // 모자 투척 속도
    public float returnSpeed = 2f;      // 모자가 돌아오는 기본 속도
    public float rotationSpeed = 720f;  // 모자 회전 속도 (도/초)
    public LayerMask enemyLayer;        // 적 레이어 마스크
    public float cooldownReduction = 0.4f;  // 쿨다운 감소율 (40%)

    private float nextAttackTime;       // 다음 공격 가능 시간
    private Transform playerTransform;   // 플레이어 Transform 캐싱
    private GameObject activeHat;       // 현재 활성화된 모자 오브젝트
    private bool isTrackingMode = false;  // 추적 모드 여부

    /// <summary>
    /// 초기화 - 필요한 참조 설정
    /// </summary>
    void Start()
    {
        playerTransform = transform.parent;
        nextAttackTime = 0f;
    }

    /// <summary>
    /// 매 프레임 실행 - 공격 가능 여부 확인 및 실행
    /// </summary>
    void Update()
    {
        // T키를 눌러 모드 전환
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleTrackingMode();
        }

        // 공격 쿨다운이 끝났고 활성화된 모자가 없을 때만 공격
        if (Time.time >= nextAttackTime && activeHat == null)
        {
            FindAndAttackNearestEnemy();
            nextAttackTime = Time.time + 1f / attackSpeed;
        }
    }

    /// <summary>
    /// 추적 모드 전환
    /// </summary>
    private void ToggleTrackingMode()
    {
        isTrackingMode = !isTrackingMode;
        // 현재 활성화된 모자가 있다면 모드 변경 적용
        if (activeHat != null)
        {
            HatProjectile hatProjectile = activeHat.GetComponent<HatProjectile>();
            if (hatProjectile != null)
            {
                hatProjectile.SetTrackingMode(isTrackingMode);
            }
        }
    }

    /// <summary>
    /// 가장 가까운 적을 찾아 공격
    /// </summary>
    void FindAndAttackNearestEnemy()
    {
        // 사정거리 내의 모든 적 검색
        Collider2D[] enemies = Physics2D.OverlapCircleAll(playerTransform.position, throwRange, enemyLayer);
        
        Transform nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        // 가장 가까운 적 찾기
        foreach (Collider2D enemy in enemies)
        {
            float distance = Vector2.Distance(playerTransform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        // 적이 있으면 모자 던지기
        if (nearestEnemy != null)
        {
            ThrowHat(nearestEnemy.position);
        }
    }

    /// <summary>
    /// 지정된 위치를 향해 모자 던지기
    /// </summary>
    /// <param name="targetPosition">목표 위치</param>
    void ThrowHat(Vector3 targetPosition)
    {
        if (activeHat == null)
        {
            // 오브젝트 풀에서 모자 가져오기
            activeHat = ObjectPool.Instance.SpawnFromPool("HatProjectile", playerTransform.position, Quaternion.identity);
            if (activeHat != null)
            {
                // 모자 발사체 초기화
                HatProjectile hatProjectile = activeHat.GetComponent<HatProjectile>();
                if (hatProjectile != null)
                {
                    hatProjectile.Initialize(this, targetPosition);
                    hatProjectile.SetTrackingMode(isTrackingMode);  // 현재 모드 설정
                }
            }
        }
    }

    /// <summary>
    /// 모자가 파괴되거나 회수되었을 때 호출
    /// </summary>
    public void OnHatDestroyed()
    {
        activeHat = null;
    }

    /// <summary>
    /// 모자가 성공적으로 돌아왔을 때 쿨다운 감소
    /// </summary>
    public void ReduceAttackCooldown()
    {
        float currentTime = Time.time;
        float remainingCooldown = nextAttackTime - currentTime;
        
        if (remainingCooldown > 0)
        {
            // 남은 쿨다운 시간을 80% 감소
            float reduction = remainingCooldown * cooldownReduction;
            nextAttackTime -= reduction;
        }
    }

    /// <summary>
    /// 에디터에서 공격 범위를 시각적으로 표시
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (playerTransform != null)
        {
            // 모자 투척 범위 표시
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(playerTransform.position, throwRange);
        }
    }
} 
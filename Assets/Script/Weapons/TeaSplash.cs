using UnityEngine;

/// <summary>
/// 홍차 뿌리기 무기 클래스
/// 부채꼴 형태로 여러 발사체를 동시에 발사하는 원거리 무기
/// </summary>
public class TeaSplash : MonoBehaviour
{
    [Header("홍차 뿌리기 속성")]
    public float damage = 15f;              // 각 발사체의 데미지
    public float attackSpeed = 1f;          // 초당 공격 횟수
    public float splashRange = 3f;          // 부채꼴의 최대 사거리
    public float splashAngle = 60f;         // 부채꼴의 전체 각도
    public float knockbackForce = 8f;       // 적을 밀어내는 힘
    public float projectileSpeed = 15f;     // 발사체 이동 속도
    public float projectileLifetime = 0.3f; // 발사체 지속 시간
    public LayerMask enemyLayer;            // 적 레이어 마스크
    public GameObject splashEffectPrefab;   // 발사 효과 프리팹
    public float detectionRange = 5f;       // 적 감지 범위

    // 프라이빗 변수들
    private Transform playerTransform;                   // 플레이어(부모) Transform 캐싱
    private float nextAttackTime;                       // 다음 공격 가능 시간
    private readonly int projectileCount = 5;           // 한 번에 발사되는 발사체 수
    private Vector2[] projectileDirections;             // 미리 계산된 발사체 방향들
    private float angleStep;                            // 발사체 사이의 각도 간격
    private const float Deg2Rad = Mathf.Deg2Rad;        // 각도->라디안 변환 상수

    /// <summary>
    /// 초기화 - 필요한 참조와 계산값들을 설정
    /// </summary>
    void Start()
    {
        // 부모(플레이어) Transform 참조
        playerTransform = transform.parent;
        nextAttackTime = 0f;
        
        // 발사체 사이의 각도 간격 계산
        angleStep = splashAngle / (projectileCount - 1);
        
        // 발사체 방향 배열 초기화
        projectileDirections = new Vector2[projectileCount];
    }

    /// <summary>
    /// 매 프레임 실행 - 공격 타이밍 체크 및 실행
    /// </summary>
    void Update()
    {
        // 공격 쿨다운이 끝났는지 확인
        if (Time.time >= nextAttackTime)
        {
            // 가장 가까운 적의 방향 찾기
            Vector2 direction = FindNearestEnemyDirection();
            // 적이 감지되었을 때만 공격 실행
            if (direction != Vector2.zero)
            {
                CreateTeaSplash(direction);
                // 다음 공격 시간 설정
                nextAttackTime = Time.time + 1f / attackSpeed;
            }
        }
    }

    /// <summary>
    /// 가장 가까운 적의 방향을 찾아 반환
    /// </summary>
    /// <returns>적이 있는 방향의 정규화된 벡터, 적이 없으면 zero vector</returns>
    Vector2 FindNearestEnemyDirection()
    {
        // 감지 범위 내의 모든 적 콜라이더 검색
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, detectionRange, enemyLayer);
        
        if (enemies.Length > 0)
        {
            float nearestDistance = float.MaxValue;
            Transform nearestEnemy = null;

            // 가장 가까운 적 찾기
            foreach (Collider2D enemy in enemies)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy.transform;
                }
            }

            if (nearestEnemy != null)
            {
                // 적 방향으로의 단위 벡터 반환
                return ((Vector2)(nearestEnemy.position - transform.position)).normalized;
            }
        }

        return Vector2.zero;  // 적이 없으면 zero vector 반환
    }

    /// <summary>
    /// 지정된 방향으로 부채꼴 형태의 발사체들을 생성
    /// </summary>
    /// <param name="direction">발사 중심 방향</param>
    void CreateTeaSplash(Vector2 direction)
    {
        // 중심 각도와 시작 각도 계산
        float centerAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float startAngle = centerAngle - splashAngle * 0.5f;

        // 모든 발사체의 방향을 미리 계산
        for (int i = 0; i < projectileCount; i++)
        {
            float currentAngle = (startAngle + angleStep * i) * Deg2Rad;
            projectileDirections[i].x = Mathf.Cos(currentAngle);
            projectileDirections[i].y = Mathf.Sin(currentAngle);
        }

        // 각 방향으로 발사체 생성
        for (int i = 0; i < projectileCount; i++)
        {
            // 오브젝트 풀에서 발사체 가져오기
            GameObject teaProjectile = ObjectPool.Instance.SpawnFromPool("TeaSplashProjectile", playerTransform.position, Quaternion.identity);
            if (teaProjectile != null)
            {
                // 발사체 초기화
                TeaSplashProjectile projectile = teaProjectile.GetComponent<TeaSplashProjectile>();
                if (projectile != null)
                {
                    projectile.Initialize(this, projectileDirections[i]);
                }

                // 발사 효과 생성
                if (splashEffectPrefab != null)
                {
                    float currentAngle = startAngle + angleStep * i;
                    GameObject effect = ObjectPool.Instance.SpawnFromPool("TeaSplashEffect", teaProjectile.transform.position, Quaternion.Euler(0, 0, currentAngle));
                    effect.transform.SetParent(teaProjectile.transform);
                }
            }
        }
    }

    /// <summary>
    /// 에디터에서 공격 범위와 각도를 시각적으로 표시
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (playerTransform != null)
        {
            Vector3 position = transform.position;
            Vector3 forward = transform.right;
            
            // 부채꼴 경계선 각도 계산
            float halfAngle = splashAngle * 0.5f;
            Vector3 leftBoundary = Quaternion.Euler(0, 0, -halfAngle) * forward;
            Vector3 rightBoundary = Quaternion.Euler(0, 0, halfAngle) * forward;

            // 부채꼴 경계선 그리기
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(position, position + leftBoundary * splashRange);
            Gizmos.DrawLine(position, position + rightBoundary * splashRange);
            
            // 부채꼴 호 그리기
            const int segments = 10;  // 호를 그릴 때 사용할 선분 수
            Vector3 previousPoint = position + leftBoundary * splashRange;
            float segmentAngle = splashAngle / segments;
            
            // 선분들을 그려서 호 형성
            for (int i = 1; i <= segments; i++)
            {
                float angle = -halfAngle + (segmentAngle * i);
                Vector3 currentPoint = position + (Quaternion.Euler(0, 0, angle) * forward * splashRange);
                Gizmos.DrawLine(previousPoint, currentPoint);
                previousPoint = currentPoint;
            }
        }
    }
}
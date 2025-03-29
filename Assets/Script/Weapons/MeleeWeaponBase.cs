using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 근접 무기의 기본 클래스
/// 모든 근접 무기들이 상속받아 사용하는 추상 클래스
/// </summary>
public abstract class MeleeWeaponBase : MonoBehaviour
{
    [Header("무기 기본 속성")]
    [SerializeField] protected float damage = 10f;          // 기본 공격력
    [SerializeField] protected float attackSpeed = 1f;      // 공격 속도 (초당 공격 횟수)
    [SerializeField] protected float attackRange = 2f;      // 공격 범위
    [SerializeField] protected float attackAngle = 180f;    // 공격 각도
    [SerializeField] protected LayerMask enemyLayer;        // 적 레이어 마스크
    [SerializeField] protected float detectionRange = 3f;   // 적 감지 범위

    protected float nextAttackTime;     // 다음 공격 가능 시간
    protected Transform playerTransform; // 플레이어 Transform 캐싱
    protected Animator animator;         // 애니메이터 컴포넌트
    protected Collider2D[] enemyCache;   // 적 캐시 배열
    protected int enemyCacheSize = 20;   // 캐시 크기

    /// <summary>
    /// 초기화 - 컴포넌트 참조 및 기본값 설정
    /// 자식 클래스에서 재정의 가능
    /// </summary>
    protected virtual void Awake()
    {
        InitializePlayerTransform();
        animator = GetComponent<Animator>();
        enemyCache = new Collider2D[enemyCacheSize];
        nextAttackTime = 0f;
    }

    /// <summary>
    /// 플레이어 Transform 초기화
    /// 무기는 플레이어의 자식 오브젝트이므로 부모 Transform을 참조
    /// </summary>
    protected void InitializePlayerTransform()
    {
        if (playerTransform == null)
        {
            playerTransform = transform.parent;
        }
    }

    /// <summary>
    /// 매 프레임 실행되는 업데이트
    /// 공격 가능 시간을 체크하고 가장 가까운 적을 공격
    /// </summary>
    protected virtual void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            // 가장 가까운 적 탐색
            Transform nearestEnemy = FindNearestEnemy();
            if (nearestEnemy != null)
            {
                Attack(nearestEnemy.position);
                OnHitEffect(nearestEnemy.position);
            }
            nextAttackTime = Time.time + 1f / attackSpeed;
        }
    }

    /// <summary>
    /// 감지 범위 내에서 가장 가까운 적을 찾음
    /// </summary>
    /// <returns>가장 가까운 적의 Transform, 없으면 null</returns>
    protected Transform FindNearestEnemy()
    {
        if (playerTransform == null)
        {
            InitializePlayerTransform();
            if (playerTransform == null) return null;
        }

        // 감지 범위 내의 모든 적 검색
        int hitCount = Physics2D.OverlapCircleNonAlloc(
            playerTransform.position,
            detectionRange,
            enemyCache,
            enemyLayer
        );
        
        Transform nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        // 가장 가까운 적 찾기
        for (int i = 0; i < hitCount; i++)
        {
            Transform enemyTransform = enemyCache[i].transform;
            float distance = Vector2.Distance(playerTransform.position, enemyTransform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemyTransform;
            }
        }

        return nearestEnemy;
    }

    /// <summary>
    /// 지정된 위치를 향해 공격 실행
    /// </summary>
    /// <param name="targetPosition">공격 목표 위치</param>
    protected virtual void Attack(Vector3 targetPosition)
    {
        if (playerTransform == null) return;

        // 공격 방향 계산
        Vector2 attackDirection = ((Vector2)(targetPosition - playerTransform.position)).normalized;
        
        // 공격 범위 내의 모든 적 검출 및 데미지 적용
        int hitCount = Physics2D.OverlapCircleNonAlloc(
            playerTransform.position,
            attackRange,
            enemyCache,
            enemyLayer
        );
        
        for (int i = 0; i < hitCount; i++)
        {
            Vector2 enemyPosition = enemyCache[i].transform.position;
            Vector2 directionToEnemy = (enemyPosition - (Vector2)playerTransform.position).normalized;
            
            // 각도 체크
            float angle = Vector2.Angle(attackDirection, directionToEnemy);
            if (angle <= attackAngle * 0.5f)
            {
                Enemy enemy = enemyCache[i].GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }

        // 공격 애니메이션 재생
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    /// <summary>
    /// 공격 적중 효과 처리
    /// 자식 클래스에서 구현하여 사용
    /// </summary>
    /// <param name="hitPosition">적중 위치</param>
    protected virtual void OnHitEffect(Vector3 hitPosition)
    {
        // 자식 클래스에서 구현할 히트 이펙트
    }

    /// <summary>
    /// 에디터에서 공격 범위와 감지 범위를 시각적으로 표시
    /// </summary>
    protected virtual void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            if (playerTransform == null)
            {
                playerTransform = transform.parent;
            }

            if (playerTransform != null)
            {
                // 감지 범위 표시
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(playerTransform.position, detectionRange);

                // 공격 범위와 각도 표시
                DrawAttackRange(playerTransform.position);
            }
            else
            {
                // Transform이 없을 경우 현재 위치 기준으로 표시
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, detectionRange);
                DrawAttackRange(transform.position);
            }
            return;
        }

        if (playerTransform != null)
        {
            // 게임 실행 중 감지 범위 표시
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(playerTransform.position, detectionRange);

            // 현재 타겟팅된 적의 공격 범위 표시
            Transform nearestEnemy = FindNearestEnemy();
            if (nearestEnemy != null)
            {
                DrawAttackRange(playerTransform.position);
            }
        }
    }

    /// <summary>
    /// 공격 범위와 각도를 시각적으로 표시
    /// </summary>
    private void DrawAttackRange(Vector3 position)
    {
        // 공격 범위 원 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, attackRange);

        // 공격 각도 표시
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Vector3 forward = transform.right;
        float halfAngle = attackAngle * 0.5f;
        Vector3 leftBoundary = Quaternion.Euler(0, 0, -halfAngle) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, halfAngle) * forward;

        Gizmos.DrawLine(position, position + leftBoundary * attackRange);
        Gizmos.DrawLine(position, position + rightBoundary * attackRange);
    }
}
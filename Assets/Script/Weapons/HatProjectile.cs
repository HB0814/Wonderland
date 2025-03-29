using UnityEngine;
using System.Collections.Generic;

public class HatProjectile : MonoBehaviour, IPooledObject
{
    private Hat hatWeapon;
    private Vector3 targetPosition;
    private Vector3 launchPosition;    // 발사 시점의 플레이어 위치
    private bool isReturning = false;
    private bool isTrackingMode = false;  // 플레이어 추적 모드
    private HashSet<int> outwardHitEnemies = new HashSet<int>();  // 나갈 때 맞은 적들
    private HashSet<int> returnHitEnemies = new HashSet<int>();   // 돌아올 때 맞은 적들
    private float returnTimer = 0f;
    private Rigidbody2D rb;
    private float returnForce = 15f;  // 기본 귀환 힘 증가
    private float returnAcceleration = 5f;  // 귀환 가속도 증가율 상향
    private float minReturnSpeed = 5f;  // 최소 귀환 속도
    private string poolTag = "HatProjectile";

    private float maxLifetime = 4f;  // 최대 생존 시간
    private float lifetime = 0f;     // 현재 생존 시간
    private bool wasSuccessfullyCaught = false;  // 성공적으로 회수되었는지 여부
    private bool isPlayerInRange = false;  // 플레이어가 회수 범위 안에 있는지 여부

    public void Initialize(Hat weapon, Vector3 target)
    {
        hatWeapon = weapon;
        targetPosition = target;
        launchPosition = weapon.transform.position;  // 발사 시점의 플레이어 위치 저장
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        
        // Rigidbody2D 설정
        rb.gravityScale = 0f;
        rb.linearDamping = 0.2f;  // 감속 효과 감소
        rb.angularDamping = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void SetTrackingMode(bool tracking)
    {
        isTrackingMode = tracking;
    }

    public void OnObjectSpawn()
    {
        isReturning = false;
        returnTimer = 0f;
        lifetime = 0f;
        wasSuccessfullyCaught = false;
        isPlayerInRange = false;  // 플레이어 범위 체크 초기화
        outwardHitEnemies.Clear();
        returnHitEnemies.Clear();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void Update()
    {
        // 생존 시간 체크
        lifetime += Time.deltaTime;
        if (lifetime >= maxLifetime)
        {
            ReturnToPool(false);  // 시간 초과로 인한 강제 회수
            return;
        }

        // 모자 회전
        transform.Rotate(Vector3.forward * hatWeapon.rotationSpeed * Time.deltaTime);

        if (!isReturning)
        {
            // 목표 지점으로 이동
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            if (distanceToTarget > 0.1f)
            {
                Vector3 direction = (targetPosition - transform.position).normalized;
                rb.linearVelocity = direction * hatWeapon.throwSpeed;

                // 목표 지점까지의 거리가 너무 멀어지면 강제 귀환
                float distanceFromStart = Vector3.Distance(transform.position, launchPosition);
                if (distanceFromStart > hatWeapon.throwRange * 1.5f)
                {
                    StartReturn();
                }
            }
            else
            {
                StartReturn();
            }
        }
        else
        {
            returnTimer += Time.deltaTime;
            
            if (isTrackingMode)
            {
                // 추적 모드일 때는 플레이어를 따라감
                ApplyReturnForce();
            }
            else
            {
                // 고정 위치 모드일 때는 현재 방향으로 계속 이동
                if (rb.linearVelocity.magnitude < minReturnSpeed)
                {
                    rb.linearVelocity = rb.linearVelocity.normalized * minReturnSpeed;
                }
            }

            // 플레이어와 충돌했을 때만 회수
            if (isPlayerInRange)
            {
                wasSuccessfullyCaught = true;
                ReturnToPool(true);
            }
        }
    }

    void ApplyReturnForce()
    {
        // 플레이어 방향으로의 힘 계산 (추적 모드에서만 사용)
        Vector2 directionToPlayer = (hatWeapon.transform.position - transform.position).normalized;
        
        // 시간에 따라 증가하는 힘
        float currentForce = returnForce * (1f + returnTimer * returnAcceleration);
        
        // 힘 적용
        rb.AddForce(directionToPlayer * currentForce, ForceMode2D.Force);

        // 최대 속도 제한 (시간에 따라 증가)
        float maxSpeed = hatWeapon.returnSpeed * (2f + returnTimer);
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
        
        // 최소 속도 보장
        if (rb.linearVelocity.magnitude < minReturnSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * minReturnSpeed;
        }
    }

    void StartReturn()
    {
        if (!isReturning)
        {
            isReturning = true;
            returnTimer = 0f;
            
            if (isTrackingMode)
            {
                // 추적 모드: 플레이어 방향으로 초기 속도 설정
                Vector2 directionToPlayer = (hatWeapon.transform.position - transform.position).normalized;
                rb.linearVelocity = directionToPlayer * minReturnSpeed;
            }
            else
            {
                // 고정 위치 모드: 발사 위치 방향으로 초기 속도 설정
                Vector2 directionToLaunch = (launchPosition - transform.position).normalized;
                rb.linearVelocity = directionToLaunch * minReturnSpeed;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 적 충돌 체크
        if (((1 << other.gameObject.layer) & hatWeapon.enemyLayer) != 0)
        {
            int enemyId = other.gameObject.GetInstanceID();
            Enemy enemy = other.GetComponent<Enemy>();
            
            if (enemy != null)
            {
                if (!isReturning && !outwardHitEnemies.Contains(enemyId))
                {
                    outwardHitEnemies.Add(enemyId);
                    enemy.TakeDamage(hatWeapon.damage);
                }
                else if (isReturning && !returnHitEnemies.Contains(enemyId))
                {
                    returnHitEnemies.Add(enemyId);
                    enemy.TakeDamage(hatWeapon.damage);
                }
            }
        }
        // 플레이어 충돌 체크
        else if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // 플레이어가 범위를 벗어났을 때
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void ReturnToPool(bool wasSuccessful)
    {
        if (wasSuccessful && hatWeapon != null)
        {
            // 성공적으로 회수된 경우에만 쿨다운 감소
            hatWeapon.ReduceAttackCooldown();
        }

        if (hatWeapon != null)
        {
            hatWeapon.OnHatDestroyed();
        }
        ObjectPool.Instance.ReturnToPool(poolTag, gameObject);
    }
} 
using UnityEngine;

public class PipeSmokeEffect : Effect
{
    [Header("담배 연기 특수 속성")]
    public float expandSpeed = 2f; // 연기가 퍼지는 속도
    public float maxRadius = 3f; // 최대 반경
    public float damageInterval = 0.5f; // 데미지를 주는 간격
    private float currentDamageTime = 0f;
    private float currentRadius = 0f;
    private bool isExpanded = false;

    public override void Initialize(float damage, float duration, float radius)
    {
        base.Initialize(damage, duration, radius);
        currentRadius = radius;
        currentDamageTime = 0f;
        isExpanded = false;
        
        // 콜라이더 크기 초기화
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            collider.radius = currentRadius;
        }
    }

    protected override void Update()
    {
        if (!isActive) return;

        // 연기가 점점 퍼져나가는 효과 (최대 반경에 도달할 때까지)
        if (!isExpanded)
        {
            currentRadius = Mathf.Min(currentRadius + expandSpeed * Time.deltaTime, maxRadius);
            
            // 최대 반경에 도달했는지 확인
            if (currentRadius >= maxRadius)
            {
                isExpanded = true;
            }
            
            // 콜라이더 크기 업데이트
            CircleCollider2D collider = GetComponent<CircleCollider2D>();
            if (collider != null)
            {
                collider.radius = currentRadius;
            }
        }

        // 일정 간격으로 데미지 적용
        currentDamageTime += Time.deltaTime;
        if (currentDamageTime >= damageInterval)
        {
            currentDamageTime = 0f;
            ApplyDamageToEnemiesInRange();
        }
    }

    private void ApplyDamageToEnemiesInRange()
    {
        // 범위 내의 모든 적 찾기
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, currentRadius, targetLayer);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    //enemy.TakeDamage(damage);
                    SpawnHitEffect();
                }
            }
        }
    }

    protected override void Deactivate()
    {
        // 연기가 사라질 때 효과
        isActive = false;
        gameObject.SetActive(false);
    }
} 
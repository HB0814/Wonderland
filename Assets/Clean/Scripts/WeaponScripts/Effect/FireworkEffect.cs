using UnityEngine;

public class FireworkEffect : Effect
{
    [Header("축하 폭죽 특수 속성")]
    public float damageInterval = 1f; // 데미지를 주는 간격
    public float effectScale = 1f; // 이펙트 크기
    private float currentDamageTime = 0f;
    private float currentDuration = 0f;

    public override void Initialize(float damage, float duration, float radius)
    {
        base.Initialize(damage, duration, radius);
        currentDamageTime = 0f;
        currentDuration = 0f;
        
        // 이펙트 크기 설정
        transform.localScale = Vector3.one * effectScale;
        
        // 콜라이더 크기 설정
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            collider.radius = radius;
        }
    }

    protected override void Update()
    {
        if (!isActive) return;

        // 지속 시간 체크
        currentDuration += Time.deltaTime;
        if (currentDuration >= duration)
        {
            Deactivate();
            return;
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
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);
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
        // 장판이 사라질 때 효과
        isActive = false;
        gameObject.SetActive(false);
    }
} 
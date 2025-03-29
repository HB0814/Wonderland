using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("적 기본 속성")]
    public float maxHealth = 100f;
    public float moveSpeed = 2f;
    public float knockbackResistance = 0.5f;  // 넉백 저항력 (0: 저항 없음, 1: 완전 저항)

    [Header("드롭 아이템")]
    public GameObject[] dropItems;  // 드롭할 아이템 프리팹
    public float dropChance = 0.3f; // 아이템 드롭 확률

    [Header("경험치 설정")]
    [SerializeField] private GameObject expGemPrefab;    // 경험치 보석 프리팹

    private float currentHealth;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isKnockedBack = false;
    private float knockbackRecoveryTimer = 0f;

    // 슬로우 효과 관련 변수
    private float originalMoveSpeed;    // 원래 이동 속도
    private float slowEffectTimer = 0f; // 슬로우 효과 지속 시간
    private bool isSlowed = false;      // 슬로우 상태 여부
    private float currentSlowAmount = 0f; // 현재 적용된 슬로우 강도

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        originalMoveSpeed = moveSpeed;  // 원래 이동 속도 저장

        // Rigidbody2D 설정
        if (rb != null)
        {
            rb.gravityScale = 0;  // 2D 탑다운 게임을 위한 설정
            rb.linearDamping = 5f;         // 마찰력 설정
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;  // 회전 방지
        }
    }

    void Update()
    {
        // 슬로우 효과 업데이트
        if (isSlowed)
        {
            slowEffectTimer -= Time.deltaTime;
            if (slowEffectTimer <= 0)
            {
                RemoveSlowEffect();
            }
        }

        // 넉백 상태에서 회복
        if (isKnockedBack)
        {
            knockbackRecoveryTimer += Time.deltaTime;
            if (knockbackRecoveryTimer >= 0.2f)  // 0.2초 후 회복
            {
                isKnockedBack = false;
                knockbackRecoveryTimer = 0f;
            }
        }
        else
        {
            // 기본 이동 로직
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        // 플레이어를 찾아 이동
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && !isKnockedBack)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy took damage: " + damage);
        Debug.Log("Current health: " + currentHealth);
        
        // 피격 효과
        StartCoroutine(HitEffect());

        // 체력이 0 이하면 사망
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ApplyKnockback(Vector2 knockbackDirection, float knockbackForce)
    {
        if (rb != null)
        {
            // 넉백 저항력 적용
            float actualKnockback = knockbackForce * (1 - knockbackResistance);
            rb.AddForce(knockbackDirection * actualKnockback, ForceMode2D.Impulse);
            isKnockedBack = true;
            knockbackRecoveryTimer = 0f;
        }
    }

    System.Collections.IEnumerator HitEffect()
    {
        // 피격 시 빨간색으로 변경
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);  // 0.3초 동안 지속
        spriteRenderer.color = originalColor;
    }

    private void Die()
    {
        // 경험치 보석 생성
        if (expGemPrefab != null)
        {
            Instantiate(expGemPrefab, transform.position, Quaternion.identity);
        }

        // 아이템 드롭
        if (dropItems != null && dropItems.Length > 0 && Random.value <= dropChance)
        {
            int randomIndex = Random.Range(0, dropItems.Length);
            Instantiate(dropItems[randomIndex], transform.position, Quaternion.identity);
        }

        // 파티클 효과 재생 (있다면)
        ParticleSystem deathEffect = GetComponent<ParticleSystem>();
        if (deathEffect != null)
        {
            deathEffect.Play();
        }

        // 적 오브젝트 제거
        Destroy(gameObject);
    }

    // 디버그용 기즈모
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }

    /// <summary>
    /// 슬로우 효과 적용
    /// </summary>
    /// <param name="slowAmount">슬로우 강도 (0.5 = 50% 감소)</param>
    /// <param name="duration">지속 시간 (초)</param>
    public void ApplySlow(float slowAmount, float duration)
    {
        // 더 강한 슬로우 효과가 들어오면 갱신
        if (!isSlowed || slowAmount > currentSlowAmount)
        {
            Debug.Log("ApplySlow: " + slowAmount + " for " + duration + " seconds");
            isSlowed = true;
            currentSlowAmount = slowAmount;
            slowEffectTimer = duration;
            
            // 이동 속도 감소 적용
            moveSpeed = originalMoveSpeed * (1f - slowAmount);
        }
        else
        {
            // 기존 슬로우 효과의 지속 시간만 갱신
            slowEffectTimer = Mathf.Max(slowEffectTimer, duration);
        }
    }

    /// <summary>
    /// 슬로우 효과 제거
    /// </summary>
    private void RemoveSlowEffect()
    {
        isSlowed = false;
        currentSlowAmount = 0f;
        moveSpeed = originalMoveSpeed;  // 원래 이동 속도로 복구
    }
} 
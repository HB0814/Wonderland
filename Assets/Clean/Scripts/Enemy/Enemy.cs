using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("기본 속성")]
    public float maxHealth = 100f;
    public float moveSpeed = 2f;
    public float attackDamage = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    public float knockbackResistance = 0.5f;

    [Header("드롭 아이템")]
    protected GameObject[] dropItems;
    protected float dropChance = 0.3f;
    protected GameObject expGemPrefab;

    public float currentHealth;
    public float nextAttackTime;
    public bool isKnockedBack;
    public float knockbackRecoveryTimer;
    public bool isSlowed;
    public float slowEffectTimer;
    public float currentSlowAmount;
    public float originalMoveSpeed;

    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    protected Color originalColor;
    protected GameObject player;
    protected Animator animator;

    // 스프라이트 업데이트 관련 변수
    protected float lastUpdateTime = 0;
    protected bool isVisible = true;
    protected float marginArea = 0.1f;

    public virtual void Initialize()
    {
        currentHealth = maxHealth;
        originalMoveSpeed = moveSpeed;
        nextAttackTime = 0f;
        isKnockedBack = false;
        knockbackRecoveryTimer = 0f;
        isSlowed = false;
        slowEffectTimer = 0f;
        currentSlowAmount = 0f;

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    protected virtual void Start()
    {
        Initialize();
    }

    protected virtual void Update()
    {
        if (isSlowed)
        {
            slowEffectTimer -= Time.deltaTime;
            if (slowEffectTimer <= 0)
            {
                RemoveSlowEffect();
            }
        }

        if (isKnockedBack)
        {
            knockbackRecoveryTimer += Time.deltaTime;
            if (knockbackRecoveryTimer >= 0.2f)
            {
                isKnockedBack = false;
                knockbackRecoveryTimer = 0f;
            }
        }
        else
        {
            MoveTowardsPlayer();
        }

        // 스프라이트 레이어 업데이트
        if (Time.time - lastUpdateTime >= 0.1f)
        {
            lastUpdateTime = Time.time;
            UpdateSpriteLayer();
            UpdateSpriteFlip();
        }
    }

    protected virtual void UpdateSpriteLayer()
    {
        if (spriteRenderer != null)
        {
            // Y 좌표가 낮을수록(화면 아래) 더 앞에 표시
            spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
        }
    }

    protected virtual void UpdateSpriteFlip()
    {
        if (spriteRenderer != null && player != null)
        {
            bool shouldFlip = player.transform.position.x < transform.position.x;
            if (shouldFlip != spriteRenderer.flipX)
            {
                spriteRenderer.flipX = shouldFlip;
            }
        }
    }

    protected virtual void MoveTowardsPlayer()
    {
        if (player != null && !isKnockedBack)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
    }

    public virtual void TakeDamage(float damage)
    {
        float actualDamage = Mathf.Max(1, damage);
        currentHealth -= actualDamage;

        Debug.Log("Enemy TakeDamage: " + actualDamage);
        
        StartCoroutine(HitEffect());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void ApplyKnockback(Vector2 direction, float force)
    {
        if (rb != null)
        {
            float actualForce = force * (1 - knockbackResistance);
            rb.AddForce(direction * actualForce, ForceMode2D.Impulse);
            isKnockedBack = true;
            knockbackRecoveryTimer = 0f;
        }
    }

    public virtual void ApplySlow(float slowAmount, float duration)
    {
        if (!isSlowed || slowAmount > currentSlowAmount)
        {
            isSlowed = true;
            currentSlowAmount = slowAmount;
            slowEffectTimer = duration;
            moveSpeed = originalMoveSpeed * (1f - slowAmount);
        }
        else
        {
            slowEffectTimer = Mathf.Max(slowEffectTimer, duration);
        }
    }

    protected virtual void RemoveSlowEffect()
    {
        isSlowed = false;
        currentSlowAmount = 0f;
        moveSpeed = originalMoveSpeed;
    }

    protected virtual System.Collections.IEnumerator HitEffect()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
        }
    }

    protected virtual void Die()
    {
        if (expGemPrefab != null)
        {
            Instantiate(expGemPrefab, transform.position, Quaternion.identity);
        }

        if (dropItems != null && dropItems.Length > 0 && Random.value <= dropChance)
        {
            int randomIndex = Random.Range(0, dropItems.Length);
            Instantiate(dropItems[randomIndex], transform.position, Quaternion.identity);
        }

        ParticleSystem deathEffect = GetComponent<ParticleSystem>();
        if (deathEffect != null)
        {
            deathEffect.Play();
        }

        Destroy(gameObject);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    protected virtual void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // 기본 공격 패턴
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Player player = hitCollider.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(attackDamage);
                }
            }
        }
    }
} 
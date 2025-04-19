using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [Header("플레이어 속성")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 50f;  // 가속도
    [SerializeField] private float deceleration = 30f;  // 감속도

    [Header("경험치 시스템")]
    [SerializeField] private float currentExp = 0f;
    [SerializeField] private float maxExp = 100f;
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private float insanityExp = 0.01f;
    // 경험치 변경 이벤트
    public System.Action<float, float> onExpChanged;  // (현재 경험치, 최대 경험치)
    public System.Action<int> onLevelChanged;         // (현재 레벨)

    [Header("체력 시스템")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    // 체력 변경 이벤트
    public System.Action<float, float> onHealthChanged;  // (현재 체력, 최대 체력)

    [Header("이동 설정")]
    [SerializeField] private float baseSpeed = 5f;
    private float currentSpeedBonus = 0f;
    private float damageMultiplier = 1f;
    
    private InsanitySystem insanitySystem;
    // 광기 변경 이벤트
    public System.Action<float> onInsanityChanged;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 currentVelocity;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // 프로퍼티
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public float CurrentExp => currentExp;
    public float MaxExp => maxExp;
    public int Level => currentLevel;

    private void Awake()
    {
        InitializeComponents();
        SetupRigidbody();
        InitializeStats();
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        insanitySystem = GetComponent<InsanitySystem>();
        if (insanitySystem == null)
        {
            Debug.LogError($"[{nameof(Player)}] InsanitySystem component not found!");
        }
    }

    private void SetupRigidbody()
    {
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.linearDamping = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void InitializeStats()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth, maxHealth);
        onLevelChanged?.Invoke(currentLevel);
        onExpChanged?.Invoke(currentExp, maxExp);
        onInsanityChanged?.Invoke(insanitySystem?.GetInsanity() ?? 0f);
    }

    private void Update()
    {
        HandleInput();
        UpdateAnimation();
        WeaponAdd();
    }
    private void WeaponAdd()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // 무기 인덱스 0은 트럼프 카드 무기로 가정
            WeaponManager.Instance.CreateWeapon(0);
            Debug.Log("Trump Card Weapon Added");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            // 무기 인덱스 0은 트럼프 카드 무기로 가정
            WeaponManager.Instance.CreateWeapon(1);
            Debug.Log("Book Throw Weapon Added");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            // 무기 인덱스 0은 트럼프 카드 무기로 가정
            WeaponManager.Instance.CreateWeapon(2);
            Debug.Log("Hat Boomerang Weapon Added");
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            // 무기 인덱스 0은 트럼프 카드 무기로 가정
            WeaponManager.Instance.CreateWeapon(3);
            Debug.Log("Roll Apple Weapon Added");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            // 무기 인덱스 0은 트럼프 카드 무기로 가정
            WeaponManager.Instance.CreateWeapon(4);
            Debug.Log("Tea Splash Weapon Added");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            // 무기 인덱스 0은 트럼프 카드 무기로 가정
            WeaponManager.Instance.CreateWeapon(5);
            Debug.Log("Pipe Weapon Added");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            // 무기 인덱스 0은 트럼프 카드 무기로 가정
            WeaponManager.Instance.CreateWeapon(6);
            Debug.Log("Firework Weapon Added");
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            // 무기 인덱스 0은 트럼프 카드 무기로 가정
            WeaponManager.Instance.CreateWeapon(7);
            Debug.Log("Vorpal Sword Weapon Added");
        }
        if (Input.GetKeyDown(KeyCode.Period))
        {
            // 무기 인덱스 0은 트럼프 카드 무기로 가정
            WeaponManager.Instance.CreateWeapon(8);
            Debug.Log("Pocket Watch Weapon Added");
        }
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            // 무기 인덱스 0은 트럼프 카드 무기로 가정
            WeaponManager.Instance.CreateWeapon(9);
            Debug.Log("Cheshire Cat Weapon Added");
        }
    }

    private void HandleInput()
    {
        // 입력 감지 (화살표 키 또는 WASD)
        moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        // 75 이상일 때 컨트롤 반전
        if (insanitySystem?.AreControlsInverted() ?? false)
        {
            moveInput = -moveInput;
        }
    }

    private void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", moveInput.magnitude);
        }

        if (moveInput.x != 0 && spriteRenderer != null)
        {
            spriteRenderer.flipX = moveInput.x < 0;
        }
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        // 물리 기반 이동 처리
        Vector2 targetVelocity = moveInput * moveSpeed;
        
        if (moveInput != Vector2.zero)
        {
            // 이동 중일 때 가속
            currentVelocity = Vector2.MoveTowards(
                currentVelocity,
                targetVelocity,
                acceleration * Time.fixedDeltaTime
            );
        }
        else
        {
            // 정지 시 감속
            currentVelocity = Vector2.MoveTowards(
                currentVelocity,
                Vector2.zero,
                deceleration * Time.fixedDeltaTime
            );
        }

        rb.linearVelocity = currentVelocity;
    }
    public void AddExperience(float amount)
    {
        currentExp += amount;
        onExpChanged?.Invoke(currentExp, maxExp);
        
        if (currentExp >= maxExp)
        {
            LevelUp();
        }

        // 경험치 획득 시 광기 2 증가
        if (insanitySystem != null)
        {
            insanitySystem.AddInsanity(insanityExp);
            onInsanityChanged?.Invoke(insanitySystem.GetInsanity());
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        currentExp = 0f;
        maxExp *= 1.2f;
        onLevelChanged?.Invoke(currentLevel);
        onExpChanged?.Invoke(currentExp, maxExp);
        UpgradeManager.Instance.ShowUpgradeOptions();
    }

    public void TakeDamage(float damage)
    {
        if (insanitySystem == null) return;

        // 광기 수준에 따른 데미지 증가 적용
        float finalDamage = damage * insanitySystem.GetDamageMultiplier();
        currentHealth = Mathf.Max(0, currentHealth - finalDamage);
        onHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        Debug.Log($"[{nameof(Player)}] Player died!");
        // TODO: 사망 처리 로직 추가
    }

    public void AddSpeedBonus(float bonus)
    {
        currentSpeedBonus += bonus;
    }

    public void SetDamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
    }

    public void OnSpecialWeaponPickup()
    {
        if (insanitySystem == null) return;

        insanitySystem.AddInsanity(25f);     // 광기 25 증가
        insanitySystem.IncreaseMinInsanity(); // 최소 광기 25 증가
        onInsanityChanged?.Invoke(insanitySystem.GetInsanity());
    }

    public void OnSpecialMonsterHit()
    {
        if (insanitySystem == null) return;

        insanitySystem.AddInsanity(3f);
        onInsanityChanged?.Invoke(insanitySystem.GetInsanity());
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
} 
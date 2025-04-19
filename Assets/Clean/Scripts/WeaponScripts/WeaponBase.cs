using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("기본 속성")]
    protected string weaponName;       // 무기 이름
    protected string description;      // 설명

    protected Sprite weaponIcon;       // 무기 아이콘

    protected int maxLevel = 5;        // 최대 레벨
    protected int currentLevel = 1;    // 현재 레벨
    protected int count = 2;           // 발사체 개수

    [Header("공격 속성")]
    [SerializeField]protected float attackCooldown; // 공격 쿨다운
    protected float damage;      // 데미지
    protected float detectionRange; // 탐지 범위
    protected float lifeTime;     // 수명
    protected float size;         // 크기
    protected float speed;        // 투사체 속도

    [Header("부가 효과")]
    protected float knockbackForce;    // 넉백 강도
    protected float slowForce;         // 슬로우 강도
    protected float slowDuration;      // 슬로우 지속시간

    [SerializeField]protected float nextAttackTime;
    protected GameObject player;
    protected Animator animator;
    [SerializeField]protected GameObject projectilePrefab; // 투사체 프리팹

    public WeaponType WeaponType { get; protected set; }
    public WeaponData weaponData;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        nextAttackTime = 0f;
                
        if (weaponData != null)
        {
            UpdateStats();
        }
    }
    public virtual void LevelUpLogic()
    {
        if (weaponData != null && currentLevel < maxLevel)
        {
            currentLevel++;
            UpdateStats();
            Debug.Log($"{weaponName} upgraded to level {currentLevel}");
        }
        else
        {
            Debug.LogWarning($"Cannot upgrade {weaponName}. Current level: {currentLevel}, Max level: {maxLevel}");
        }
    }

    protected virtual void UpdateStats()
    {
        if (weaponData != null)
        {
            var stats = weaponData.levelStats;
            int index = currentLevel - 1;

            count = stats.count[index];
            weaponData.currentLevel = currentLevel;

            attackCooldown = stats.attackCooldown[index];
            damage = stats.damage[index];
            detectionRange = stats.detectionRange[index];
            lifeTime = stats.lifeTime[index];
            size = stats.size[index];
            speed = stats.speed[index];

            knockbackForce = stats.knockbackForce[index];
            slowForce = stats.slowForce[index];
            slowDuration = stats.slowDuration[index];

            Debug.Log($"Updated {weaponName} stats - Level: {currentLevel}, Damage: {damage}");
        }
    }
    protected virtual void Update()
    {
        nextAttackTime += Time.deltaTime;

        if (attackCooldown <= nextAttackTime)
        {
            Attack();
            nextAttackTime = 0f;
        }
    }

    protected abstract void Attack();

    public virtual void Initialize()
    {
        currentLevel = 1;
        nextAttackTime = 0f;
    }

    protected void FireProjectile(Vector2 direction)
    {
        GameObject projectileObj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        
        if (projectile != null)
        {
            projectile.BaseInitialize(damage, size, lifeTime, speed);
            projectile.DebuffInitialize(knockbackForce, slowForce, slowDuration);
            projectile.SetDirection(direction);
        }

        Rigidbody2D rb = projectileObj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
    }
} 
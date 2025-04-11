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
        if (weaponData != null && currentLevel < weaponData.UpgradeDetails.Length)
        {
            currentLevel++;
            UpdateStats();
        }
    }

    protected virtual void UpdateStats()
    {
        if (weaponData != null)
        {
            int index = currentLevel - 1;
            var stats = weaponData.levelStats;

            if (index < 0 || index >= stats.count.Length)
            {
                Debug.LogWarning($"Invalid level index: {index}. Current level: {currentLevel}");
                return;
            }

            count = stats.count[index];
            weaponData.currentLevel=currentLevel;

            attackCooldown = stats.attackCooldown[index];
            damage = stats.damage[index];
            detectionRange = stats.detectionRange[index];
            lifeTime = stats.lifeTime[index];
            size = stats.size[index];
            speed = stats.speed[index];

            knockbackForce = stats.knockbackForce[index];
            slowForce = stats.slowForce[index];
            slowDuration = stats.slowDuration[index];
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
} 
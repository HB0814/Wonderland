using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("기본 속성")]
    public string weaponName;
    public string description;
    public Sprite weaponIcon;
    public int maxLevel = 5;
    public int currentLevel = 1;

    [Header("공격 속성")]
    public float baseDamage = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;

    [SerializeField]protected float nextAttackTime;
    protected GameObject player;
    protected Animator animator;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        nextAttackTime = 0f;
    }

    protected virtual void Update()
    {
        nextAttackTime += Time.deltaTime;

        if (attackCooldown <= nextAttackTime)
        {
            Attack();
        }
    }

    protected abstract void Attack();

    public virtual void LevelUp()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
        }
    }

    public virtual void Initialize()
    {
        currentLevel = 1;
        nextAttackTime = 0f;
    }
} 
using UnityEngine;

public abstract class NormalEnemyBase : Enemy
{
    TakeDamage takeDamage;

    [Header("기본 속성")]
    public float maxHp; //최대 체력
    public float hp; //현재 체력
    public float addHp; //증가 체력
    public float defense; //방어력
    public float speed; //이동 속도
    public float knockbackDefense; //밀려나가는 저항력

    protected override void Start()
    {
        base.Start();
        takeDamage = GetComponent<TakeDamage>();
        
        // 초기 속성 설정
        maxHealth = maxHp;
        moveSpeed = speed;
        knockbackResistance = knockbackDefense;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void MoveTowardsPlayer()
    {
        if (player != null && !isKnockedBack)
        {
            // 항상 플레이어를 향해 이동
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
    }
} 
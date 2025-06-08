using UnityEngine;

public class TrumpCardWeapon : WeaponBase
{
    [Header("트럼프 카드 특수 속성")]
    public string cardPoolTag = "TrumpCard"; // ObjectPool에서 사용할 태그
    public float angleStep = 15f; // 카드 간의 각도 차이
    
    private void Awake()
    {
        weaponData = WeaponDataManager.Instance.GetWeaponData(WeaponType.Card);
        WeaponType = WeaponType.Card;
    }
    private void Start()
    {
        base.Start();
    }

    private void LevelUpLogic()
    {
        base.LevelUpLogic();
    }

    private void UpdateStats()
    {
        base.UpdateStats();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            LevelUpLogic();
        }
        base.Update();
    }
    protected override void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // 첫 번째 카드의 랜덤 방향 계산
        float baseAngle = Random.Range(0f, 360f);
        Vector2 baseDirection = Quaternion.Euler(0, 0, baseAngle) * Vector2.right;

        // 카드 발사
        for (int i = 0; i < count; i++)
        {
            // 첫 번째 카드는 baseAngle, 나머지는 angleStep만큼씩 회전
            float currentAngle = baseAngle + (i * angleStep);
            Vector2 direction = Quaternion.Euler(0, 0, currentAngle) * Vector2.right;
            
            // 카드 생성 및 발사
            GameObject card = WeaponManager.Instance.SpawnProjectile(cardPoolTag, transform.position, Quaternion.Euler(0, 0, currentAngle+90));
            if (card != null)
            {
                Rigidbody2D rb = card.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = direction * speed;
                }

                Projectile trumpCard=card.GetComponent<Projectile>();
                if(trumpCard!=null)
                {
                    trumpCard.BaseInitialize(damage, size, lifeTime, speed);
                    trumpCard.DebuffInitialize(knockbackForce, slowForce, slowDuration);
                }
            }
        }
        nextAttackTime = 0f;
        SoundManager.Instance?.PlayWeaponSound(weaponData.weaponType);
    }
} 
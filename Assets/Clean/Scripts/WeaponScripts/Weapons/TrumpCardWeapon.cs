using UnityEngine;

public class TrumpCardWeapon : WeaponBase
{
    [Header("트럼프 카드 특수 속성")]
    public string cardPoolTag = "TrumpCard"; // ObjectPool에서 사용할 태그
    public int cardCount = 2;
    public float cardSpeed = 10f;
    public float angleStep = 15f; // 카드 간의 각도 차이
    public float cardLifetime = 2f;
    public TrumpCardStats weaponStats;

    private void Start()
    {
        UpdateStats();
    }
    public void LevelUp()
    {
        if (weaponStats != null && currentLevel < weaponStats.levelStats.Length)
        {
            currentLevel++;
            UpdateStats();
        }
    }
    private void UpdateStats()
    {
        if (weaponStats != null && currentLevel <= weaponStats.levelStats.Length)
        {
            TrumpCardStats.LevelStats stats = weaponStats.levelStats[currentLevel - 1];
            currentLevel = stats.currentLevel;
            cardCount = stats.cardCount;
            cardSpeed = stats.cardSpeed;
            baseDamage = stats.damage;
            attackCooldown = stats.attackCooldown;
            cardLifetime = stats.cardLifetime;

        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            LevelUp();
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
        for (int i = 0; i < cardCount; i++)
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
                    rb.linearVelocity = direction * cardSpeed;
                }

                // 카드에 데미지 설정
                CardProjectile cardProjectile = card.GetComponent<CardProjectile>();
                if (cardProjectile != null)
                {
                    cardProjectile.damage = baseDamage;
                }

                // 카드 수명 설정
                if (cardProjectile != null)
                {
                    cardProjectile.cardLifetime = cardLifetime;
                }
            }
        }
        nextAttackTime = 0f;
    }
} 
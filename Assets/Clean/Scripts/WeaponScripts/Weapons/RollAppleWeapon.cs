using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class RollAppleWeapon : WeaponBase
{
    [Header("사과 굴리기 무기 특수 속성")]
    public string applePoolTag = "AppleProjectile";
    public float knockbackForce = 5f;
    public float attackRangeX = 5f;
    public float attackRangeY = 5f;
    Player player;
    public RollAppleStats weaponStats;

    private void Start()
    {
        player = transform.parent.gameObject.GetComponent<Player>();
        
        if (weaponStats != null && weaponStats.levelStats.Length > 0)
        {
            UpdateStats();
        }
    }

    public override void LevelUp()
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
            RollAppleStats.LevelStats stats = weaponStats.levelStats[currentLevel - 1];
            currentLevel = stats.currentLevel;
            baseDamage = stats.damage;
            attackCooldown = stats.attackCooldown;
            knockbackForce = stats.knockbackForce;
            attackRangeX = stats.attackRangeX;
            attackRangeY = stats.attackRangeY;
        }
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
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

        SpawnApple();
    }

    private void SpawnApple()
    {
        nextAttackTime=0f;
        // 랜덤 시작 위치 설정
        Vector3 spawnPosition = GetRandomSpawnPosition();
        
        // 랜덤 회전 설정
        float randomRotation = Random.Range(0f, 360f);
        Quaternion spawnRotation = Quaternion.Euler(0, 0, randomRotation);
        
        // 사과 생성
        GameObject apple = WeaponManager.Instance.SpawnProjectile(applePoolTag, spawnPosition, spawnRotation);
        if (apple != null)
        {
            RollAppleProjectile projectile = apple.GetComponent<RollAppleProjectile>();
            if (projectile != null)
            {
                // 공격 방향 설정
                Vector3 targetPos = GetRandomTargetPosition();
                projectile.SetDirection(targetPos);
                
                // 무기 속성 설정
                projectile.SetWeaponProperties(baseDamage, knockbackForce);
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPosition = Vector3.zero;
        float min = -0.1f;
        float max = 1.0f;
        float zPos = 10;

        int flag = Random.Range(0, 4);

        switch (flag)
        {
            case 0: // 오른쪽에서
                randomPosition = new Vector3(max, Random.Range(min, max), zPos);
                break;
            case 1: // 왼쪽에서
                randomPosition = new Vector3(min, Random.Range(min, max), zPos);
                break;
            case 2: // 위에서
                randomPosition = new Vector3(Random.Range(min, max), max, zPos);
                break;
            case 3: // 아래에서
                randomPosition = new Vector3(Random.Range(min, max), min, zPos);
                break;
        }

        return Camera.main.ViewportToWorldPoint(randomPosition);
    }

    private Vector3 GetRandomTargetPosition()
    {
        float ranX = math.floor(Random.Range(player.transform.position.x - attackRangeX,
            player.transform.position.x + attackRangeX) * 10) * 0.1f;
        float ranY = math.floor(Random.Range(player.transform.position.y - attackRangeY,
            player.transform.position.y + attackRangeY) * 10) * 0.1f;

        return new Vector3(ranX, ranY, 0);
    }
} 
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class RollAppleWeapon : WeaponBase
{
    [Header("사과 굴리기 무기 특수 속성")]
    public string applePoolTag = "AppleProjectile";
    public float knockbackForce = 5f;
    public float attackRangeX = 2f;
    public float attackRangeY = 2f;
    private Player player;

    private void Awake()
    {
        weaponData = WeaponDataManager.Instance.GetWeaponData(WeaponType.Apple);
        WeaponType = WeaponType.Apple;
    }
    private void Start()
    {
        base.Start();
        player=GetComponentInParent<Player>();
    }

    private void LevelUpLogic()
    {
        base.LevelUpLogic();
    }

    private void UpdateStats()
    {
        base.UpdateStats();
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
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

        SpawnApple();
    }

    private void SpawnApple()
    {
        // 랜덤 시작 위치 설정
        Vector3 spawnPosition = GetRandomSpawnPosition();
        
        // 랜덤 회전 설정
        float randomRotation = Random.Range(0f, 360f);
        Quaternion spawnRotation = Quaternion.Euler(0, 0, randomRotation);
        
        // 사과 생성
        GameObject apple = WeaponManager.Instance.SpawnProjectile(applePoolTag, spawnPosition, spawnRotation);
        if (apple != null)
        {
            Projectile projectile = apple.GetComponent<Projectile>();
            if (projectile != null)
            {
                // 공격 방향 설정
                Vector3 targetPos = GetRandomTargetPosition(spawnPosition);
                projectile.SetDirection(targetPos);
                
                // 무기 속성 설정
                projectile.BaseInitialize(damage, size, lifeTime, speed);
                projectile.DebuffInitialize(knockbackForce, slowForce, slowDuration);
            }
            nextAttackTime = 0f;
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

    private Vector3 GetRandomTargetPosition(Vector3 spawnPosition)
    {
        // 플레이어의 위치 기준으로 랜덤 위치 설정
        float randomX = Random.Range(player.transform.position.x - attackRangeX,
            player.transform.position.x + attackRangeX);
        float randomY = Random.Range(player.transform.position.y - attackRangeY,
            player.transform.position.y + attackRangeY);

        // 생성 위치에서 플레이어로의 방향 벡터 계산
        Vector3 direction = new Vector3(randomX, randomY, 0) - spawnPosition;
        
        // 정규화된 방향 벡터 반환
        return direction.normalized;
    }
} 
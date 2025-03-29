using UnityEngine;

public class Book : MonoBehaviour
{
    [Header("책 무기 속성")]
    public float attackSpeed = 1f;
    public float attackRange = 8f;
    public LayerMask enemyLayer;
    public GameObject projectilePrefab;
    public int penetration = 0;  // 관통 수치
    public float damage = 20f;   // 데미지

    private float nextAttackTime;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = transform.parent;
        nextAttackTime = 0f;
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            FindAndAttackNearestEnemy();
            nextAttackTime = Time.time + 1f / attackSpeed;
        }
    }

    void FindAndAttackNearestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(playerTransform.position, attackRange, enemyLayer);
        
        Transform nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider2D enemy in enemies)
        {
            float distance = Vector2.Distance(playerTransform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        if (nearestEnemy != null)
        {
            ShootProjectile(nearestEnemy);
        }
    }

    void ShootProjectile(Transform target)
    {
        // 발사체 생성
        GameObject projectileObj = ObjectPool.Instance.SpawnFromPool("BookProjectile", playerTransform.position, Quaternion.identity);
        if (projectileObj != null)
        {
            BookProjectile projectile = projectileObj.GetComponent<BookProjectile>();
            if (projectile != null)
            {
                // 발사 방향 계산
                Vector2 direction = (target.position - playerTransform.position).normalized;
                
                // 발사체 초기화
                projectile.Initialize(direction);
                projectile.damage = damage;
                projectile.penetration = penetration;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (playerTransform != null)
        {
            // 사거리 시각화
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(playerTransform.position, attackRange);
        }
    }
} 
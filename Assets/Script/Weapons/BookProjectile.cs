using UnityEngine;

public class BookProjectile : MonoBehaviour, IPooledObject
{
    [Header("발사체 속성")]
    public float damage = 20f;
    public float speed = 8f;
    public float maxDistance = 10f;
    public int penetration = 1;  // 관통 횟수
    public LayerMask enemyLayer;
    public GameObject hitEffectPrefab;

    private Vector2 direction;
    private Vector3 startPosition;
    private string poolTag = "BookProjectile";
    private int currentPenetrationCount = 0;

    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
        startPosition = transform.position;
        currentPenetrationCount = 0;
        
        // 발사체 회전
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void OnObjectSpawn()
    {
        // Reset any necessary variables when object is spawned from pool
        startPosition = transform.position;
        currentPenetrationCount = 0;
    }

    void Update()
    {
        // 발사체 이동
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // 최대 거리 체크
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            ReturnToPool();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            // 적중 효과 생성
            if (hitEffectPrefab != null)
            {
                ObjectPool.Instance.SpawnFromPool("BookHitEffect", transform.position, Quaternion.identity);
            }

            // 데미지 적용
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // 관통 처리
            currentPenetrationCount++;
            if (currentPenetrationCount >= penetration)
            {
                ReturnToPool();
            }
        }
    }

    private void ReturnToPool()
    {
        ObjectPool.Instance.ReturnToPool(poolTag, gameObject);
    }
} 
using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{
    [Header("기본 속성")]
    public float damage = 10f;
    public float speed = 10f;
    public float lifetime = 5f;
    public bool isEnemyProjectile = false; // 적의 투사체인지 플레이어의 투사체인지 구분
    public string poolTag; // ObjectPool에서 사용할 태그

    [Header("투사체 효과")]
    public GameObject hitEffect; // 충돌 시 이펙트

    protected Vector2 direction;
    private float currentLifetime;

    public virtual void OnObjectSpawn()
    {
        currentLifetime = lifetime;
    }

    protected virtual void Update()
    {
        // 투사체 이동
        transform.position += (Vector3)direction * speed * Time.deltaTime;
        
        // 수명 관리
        currentLifetime -= Time.deltaTime;
        if (currentLifetime <= 0)
        {
            ReturnToPool();
        }
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEnemyProjectile)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player player = collision.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                    SpawnHitEffect();
                    ReturnToPool();
                }
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    //enemy.TakeDamage(damage);
                    SpawnHitEffect();
                    ReturnToPool();
                }
            }
        }
    }

    private void SpawnHitEffect()
    {
        if (hitEffect != null)
        {
            GameObject effect = WeaponManager.Instance.SpawnProjectile("HitEffect", transform.position, Quaternion.identity);
            if (effect != null)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                effect.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }

    protected virtual void ReturnToPool()
    {
        if (!string.IsNullOrEmpty(poolTag))
        {
            ObjectPool.Instance.ReturnToPool(poolTag, gameObject);
        }
    }
} 
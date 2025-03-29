using UnityEngine;
using System.Collections.Generic;

public class TeaSplashProjectile : MonoBehaviour, IPooledObject
{
    private TeaSplash teaWeapon;
    private Vector2 direction;
    private float lifetime;
    private HashSet<int> hitEnemies = new HashSet<int>();
    private string poolTag = "TeaSplashProjectile";

    public void Initialize(TeaSplash weapon, Vector2 dir)
    {
        teaWeapon = weapon;
        direction = dir;
        lifetime = 0f;

        // 콜라이더 추가
        CircleCollider2D collider = gameObject.GetComponent<CircleCollider2D>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = 0.3f;
        }
    }

    public void OnObjectSpawn()
    {
        lifetime = 0f;
        hitEnemies.Clear();
        transform.localScale = Vector3.one;
    }

    void Update()
    {
        // 발사체 이동
        transform.Translate(direction * teaWeapon.projectileSpeed * Time.deltaTime);

        // 수명 관리
        lifetime += Time.deltaTime;
        if (lifetime >= teaWeapon.projectileLifetime)
        {
            ReturnToPool();
            return;
        }

        // 크기 점점 커지게
        float scale = 1f + lifetime * 2f;  // 시간에 따라 크기 증가
        transform.localScale = new Vector3(scale, scale, 1f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & teaWeapon.enemyLayer) != 0)
        {
            int enemyId = other.gameObject.GetInstanceID();
            
            // 같은 적을 두 번 이상 타격하지 않음
            if (!hitEnemies.Contains(enemyId))
            {
                hitEnemies.Add(enemyId);
                
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {
                    // 데미지 적용
                    enemy.TakeDamage(teaWeapon.damage);

                    // 넉백 방향 계산 (발사 방향으로)
                    enemy.ApplyKnockback(direction, teaWeapon.knockbackForce);
                }
            }
        }
    }

    private void ReturnToPool()
    {
        ObjectPool.Instance.ReturnToPool(poolTag, gameObject);
    }
} 
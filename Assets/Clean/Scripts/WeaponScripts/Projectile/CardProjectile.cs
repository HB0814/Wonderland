using UnityEngine;

public class CardProjectile : Projectile
{
    [SerializeField]public float cardLifetime;
    private float cardCurrentLifetime;

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        cardCurrentLifetime = cardLifetime;
    }

    private void Update()
    {
        // 수명 관리
        cardCurrentLifetime -= Time.deltaTime;
        if (cardCurrentLifetime <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                //enemy.TakeDamage(damage);
            }
        }
    }
} 
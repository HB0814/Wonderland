using UnityEngine;

public class KamikazeEnemy : NormalEnemyBase
{
    [Header("자폭 설정")]
    public float explosionRange = 2f; // 폭발 범위
    public float explosionDamage = 20f; // 폭발 데미지
    public GameObject explosionEffect; // 폭발 이펙트

    protected override void Update()
    {
        base.Update();

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        
        // 폭발 범위 안에 들어오면 자폭
        if (distanceToPlayer <= explosionRange)
        {
            Explode();
        }
    }

    protected void Explode()
    {
        // 폭발 이펙트 생성
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // 폭발 범위 내의 플레이어 검출
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRange);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Player player = hitCollider.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(explosionDamage);
                }
            }
        }

        // 자폭 후 제거
        Destroy(gameObject);
    }
} 
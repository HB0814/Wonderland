using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("적 원거리 공격 설정")]
    public float power;
    public float damage;
    [SerializeField]
    private Player player;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();

        if(player != null)
        {
            Vector2 dir = (player.transform.position - transform.position).normalized;
            rb.AddForce(dir * power, ForceMode2D.Impulse);
            Debug.Log("발사!");
        }

        Invoke(nameof(DisableObject), 5f);
    }

    private void DisableObject()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        rb.linearVelocity = Vector2.zero;
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 적의 투사체에 피격되었습니다.");
            player.TakeDamage(damage);
        }
    }
}

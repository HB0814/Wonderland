using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("�� ���Ÿ� ���� ����")]
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
            Debug.Log("�߻�!");
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
            Debug.Log("�÷��̾ ���� ����ü�� �ǰݵǾ����ϴ�.");
            player.TakeDamage(damage);
        }
    }
}

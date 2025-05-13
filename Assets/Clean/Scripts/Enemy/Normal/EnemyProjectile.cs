using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("�� ���Ÿ� ���� ����")]
    public float speed; //�̵� ũ��
    public float damage; //������
    public float lifeTime = 5f; //���ӽð�
    [SerializeField] private Player player; //�÷��̾� ��ũ��Ʈ
    private Rigidbody2D rb; //������ٵ�
    string projectileType; //����ü�� Ÿ��

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //Ȱ��ȭ ��
    private void OnEnable()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();

        if(player != null)
        {
            Vector2 dir = (player.transform.position - transform.position).normalized; //�߻� ����
            rb.AddForce(dir * speed, ForceMode2D.Impulse); //AddForce�Լ��� �������� ���� ���� �߻�
        }

        Invoke(nameof(DisableObject), lifeTime); //5�� �� ����ü ��Ȱ��ȭ �Լ� ����
    }

    //��Ȱ��ȭ �Լ�
    private void DisableObject()
    {
        gameObject.SetActive(false);
    }

    //��Ȱ��ȭ ��
    private void OnDisable()
    {
        rb.linearVelocity = Vector2.zero; //�̵� ũ�� 0
        CancelInvoke(); //�κ�ũ ���
    }

    //�浹 ��
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")) //�÷��̾� �浹 ��
        {
            player.TakeDamage(damage); //�÷��̾��� ������ ���ط� ���� �����Ͽ� �ǰ� �Լ� ����
        }
    }
}

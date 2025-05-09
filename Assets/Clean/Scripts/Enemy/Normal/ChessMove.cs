using UnityEngine;
using static ChessEnemy;

public class ChessMove : MonoBehaviour
{
    public enum EventType //ü�� �� ����
    {
        Rook, Bishop, //�̵��ϴ� �̺�Ʈ�� ��
    }

    [Header("�̺�Ʈ�� ü�� ����")]
    public EventType type; //ü�� �� ����
    public Vector2 moveDir; //ü�� ���� �̵��� ����
    public int dirNum; //�̵� ���� ��ȣ
    [SerializeField]float moveSpeed;
    private bool hasDir = false; //������ ������������ ���� ����

    private Rigidbody2D rb;
    float lifeTime = 10.0f;
    float timer = 0.0f;

    private void Update()
    {
        switch (type)
        {
            case EventType.Rook: //���� ��: ���� ������θ� �̵��ϴ� �̺�Ʈ�� ü����
                if (!hasDir) //���� ���� ���� ���� ��
                {
                    moveDir = RookMove(); //���� �̵� ���� �Լ� ��� ��������
                }
                rb.linearVelocity = moveDir * moveSpeed; //�̵� �������� �̵�

                timer += Time.deltaTime; //Ÿ�̸� �� ����
                if (timer >= lifeTime) //Ȱ��ȭ �ð� �̻� �޼�
                {
                    gameObject.SetActive(false); //���ӿ�����Ʈ ��Ȱ��ȭ
                }
                break;

            case EventType.Bishop: //���� ���: ������ ������θ� �̵��ϴ� �̺�Ʈ�� ü����
                if (!hasDir)
                {
                    moveDir = BishopMove(); //����� �̵� ���� �Լ� ��� ��������
                }
                rb.linearVelocity = moveDir * moveSpeed; //�̵� �������� �̵�

                timer += Time.deltaTime; //Ÿ�̸� �� ����
                if (timer >= lifeTime) //Ȱ��ȭ �ð� �̻� �޼�
                {
                    gameObject.SetActive(false); //���ӿ�����Ʈ ��Ȱ��ȭ
                }
                break;
        }
    }
    //���� �̵�����
    private Vector2 RookMove()
    {
        hasDir = true; //���� ���� ���� ��
        switch (dirNum)
        {
            case 0:
                return Vector2.up; //��
            case 1:
                return Vector2.down; //�Ʒ�
            case 2:
                return Vector2.left; //����
            default:
                return Vector2.right; //������
        }
    }

    //����� �̵�����
    private Vector2 BishopMove()
    {
        hasDir = true; //���� ���� ���� ��
        switch (dirNum)
        {
            case 0:
                return new Vector2(1, 1).normalized; //������ ��
            case 1:
                return new Vector2(1, -1).normalized; //������ �Ʒ�
            case 2:
                return new Vector2(-1, -1).normalized; //���� �Ʒ�
            default:
                return new Vector2(-1, 1).normalized; //���� ��
        }
    }

    //��Ȱ��ȭ ��
    private void OnDisable()
    {
        hasDir = false; //���� ���� ���� ����
        timer = 0.0f; //Ÿ�̸� �ʱ�ȭ
    }
}

using UnityEngine;

public class Hats : MonoBehaviour
{
    //���� ����
    public enum HatType
    {
        Red, 
        Blue, 
        Yellow,
        Green
    }

    public HatType hatType;

    [Header("����")]
    public float damage; //���ط�
    public float speed; //�ӵ�
    Player player; //�÷��̾� ��ũ��Ʈ
    Rigidbody2D rb; //������ٵ�2D


    [Header("���� - ����")] 
    public bool isReady = false; //���� �غ� ����
    public float explosionReadyRange = 6.0f; //���� �غ� ���� ����
    public float explosionMoveSpeed = 3.0f; //���� �غ� �� �̵��ӵ�
    public float explosionRange = 1.0f; //����  ����
    public float explosionDamage = 50.0f; //���� ������
    public GameObject explosionEffect; //���� ����Ʈ -> ���ӿ�����Ʈ ��� ��ƼŬ�ý��� ����Ҽ���
    public GameObject center; //�߾ӿ� ��ġ�� ���ӿ�����Ʈ

    [Header("�Ķ� - ���ο�")]
    public float slowForce;
    public float slowDuration;

    [Header("��� - ���")]
    public float maxSpeed;

    [Header("�ʷ� - �н�")]
    public int count; //���� ����
}

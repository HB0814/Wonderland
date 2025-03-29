using UnityEngine;

public class DataManager : MonoBehaviour
{
    [Header("���� ����")]
    public float damage; //������
    public float duration; //���ӽð�
    public float cooldown; //��Ÿ��
    public float timer; //��Ÿ�� Ÿ�̸�
    public float attackRange; //���� ���� => ���� ������ ���� ���������� ���� ������ ����.
    public bool hasCritical; //ġ��Ÿ ����
    public float criticalRate; //ġ��Ÿ Ȯ��
    public float knockbackForce; //�˹� ũ��
    public int strike; //���� ī��Ʈ
    public float projectileSpeed; //����ü �ӵ�
    public int projectileCount; //����ü ����

    public bool hasDefenseDecrease; //���� ���� ����
    public float defenseDecrease; //���� ����

    public bool hasIgnoreDefense; //���� ���� ����
    public float ignoreDefense; //���� ����

    public bool hasSlow; //���ο� ����
    public float slowForce;  //���ο� ũ��
}

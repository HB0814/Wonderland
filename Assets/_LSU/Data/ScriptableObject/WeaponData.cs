using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string type;

    public float damage_Default; //������
    public float duration_Default; //���ӽð�
    public float cooldown_Default; //��Ÿ��
    //public float timer = 0.0f; //��Ÿ�� Ÿ�̸�
    public float attackRange_Default; //���� ���� => ���� ������ ���� ���������� ���� ������ ����.

    public bool hasCritical_Default; //ġ��Ÿ ����
    public float criticalRate_Default; //ġ��Ÿ Ȯ��

    public float knockbackForce_Default; //�˹� ũ��
    public int strike_Default; //���� ī��Ʈ

    public float projectileSpeed_Default; //����ü �ӵ�
    public int projectileCount_Default; //����ü ����

    public bool hasDefenseDecrease_Default; //���� ���� ����
    public float defenseDecrease_Default; //���� ����

    public bool hasIgnoreDefense_Default; //���� ���� ����
    public float ignoreDefense_Default; //���� ����

    public bool hasSlow_Default; //���ο� ����
    public float slowForce_Default;  //���ο� ũ��
}

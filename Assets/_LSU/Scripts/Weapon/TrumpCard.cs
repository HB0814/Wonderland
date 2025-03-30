using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

using Random = UnityEngine.Random;

public class TrumpCard : MonoBehaviour
{
    static ScriptableObject trump;

    //[Header("���� ����")]
    float damage; //������
    //float duration; //���ӽð�
    //public float cooldown = 2f; //��Ÿ��
    //public float timer = 0.0f; //��Ÿ�� Ÿ�̸�
    float attackRange; //���� ���� => ���� ������ ���� ���������� ���� ������ ����.
    bool hasCritical; //ġ��Ÿ ����
    float criticalRate; //ġ��Ÿ Ȯ��
    float knockbackForce; //�˹� ũ��
    //int strike; //���� ī��Ʈ
    float projectileSpeed = 10.0f; //����ü �ӵ�
    int projectileCount; //����ü ����

    //bool hasDefenseDecrease; //���� ���� ����
    //float defenseDecrease; //���� ����

    //bool hasIgnoreDefense; //���� ���� ����
    //float ignoreDefense; //���� ����

    //bool hasSlow; //���ο� ����
    //float slowForce;  //���ο� ũ��

    float randomRot; //ī���� ���� ���� ����

    private void Update()
    {
        ShootTrump(); //ī�� �̵� �Լ� ����
    }

    //��ũ���ͺ� ������ ��������
    void GetData()
    {
        
    }

    //ī�� �̵� �Լ�
    public void ShootTrump() 
    {
        transform.Translate(Vector3.up * projectileSpeed * Time.deltaTime); //ī�� �̵�
    }

    //ī���� ���� ���� �Լ� 
    public void SetRotation() 
    {
        randomRot = math.floor(Random.Range(-180.0f, 180.0f) * 10) * 0.1f; //-180.0~180.0�� ���� �Ҽ��� ���� �ڸ� ������
        transform.rotation = Quaternion.Euler(0, 0, randomRot); //ī���� ���� ����
    }

    //ȭ�� ������ ���� ��
    private void OnBecameInvisible()
    {
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }
}

using Unity.Mathematics;
using UnityEngine;

using Random = UnityEngine.Random;

public class NonBirthdayFirecracker : MonoBehaviour
{
    GameObject player;

    public float posX; //������ ������ ��ǥ ���� ����
    public float posY;

    float ranX; //���� ��ǥ ����
    float ranY;

    //float randomRot; //ȸ�� ���� ����

    float damage; //������
    public float duration; //���ӽð�
    public float durationTimer; //���ӽð� Ÿ�̸�
    public float cooldown; //��Ÿ��
    public float timer; // Ÿ�̸�
    public float attackRange; //���� ���� => ���� ������ ���� ���������� ���� ������ ����.

    bool hasSlow; //���ο� ����
    float slowForce;  //���ο� ũ��

    private void Start()
    {

    }

    private void Update()
    {
        durationTimer += Time.deltaTime; //���ӽð� Ÿ�̸� ����

        if(durationTimer >= duration) //���ӽð� �޼� ��
        {
            durationTimer= 0; //���ӽð� Ÿ�̸� �ʱ�ȭ
            gameObject.SetActive(false); //�������� ��Ȱ��ȭ
        }
    }

    //���� ���� ��ġ ���� �Լ�
    public void SetAttackPos()
    {
        //�÷��̾� ���� X, Y ���� ����
        //���� ������ X, Y ��ǥ ��
        ranX = math.floor(Random.Range(player.transform.position.x - posX,
                                    player.transform.position.x + posX) * 10) * 0.1f;
        ranY = math.floor(Random.Range(player.transform.position.y - posY,
                                    player.transform.position.y + posY) * 10) * 0.1f;

        transform.position = new Vector3(ranX, ranY, transform.position.z); //�÷��̾� ���� ���� ��ǥ���� �Ҵ�
    }

    //�÷��̾� �Ҵ� �Լ� (������Ʈ Ǯ������ �Ҵ�)
    public void SetPlayer(GameObject _player)
    {
        player = _player;
    }
}

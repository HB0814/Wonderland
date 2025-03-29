using Unity.Mathematics;
using UnityEngine;

using Random = UnityEngine.Random;

public class RollApple : MonoBehaviour
{
    GameObject player; //�÷��̾� ���ӿ�����Ʈ

    public float posX; //����� ������ ��ǥ ���� ����
    public float posY;
    float ranX; //���� ��ǥ ����
    float ranY;

    Vector3 dis; //�Ÿ� ���� ����
    Vector3 dir; //���� ���� ����

    float randomRot; //ȸ�� ���� ����

    float damage;  //������
    float cooldown; //��Ÿ��
    float timer; //Ÿ�̸�
    float attackRange; //���ݹ���
    float knockbackForce;  //�˹� ũ��
    float projectileSpeed = 5; //����ü �ӵ�


    private void Update()
    {
        Move(); //�̵� �Լ� ����
    }

    //��� ���� ��ġ ��ġ
    //���� ���� ��ġ ���� �ڵ忡 Ȱ�� ����
    public void SetRandomPos()
    {
        Vector3 randomPosition = Vector3.zero; //���� ��ġ
        float min = -0.1f; //ȭ���� ����� �ּ� ����
        float max = 1.0f; //ȭ���� ����� �ִ� ����
        float zPos = 10; //3D Z�� ��ġ ����

        int flag = Random.Range(0, 4); //���� ����

        switch (flag)
        {
            case 0: //������ �ٱ�
                randomPosition = new Vector3(max, Random.Range(min, max), zPos);
                break;
            case 1: //���� �ٱ�
                randomPosition = new Vector3(min, Random.Range(min, max), zPos);
                break;
            case 2: //���� �ٱ�
                randomPosition = new Vector3(Random.Range(min, max), max, zPos);
                break;
            case 3: //�Ʒ��� �ٱ�
                randomPosition = new Vector3(Random.Range(min, max), min, zPos);
                break;
        }
        //����Ʈ ��ǥ�� ������ǥ�� ��ȯ. ����Ʈ ��ǥ�� �������� (0,0)�� ���� �ϴ�, (1,1)�� ���� ���.
        randomPosition = Camera.main.ViewportToWorldPoint(randomPosition);
        transform.position = randomPosition; //��� ���� ��ġ ����
    }
    
    //���� ���� ����
    public void SetAttackDir()
    {
        //�÷��̾� ���� X, Y ���� ����
        //���� ������ X, Y ��ǥ ��
        ranX = math.floor(Random.Range(player.transform.position.x - posX,
                                    player.transform.position.x + posX) * 10) * 0.1f;
        ranY = math.floor(Random.Range(player.transform.position.y - posY,
                                    player.transform.position.y + posY) * 10) * 0.1f;

        Vector3 pos; //����� ������ ��ǥ ����
        pos = new Vector3(ranX, ranY, transform.position.z); //�÷��̾� ���� ���� ��ǥ���� �Ҵ�

        dis = pos - transform.position; //�Ÿ� ���
        dir = dis.normalized; //��ֶ�����(���� ����ȭ)�� �Ͽ� ���� ���ϱ�
    }

    //��� �̵� �Լ�
    void Move()
    {
        transform.position += dir * projectileSpeed * Time.deltaTime; //�����ӵ��� dir �������� �̵�
    }

    //ȸ�� ���� ���� �Լ�
    public void SetRotation()
    {
        randomRot = math.floor(Random.Range(-180.0f, 180.0f) * 10) * 0.1f; //-180.0~180.0�� ���� �Ҽ��� ���� �ڸ� ������
        transform.rotation = Quaternion.Euler(0, 0, randomRot); //ȸ�� ���� ����
    }

    //ȭ�� ������ ���� �� �Լ�
    private void OnBecameInvisible()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0); //ȸ�� ���� �ʱ�ȭ
        gameObject.SetActive(false); //������Ʈ ��Ȱ��ȭ
    }

    //�÷��̾� �Ҵ� �Լ�
    public void SetPlayer(GameObject _player)
    {
        player = _player;
    }
}

using Unity.Mathematics;
using UnityEngine;

using Random = UnityEngine.Random;

public class CheshireCat : MonoBehaviour
{
    GameObject player;
    GameObject catClaw;

    public float posX; //ä��Ĺ�� ��Ÿ�� ��ǥ ���� ����
    public float posY;
    float ranX; //���� ��ǥ ����
    float ranY;

    float randomRot; //ȸ�� ���� ����

    bool isTrigger = false;//���� �浹 ���� üũ
    float checkTimer; //���浹 üũ Ÿ�̸�
    float disappearTime = 0.5f; //���浹 ���ѽð�

    float damage; //������
    float cooldown; //��Ÿ��
    float timer; //��Ÿ�� Ÿ�̸�
    float attackRange; //���� ���� => ���� ������ ���� ���������� ���� ������ ����.
    bool hasCritical; //ġ��Ÿ ����
    float criticalRate; //ġ��Ÿ Ȯ��

    private void Update()
    {
        if(!isTrigger) //���浹 �� Ÿ�̸� ����
        {
            checkTimer += Time.deltaTime;
        }

        if(checkTimer >= disappearTime) //���浹 ���ѽð� ����
        {
            checkTimer= 0.0f; //���浹 Ÿ�̸� �ʱ�ȭ
            gameObject.SetActive(false); //ü��Ĺ ��Ȱ��ȭ
        }
    }

    //���� ���� ��ġ ����
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

    //ȸ�� ���� ���� �Լ�
    public void SetRotation()
    {
        randomRot = math.floor(Random.Range(-15.0f, 15.0f) * 10) * 0.1f; //-15.0~15.0�� ���� �Ҽ��� ���� �ڸ� ������
        transform.rotation = Quaternion.Euler(0, 0, randomRot); //ȸ�� ���� ����
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Enemy")) //Enemy��� �±׸� ���� ������Ʈ�� �浹 ��
        {
            isTrigger = true; //�浹���� ��
            checkTimer = 0.0f; //���浹 Ÿ�̸� �ʱ�ȭ
            //cheshireCat.SetActive(true); //���Ϳ� ���� �� ���� ������Ʈ ����
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) //Enemy��� �±׸� ���� ������Ʈ�� �浹 ��
        {
            isTrigger= false; //�浹���� ����
        }
    }

    //�÷��̾� �Ҵ� �Լ� (������Ʈ Ǯ�� ��ũ��Ʈ���� �Ҵ�)
    public void SetPlayer(GameObject _player)
    {
        player = _player;
    }
}

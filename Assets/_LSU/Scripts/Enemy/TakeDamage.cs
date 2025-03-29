using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    NormalEnemy normalEnemy;
    DataManager dataManager;

    float attackCoolTime = 0.5f; //�÷��̾� ������ ��Ÿ��
    float timer; //�÷��̾� ������ ��Ÿ�� Ÿ�̸�

    bool canAttack = false; //���� ���� ����
    Collider2D playerCol; //�÷��̾� �ݶ��̴�


    //�˻��� ���� �ڷᱸ�� �̿�
    private HashSet<string> trumpCardTags = new HashSet<string>
    {
        "TrumpCard_0", "TrumpCard_1", "TrumpCard_2", "TrumpCard_3",
        "TrumpCard_4", "TrumpCard_5" //Ʈ���� ī���� �±׸� �̸� ����
    };

    private void Awake()
    {
        if(gameObject.CompareTag("Enemy"))
        {
            normalEnemy = GetComponent<NormalEnemy>();
        }
        else if(gameObject.CompareTag("HeartQueen"))
        {

        }
    }

    private void Update()
    {
        timer += Time.deltaTime; //Ÿ�̸� �ð� ����

        if (timer >= attackCoolTime) //��Ÿ�� �Ϸ�
        {
            canAttack = true; //���� ����
        }

        if (canAttack && playerCol != null) //���� ���� ���ο� �÷��̾� �ݶ��̴� ���� Ȯ��
        {
            AttackPlayer(); //�÷��̾� ����
        }
    }

    //�÷��̾� ����
    public void AttackPlayer()
    {
        timer = 0.0f; //Ÿ�̸� ��Ÿ�� �ʱ�ȭ
        canAttack = false; //���� ���� ���� ����
        Debug.Log("�÷��̾ ���ظ� ����."); //������
    }

    //Ʈ���� ī�� �ǰ�
    void HitTrumpCard()
    {
        //normalEnemy.hp -= ;
    }

    void HitCheshireCat()
    {
        //normalEnemy.hp -= ;
    }

    void HitNonBirthdayFirecracker()
    {
        //normalEnemy.hp -= ;
    }

    void HitRollApple()
    {
        //normalEnemy.hp -= ;
        //�˹� �Լ�
    }

    void HitJabberwockyBreath()
    {
        //normalEnemy.hp -= ;
    }

    //Ʈ���� ���� ���� ���� ��
    private void OnTriggerEnter2D(Collider2D other)
    {   
        string tag = other.tag;

        //Ʈ���� ī�� �ǰ�����
        if (trumpCardTags.Contains(other.tag)) //�浹�� ��ü�� �±װ� ������ �Ǿ��ִٸ� ����
        {
            HitTrumpCard();
        }

        switch (tag)
        {
            case "CheshireCat": //ä��Ĺ �ǰ�
                HitCheshireCat();
                break;

            case "RollApple": //��� �ǰ�
                HitRollApple();
                break;

            case "JabberwockyBreath": //�����Ű �ǰ�
                HitJabberwockyBreath();
                break;
        }
    }

    //Ʈ���� ���� ���� ���� ��
    private void OnTriggerStay2D(Collider2D other)
    {
        string tag = other.tag;

        switch(tag)
        {
            case "Player":
                playerCol = other; //�÷��̾� �ݶ��̴� ���� ����
                break;

            case "NonBirthdayFirecracker": //�Ȼ��� ���� �ǰ� > ��ġ�� �����̶� stay��
                HitNonBirthdayFirecracker();
                break;
        }
    }

    //Ʈ���� ���� ���� ���� ��
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) //������ �ݶ��̴��� �÷��̾� �±��� ��
        {
            playerCol = null; //�÷��̾� �ݶ��̴� ���� �� �ֱ�
        }
    }
}

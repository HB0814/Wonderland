using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : Enemy
{
    DataManager dataManager;

    float attackCoolTime = 0.5f; //플레이어 공격 쿨타임
    float timer; //플레이어 공격 쿨타임 타이머

    bool canAttack = false; //공격 가능 여부
    Collider2D playerCol; //플레이어 콜라이더

    //트럼프 카드 태그를 HashSet으로 관리
    private HashSet<string> trumpCardTags = new HashSet<string>
    {
        "TrumpCard_0", "TrumpCard_1", "TrumpCard_2", "TrumpCard_3",
        "TrumpCard_4", "TrumpCard_5" //트럼프 카드의 태그를 여기에 추가
    };

    private void Awake()
    {
        dataManager = FindObjectOfType<DataManager>();
    }

    private void Update()
    {
        timer += Time.deltaTime; //타이머 시간 증가

        if (timer >= attackCoolTime) //쿨타임 완료
        {
            canAttack = true; //공격 가능
        }

        if (canAttack && playerCol != null) //공격 가능하고 플레이어 콜라이더가 있다면
        {
            AttackPlayer(); //플레이어 공격
        }
    }

    //플레이어 공격
    public void AttackPlayer()
    {
        timer = 0.0f; //타이머 쿨타임 초기화
        canAttack = false; //공격 가능 여부 초기화
        Debug.Log("플레이어가 데미지를 받음."); //디버그
    }

    //트럼프 카드 맞음
    void HitTrumpCard()
    {
        TakeDamage(10f); //기본 데미지 10
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

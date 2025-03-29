using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    NormalEnemy normalEnemy;
    DataManager dataManager;

    float attackCoolTime = 0.5f; //플레이어 데미지 쿨타임
    float timer; //플레이어 데미지 쿨타임 타이머

    bool canAttack = false; //공격 가능 여부
    Collider2D playerCol; //플레이어 콜라이더


    //검색을 위한 자료구조 이용
    private HashSet<string> trumpCardTags = new HashSet<string>
    {
        "TrumpCard_0", "TrumpCard_1", "TrumpCard_2", "TrumpCard_3",
        "TrumpCard_4", "TrumpCard_5" //트럼프 카드의 태그를 미리 저장
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
        timer += Time.deltaTime; //타이머 시간 증가

        if (timer >= attackCoolTime) //쿨타임 완료
        {
            canAttack = true; //공격 가능
        }

        if (canAttack && playerCol != null) //공격 가능 여부와 플레이어 콜라이더 정보 확인
        {
            AttackPlayer(); //플레이어 공격
        }
    }

    //플레이어 공격
    public void AttackPlayer()
    {
        timer = 0.0f; //타이머 쿨타임 초기화
        canAttack = false; //공격 가능 여부 해제
        Debug.Log("플레이어가 피해를 입음."); //데미지
    }

    //트럼프 카드 피격
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
        //넉백 함수
    }

    void HitJabberwockyBreath()
    {
        //normalEnemy.hp -= ;
    }

    //트리거 접촉 상태 진입 시
    private void OnTriggerEnter2D(Collider2D other)
    {   
        string tag = other.tag;

        //트럼프 카드 피격판정
        if (trumpCardTags.Contains(other.tag)) //충돌한 객체의 태그가 포함이 되어있다면 실행
        {
            HitTrumpCard();
        }

        switch (tag)
        {
            case "CheshireCat": //채셔캣 피격
                HitCheshireCat();
                break;

            case "RollApple": //사과 피격
                HitRollApple();
                break;

            case "JabberwockyBreath": //재버워키 피격
                HitJabberwockyBreath();
                break;
        }
    }

    //트리거 접촉 상태 유지 시
    private void OnTriggerStay2D(Collider2D other)
    {
        string tag = other.tag;

        switch(tag)
        {
            case "Player":
                playerCol = other; //플레이어 콜라이더 정보 저장
                break;

            case "NonBirthdayFirecracker": //안생일 폭죽 피격 > 설치형 장판이라 stay로
                HitNonBirthdayFirecracker();
                break;
        }
    }

    //트리거 접촉 상태 종료 시
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) //접촉한 콜라이더가 플레이어 태그일 시
        {
            playerCol = null; //플레이어 콜라이더 값에 널 넣기
        }
    }
}

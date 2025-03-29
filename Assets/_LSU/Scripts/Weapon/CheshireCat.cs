using Unity.Mathematics;
using UnityEngine;

using Random = UnityEngine.Random;

public class CheshireCat : MonoBehaviour
{
    GameObject player;
    GameObject catClaw;

    public float posX; //채셔캣이 나타날 좌표 범위 변수
    public float posY;
    float ranX; //랜덤 좌표 변수
    float ranY;

    float randomRot; //회전 각도 변수

    bool isTrigger = false;//몬스터 충돌 여부 체크
    float checkTimer; //미충돌 체크 타이머
    float disappearTime = 0.5f; //미충돌 제한시간

    float damage; //데미지
    float cooldown; //쿨타임
    float timer; //쿨타임 타이머
    float attackRange; //공격 범위 => 기존 스케일 값에 곱연산으로 들어가지 않을까 싶음.
    bool hasCritical; //치명타 여부
    float criticalRate; //치명타 확률

    private void Update()
    {
        if(!isTrigger) //미충돌 시 타이머 증가
        {
            checkTimer += Time.deltaTime;
        }

        if(checkTimer >= disappearTime) //미충돌 제한시간 도달
        {
            checkTimer= 0.0f; //미충돌 타이머 초기화
            gameObject.SetActive(false); //체셔캣 비활성화
        }
    }

    //공격 랜덤 위치 설정
    public void SetAttackPos()
    {
        //플레이어 기준 X, Y 범위 설정
        //랜덤 범위의 X, Y 좌표 값
        ranX = math.floor(Random.Range(player.transform.position.x - posX,
                                    player.transform.position.x + posX) * 10) * 0.1f;
        ranY = math.floor(Random.Range(player.transform.position.y - posY,
                                    player.transform.position.y + posY) * 10) * 0.1f;

        transform.position = new Vector3(ranX, ranY, transform.position.z); //플레이어 기준 범위 좌표값을 할당
    }

    //회전 각도 설정 함수
    public void SetRotation()
    {
        randomRot = math.floor(Random.Range(-15.0f, 15.0f) * 10) * 0.1f; //-15.0~15.0도 값의 소수점 일의 자리 버리기
        transform.rotation = Quaternion.Euler(0, 0, randomRot); //회전 각도 변경
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Enemy")) //Enemy라는 태그를 가진 오브젝트와 충돌 시
        {
            isTrigger = true; //충돌여부 참
            checkTimer = 0.0f; //미충돌 타이머 초기화
            //cheshireCat.SetActive(true); //몬스터와 접촉 시 공격 오브젝트 실행
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) //Enemy라는 태그를 가진 오브젝트와 충돌 시
        {
            isTrigger= false; //충돌여부 거짓
        }
    }

    //플레이어 할당 함수 (오브젝트 풀링 스크립트에서 할당)
    public void SetPlayer(GameObject _player)
    {
        player = _player;
    }
}

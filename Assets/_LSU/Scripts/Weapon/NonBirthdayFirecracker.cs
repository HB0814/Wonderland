using Unity.Mathematics;
using UnityEngine;

using Random = UnityEngine.Random;

public class NonBirthdayFirecracker : MonoBehaviour
{
    GameObject player;

    public float posX; //폭죽이 떨어질 좌표 범위 변수
    public float posY;

    float ranX; //랜덤 좌표 변수
    float ranY;

    //float randomRot; //회전 각도 변수

    float damage; //데미지
    public float duration; //지속시간
    public float durationTimer; //지속시간 타이머
    public float cooldown; //쿨타임
    public float timer; // 타이머
    public float attackRange; //공격 범위 => 기존 스케일 값에 곱연산으로 들어가지 않을까 싶음.

    bool hasSlow; //슬로우 여부
    float slowForce;  //슬로우 크기

    private void Start()
    {

    }

    private void Update()
    {
        durationTimer += Time.deltaTime; //지속시간 타이머 증가

        if(durationTimer >= duration) //지속시간 달성 시
        {
            durationTimer= 0; //지속시간 타이머 초기화
            gameObject.SetActive(false); //폭죽장판 비활성화
        }
    }

    //랜덤 공격 위치 설정 함수
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

    //플레이어 할당 함수 (오브젝트 풀링에서 할당)
    public void SetPlayer(GameObject _player)
    {
        player = _player;
    }
}

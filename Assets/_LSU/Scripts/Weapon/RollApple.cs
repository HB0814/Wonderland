using Unity.Mathematics;
using UnityEngine;

using Random = UnityEngine.Random;

public class RollApple : MonoBehaviour
{
    GameObject player; //플레이어 게임오브젝트

    public float posX; //사과가 지나갈 좌표 범위 변수
    public float posY;
    float ranX; //랜덤 좌표 변수
    float ranY;

    Vector3 dis; //거리 계산용 변수
    Vector3 dir; //방향 계산용 변수

    float randomRot; //회전 각도 변수

    float damage;  //데미지
    float cooldown; //쿨타임
    float timer; //타이머
    float attackRange; //공격범위
    float knockbackForce;  //넉백 크기
    float projectileSpeed = 5; //투사체 속도


    private void Update()
    {
        Move(); //이동 함수 실행
    }

    //사과 랜덤 위치 배치
    //몬스터 랜덤 위치 스폰 코드에 활용 가능
    public void SetRandomPos()
    {
        Vector3 randomPosition = Vector3.zero; //랜덤 위치
        float min = -0.1f; //화면을 벗어나는 최소 범위
        float max = 1.0f; //화면을 벗어나는 최대 범위
        float zPos = 10; //3D Z축 위치 고정

        int flag = Random.Range(0, 4); //랜덤 방향

        switch (flag)
        {
            case 0: //오른쪽 바깥
                randomPosition = new Vector3(max, Random.Range(min, max), zPos);
                break;
            case 1: //왼쪽 바깥
                randomPosition = new Vector3(min, Random.Range(min, max), zPos);
                break;
            case 2: //위쪽 바깥
                randomPosition = new Vector3(Random.Range(min, max), max, zPos);
                break;
            case 3: //아래쪽 바깥
                randomPosition = new Vector3(Random.Range(min, max), min, zPos);
                break;
        }
        //뷰포트 좌표를 월드좌표로 변환. 뷰포트 좌표계 기준으로 (0,0)은 좌측 하단, (1,1)은 우측 상단.
        randomPosition = Camera.main.ViewportToWorldPoint(randomPosition);
        transform.position = randomPosition; //사과 랜덤 위치 적용
    }
    
    //공격 방향 설정
    public void SetAttackDir()
    {
        //플레이어 기준 X, Y 범위 설정
        //랜덤 범위의 X, Y 좌표 값
        ranX = math.floor(Random.Range(player.transform.position.x - posX,
                                    player.transform.position.x + posX) * 10) * 0.1f;
        ranY = math.floor(Random.Range(player.transform.position.y - posY,
                                    player.transform.position.y + posY) * 10) * 0.1f;

        Vector3 pos; //사과가 굴러갈 좌표 변수
        pos = new Vector3(ranX, ranY, transform.position.z); //플레이어 기준 범위 좌표값을 할당

        dis = pos - transform.position; //거리 계산
        dir = dis.normalized; //노멀라이즈(백터 정규화)를 하여 방향 구하기
    }

    //사과 이동 함수
    void Move()
    {
        transform.position += dir * projectileSpeed * Time.deltaTime; //일정속도로 dir 방향으로 이동
    }

    //회전 각도 설정 함수
    public void SetRotation()
    {
        randomRot = math.floor(Random.Range(-180.0f, 180.0f) * 10) * 0.1f; //-180.0~180.0도 값의 소수점 일의 자리 버리기
        transform.rotation = Quaternion.Euler(0, 0, randomRot); //회전 각도 변경
    }

    //화면 밖으로 나갈 시 함수
    private void OnBecameInvisible()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0); //회전 각도 초기화
        gameObject.SetActive(false); //오브젝트 비활성화
    }

    //플레이어 할당 함수
    public void SetPlayer(GameObject _player)
    {
        player = _player;
    }
}

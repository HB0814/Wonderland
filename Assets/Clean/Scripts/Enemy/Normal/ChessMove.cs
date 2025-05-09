using UnityEngine;
using static ChessEnemy;

public class ChessMove : MonoBehaviour
{
    public enum EventType //체스 말 종류
    {
        Rook, Bishop, //이동하는 이벤트형 적
    }

    [Header("이벤트형 체스 관련")]
    public EventType type; //체스 말 종류
    public Vector2 moveDir; //체스 말이 이동할 방향
    public int dirNum; //이동 방향 번호
    [SerializeField]float moveSpeed;
    private bool hasDir = false; //방향이 정해졌는지에 대한 여부

    private Rigidbody2D rb;
    float lifeTime = 10.0f;
    float timer = 0.0f;

    private void Update()
    {
        switch (type)
        {
            case EventType.Rook: //검정 룩: 십자 모양으로만 이동하는 이벤트형 체스말
                if (!hasDir) //방향 보유 여부 거짓 시
                {
                    moveDir = RookMove(); //룩의 이동 방향 함수 결과 가져오기
                }
                rb.linearVelocity = moveDir * moveSpeed; //이동 방향으로 이동

                timer += Time.deltaTime; //타이머 값 증가
                if (timer >= lifeTime) //활성화 시간 이상 달성
                {
                    gameObject.SetActive(false); //게임오브젝트 비활성화
                }
                break;

            case EventType.Bishop: //검정 비숍: 엑스자 모양으로만 이동하는 이벤트형 체스말
                if (!hasDir)
                {
                    moveDir = BishopMove(); //비숍의 이동 방향 함수 결과 가져오기
                }
                rb.linearVelocity = moveDir * moveSpeed; //이동 방향으로 이동

                timer += Time.deltaTime; //타이머 값 증가
                if (timer >= lifeTime) //활성화 시간 이상 달성
                {
                    gameObject.SetActive(false); //게임오브젝트 비활성화
                }
                break;
        }
    }
    //룩의 이동방향
    private Vector2 RookMove()
    {
        hasDir = true; //방향 보유 여부 참
        switch (dirNum)
        {
            case 0:
                return Vector2.up; //위
            case 1:
                return Vector2.down; //아래
            case 2:
                return Vector2.left; //왼쪽
            default:
                return Vector2.right; //오른쪽
        }
    }

    //비숍의 이동방향
    private Vector2 BishopMove()
    {
        hasDir = true; //방향 보유 여부 참
        switch (dirNum)
        {
            case 0:
                return new Vector2(1, 1).normalized; //오른쪽 위
            case 1:
                return new Vector2(1, -1).normalized; //오른쪽 아래
            case 2:
                return new Vector2(-1, -1).normalized; //왼쪽 아래
            default:
                return new Vector2(-1, 1).normalized; //왼쪽 위
        }
    }

    //비활성화 시
    private void OnDisable()
    {
        hasDir = false; //방향 보유 여부 거짓
        timer = 0.0f; //타이머 초기화
    }
}

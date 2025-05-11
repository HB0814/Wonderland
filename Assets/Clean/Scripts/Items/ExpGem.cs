using System.Collections;
using UnityEngine;

public class ExpGem : MonoBehaviour
{
    [Header("경험치 설정")]
    [SerializeField] private float expAmount = 10f;        //보석이 제공하는 경험치량
    [SerializeField] private float magnetDistance = 0.5f;    //자석 기능 거리
    [SerializeField] private float baseSpeed = 5f;         //기본 속도
    [SerializeField] private float maxSpeed = 15f;         //최대 속도
    [SerializeField] private float accelerationRate = 2f;  //초당 속도 증가량
    [SerializeField] private float force = 3f;

    private Player player; //플레이어 스크립트
    private Transform target; //플레이어 중심
    [SerializeField] private SpriteRenderer spriteRenderer; //스프라이트 렌더러
    [SerializeField] private Rigidbody2D rb; //리지드바디2D

    private bool isAttracting; //기본 자석 기능 활성화 여부
    private bool OnMagnet = false; //자석 아이템 활성화 여부
    private float currentSpeed; //현재 속도
    private float attractTimer = 0f; //자석 타이머

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        target = player.gameObject.transform.GetChild(0).transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = baseSpeed;
    }

    private void Update()
    {
        if (player == null)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, target.position); //경험치 잼과 타겟의 거리
        
        // 플레이어와의 거리가 자석 거리보다 가까우면
        if (distanceToPlayer <= magnetDistance)
        {
            isAttracting = true; //기본 자석 기능 활성화
        }

        //기본 자석 기능 활성화 시
        if (isAttracting)
        {
            attractTimer += Time.deltaTime; //자석 타이머 시간 증가
            currentSpeed = Mathf.Min(baseSpeed + (accelerationRate * attractTimer), maxSpeed); //시간에 따라 현재 속도 증가

            transform.position = Vector2.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);
            // 플레이어 방향으로 이동
        }

        //자석 아이템 기능
        //자석 아이템 획득 시 플레이어 측으로 이동
        if (OnMagnet && player != null)
        {
            attractTimer += Time.deltaTime; //자석 타이머 시간 증가
            currentSpeed = Mathf.Min(baseSpeed + (accelerationRate * attractTimer), maxSpeed); //시간에 따라 현재 속도 증가
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, currentSpeed * Time.deltaTime);
            // 플레이어 방향으로 이동
        }
    }

    //자석 아이템 획득
    public void StartAttraction()
    {
        OnMagnet = true; //자석 아이템 활성화
    }

    //경험치 잼 튕기는 효과
    private void Launch()
    {
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        force = Random.Range(0.5f, 1.5f);
        rb.AddForce(randomDir * force, ForceMode2D.Impulse);
        Invoke(nameof(AddForceZero), 0.1f);
    }

    private void AddForceZero()
    {
        rb.linearVelocity = Vector2.zero;
    }

    //경험치 잼 활성화 시
    private void OnEnable()
    {
        Launch();
    }

    //비활성화 시
    private void OnDisable()
    {
        isAttracting = false; //기본 자석 기능 여부 비활성화
        OnMagnet = false; //자석 아이템 비활성화
        currentSpeed = baseSpeed; //속도 초기화
        attractTimer = 0; //타이머 초기화
        rb.linearVelocity = Vector2.zero;
    }

    //충돌 시
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) //플레이어 충돌 시
        {
            player.AddExperience(expAmount); //플레이어 경험치 증가 함수 실행
            gameObject.SetActive(false); //경험치 잼 비활성화
        }
    }
} 
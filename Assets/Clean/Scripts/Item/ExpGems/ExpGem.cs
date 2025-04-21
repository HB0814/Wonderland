using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ExpGem : MonoBehaviour
{
    [Header("경험치 설정")]
    [SerializeField] private float expAmount = 10f;        //보석이 제공하는 경험치량
    [SerializeField] private float magnetDistance = 0.5f;    //자석 기능 거리
    [SerializeField] private float baseSpeed = 5f;         //기본 속도
    [SerializeField] private float maxSpeed = 15f;         //최대 속도
    [SerializeField] private float accelerationRate = 2f;  //초당 속도 증가량

    private Player player;
    private Transform target;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool isAttracting;
    private bool OnMagnet = false; //자석 기능 여부
    private float currentSpeed;
    private float attractTimer = 0f; //자석 타이머

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        target = player.gameObject.transform.GetChild(0).transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentSpeed = baseSpeed;
    }

    private void Update()
    {
        //기본 경험치 잼을 획득 시에도 자석 기능을 사용할 시 활성화
        if (player == null)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        // 플레이어와의 거리가 자석 거리보다 가까우면
        if (distanceToPlayer <= magnetDistance)
        {
            isAttracting = true;
        }

        //기본 자석 기능
        if (isAttracting)
        {
            // 시간에 따라 속도 증가
            attractTimer += Time.deltaTime;
            currentSpeed = Mathf.Min(baseSpeed + (accelerationRate * attractTimer), maxSpeed);

            // 플레이어 방향으로 이동
            transform.position = Vector2.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);
        }

        //자석 아이템 기능
        //자석 아이템 획득 시 플레이어 측으로 이동
        if (OnMagnet && player != null)
        {
            attractTimer += Time.deltaTime;
            currentSpeed = Mathf.Min(baseSpeed + (accelerationRate * attractTimer), maxSpeed);
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, currentSpeed * Time.deltaTime);
        }

        //자석 아이템 기능 테스트용
        if (Input.GetKeyDown(KeyCode.G))
        {
            OnMagnet=true;
        }

    }

    //자석 아이템 획득
    public void StartAttraction()
    {
        OnMagnet = true;
    }

    //활성화 시
    private void OnEnable()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100); //활성화 시 레이어 순서 설정
    }

    //비활성화 시
    private void OnDisable()
    {
        isAttracting = false;
        OnMagnet = false; //자석 비활성화
        currentSpeed = baseSpeed; //속도 초기화
        attractTimer = 0; //타이머 초기화
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.AddExperience(expAmount); //플레이어 경험치 증가 함수 실행
            gameObject.SetActive(false); //경험치 잼 비활성화
        }
    }
} 
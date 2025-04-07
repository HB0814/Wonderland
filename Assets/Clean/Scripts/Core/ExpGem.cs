using UnityEngine;

public class ExpGem : MonoBehaviour
{
    [Header("경험치 설정")]
    [SerializeField] private float expAmount = 10f;        // 보석이 제공하는 경험치량
    [SerializeField] private float magnetDistance = 5f;    // 플레이어를 끌어당기는 거리
    [SerializeField] private float baseSpeed = 5f;         // 기본 이동 속도
    [SerializeField] private float maxSpeed = 15f;         // 최대 이동 속도
    [SerializeField] private float accelerationRate = 2f;  // 초당 속도 증가량

    private Transform player;
    private bool isAttracting = false;
    private float currentSpeed;
    private float attractTimer = 0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentSpeed = baseSpeed;
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 플레이어와의 거리가 자석 거리보다 가까우면
        if (distanceToPlayer <= magnetDistance)
        {
            isAttracting = true;
        }

        if (isAttracting)
        {
            // 시간에 따라 속도 증가
            attractTimer += Time.deltaTime;
            currentSpeed = Mathf.Min(baseSpeed + (accelerationRate * attractTimer), maxSpeed);

            // 플레이어 방향으로 이동
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                currentSpeed * Time.deltaTime
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어의 경험치 증가
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.AddExperience(expAmount);
            }
            
            // 보석 제거
            Destroy(gameObject);
        }
    }
} 
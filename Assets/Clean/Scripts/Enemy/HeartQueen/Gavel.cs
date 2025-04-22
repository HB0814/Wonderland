using UnityEngine;

public class Gavel : MonoBehaviour
{
    private Vector3 targetPos;
    private float dropDelay = 1f;
    private bool isDrop = false;

    [SerializeField] private float attackDamage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dropSpeed = 10.0f;
    [SerializeField] private Animator animator;

    private Player player;
    private Rigidbody rb;
    private BoxCollider2D boxCol;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem dustParticle;

    /*
     *  플레이어를 쫓아다니며 의사봉이 바닥을 세번 내려침
     *  플레이어 추격 필요 -> 플레이어, 이동속도
     *  내려치고나서 다음 공격까지의 딜레이 필요, 또한 공격 중에는 추격x
     *  
     *  애니메이션을 통한 콜라이더 활성화
     *  
     *
    */

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        dustParticle = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (player != null && !isDrop)
        {
            Vector2 dir = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = dir * moveSpeed;
        }

        if (spriteRenderer.color.a == 0.0f)
        {
            gameObject.SetActive(false);
        }
    }

    public void Init(Player _player, float delay)
    {
        player = _player;
        Transform target = player.transform;
        transform.position = new Vector3(target.position.x, target.position.y + 0.6f, transform.position.z);
        targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        dropDelay = delay;

        gameObject.SetActive(true);

        // Drop 애니메이션 준비 상태
        if (animator != null)
        {
            animator.SetBool("isAttack", true); // 낙하 애니메이션 시작
        }

        Invoke(nameof(Drop), dropDelay);
    }

    private void Drop()
    {
        isDrop = true;
    }

    private void OnHit()
    {
        dustParticle.Play();
        //충격 먼지 파티클 실행
    }


    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        //animator.SetBool("isDrop", false);
        isDrop = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.TakeDamage(attackDamage);
        }
    }
}

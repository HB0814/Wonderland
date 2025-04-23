using System.Collections;
using UnityEngine;

public class Gavel : MonoBehaviour
{
    private Vector3 targetPos;
    private float animDelay = 1f;
    private bool isAttack = false;
    private float torqueForce = 10f;

    WaitForSeconds nextAttackTime;

    [SerializeField] private float attackDamage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Animator animator;

    [SerializeField] private Player player;
    private Rigidbody2D rb;
    private BoxCollider2D boxCol;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem dustParticle;

    //왼쪽 오른쪽 좌표를 가지고 있는 자식 오브젝트로 왼쪽 오른쪽

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
        rb = GetComponent<Rigidbody2D>();
        dustParticle = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if(player != null && !isAttack)
        {
            Vector2 dir = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = dir * moveSpeed;
        }
        else if(player != null && isAttack)
        {
            rb.linearVelocity = Vector2.zero;
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
        transform.position = new Vector3(target.position.x + 0.8f, target.position.y + 0.6f, transform.position.z);
        targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        animDelay = delay;

        nextAttackTime = new WaitForSeconds(1.0f);

        gameObject.SetActive(true);

        Invoke(nameof(Attack), delay);
    }

    private void Attack()
    {
        isAttack = true;
        animator.SetBool("isMove", false);
        StartCoroutine("GavelAttack");
    }

    private void OnHit()
    {
        dustParticle.Play();
        //충격 먼지 파티클 실행
    }

    //공격 시 애니메이션 실행 -> 바닥에 닿았을 타이밍에 파티클 실행, 이후 의사봉 다시 들어올리기
    //좌우 비교 이후 해당 방향을 내려 찍는 애니메이션 실행
    private IEnumerator GavelAttack()
    {
        if(player.transform.position.x <= transform.position.x)
            animator.SetBool("isAttack_L", true); //의사봉 내려찍기 애니메이션
        else if(player.transform.position.x > transform.position.x)
            animator.SetBool("isAttack_R", true); //의사봉 내려찍기 애니메이션

        Invoke(nameof(OnHit), 0.5f);

        yield return nextAttackTime;

        animator.SetBool("isAttack_L", false);
        animator.SetBool("isAttack_R", false);
        animator.SetBool("isMove", true);
        isAttack = false;

        yield return nextAttackTime;

        Attack();
    }


    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        //animator.SetBool("isDrop", false);
        isAttack = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.TakeDamage(attackDamage);
        }
    }
}

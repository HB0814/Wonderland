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

    //���� ������ ��ǥ�� ������ �ִ� �ڽ� ������Ʈ�� ���� ������

    /*
     *  �÷��̾ �Ѿƴٴϸ� �ǻ���� �ٴ��� ���� ����ħ
     *  �÷��̾� �߰� �ʿ� -> �÷��̾�, �̵��ӵ�
     *  ����ġ���� ���� ���ݱ����� ������ �ʿ�, ���� ���� �߿��� �߰�x
     *  
     *  �ִϸ��̼��� ���� �ݶ��̴� Ȱ��ȭ
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
        //��� ���� ��ƼŬ ����
    }

    //���� �� �ִϸ��̼� ���� -> �ٴڿ� ����� Ÿ�ֿ̹� ��ƼŬ ����, ���� �ǻ�� �ٽ� ���ø���
    //�¿� �� ���� �ش� ������ ���� ��� �ִϸ��̼� ����
    private IEnumerator GavelAttack()
    {
        if(player.transform.position.x <= transform.position.x)
            animator.SetBool("isAttack_L", true); //�ǻ�� ������� �ִϸ��̼�
        else if(player.transform.position.x > transform.position.x)
            animator.SetBool("isAttack_R", true); //�ǻ�� ������� �ִϸ��̼�

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

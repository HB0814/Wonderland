using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Guillotione : MonoBehaviour
{
    private Vector3 targetPos;
    private float dropDelay = 1f;
    private bool isDrop = false;

    [SerializeField] private float attackDamage;
    [SerializeField] private float dropSpeed = 10.0f;
    [SerializeField] private Animator animator;

    private Player player;
    private BoxCollider2D boxCol;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem dustParticle;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        dustParticle = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!isDrop)
            return;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, dropSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
        {
            // �ٴ� ���� �� ó��
            OnHit();
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
        transform.position = new Vector3(target.position.x, target.position.y + 2, transform.position.z);
        targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        dropDelay = delay;

        gameObject.SetActive(true);

        // Drop �ִϸ��̼� �غ� ����
        if (animator != null)
        {
            animator.SetBool("isDrop", true); //���� �ִϸ��̼� ����
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
        //��� ���� ��ƼŬ ����
    }


    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        animator.SetBool("isDrop", false);
        isDrop = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("���ƾ!");
            player.TakeDamage(attackDamage);
        }
    }
}

using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Guillotione : MonoBehaviour
{
    public enum GuillotioneType
    {
        Moved, Fixed
    }
    public GuillotioneType type;

    [Header("공통")]
    [SerializeField] private float attackDamage;
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private Animator animator;
    private Player player;
    private BoxCollider2D boxCol;
    [SerializeField]private SpriteRenderer spriteRenderer;

    [Header("Fixed")]
    private Vector3 targetPos;
    private float dropDelay = 2f;
    private bool isPlay = false;
    private bool isDrop = false;
    [SerializeField] GameObject blade;
    [SerializeField] private SpriteRenderer bladeSpriteRenderer;
    [SerializeField]private ParticleSystem dustParticle;

    //[Header("Moved")]

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        boxCol = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(type == GuillotioneType.Fixed)
        {
            FixedGuillotione();
        }
        else if(type == GuillotioneType.Moved)
        {
            MovedGuillotione();
        }
    }

    private void FixedGuillotione()
    {
        if (!isDrop)
            return;

        blade.transform.localPosition = Vector3.MoveTowards(blade.transform.localPosition, targetPos, speed * Time.deltaTime);

        if (Vector3.Distance(blade.transform.localPosition, targetPos) < 0.05f)
        {
            // 바닥 도착 시 처리
            OnHit();
        }

        if (bladeSpriteRenderer.color.a == 0.0f)
        {
            gameObject.SetActive(false);
        }
    }
    
    public void Init_Fixed()
    {
        targetPos = new Vector3(blade.transform.localPosition.x, blade.transform.localPosition.y - 1.05f, blade.transform.localPosition.z);

        if (animator != null)
        {
            animator.SetBool("isReady", true); //공격 애니메이션 시작
        }

        Invoke(nameof(Drop), dropDelay);
    }

    private void Drop()
    {
        isDrop = true;
        animator.SetBool("isDrop", true);
    }

    private void OnHit()
    {
        dustParticle.Play();
        //충격 먼지 파티클 실행
    }

    private void MovedGuillotione()
    {

    }

    private void OnEnable()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
        bladeSpriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
    }

    private void OnDisable()
    {
        if (type == GuillotioneType.Fixed)
        {
            animator.SetBool("isDrop", false);
            animator.SetBool("isReady", false);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
            bladeSpriteRenderer.color = new Color(bladeSpriteRenderer.color.r, bladeSpriteRenderer.color.g, bladeSpriteRenderer.color.b, 1f);
            blade.transform.localPosition = new Vector3(0, 1.3f, 0);
            isDrop = false;
            isPlay = false;
        }
        else if (type == GuillotioneType.Moved)
        {

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !isPlay)
        {
            isPlay = true;
            Init_Fixed();
        }
    }
}

using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FixedGuillotione : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    private float dropDelay = 2f;
    private bool isPlay = false;
    private bool isDrop = false;

    private Vector3 targetVec;
    [SerializeField] GameObject blade;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer bladeSpriteRenderer;
    [SerializeField] private ParticleSystem dustParticle;
    private Player player;
    private BoxCollider2D boxCol;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        boxCol = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Attack_Guillotione();
    }

    private void Attack_Guillotione()
    {
        if (!isDrop)
            return;

        blade.transform.localPosition = Vector3.MoveTowards(blade.transform.localPosition, targetVec, speed * Time.deltaTime);

        if (Vector3.Distance(blade.transform.localPosition, targetVec) < 0.05f)
        {
            // 바닥 도착 시 처리
            OnHit();
        }

        if (bladeSpriteRenderer.color.a == 0.0f)
        {
            gameObject.SetActive(false);
        }
    }

    private void Ready()
    {
        targetVec = new Vector3(blade.transform.localPosition.x, blade.transform.localPosition.y - 1.05f, blade.transform.localPosition.z);

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

    private void OnEnable()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
        bladeSpriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
    }

    private void OnDisable()
    {
        animator.SetBool("isDrop", false);
        animator.SetBool("isReady", false);
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        bladeSpriteRenderer.color = new Color(bladeSpriteRenderer.color.r, bladeSpriteRenderer.color.g, bladeSpriteRenderer.color.b, 1f);
        blade.transform.localPosition = new Vector3(0, 1.3f, 0);
        isDrop = false;
        isPlay = false;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPlay)
        {
            isPlay = true;
            Ready();
        }
    }
}
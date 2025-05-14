using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MovedGuillotione : MonoBehaviour
{
    [SerializeField] private float speed = 15.0f;
    private float dir; 
    private float attackTime = 0.0f;
    private bool isMoving = false;
    private bool isAttack = false;
    [SerializeField] private Vector3 guillotioneVec;
    [SerializeField] GameObject warn;
    [SerializeField] GameObject guillotione;
    [SerializeField] private Animator warnAnimator;
    [SerializeField] private Animator guillotioneAnimator;
    [SerializeField] private SpriteRenderer guillotioneSpriteRenderer;
    [SerializeField] private SpriteRenderer bladeSpriteRenderer;
    private Player player;
    private BoxCollider2D boxCol;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (isMoving)
            MoveGuillotine();

        Attack_Guillotione();
    }

    private void Attack_Guillotione()
    {
        if (isAttack && !isMoving)
        {
            if (!isMoving)
            {
                boxCol.enabled = true;
                isMoving = true;
                attackTime = 0f;
            }
        }

        // ���̵� ������ 0�� �Ǹ� ��Ȱ��ȭ
        if (bladeSpriteRenderer.color.a == 0.0f)
        {
            gameObject.SetActive(false);
            guillotione.SetActive(false);
            guillotioneAnimator.SetBool("isReady", false);

            isMoving = false;
        }
    }

    private void MoveGuillotine()
    {
        attackTime += Time.deltaTime;

        // �ӵ� ���: 0~1�ʱ��� ���� ��������, ���� 1~2�� ���� Ȯ ������
        float t = attackTime / 2.0f;
        float speedMultiplier = -Mathf.Pow(t - 1, 2) + 1; // ������ ���·� ��ȭ (�ִ밪 1)
        float currentSpeed = speed * speedMultiplier;

        // �̵� ���� ����
        Vector3 moveDirection = (dir > 0.5f) ? Vector3.left : Vector3.right;

        transform.position += moveDirection * currentSpeed * Time.deltaTime;

        // �̵� ���� ���� (��: 2�� ��)
        if (attackTime > 1.0f)
        {
            boxCol.enabled = false;
            isMoving = false;
            isAttack = false;
            guillotioneAnimator.SetBool("isAttack", false);
        }
    }

    public void Ready()
    {
        transform.position = player.transform.position;
        guillotione.transform.localPosition = guillotioneVec;

        guillotioneSpriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
        bladeSpriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);

        warn.SetActive(true);

        StartCoroutine("Attack");
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(1.0f);
        warn.gameObject.SetActive(false);
        isAttack = true;
        guillotione.gameObject.SetActive(true);
        guillotioneAnimator.SetBool("isReady", true);
        //guillotioneAnimator.SetBool("isAttack", true);
    }

    private void OnEnable()
    {
        dir = Random.value;
        transform.position = player.transform.position;

        if (dir > 0.5f)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }

        Ready();
    }

    private void OnDisable()
    {
        guillotioneSpriteRenderer.color = new Color(guillotioneSpriteRenderer.color.r, guillotioneSpriteRenderer.color.g, guillotioneSpriteRenderer.color.b, 1f);
        bladeSpriteRenderer.color = new Color(bladeSpriteRenderer.color.r, bladeSpriteRenderer.color.g, bladeSpriteRenderer.color.b, 1f);

    }
}

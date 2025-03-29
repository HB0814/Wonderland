using UnityEngine;

public class HeartQueen : MonoBehaviour
{
    TakeDamage takeDamage;

    [SerializeField]
    GameObject player; //�÷��̾�

    Rigidbody2D rb; //������ٵ�2D
    SpriteUpdater spriteUpdater;

    [SerializeField]
    Bounds moveBounds; //�ٿ�� > ĳ������ ���� �̵��� ����ϱ� ����
    Vector2 targetPos; //��ǥ����
    float moveTime = 0.0f; //�̵� ��Ÿ��
    float moveTimer = 0.0f; //�̵� Ÿ�̸�

    int attackType; //��������

    float scaffoldCooltime; //�ܵδ� ��Ÿ��
    float soliderCooltime; //���� ��ȯ ��Ÿ��
    float gavelCooltime; //�ǻ�� ��Ÿ��
    float roseCooltime; //��� ��Ÿ��

    float attackTimer = 0.0f;
    float lastUpdateTime = 0.0f;
    bool canMove = true; //�̵����� ����

    public float maxHp; //�ִ�ü��
    public float hp; //����ü��
    public float addHp; //�߰�ü��
    public float damage; //������
    public float offense; //���ݷ�
    public float defense; //����
    public float speed; //�̵��ӵ�
    public float knockbackDefense; //�˹����׷�

    private void Awake()
    {
        takeDamage = GetComponent<TakeDamage>();
        rb = GetComponent<Rigidbody2D>();
        spriteUpdater = GetComponent<SpriteUpdater>();

        SetTargetPosition(); //��ǥ���� ����
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V)) //�ӽ÷� vŰ ���� �� >>> ���� ���� �÷� ����
            SetBoundsCenter(); //���Ͱ� ����

        moveTimer += Time.deltaTime; //�̵� Ÿ�̸� �� ����

        if(moveTimer >= moveTime) //�̵� ��Ÿ�� �Ϸ�
        {
            moveTimer = 0.0f; //�̵� Ÿ�̸� �� �ʱ�ȭ
            SetTargetPosition(); //Ÿ�� ��ġ ����
        }

        if (canMove) //�߰� �������� �񱳸� ���� �̵� ����
        {
            Move(); //�̵� �Լ� ȣ��
        }

        if (Time.time - lastUpdateTime < 0.1f) //0.1�ʸ��� ���̾ ���� �Լ��� �����ϰ�
            return;

        lastUpdateTime = Time.time;
        spriteUpdater.ChangeLayer(); //���̾ ����
    }

    //Bounds ���� �� ���� > �̵����� ���� �߽��� �÷��̾� ��ġ�� ���� / ���� ���� �� �ʰ� �̵� ���� �α�
    void SetBoundsCenter()
    {
        moveBounds.center = player.transform.position; //�ٿ����� ���Ͱ��� �÷��̾��� ��ġ������ ����
    }

    //���� ������ �̵� ��ġ ����
    void SetTargetPosition()
    {
        float x = Random.Range(moveBounds.min.x, moveBounds.max.x); //�ٿ����� x ���� ��
        float y = Random.Range(moveBounds.min.y, moveBounds.max.y); //�ٿ����� y ���� ��
        targetPos = new Vector2(x, y); //������ ��ġ ����

        //���� ����� �ٸ� ���� �ø��ϰ�
        bool isFlip = (transform.position.x < x) != spriteUpdater.spriteRenderer.flipX;
        if(isFlip)
            spriteUpdater.SpriteFlip(transform.position.x < x); //��������Ʈ ������ �ø�

        moveTime = Random.Range(2.0f, 4.0f); //2~4�� ���� �̵� ��Ÿ��
    }

    //�̵�
    void Move()
    {
        Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPos, speed * Time.fixedDeltaTime); //���� ��ġ���� ��ǥ ��ġ�� ���� �ӵ��� �̵�
        rb.MovePosition(newPosition); //������ٵ� �̵�

        if (Vector2.Distance(rb.position, targetPos) < 0.1f) //��ǥ���� ���� �� ���� ����
        {
            SetTargetPosition(); //���ο� ��ǥ���� ����
        }
    }

    //���� ����
    void Attack()
    {
        attackType = Random.Range(0, 4);

        canMove = false;

        switch (attackType)
        {
            case 0: //���ƾor�ܵδ�
                //�÷��̾� ��ġ�� �����ð� �� ������
                //ī�޶� �ȷο�� ����ϰ� �ص� ��������
                //�׸��ڷ� ���� ǥ�� > �ڽ� ������Ʈ: �׸���, ����
                //
                break;

            case 1: //���� ��ȯ
                //���� ���� x,y�ึ�� ���縦 3~5���� ��ȯ
                //�÷��̾� �������� ����, ���������� ����or�ѹ��� ����
                //�˹�x
                break;

            case 2: //�ǻ��
                //�ǻ�� ��ġ�� �÷��̾� ��ġ�� �̵� �� Ȱ��ȭ
                //�÷��̾ �̵��� �ϸ� ����� ���� �� �ִ� ���� ������
                //�ǻ�� �⺻ 3��, ��ȭ(����) �� 4~5�� ����
                //�˹�o
                break;

            case 3: //��� �� ��ȯ
                //��� �ɿ� �÷��̾� ���� �� ���� ������ ���
                //���� ������ ��� �� ����ȭ ����
                break;
        }
    }
}

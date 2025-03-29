using UnityEngine;

public class NormalEnemy : MonoBehaviour
{
    TakeDamage takeDamage;
    SpriteUpdater spriteUpdater;

    GameObject player; //�÷��̾�
    public float range; //�߰� ����
    
    float lastUpdateTime = 0; //������Ʈ �ֱ�
    bool isVisible = true; //ȭ�� �� ����
    float marginArea = 0.1f; //������ ��������

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
        spriteUpdater = GetComponent<SpriteUpdater>();
    }

    private void Update()
    {
        spriteUpdater.ChangeLayer(); //���̾ ����

        float dis = Vector2.SqrMagnitude(transform.position - player.transform.position); //�÷��̾���� �Ÿ� ���

        if (dis >= range) //�߰� �������� �񱳸� ���� �̵� ����
        {
            Move(); //�̵� �Լ� ȣ��
        }

        if (Time.time - lastUpdateTime < 0.1f) //0.1�ʸ��� ���̾ ���� �Լ��� �����ϰ�
            return;

        lastUpdateTime = Time.time; //������ ������Ʈ �ð� ����

        spriteUpdater.spriteRenderer.enabled = IsVisible(); //������ �Լ�
        //���� ����� �ٸ� ���� �ø��ϰ�
        bool isFlip = (player.transform.position.x < transform.position.x) != spriteUpdater.spriteRenderer.flipX;
        if (isFlip)
            spriteUpdater.SpriteFlip(player.transform.position.x < transform.position.x); //��������Ʈ ������ �ø�

    }

    //�̵�
    void Move()
    {
        Vector3 dir = (player.transform.position - transform.position).normalized; //��ֶ�����ȭ�� ���� ���� ���� ���ϱ�
        transform.position += dir * speed * Time.deltaTime; //�ش� �������� �̵�
    }

    //������ ���
    bool IsVisible()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position); //���� ��ǥ�� ����Ʈ�� ��ȯ�Ͽ� ���� �� ������Ʈ ��ġ Ȯ��
        bool newVisibility = screenPoint.x > -marginArea && screenPoint.x < 1 + marginArea &&
                             screenPoint.y > -marginArea && screenPoint.y < 1 + marginArea;
                             //ȭ�� ���� �ִ��� ���� Ȯ��

        if (isVisible != newVisibility) //ȭ�� �� ���� ���� Ȯ��
        {
            isVisible = newVisibility; //���� ������Ʈ
            spriteUpdater.spriteRenderer.enabled = isVisible; //������ ���� ����
        }

        return isVisible; //���� ��ȯ
    }

    //�÷��̾� �Ҵ�
    public void SetPlayer(GameObject _player)
    {
        player = _player;
    }

}

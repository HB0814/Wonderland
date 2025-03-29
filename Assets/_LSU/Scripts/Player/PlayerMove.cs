using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    SpriteUpdater spriteUpdater;
    
    public float speed;
    float lastX = 0.0f;
    float lastUpdateTime = 0.0f;

    private void Awake()
    {
        spriteUpdater = GetComponent<SpriteUpdater>();
    }

    private void Update()
    {
        Move(); //�̵�
        if (Time.time - lastUpdateTime < 0.1f) //0.1�ʸ��� ���̾ ���� �Լ��� �����ϰ�
            return;

        lastUpdateTime = Time.time;
        spriteUpdater.ChangeLayer(); //���̾ ����
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (x != 0 && x != lastX) //�Է� �ִ���, �Է��� ������ ���� �ٸ� �������� üũ
        {
            spriteUpdater.SpriteFlip(x < 0); //��������Ʈ ������ �ø�
        }

        Vector3 moveVelocity = new Vector3(x, y, 0).normalized; //��ֶ�����

        this.transform.position += moveVelocity * speed * Time.deltaTime;
        lastX = x;
    }
}

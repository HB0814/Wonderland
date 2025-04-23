using UnityEngine;

public class ChessEnemy : Enemy
{
    public enum ChessType //ü�� �� ����
    {
        Pawn, Rook, Bishop, //�Ϲ� ��
        Rook_Event_Move, Bishop_Event_Move, //�̵��ϴ� �̺�Ʈ�� ��
        Rook_Event_NoMove //������ �̺�Ʈ�� ��
    } 

    [Header("�̺�Ʈ�� ü�� ����")]
    public ChessType type; //ü�� �� ����
    public Vector2 moveDir; //ü�� ���� �̵��� ����
    public int dirNum; //�̵� ���� ��ȣ
    private bool hasDir = false; //������ ������������ ���� ����

    //Rook_Event_NoMove ���� ����
    private bool isSpawn = true; //���� ����
    private float lifeTime = 30.0f; //Rook_Event_NoMove�� Ȱ��ȭ �ð�
    private float timer = 0; //Ȱ��ȭ �ð� Ÿ�̸�

    private new void Update()
    {
        switch (type)
        {
            case ChessType.Pawn: //�� ��: �÷��̾� �߰��ϴ� �Ϲ��� ü����
                base.Update(); //����� Enemy�� Update �Լ� ȣ��
                break;

            case ChessType.Rook: //�� ��: �÷��̾� �߰��ϴ� �Ϲ��� ü����, ���� �߰��� ����?
                base.Update(); //����� Enemy�� Update �Լ� ȣ��
                break;

            case ChessType.Bishop: //�� ���: �÷��̾� �߰��ϴ� �Ϲ��� ü����, ���� �߰��� ����?
                base.Update(); //����� Enemy�� Update �Լ� ȣ��
                break;

            case ChessType.Rook_Event_Move: //���� ��: ���� ������θ� �̵��ϴ� �̺�Ʈ�� ü����
                if (!hasDir) //���� ���� ���� ���� ��
                {
                    moveDir = RookMove(); //���� �̵� ���� �Լ� ��� ��������
                    UpdateSpriteFlip(); //����� Enemy�� UpdateSpriteFlip �Լ� ȣ��
                }
                UpdateSpriteLayer(); //����� Enemy�� UpdateSpriteLayer �Լ� ȣ��
                rb.linearVelocity = moveDir * moveSpeed; //�̵� �������� �̵�
                break;

            case ChessType.Bishop_Event_Move: //���� ���: ������ ������θ� �̵��ϴ� �̺�Ʈ�� ü����
                if (!hasDir)
                {
                    moveDir = BishopMove(); //����� �̵� ���� �Լ� ��� ��������
                    UpdateSpriteFlip(); //����� Enemy�� UpdateSpriteFlip �Լ� ȣ��
                }
                UpdateSpriteLayer(); //����� Enemy�� UpdateSpriteLayer �Լ� ȣ��
                rb.linearVelocity = moveDir * moveSpeed; //�̵� �������� �̵�
                break;

            case ChessType.Rook_Event_NoMove: //���� ��: �̵��� ���� �ʰ�, Ÿ�������� �����Ǿ� �÷��̾ ���δ� ü����
                if(!isSpawn) //���� ���� üũ
                {
                    isSpawn= true; //���� ���� ��
                    UpdateSpriteFlip(); //����� Enemy�� UpdateSpriteFlip �Լ� ȣ��
                }
                base.Update();
                UpdateSpriteLayer(); //����� Enemy�� UpdateSpriteLayer �Լ� ȣ��
                timer += Time.deltaTime; //Ÿ�̸� �� ����
                if(timer >= lifeTime) //Ȱ��ȭ �ð� �̻� �޼�
                {
                    gameObject.SetActive(false); //���ӿ�����Ʈ ��Ȱ��ȭ
                }
                break;
        }
    }

    //���� �̵�����
    private Vector2 RookMove()
    {
        hasDir = true; //���� ���� ���� ��
        switch (dirNum)
        {
            case 0: 
                return Vector2.up; //��
            case 1: 
                return Vector2.down; //�Ʒ�
            case 2: 
                return Vector2.left; //����
            default: 
                return Vector2.right; //������
        }
    }

    //����� �̵�����
    private Vector2 BishopMove()
    {
        hasDir = true; //���� ���� ���� ��
        switch (dirNum)
        {
            case 0: 
                return new Vector2(1, 1).normalized; //������ ��
            case 1:
                return new Vector2(1, -1).normalized; //������ �Ʒ�
            case 2: 
                return new Vector2(-1, -1).normalized; //���� �Ʒ�
            default: 
                return new Vector2(-1, 1).normalized; //���� ��
        }
    }

    //��Ȱ��ȭ ��
    private void OnDisable()
    {
        hasDir = false; //���� ���� ���� ����
        timer = 0.0f; //Ÿ�̸� �ʱ�ȭ
    }
}

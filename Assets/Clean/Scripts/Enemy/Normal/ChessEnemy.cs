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

    //Rook_Event_NoMove ���� ����
    private bool isSpawn = true; //���� ����
    [SerializeField] float lifeTime = 30.0f; //�̺�Ʈ Ȱ��ȭ �ð�
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
                UpdateSpriteFlip(); //����� Enemy�� UpdateSpriteFlip �Լ� ȣ��
                UpdateSpriteLayer(); //����� Enemy�� UpdateSpriteLayer �Լ� ȣ��
                break;

            case ChessType.Bishop_Event_Move: //���� ���: ������ ������θ� �̵��ϴ� �̺�Ʈ�� ü����
                UpdateSpriteFlip(); //����� Enemy�� UpdateSpriteFlip �Լ� ȣ��
                UpdateSpriteLayer(); //����� Enemy�� UpdateSpriteLayer �Լ� ȣ��
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
}

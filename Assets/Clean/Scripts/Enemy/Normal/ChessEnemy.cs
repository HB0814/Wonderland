using Microsoft.Win32.SafeHandles;
using UnityEngine;

public class ChessEnemy : Enemy
{
    public enum ChessType { Rook_Black, Bishop_Black, Rook_White, Bishop_White, Pawn } //ü�� �� ����

    [Header("�̺�Ʈ�� ü��")]
    public ChessType type; //ü�� �� ����
    public Vector2 moveDir; //ü�� ���� �̵��� ����
    public int dirNum; //�̵� ���� ��ȣ
    private bool hasDir = false; //������ ������������ ���� ����

    private new void Update()
    {
        switch(type)
        {
            case ChessType.Rook_Black: //���� ��: ���� ������θ� �̵��ϴ� �̺�Ʈ�� ü����
                if(!hasDir)
                    moveDir = RookMove();

                UpdateSprite();
                rb.linearVelocity = moveDir * moveSpeed;
                break;

            case ChessType.Bishop_Black: //���� ���: ������ ������θ� �̵��ϴ� �̺�Ʈ�� ü����
                if (!hasDir)
                    moveDir = BishopMove();

                UpdateSprite();
                rb.linearVelocity = moveDir * moveSpeed;
                break;

            case ChessType.Rook_White: //�� ��: �÷��̾� �߰��ϴ� �Ϲ��� ü����, ���� �߰��� ����?
                base.Update();
                break;

            case ChessType.Bishop_White: //�� ���: �÷��̾� �߰��ϴ� �Ϲ��� ü����, ���� �߰��� ����?
                base.Update();
                break;


            case ChessType.Pawn: //�� ��: �÷��̾� �߰��ϴ� �Ϲ��� ü����
                base.Update();
                break;
        }
    }

    //���� �̵�����
    Vector2 RookMove()
    {
        hasDir = true;
        switch (dirNum)
        {
            case 0: 
                return Vector2.up;
            case 1: 
                return Vector2.down;
            case 2: 
                return Vector2.left;
            default: 
                return Vector2.right;
        }
    }

    //����� �̵�����
    Vector2 BishopMove()
    {
        hasDir = true;
        switch (dirNum)
        {
            case 0: 
                return new Vector2(1, 1).normalized;
            case 1:
                return new Vector2(1, -1).normalized;
            case 2: 
                return new Vector2(-1, -1).normalized;
            default: 
                return new Vector2(-1, 1).normalized;
        }
    }


    private void OnDisable()
    {
        hasDir = false;
    }
}

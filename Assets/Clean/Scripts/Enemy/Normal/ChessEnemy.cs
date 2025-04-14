using Microsoft.Win32.SafeHandles;
using UnityEngine;

public class ChessEnemy : Enemy
{
    public enum ChessType { Rook_Black, Bishop_Black, Rook_White, Bishop_White, Pawn } //체스 말 종류

    [Header("이벤트형 체스")]
    public ChessType type; //체스 말 종류
    public Vector2 moveDir; //체스 말이 이동할 방향
    public int dirNum; //이동 방향 번호
    private bool hasDir = false; //방향이 정해졌는지에 대한 여부

    private new void Update()
    {
        switch(type)
        {
            case ChessType.Rook_Black: //검정 룩: 십자 모양으로만 이동하는 이벤트형 체스말
                if(!hasDir)
                    moveDir = RookMove();

                UpdateSprite();
                rb.linearVelocity = moveDir * moveSpeed;
                break;

            case ChessType.Bishop_Black: //검정 비숍: 엑스자 모양으로만 이동하는 이벤트형 체스말
                if (!hasDir)
                    moveDir = BishopMove();

                UpdateSprite();
                rb.linearVelocity = moveDir * moveSpeed;
                break;

            case ChessType.Rook_White: //흰 룩: 플레이어 추격하는 일반형 체스말, 공격 추가할 수도?
                base.Update();
                break;

            case ChessType.Bishop_White: //흰 비숍: 플레이어 추격하는 일반형 체스말, 공격 추가할 수도?
                base.Update();
                break;


            case ChessType.Pawn: //흰 폰: 플레이어 추격하는 일반형 체스말
                base.Update();
                break;
        }
    }

    //룩의 이동방향
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

    //비숍의 이동방향
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

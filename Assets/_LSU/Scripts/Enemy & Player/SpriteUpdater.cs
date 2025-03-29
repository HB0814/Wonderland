using System.Xml.Serialization;
using UnityEngine;

public class SpriteUpdater : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; //스프라이트 렌더러
    //float lastPosY; //y 좌표 이동 비교 변수

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); //자식 오브젝트의 스프라이트 렌더러
    }

    //레이어 변경
    public void ChangeLayer()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100); //y값에 따른 레이어 값 변경
    }

    //스프라이트 플립
    public void SpriteFlip(bool flip)
    {
        if(flip)
            spriteRenderer.flipX = true; //플립 참
        else
            spriteRenderer.flipX = false; //플립 거짓
    }
}

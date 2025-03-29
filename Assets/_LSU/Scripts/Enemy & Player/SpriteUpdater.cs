using System.Xml.Serialization;
using UnityEngine;

public class SpriteUpdater : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; //��������Ʈ ������
    //float lastPosY; //y ��ǥ �̵� �� ����

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); //�ڽ� ������Ʈ�� ��������Ʈ ������
    }

    //���̾� ����
    public void ChangeLayer()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100); //y���� ���� ���̾� �� ����
    }

    //��������Ʈ �ø�
    public void SpriteFlip(bool flip)
    {
        if(flip)
            spriteRenderer.flipX = true; //�ø� ��
        else
            spriteRenderer.flipX = false; //�ø� ����
    }
}

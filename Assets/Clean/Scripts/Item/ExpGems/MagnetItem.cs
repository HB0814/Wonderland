using UnityEngine;

public class MagnetItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer; //��������Ʈ ������

    //�浹 ��
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) //�÷��̾� �浹 ��
        {
            AttractAllExpGems(); //����ġ �� �ڼ� ��� �Լ� ȣ��
            gameObject.SetActive(false); //�ڼ� ������ ��Ȱ��ȭ
        }
    }

    //����ġ �� �ڼ� ������
    private void AttractAllExpGems()
    {
        GameObject[] gems = GameObject.FindGameObjectsWithTag("ExpGem"); //��� ����ġ �� �迭�� �ֱ�

        //�ݺ���
        foreach (GameObject gem in gems)
        {
            ExpGem expGem = gem.GetComponent<ExpGem>(); //ExpGem ��ũ��Ʈ ����
            if (expGem != null)
            {
                expGem.StartAttraction(); //�ڼ� ��� Ȱ��ȭ
            }
        }
    }

    //���ӿ�����Ʈ Ȱ��ȭ ��
    private void OnEnable()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100); //Ȱ��ȭ �� ���̾� ���� ����
    }
}

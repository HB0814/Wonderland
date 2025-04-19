using UnityEngine;

public class MagnetItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AttractAllExpGems();
            gameObject.SetActive(false);
        }
    }

    private void AttractAllExpGems()
    {
        GameObject[] gems = GameObject.FindGameObjectsWithTag("ExpGem");

        foreach (GameObject gem in gems)
        {
            ExpGem expGem = gem.GetComponent<ExpGem>();
            if (expGem != null)
            {
                expGem.StartAttraction();
            }
        }
    }

    //Ȱ��ȭ ��
    private void OnEnable()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100); //Ȱ��ȭ �� ���̾� ���� ����
    }
}

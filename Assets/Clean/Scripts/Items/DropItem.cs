using UnityEngine;

public class DropItem : MonoBehaviour
{
    public GameObject magnet; //�ڼ� ������ -> ����ġ �� ��ܿ���
    public GameObject heal; //ȸ�� ������ -> �÷��̾� ü�� ȸ��
    public GameObject anvil; //��� ������ -> ���� ��ȭ

    float ran;

    private void OnDisable()
    {
        ran = Random.value;
        DropMagnet();
    }

    private void DropMagnet()
    {
        Instantiate(magnet, transform.position, Quaternion.identity);
    }

    private void DropHeal()
    {
        Instantiate(heal, transform.position, Quaternion.identity);
    }

    private void DropAnvil()
    {
        Instantiate(anvil, transform.position, Quaternion.identity);
    }
}

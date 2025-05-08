using UnityEngine;

public class DropItem : MonoBehaviour
{
    public GameObject magnet; //자석 아이템 -> 경험치 잼 당겨오기
    public GameObject heal; //회복 아이템 -> 플레이어 체력 회복
    public GameObject anvil; //모루 아이템 -> 무기 강화

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

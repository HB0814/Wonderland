using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float cameraSpeed = 5.0f;
    public GameObject player;

    private void FixedUpdate()
    {
        Vector3 dir = player.transform.position - transform.position; //ī�޶�� �÷��̾��� �Ÿ� ����Ͽ� ���� ���ϱ�
        Vector3 moveVec = new Vector3(dir.x * cameraSpeed * Time.deltaTime, dir.y * cameraSpeed * Time.deltaTime, 0.0f); //�ش� �������� �̵��ϴ� ���� ��
        transform.Translate(moveVec);
    }
}

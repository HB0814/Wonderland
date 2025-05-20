using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float cameraSpeed = 5.0f;
    public GameObject player;

    private void FixedUpdate()
    {
        Vector3 dir = player.transform.position - transform.position; //카메라와 플레이어의 거리 계산하여 방향 구하기
        Vector3 moveVec = new Vector3(dir.x * cameraSpeed * Time.deltaTime, dir.y * cameraSpeed * Time.deltaTime, 0.0f); //해당 방향으로 이동하는 벡터 값
        transform.Translate(moveVec);
    }
}

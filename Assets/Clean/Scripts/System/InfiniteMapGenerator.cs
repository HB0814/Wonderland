using UnityEngine;

public class InfiniteMapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] mapPositions = new GameObject[4]; // 4개의 맵 위치
    private Vector3 cameraPos;
    private float cameraSizeX;
    private float cameraSizeY;
    private Camera mainCamera;

    // 맵 위치 인덱스 (시계방향)
    // 0: 좌상, 1: 우상, 2: 좌하, 3: 우하
    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
            return;
        }
        cameraPos = mainCamera.transform.position;
        cameraSizeX = mainCamera.orthographicSize * 2 * mainCamera.aspect;
        cameraSizeY = mainCamera.orthographicSize * 2;
    }

    private void Update()
    {
        if (mainCamera == null) return;
        
        cameraPos = mainCamera.transform.position;
        UpdateMapPositions();
    }

    private void UpdateMapPositions()
    {
        // 오른쪽으로 이동시
        if (cameraPos.x > mapPositions[1].transform.position.x)
        {
            // 왼쪽 맵들을 오른쪽으로 이동
            mapPositions[0].transform.position = new Vector3(
                mapPositions[1].transform.position.x + cameraSizeX,
                mapPositions[0].transform.position.y,
                mapPositions[0].transform.position.z
            );
            
            mapPositions[2].transform.position = new Vector3(
                mapPositions[3].transform.position.x + cameraSizeX,
                mapPositions[2].transform.position.y,
                mapPositions[2].transform.position.z
            );

            GameObject temp1 = mapPositions[1];
            GameObject temp3 = mapPositions[3];
            
            mapPositions[1]=mapPositions[0];
            mapPositions[3]=mapPositions[2];

            mapPositions[0]=temp1;
            mapPositions[2]=temp3;

        }
        // 왼쪽으로 이동시
        if (cameraPos.x < mapPositions[0].transform.position.x)
        {
            // 오른쪽 맵들을 왼쪽으로 이동
            mapPositions[1].transform.position = new Vector3(
                mapPositions[0].transform.position.x - cameraSizeX,
                mapPositions[1].transform.position.y,
                mapPositions[1].transform.position.z
            );

            mapPositions[3].transform.position = new Vector3(
                mapPositions[2].transform.position.x - cameraSizeX,
                mapPositions[3].transform.position.y,
                mapPositions[3].transform.position.z
            );

            GameObject temp0 = mapPositions[0];
            GameObject temp2 = mapPositions[2];

            mapPositions[0]=mapPositions[1];
            mapPositions[2]=mapPositions[3];

            mapPositions[1]=temp0;
            mapPositions[3]=temp2;
        }
        // 위쪽으로 이동시
        if (cameraPos.y > mapPositions[1].transform.position.y)
        {
            // 아래쪽 맵들을 위쪽으로 이동
            mapPositions[2].transform.position = new Vector3(
                mapPositions[2].transform.position.x,
                mapPositions[1].transform.position.y + cameraSizeY,
                mapPositions[2].transform.position.z
            );

            mapPositions[3].transform.position = new Vector3(
                mapPositions[3].transform.position.x,
                mapPositions[1].transform.position.y + cameraSizeY,
                mapPositions[3].transform.position.z
            );

            GameObject temp2 = mapPositions[0];
            GameObject temp3 = mapPositions[1];

            mapPositions[0]=mapPositions[2];
            mapPositions[1]=mapPositions[3];

            mapPositions[2]=temp2;
            mapPositions[3]=temp3;
        }
        // 아래쪽으로 이동시
        if (cameraPos.y < mapPositions[3].transform.position.y)
        {
            // 위쪽 맵들을 아래쪽으로 이동
            mapPositions[0].transform.position = new Vector3(
                mapPositions[0].transform.position.x,
                mapPositions[2].transform.position.y - cameraSizeY,
                mapPositions[0].transform.position.z
            );

            mapPositions[1].transform.position = new Vector3(
                mapPositions[1].transform.position.x,
                mapPositions[2].transform.position.y - cameraSizeY,
                mapPositions[1].transform.position.z
            );

            GameObject temp2 = mapPositions[2];
            GameObject temp3 = mapPositions[3];
            
            mapPositions[2]=mapPositions[0];
            mapPositions[3]=mapPositions[1];

            mapPositions[0]=temp2;
            mapPositions[1]=temp3;
        }
    }
} 
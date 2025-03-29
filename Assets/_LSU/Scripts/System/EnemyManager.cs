using System;
using UnityEngine;
using UnityEngine.VFX;

using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public int type;

    public EnemyPool[] enemyPool;

    public float enemySpawnTime;
    float timer;

    int cardIndex = 0;
    int pawnIndex = 0;
    int rookIndex = 0;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= enemySpawnTime)
        {
            timer = 0;

            switch(type)
            {
                case 0:
                    SetRandomPos(enemyPool[type].normalEnemyObjects[cardIndex]);
                    enemyPool[type].normalEnemyObjects[cardIndex].SetActive(true);
                    cardIndex++;

                    if (cardIndex >= enemyPool[type].normalEnemyObjects.Length)
                    {
                        cardIndex = 0;
                    }
                    break;

                case 1:
                    SetRandomPos(enemyPool[type].normalEnemyObjects[pawnIndex]);
                    enemyPool[type].normalEnemyObjects[pawnIndex].SetActive(true);
                    pawnIndex++;

                    if (pawnIndex >= enemyPool[type].normalEnemyObjects.Length)
                    {
                        pawnIndex = 0;
                    }
                    break;

                case 2:
                    SetRandomPos(enemyPool[type].normalEnemyObjects[rookIndex]);
                    enemyPool[type].normalEnemyObjects[rookIndex].SetActive(true);
                    rookIndex++;

                    if (rookIndex >= enemyPool[type].normalEnemyObjects.Length)
                    {
                        rookIndex = 0;
                    }
                    break;
            }
        }
    }

    public void SetRandomPos(GameObject enemy)
    {
        Vector3 randomPos = Vector3.zero; //랜덤 위치
        float min = -0.2f; //화면을 벗어나는 최소 범위
        float max = 1.2f; //화면을 벗어나는 최대 범위
        float zPos = 10; //3D Z축 위치 고정

        int flag = Random.Range(0, 4); //랜덤 방향

        switch (flag)
        {
            case 0: //오른쪽 바깥
                randomPos = new Vector3(max, Random.Range(min, max), zPos);
                break;
            case 1: //왼쪽 바깥
                randomPos = new Vector3(min, Random.Range(min, max), zPos);
                break;
            case 2: //위쪽 바깥
                randomPos = new Vector3(Random.Range(min, max), max, zPos);
                break;
            case 3: //아래쪽 바깥
                randomPos = new Vector3(Random.Range(min, max), min, zPos);
                break;
        }
        //뷰포트 좌표를 월드좌표로 변환. 뷰포트 좌표계 기준으로 (0,0)은 좌측 하단, (1,1)은 우측 상단.
        randomPos = Camera.main.ViewportToWorldPoint(randomPos);
        enemy.transform.position = randomPos; //몬스터 랜덤 위치 적용
    }
}

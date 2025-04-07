using System;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class EnemyManager : Enemy
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
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemyToSpawn = null;
        int index = 0;

        switch(type)
        {
            case 0:
                enemyToSpawn = enemyPool[type].normalEnemyObjects[cardIndex];
                index = cardIndex;
                cardIndex = (cardIndex + 1) % enemyPool[type].normalEnemyObjects.Length;
                break;

            case 1:
                enemyToSpawn = enemyPool[type].normalEnemyObjects[pawnIndex];
                index = pawnIndex;
                pawnIndex = (pawnIndex + 1) % enemyPool[type].normalEnemyObjects.Length;
                break;

            case 2:
                enemyToSpawn = enemyPool[type].normalEnemyObjects[rookIndex];
                index = rookIndex;
                rookIndex = (rookIndex + 1) % enemyPool[type].normalEnemyObjects.Length;
                break;
        }

        if (enemyToSpawn != null)
        {
            SetRandomPos(enemyToSpawn);
            enemyToSpawn.SetActive(true);
            
            // 적 초기화
            Enemy enemy = enemyToSpawn.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Initialize();
            }
        }
    }

    public void SetRandomPos(GameObject enemy)
    {
        Vector3 randomPos = Vector3.zero;
        float min = -0.2f; //화면에서 벗어나는 최소 거리
        float max = 1.2f; //화면에서 벗어나는 최대 거리
        float zPos = 10; //3D Z축 위치 설정

        int flag = Random.Range(0, 4); //방향 선택

        switch (flag)
        {
            case 0: //오른쪽 벽
                randomPos = new Vector3(max, Random.Range(min, max), zPos);
                break;
            case 1: //왼쪽 벽
                randomPos = new Vector3(min, Random.Range(min, max), zPos);
                break;
            case 2: //위쪽 벽
                randomPos = new Vector3(Random.Range(min, max), max, zPos);
                break;
            case 3: //아래쪽 벽
                randomPos = new Vector3(Random.Range(min, max), min, zPos);
                break;
        }

        //월드 좌표로 변환. 월드 좌표는 카메라 기준으로 (0,0)이 왼쪽 아래, (1,1)이 오른쪽 위
        randomPos = Camera.main.ViewportToWorldPoint(randomPos);
        enemy.transform.position = randomPos;
    }
}

using System;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public enum EnemyType {HeartCardSolider, ChessPawn, ChessRook, Teapot}
    public EnemyType enemyType;
    public float enemySpawnTime;
    float timer;
    //enemyType과 enemySpawnTime은 csv 파일 혹은 스크립터블을 사용하여 관리하면 될듯?

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= enemySpawnTime)
        {
            timer = 0;
            Debug.Log("스폰 시도");
            SpawnEnemy();
        }

        if (Input.GetKeyDown(KeyCode.U))
            enemyType = EnemyType.HeartCardSolider;

        if (Input.GetKeyDown(KeyCode.I))
            enemyType = EnemyType.ChessPawn;

        if (Input.GetKeyDown(KeyCode.O))
            enemyType = EnemyType.ChessRook;

        if (Input.GetKeyDown(KeyCode.P))
            enemyType = EnemyType.Teapot;
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPos = GetRandomSpawnPosition();

        GameObject enemyToSpawn = ObjectPool.Instance.SpawnFromPool_Enemy(enemyType.ToString(), spawnPos);

        if (enemyToSpawn != null)
        {
            // 초기화 및 활성화
            enemyToSpawn.SetActive(true);
            Debug.Log("몬스터 활성화");
        }
    }

    public Vector3 GetRandomSpawnPosition()
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

        Debug.Log("랜덤 좌표 실행");
        return Camera.main.ViewportToWorldPoint(randomPos);
    }
}

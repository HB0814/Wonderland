using System;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public enum EnemyType //적타입
    {
        HeartCard, DiamondCard, SpadeCard, CloverCard,
        Pawn, Rook, Bishop, Rook_Event, Bishop_Event,
        Teapot, CheshireCat
    }
    public EnemyType firstEnemyType; //첫번째 적 타입
    public EnemyType secondEnemyType; //두번째 적 타입

    public bool onFirstEnemySpawn = true; //첫번째 적 스폰 여부
    public bool onSecondEnemySpawn = true; //두번째 적 스폰 여부

    public float firstSpawnCool; //첫번째 적 스폰 시간
    public float secondSpawnCool; //두번째 적 스폰 시간

    private float firstTimer; //첫번째 적 타이머
    private float secondTimer; //두번째 적 타이머

    public int firstSpawnNum; //첫번째 적 소환 가능한 수
    public int secondSpawnNum; //두번째 적 소환 가능한 수 

    public int firstSpawnCount = 0; //몬스터 소환 카운트
    public int secondSpawnCount = 0; //몬스터 소환 카운트

    //enemyType과 enemySpawnTime은 csv 파일 혹은 스크립터블을 사용하여 관리

    private void Update()
    {
        SpawnCheck();

        //if (Input.GetKeyDown(KeyCode.Keypad0))
        //     firstEnemyType = EnemyType.HeartCard;
    }

    private void SpawnCheck()
    {
        firstTimer += Time.deltaTime;
        secondTimer += Time.deltaTime;

        if (firstTimer >= firstSpawnCool && onFirstEnemySpawn && firstSpawnCount < firstSpawnNum)
        {
            firstTimer = 0;
            SpawnEnemy(firstEnemyType);
            firstSpawnCount++;
        }

        if (secondTimer >= secondSpawnCool && onSecondEnemySpawn && secondSpawnCount < secondSpawnNum)
        {
            secondTimer = 0;
            SpawnEnemy(secondEnemyType);
            secondSpawnCount++;
        }
    }

    private void SpawnEnemy(EnemyType enemyType)
    {
        Vector3 spawnPos = GetRandomSpawnPosition();

        GameObject enemyToSpawn = ObjectPool.Instance.SpawnFromPool_Enemy(enemyType.ToString(), spawnPos);

        if (enemyToSpawn != null)
        {
            // 초기화 및 활성화
            enemyToSpawn.SetActive(true);
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
        return Camera.main.ViewportToWorldPoint(randomPos);
    }
}

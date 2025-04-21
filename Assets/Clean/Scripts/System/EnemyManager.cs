using System;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Collections;
using static EnemyManager;

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

    [SerializeField] private List<WaveData> waveList;
    [SerializeField] private float timer = 0f;
    private int currentWaveIndex = 0;

    Player player;
    Transform target;
    
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        target = player.transform;
        waveList = CSVLoader.LoadWaveData("WaveData");
    }

    private void Update()
    {
        timer += Time.deltaTime;

        while (currentWaveIndex < waveList.Count && timer >= waveList[currentWaveIndex].startTime)
        {
            StartCoroutine(SpawnWave(waveList[currentWaveIndex]));
            currentWaveIndex++;
        }

        //if (Input.GetKeyDown(KeyCode.Keypad0))
        //     firstEnemyType = EnemyType.HeartCard;

        //if(Input.GetKeyDown(KeyCode.Keypad0))
        //{
        //    EllipseSpawn(100, 14f, 9f, "Rook_Event_NoMove");
        //}
    }

    private IEnumerator SpawnWave(WaveData wave)
    {
        foreach (var subWave in wave.subWaves)
        {
            StartCoroutine(SpawnSubWave(subWave));
        }
        yield return null;
    }

    private IEnumerator SpawnSubWave(SubWaveData subWave)
    {
        for (int i = 0; i < subWave.spawnCount; i++)
        {
            SpawnEnemy(subWave.enemyType); // 실제 스폰
            yield return new WaitForSeconds(subWave.spawnInterval);
        }
    }

    /*
    //첫번째, 두번째 적 스폰
    //private void SpawnCheck()
    //{
    //    firstTimer += Time.deltaTime;
    //    secondTimer += Time.deltaTime;

    //    if (firstTimer >= firstSpawnCool && onFirstEnemySpawn )//&& firstSpawnCount < firstSpawnNum)
    //    {
    //        firstTimer = 0;
    //        SpawnEnemy(firstEnemyType.ToString());
    //        firstSpawnCount++;
    //    }

    //    if (secondTimer >= secondSpawnCool && onSecondEnemySpawn)// && secondSpawnCount < secondSpawnNum)
    //    {
    //        secondTimer = 0;
    //        SpawnEnemy(secondEnemyType.ToString());
    //        secondSpawnCount++;
    //    }
    //}
    */

    //EnemyType enemyType
    private void SpawnEnemy(string enemyType)
    {
        if (enemyType == "Rook_Event_NoMove")
        {
            EllipseSpawn(100, 14f, 9f, enemyType);
            return;
        }

        Vector3 spawnPos = GetRandomSpawnPosition();

        GameObject enemyToSpawn = ObjectPool.Instance.SpawnFromPool_Enemy(enemyType, spawnPos);

        if (enemyToSpawn != null)
        {
            // 초기화 및 활성화
            enemyToSpawn.SetActive(true);
        }
    }

    //일반 몬스터 스폰 -> 화면 밖에서 스폰
    private Vector3 GetRandomSpawnPosition()
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

    //타원 모양으로 몬스터 스폰 -> 플레이어를 이동 제한
    private void EllipseSpawn(int count, float a, float b, string enemyType) //몬스터 수, +- x값, +- y갑, 몬스터 타입
    {
        Vector3 center = target.transform.position;

        for (int i = 0; i < count; i++)
        {
            float angle = (float)i / (float)count * 2 * Mathf.PI; //균등 분할
            float x = a * Mathf.Cos(angle);
            float y = b * Mathf.Sin(angle);

            Vector3 spawnPos = center + new Vector3(x, y, 0f);
            GameObject enemyToSpawn = ObjectPool.Instance.SpawnFromPool_Enemy(enemyType, spawnPos);
        }

    }
}

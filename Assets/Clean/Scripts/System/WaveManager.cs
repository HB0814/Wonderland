using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManagerCSV : MonoBehaviour
{
    private List<WaveData> waveList;
    private int currentWaveIndex = 0;
    private float elapsedTime = 0f;

    //void Start()
    //{
    //    waveList = CSVLoader.LoadWaveData("WaveData");
    //}

    //void Update()
    //{
    //    elapsedTime += Time.deltaTime;

    //    if (currentWaveIndex < waveList.Count &&
    //        elapsedTime >= waveList[currentWaveIndex].startTime)
    //    {
    //        StartCoroutine(SpawnWave(waveList[currentWaveIndex]));
    //        currentWaveIndex++;
    //    }
    //}

    //IEnumerator SpawnWave(WaveData wave)
    //{
    //    GameObject prefab = Resources.Load<GameObject>(wave.prefabName);
    //    for (int i = 0; i < wave.spawnCount; i++)
    //    {
    //        Vector3 spawnPos = GetRandomSpawnPosition();
    //        Instantiate(prefab, spawnPos, Quaternion.identity);
    //        yield return new WaitForSeconds(wave.spawnInterval);
    //    }
    //}

    //Vector3 GetRandomSpawnPosition()
    //{
    //    Vector2 dir = Random.insideUnitCircle.normalized;
    //    return Player.Instance.transform.position + (Vector3)(dir * 10f);
    //}
}
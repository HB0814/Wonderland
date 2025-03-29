using UnityEngine;

public class GemCreater : MonoBehaviour
{
    public GameObject gemPrefab;
    public GameObject[] enemyPrefab;

    public Transform[] gemSpawnPoints;
    public Transform[] enemySpawnPoints;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateGems();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CreateEnemies();
        }
    }

    private void CreateGems()
    {
        foreach (var spawnPoint in gemSpawnPoints)
        {
            Instantiate(gemPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    private void CreateEnemies()
    {
        foreach (var spawnPoint in enemySpawnPoints)
        {
            Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], spawnPoint.position, Quaternion.identity);
        }
    }
}

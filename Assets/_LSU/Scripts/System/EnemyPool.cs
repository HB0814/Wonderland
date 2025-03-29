using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    GameObject player;

    public GameObject normalEnemyGroup; //부모로 할 게임오브젝트
    public GameObject normalEnemyPrefab; //프리팹
    public GameObject[] normalEnemyObjects; //풀링된 오브젝트
    public NormalEnemy[] normalEnemyScript; //풀링된 오브젝트의 무기 스크립트

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        for (int i = 0; i < normalEnemyObjects.Length; i++)
        {
            GameObject normalObj = Instantiate(normalEnemyPrefab);
            normalObj.transform.parent = normalEnemyGroup.transform;
            normalEnemyObjects[i] = normalObj;
            normalEnemyScript[i] = normalEnemyObjects[i].GetComponent<NormalEnemy>();
            normalEnemyScript[i].SetPlayer(player);
            normalObj.SetActive(false);
        }
    }
}

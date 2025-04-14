using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    GameObject player;

    public GameObject normalEnemyGroup; //부모가 되는 빈 게임오브젝트
    public GameObject normalEnemyPrefab; //일반적
    public GameObject[] normalEnemyObjects; //풀링할 오브젝트
    public Enemy[] normalEnemyScript; //풀링할 오브젝트의 스크립트

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        for (int i = 0; i < normalEnemyObjects.Length; i++)
        {
            GameObject normalObj = Instantiate(normalEnemyPrefab);
            normalObj.transform.parent = normalEnemyGroup.transform;
            normalEnemyObjects[i] = normalObj;
            normalEnemyScript[i] = normalEnemyObjects[i].GetComponent<Enemy>();
            normalObj.SetActive(false);
        }
    }
}

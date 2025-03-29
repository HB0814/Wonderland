using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    GameObject player;

    public GameObject normalEnemyGroup; //�θ�� �� ���ӿ�����Ʈ
    public GameObject normalEnemyPrefab; //������
    public GameObject[] normalEnemyObjects; //Ǯ���� ������Ʈ
    public NormalEnemy[] normalEnemyScript; //Ǯ���� ������Ʈ�� ���� ��ũ��Ʈ

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

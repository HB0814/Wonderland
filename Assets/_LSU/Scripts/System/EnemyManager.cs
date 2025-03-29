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
        Vector3 randomPos = Vector3.zero; //���� ��ġ
        float min = -0.2f; //ȭ���� ����� �ּ� ����
        float max = 1.2f; //ȭ���� ����� �ִ� ����
        float zPos = 10; //3D Z�� ��ġ ����

        int flag = Random.Range(0, 4); //���� ����

        switch (flag)
        {
            case 0: //������ �ٱ�
                randomPos = new Vector3(max, Random.Range(min, max), zPos);
                break;
            case 1: //���� �ٱ�
                randomPos = new Vector3(min, Random.Range(min, max), zPos);
                break;
            case 2: //���� �ٱ�
                randomPos = new Vector3(Random.Range(min, max), max, zPos);
                break;
            case 3: //�Ʒ��� �ٱ�
                randomPos = new Vector3(Random.Range(min, max), min, zPos);
                break;
        }
        //����Ʈ ��ǥ�� ������ǥ�� ��ȯ. ����Ʈ ��ǥ�� �������� (0,0)�� ���� �ϴ�, (1,1)�� ���� ���.
        randomPos = Camera.main.ViewportToWorldPoint(randomPos);
        enemy.transform.position = randomPos; //���� ���� ��ġ ����
    }
}

using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public WeaponPool weaponPool; //������Ʈ Ǯ�� ��ũ��Ʈ

    //Ʈ����
    [Header("Trump")]
    public int trumpIndex = 0; //Ʈ��Ʈ �迭 �ε��� ��
    public int trumpCount = 1; //Ʈ���� ����
    public float trumpCooldown; //Ʈ���� ��Ÿ��
    public float trumpTimer = 0.0f; //Ʈ���� ��Ÿ�� Ÿ�̸�
    public bool hasTrump = true; //Ʈ���� ���� �ر� ����
    public bool isTrump = false; //Ʈ���� ���� ����
    public float randomRot = 0.0f; //Ʈ���� ȸ�� ����

    //ä��Ĺ
    [Header("CheshireCat")]
    public int cheshireCatIndex = 0; //ä��Ĺ �迭 �ε��� ��
    public int cheshireCatCount = 1; //ä��Ĺ ����
    public float cheshireCatCooldown; //ä��Ĺ ��Ÿ��
    public float cheshireCatTimer = 0.0f; //ä��Ĺ ��Ÿ�� Ÿ�̸�
    public bool hasCheshireCat = true; //ä��Ĺ ���� �ر� ����
    public bool isCheshireCat = false; //ä��Ĺ ���� ����

    //�Ȼ��� ���� ����
    [Header("NonBrithdayFirecracker")]
    public int firecrackerIndex = 0; //ä��Ĺ �迭 �ε��� ��
    public int firecrackerCount = 1; //ä��Ĺ ����
    public float firecrackerCooldown; //ä��Ĺ ��Ÿ��
    public float firecrackerTimer = 0.0f; //ä��Ĺ ��Ÿ�� Ÿ�̸�
    public bool hasFirecracker = true; //ä��Ĺ ���� �ر� ����
    public bool isFirecracker = false; //ä��Ĺ ���� ����

    //���
    [Header("Apple")]
    public int appleIndex = 0; //ä��Ĺ �迭 �ε��� ��
    public int appleCount = 1; //ä��Ĺ ����
    public float appleCooldown; //ä��Ĺ ��Ÿ��
    public float appleTimer = 0.0f; //ä��Ĺ ��Ÿ�� Ÿ�̸�
    public bool hasApple = true; //ä��Ĺ ���� �ر� ����
    public bool isApple = false; //ä��Ĺ ���� ����

    private void Update()
    {
        //Ʈ���� ����
        if (hasTrump) //Ʈ���� ���� ���� ���� üũ
        {
            trumpTimer += Time.deltaTime; //Ʈ���� ��Ÿ�� Ÿ�̸� �ð� ����

            if (trumpTimer >= trumpCooldown) //��Ÿ�� ����
            {
                trumpTimer = 0.0f; //��Ÿ�� �ʱ�ȭ
                for (int i = 0; i < trumpCount; i++) //Ʈ���� ���� üũ
                {
                    weaponPool.trumpCardScript[trumpIndex].SetRotation(); //�ε��� ���� Ʈ���� ī�� ȸ�� �Լ� ����
                    weaponPool.trumpCardScript[trumpIndex].transform.position = transform.position;

                    weaponPool.trumpObjects[trumpIndex].SetActive(true); //�ε��� ���� Ʈ���� ī�� Ȱ��ȭ

                    trumpIndex++; //Ʈ���� �ε��� �� ����

                    if (trumpIndex >= weaponPool.trumpObjects.Length) //Ʈ���� �ε��� �� ���� �ʰ� üũ
                        trumpIndex = 0; //Ʈ���� �ε��� �� �ʱ�ȭ
                }
            }
        }

        //ä��Ĺ ����
        if (hasCheshireCat) //ä��Ĺ ���� ���� ���� üũ
        {
            cheshireCatTimer += Time.deltaTime; //ä��Ĺ ��Ÿ�� Ÿ�̸� �ð� ����

            if (cheshireCatTimer >= cheshireCatCooldown) //��Ÿ�� ����
            {
                cheshireCatTimer = 0.0f; //��Ÿ�� �ʱ�ȭ
                for (int i = 0; i < cheshireCatCount; i++) //ä��Ĺ ���� üũ
                {
                    weaponPool.cheshireCatScript[cheshireCatIndex].SetRotation(); //ä��Ĺ ȸ��
                    weaponPool.cheshireCatScript[cheshireCatIndex].SetAttackPos(); //ä��Ĺ ���� ��ġ ����
                    weaponPool.cheshireCatObjects[cheshireCatIndex].SetActive(true); //�ε��� ���� ä��Ĺ Ȱ��ȭ
                    
                    cheshireCatIndex++; //ä��Ĺ �ε��� �� ����

                    if (cheshireCatIndex >= weaponPool.cheshireCatObjects.Length) //ä��Ĺ �ε��� �� ���� �ʰ� üũ
                        cheshireCatIndex = 0; //ä��Ĺ �ε��� �� �ʱ�ȭ
                }
            }
        }

        //�Ȼ��� ���� ���� ����
        if (hasFirecracker) //���� ���� ���� ���� üũ
        {
            firecrackerTimer += Time.deltaTime; //���� ��Ÿ�� Ÿ�̸� �ð� ����

            if (firecrackerTimer >= firecrackerCooldown) //��Ÿ�� ����
            {
                firecrackerTimer = 0.0f; //��Ÿ�� �ʱ�ȭ
                for (int i = 0; i < firecrackerCount; i++) //���� ���� üũ
                {
                    weaponPool.firecrackerScript[firecrackerIndex].SetAttackPos(); //���� ���� ��ġ
                    weaponPool.firecrackerObjects[firecrackerIndex].SetActive(true); //�ε��� ���� ���� Ȱ��ȭ

                    firecrackerIndex++; //���� �ε��� �� ����

                    if (firecrackerIndex >= weaponPool.firecrackerObjects.Length) //�����ε��� �� ���� �ʰ� üũ
                        firecrackerIndex = 0; //���� �ε��� �� �ʱ�ȭ
                }
            }
        }

        //��� ����
        if (hasApple) //��� ���� ���� ���� üũ
        {
            appleTimer += Time.deltaTime; //��� ��Ÿ�� Ÿ�̸� �ð� ����

            if (appleTimer >= appleCooldown) //��Ÿ�� ����
            {
                appleTimer = 0.0f; //��Ÿ�� �ʱ�ȭ
                for (int i = 0; i < appleCount; i++) //��� ���� üũ
                {
                    weaponPool.appleScript[appleIndex].SetRotation(); //��� ȸ��
                    weaponPool.appleScript[appleIndex].SetRandomPos(); //��� ���� ��ġ ���� �Լ�
                    weaponPool.appleScript[appleIndex].SetAttackDir(); //����� ���ݹ��� ���� �Լ�
                    weaponPool.appleObjects[appleIndex].SetActive(true); //�ε��� ���� ��� Ȱ��ȭ

                    appleIndex++; //��� �ε��� �� ����

                    if (appleIndex >= weaponPool.appleObjects.Length) //��� �ε��� �� ���� �ʰ� üũ
                        appleIndex = 0; //��� �ε��� �� �ʱ�ȭ
                }
            }
        }
    }
}

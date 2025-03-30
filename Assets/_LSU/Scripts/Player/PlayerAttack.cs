using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public WeaponPool weaponPool; //������Ʈ Ǯ�� ��ũ��Ʈ

    [System.Serializable]
    public class Info
    {
        public int index = 0; //�迭 �ε��� ��
        public int count = 1; //���� ����
        public float cooldown; //��Ÿ��
        public float timer = 0.0f; //��Ÿ�� Ÿ�̸�
        public bool hasWeapon= true; // ���� �ر� ����
        public bool isAttcak = false; //���� ����
        public float randomRot = 0.0f; //���� ȸ�� ����
    }

    public Info trump;
    public Info cheshireCat;
    public Info firecracker;
    public Info apple;

    private void Update()
    {
        Attack_TrumpCard(); //Ʈ���� ����
        Attack_CheshireCat(); //ä��Ĺ ����
        Attack_Firecracker(); //�Ȼ��� ���� ���� ����
        Attack_Apple(); //��� ����
    }

    //Ʈ���� ī�� ����
    void Attack_TrumpCard()
    {
        //Ʈ���� ����
        if (trump.hasWeapon) //Ʈ���� ���� ���� ���� üũ
        {
            trump.timer += Time.deltaTime; //Ʈ���� ��Ÿ�� Ÿ�̸� �ð� ����

            if (trump.timer >= trump.cooldown) //��Ÿ�� ����
            {
                trump.timer = 0.0f; //��Ÿ�� �ʱ�ȭ
                for (int i = 0; i < trump.count; i++) //Ʈ���� ���� üũ
                {
                    weaponPool.trumpCardScript[trump.index].SetRotation(); //�ε��� ���� Ʈ���� ī�� ȸ�� �Լ� ����
                    weaponPool.trumpCardScript[trump.index].transform.position = transform.position;

                    weaponPool.trumpPool.objects[trump.index].SetActive(true); //�ε��� ���� Ʈ���� ī�� Ȱ��ȭ

                    trump.index++; //Ʈ���� �ε��� �� ����

                    if (trump.index >= weaponPool.trumpPool.objects.Length) //Ʈ���� �ε��� �� ���� �ʰ� üũ
                        trump.index = 0; //Ʈ���� �ε��� �� �ʱ�ȭ
                }
            }
        }
    }

    //ä��Ĺ ����
    void Attack_CheshireCat()
    {
        //ä��Ĺ ����
        if (cheshireCat.hasWeapon) //ä��Ĺ ���� ���� ���� üũ
        {
            cheshireCat.timer += Time.deltaTime; //ä��Ĺ ��Ÿ�� Ÿ�̸� �ð� ����

            if (cheshireCat.timer >= cheshireCat.cooldown) //��Ÿ�� ����
            {
                cheshireCat.timer = 0.0f; //��Ÿ�� �ʱ�ȭ
                for (int i = 0; i < cheshireCat.count; i++) //ä��Ĺ ���� üũ
                {
                    weaponPool.cheshireCatScript[cheshireCat.index].SetRotation(); //ä��Ĺ ȸ��
                    weaponPool.cheshireCatScript[cheshireCat.index].SetAttackPos(); //ä��Ĺ ���� ��ġ ����
                    weaponPool.cheshireCatPool.objects[cheshireCat.index].SetActive(true); //�ε��� ���� ä��Ĺ Ȱ��ȭ

                    cheshireCat.index++; //ä��Ĺ �ε��� �� ����

                    if (cheshireCat.index >= weaponPool.cheshireCatPool.objects.Length) //ä��Ĺ �ε��� �� ���� �ʰ� üũ
                        cheshireCat.index = 0; //ä��Ĺ �ε��� �� �ʱ�ȭ
                }
            }
        }
    }

    //�Ȼ��� ���� ���� ����
    void Attack_Firecracker()
    {
        //�Ȼ��� ���� ���� ����
        if (firecracker.hasWeapon) //���� ���� ���� ���� üũ
        {
            firecracker.timer += Time.deltaTime; //���� ��Ÿ�� Ÿ�̸� �ð� ����

            if (firecracker.timer >= firecracker.cooldown) //��Ÿ�� ����
            {
                firecracker.timer = 0.0f; //��Ÿ�� �ʱ�ȭ
                for (int i = 0; i < firecracker.count; i++) //���� ���� üũ
                {
                    weaponPool.firecrackerScript[firecracker.index].SetAttackPos(); //���� ���� ��ġ
                    weaponPool.firecrackerPool.objects[firecracker.index].SetActive(true); //�ε��� ���� ���� Ȱ��ȭ

                    firecracker.index++; //���� �ε��� �� ����

                    if (firecracker.index >= weaponPool.firecrackerPool.objects.Length) //�����ε��� �� ���� �ʰ� üũ
                        firecracker.index = 0; //���� �ε��� �� �ʱ�ȭ
                }
            }
        }
    }

    //��� ����
    void Attack_Apple()
    {
        //��� ����
        if (apple.hasWeapon) //��� ���� ���� ���� üũ
        {
            apple.timer += Time.deltaTime; //��� ��Ÿ�� Ÿ�̸� �ð� ����

            if (apple.timer >= apple.cooldown) //��Ÿ�� ����
            {
                apple.timer = 0.0f; //��Ÿ�� �ʱ�ȭ
                for (int i = 0; i < apple.count; i++) //��� ���� üũ
                {
                    weaponPool.appleScript[apple.index].SetRotation(); //��� ȸ��
                    weaponPool.appleScript[apple.index].SetRandomPos(); //��� ���� ��ġ ���� �Լ�
                    weaponPool.appleScript[apple.index].SetAttackDir(); //����� ���ݹ��� ���� �Լ�
                    weaponPool.applePool.objects[apple.index].SetActive(true); //�ε��� ���� ��� Ȱ��ȭ

                    apple.index++; //��� �ε��� �� ����

                    if (apple.index >= weaponPool.applePool.objects.Length) //��� �ε��� �� ���� �ʰ� üũ
                        apple.index = 0; //��� �ε��� �� �ʱ�ȭ
                }
            }
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LevelUpScript : MonoBehaviour
{
    //public PlayerScript playerScript; //�÷��̾� ��ũ��Ʈ > �ɷ�ġ ���� ���� �����ϱ� ����
    public GameObject fade;
    public Button pasue; //�ɷ� ����â Ȱ��ȭ �� ��ư ��� ��Ȱ��ȭ�ϱ� ����

    [Header("Upgrade")] //����� ���� �ɷ� ���� ī��
    public GameObject upgradePanel; //���׷��̵� �ǳ� ���ӿ�����Ʈ
    public GameObject[] tempUpgrades; //�ӽ� ����
    public GameObject[] upgrades; //���׷��̵� �迭

    public GameObject trumpWeapon; //UI ���׷��̵� �θ�� �� ���ӿ�����Ʈ
    public GameObject cheshireWeapon;
    public GameObject jabberwockyWeapon;
    public GameObject firecrackerWeapon;
    public GameObject appleWeapon;

    public GameObject[] trumpUpgrades; //�̻��� ���׷��̵� ������Ʈ�� > ���� ���� �ϳ��� ������ ���
    public GameObject[] cheshireUpgrades; //������, �ؽ�Ʈ�� ���� �����Ͽ� ���� �� ������Ʈ �迭 ��� �ʿ�x. �ٸ�, csv �����̳� �̹��� ��������Ʈ ���� �� �ʿ�
    public GameObject[] jabberwockyUpgrades;
    public GameObject[] firecrackerUpgrades;
    public GameObject[] appleUpgrades;
    public GameObject[] unlockUpgrades;

    [Header("Data")] //���� ������
    public WeaponData trumpData;
    public WeaponData cheshireData;
    public WeaponData jabberwockyData;
    public WeaponData firecrackerData;
    public WeaponData appleData;
    //public WeaponData ; //�ٸ� ���� ������ �߰� �ۼ�
    //public WeaponData ;
    //public WeaponData ;
    //public WeaponData ;
    //public WeaponData ;
    //public WeaponData ;

    //���� �ر� ����
    public bool hasTrump = false;                     
    public bool hasCheshire = false;
    public bool hasJabberwocky = false;
    public bool hasFirecracker = false;
    public bool hasApple = false;
    //public bool has = false; //�ٸ� ���� �ر� ���� �߰� �ۼ�
    //public bool has = false;
    //public bool has = false;
    //public bool has = false;
    //public bool has = false;
    //public bool has = false;


    //���� �� ����
    int trumpCard_Lv = 0; //Ʈ���� ī�� ����
    int cheshireCat_Lv = 0; //ä��Ĺ ����
    int jabberwockyBreath_Lv = 0; //�����Ű ����
    int nonBirthdayFirecracker_Lv = 0; //�Ȼ��� ���� ���� ����
    int rollApple_Lv = 0; //�����ٴϴ� ��� ����
    //int _Lv = 0; //�ٸ� ���� ���� �߰� �ۼ�
    //int _Lv = 0;
    //int _Lv = 0; 
    //int _Lv = 0;
    //int _Lv = 0;
    //int _Lv = 0;

    int index; //���׷��̵� �迭 �ε��� ũ��
    int ran; //���׷��̵� �迭 ���� �ε��� ��

    //�ӽ÷� ���� �ɷ��� �����ϴ� ������Ʈ
    GameObject tempU_0;
    GameObject tempU_1;
    GameObject tempU_2;

    public GameObject weaponGroup; //���� �ɷ� ������Ʈ �׷�
    public Button reroll; //���ѹ�ư
    public Image rerollImage; //���ѹ�ư �̹���

    bool onUpgrade = false;

    float time = 0;

    private void Awake()
    {
        SetStat(); //���� �缳��
        SetWeapon(); //���� �ɷ� ����
    }

    //���� �缳��
    void SetStat()
    {
        
    }

    //���� ���� �ڽ� ������Ʈ�� �ִ� �ɷ��� �� ������ �迭�� �ֱ�
    void SetWeapon()
    {
        for (int i = 0; i < trumpWeapon.transform.childCount; i++) //Ʈ���� ī�� ������Ʈ �׷��� �ڽĸ�ŭ
        {
            trumpUpgrades[i] = trumpWeapon.transform.GetChild(i).gameObject; //Ʈ���� ���׷��̵� �迭�� Ʈ���� ������Ʈ �ڽ� ���� ����
        }
        for (int i = 0; i < cheshireWeapon.transform.childCount; i++)
        {
            cheshireUpgrades[i] = cheshireWeapon.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < jabberwockyWeapon.transform.childCount; i++)
        {
            jabberwockyUpgrades[i] = jabberwockyWeapon.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < firecrackerWeapon.transform.childCount; i++)
        {
            firecrackerUpgrades[i] = firecrackerWeapon.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < appleWeapon.transform.childCount; i++)
        {
            appleUpgrades[i] = appleWeapon.transform.GetChild(i).gameObject;
        }
    }

    //���� ���� > ���� + �������� �� �����̴� �Լ���� ���� ���� �ʿ� >> ������ 4���� ������
    public void UpgradesArr()
    {
        index = 0;

        TempUpgradesArr();                                                           //�ӽ� ���׷��̵� �迭 �� ����

        upgrades = new GameObject[3];                                                //���� �ɷ� ���ÿ� �� �迭 ũ�� �Ҵ�

        ran = Random.Range(0, index);                                                //�迭 �ε��� n ������
        upgrades[0] = tempUpgrades[ran];                                             //������ ���� �ɷ�
        tempU_0 = upgrades[0].transform.parent.gameObject;                           //������ ���� �ɷ��� �θ� �ӽ÷� tempU_0�� �ִ´� -> ���߿� �θ� ������ �ڿ� �ٽ� �������� ����
        upgrades[0].transform.SetParent(upgradePanel.transform);                     //�ش� ���� �ɷ��� �θ� ����
        upgrades[0].transform.localPosition = new Vector3(-720, 0, 0);               //���� �ɷ� ���� ī���� ��ġ ����
        upgrades[0].SetActive(true);                                                 //���� �ɷ� ī�� ���� Ȱ��ȭ

        ran = Random.Range(0, index);
        upgrades[1] = tempUpgrades[ran];
        while (upgrades[0] == upgrades[1])                                           //ù��°�� �ι�° �ɷ��� ���� ��� �ٸ� �ɷ��� ���õ� ������ �ݺ��� ����
        {
            ran = Random.Range(0, index);
            upgrades[1] = tempUpgrades[ran];
        }
        tempU_1 = upgrades[1].transform.parent.gameObject;
        upgrades[1].transform.SetParent(upgradePanel.transform);
        upgrades[1].transform.localPosition = new Vector3(0, 0, 0);
        upgrades[1].SetActive(true);

        ran = Random.Range(0, index);
        upgrades[2] = tempUpgrades[ran];
        while (upgrades[0] == upgrades[2] || upgrades[1] == upgrades[2])
        {
            ran = Random.Range(0, index);
            upgrades[2] = tempUpgrades[ran];
        }
        tempU_2 = upgrades[2].transform.parent.gameObject;
        upgrades[2].transform.SetParent(upgradePanel.transform);
        upgrades[2].transform.localPosition = new Vector3(720, 0, 0);
        upgrades[2].SetActive(true);

    }

    //�ӽ� ���׷��̵� �迭 �� ����
    void TempUpgradesArr()
    {
        //������ ���� �迭�� �ִ� �ε��� �� �ʱ�ȭ
        if (hasTrump)
        {
            switch (trumpCard_Lv) //�� ���� ���� ���� ���׷��̵� ������Ʈ �ֱ�
            {
                case 0:
                    tempUpgrades[index] = trumpUpgrades[trumpCard_Lv]; //�ӽ� ���� �迭�� �ش� ���� �ɷ� ���� ���ӿ�����Ʈ ����
                    index++; //�迭 ũ�� ����
                    break;

                case 1:
                    tempUpgrades[index] = trumpUpgrades[trumpCard_Lv]; //�ӽ� ���� �迭�� �ش� ���� �ɷ� ���� ���ӿ�����Ʈ ����
                    index++; //�迭 ũ�� ����
                    break;

                case 2:
                    tempUpgrades[index] = trumpUpgrades[trumpCard_Lv]; //�ӽ� ���� �迭�� �ش� ���� �ɷ� ���� ���ӿ�����Ʈ ����
                    index++; //�迭 ũ�� ����
                    break;

                case 3:
                    tempUpgrades[index] = trumpUpgrades[trumpCard_Lv]; //�ӽ� ���� �迭�� �ش� ���� �ɷ� ���� ���ӿ�����Ʈ ����
                    index++; //�迭 ũ�� ����
                    break;

                case 4:
                    tempUpgrades[index] = trumpUpgrades[trumpCard_Lv]; //�ӽ� ���� �迭�� �ش� ���� �ɷ� ���� ���ӿ�����Ʈ ����
                    index++; //�迭 ũ�� ����
                    break;
            }
        }
        else if (!hasTrump) //���ر� ��
        {
            tempUpgrades[index] = unlockUpgrades[0]; //�ر� ������Ʈ ����
            index++; //�迭 ũ�� ����
        }
    }

    //�θ� ����� ���� �ɷ¸� ������ �θ�� �ٽ� ����, �ɷ� ���� �� ����
    public void RelocationUpgrades()
    {
        upgrades[0].transform.SetParent(tempU_0.transform);
        upgrades[1].transform.SetParent(tempU_1.transform);
        upgrades[2].transform.SetParent(tempU_2.transform);
    }

    //�θ� ����� ���� �ɷ¸� ������ �θ�� �ٽ� ����, ���� ��ư�� ������ �� ����
    public void UpgradesReroll()                                             
    {
        upgrades[0].transform.SetParent(tempU_0.transform);                         //�ӽ÷� �־�� �θ� �ٽ� �ҷ��� ���� �ɷ��� �θ� ������� ���������
        upgrades[1].transform.SetParent(tempU_1.transform);
        upgrades[2].transform.SetParent(tempU_2.transform);

        reroll.interactable = false;                                                //���� ��ư ��� ��Ȱ��ȭ
        rerollImage.color = new Color32(52, 255, 0, 60);                            //���� ��Ȱ��ȭ ǥ�ø� ���� ��ư �̹����� �÷� �� ����
        UpgradesArr();
    }

    //Ʈ���� ī�� �������
    public void UnlockUpgrades_0()
    {
        hasTrump = true;                                        //Ʈ���� ī�� �ر�(����) ����
    }

    //Ʈ���� ī�� ���׷��̵� ��� �Լ�
    public void TrumpCardUpgrades()
    {
        switch (trumpCard_Lv) //�� ���� ���� �� ���׷��̵�
        {
            case 0:
                //Ʈ���� ������ ��ġ ����
                //���� ���ݷ� ����, �ӵ� ����, ���� ���� ���
                trumpCard_Lv++;
                break;

            case 1:
                trumpCard_Lv++;
                break;

            case 2:
                trumpCard_Lv++;
                break;

            case 3:
                trumpCard_Lv++;
                break;

            case 4:
                trumpCard_Lv++;
                break;
        }
    }

    //���� �ɷ� ���� Ȱ��ȭ �̺�Ʈ
    public void UpgradeSelectEvent_0()
    {
        onUpgrade = true;                                           //���� �ɷ� ���� �̺�Ʈ Ȱ��ȭ ����
        pasue.interactable = false;                                //�Ͻ����� ��ư ��� ��Ȱ��ȭ
        Time.timeScale = 0.0f;                                     //Ÿ�ӽ������� 0���� �Ͽ� ��������
        //gameManager.joystick.SetActive(false);                     //���̽�ƽ ��Ȱ��ȭ
        UpgradesArr();                                              //������ ���� �ɷ� ���ϱ�
        fade.SetActive(true);                                      //���̵� Ȱ��ȭ
        upgradePanel.SetActive(true);                               //���� �ɷ� ���� �ǳ� Ȱ��ȭ
        reroll.interactable = true;                                //���� ��ư ��� Ȱ��ȭ
        rerollImage.color = new Color32(52, 255, 0, 255);          //���� ��ư �̹����� �÷� �� ������� ����
    }

    //���� �ɷ� ���� ��Ȱ��ȭ �̺�Ʈ
    public void UpgradeSelectEvent_1()
    {
        //gameManager.joystick.SetActive(true);                      //���̽�ƽ Ȱ��ȭ
        TimeSlowUp();                                                //Ÿ�ӽ������� 1�� �ϴ� �Լ�
        //joystick.handle.anchoredPosition = Vector2.zero;           //���̽�ƽ �ڵ��� ��ġ �ʱ�ȭ
        //joystick.input = Vector2.zero;                             //���̽�ƽ�� �Է°� �ʱ�ȭ
        upgradePanel.SetActive(false);                              //���� �ɷ� ���� �ǳ� ��Ȱ��ȭ
        onUpgrade = false;                                          //���� �ɷ� ���� �̺�Ʈ Ȱ��ȭ ����
    }

    //���� �ɷ� ���� �̺�Ʈ �� õõ�� ���� ���� Ǯ����
    public void TimeSlowUp()
    {
        if (Time.timeScale <= 1)                                   //Ÿ�ӽ������� 1�� ���ų� ���� �� ����
        {
            if (fade.activeSelf == false)                          //���̵� ��Ȱ��ȭ
                fade.SetActive(true);

            pasue.interactable = true;                             //�Ͻ����� ��ư ��� Ȱ��ȭ

            time += 0.15f;                                         //���������� �Ͻ������� �����ϱ� ���� ����
            Time.timeScale += time;                                //Ÿ�ӽ������� ���� Ÿ�Ӹ�ŭ ���Ͽ� �ð��� �帣�� �ϱ�

            if (Time.timeScale >= 1)                               //Ÿ�ӽ������� ���� 1�� ���ų� 1���� Ŭ ��� ����
            {
                fade.SetActive(false);                             //���̵� ��Ȱ��ȭ
                Time.timeScale = 1.0f;                             //�������� ���Ӽӵ��� ���� Ÿ�ӽ����Ͽ� 1 ����
                time = 0;                                          //Ÿ�� �� 0���� �ʱ�ȭ
                upgradePanel.SetActive(false);                      //���� �ɷ� ���� �ǳ� ��Ȱ��ȭ
                return;                                            //����
            }
            else
                Invoke(nameof(TimeSlowUp), 0.06f);                       //�κ�ũ�� Ÿ�ӽ��ο���� 0.06���� �����̸� ������ ����
        }
    }

    //���� ���׷��̵� ���� ���� �Լ��� �ۼ��ϰų� ����ġ ���� Ȱ���Ͽ� �ϳ��� �Լ��� ���
    //public void trumpEvent_0()                                              //�̻��� �ɷ�
    //{
    //    trumpData.damage += (trumpData.baseDamage / 100) * 30;            //�̻��� ������ ����
    //    mStar_0[trump_Lv_0].sprite = starSprite;                            //�̻��� �ɷ��� �� �̹��� ����
    //    trump_Lv_0++;                                                       //�̻��� ���� ����
    //}
}
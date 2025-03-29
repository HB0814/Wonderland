using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    GameObject player;

    //Ʈ���� ī��
    public GameObject trumpGroup; //�θ�� �� ���ӿ�����Ʈ
    public GameObject trumpPrefab; //������
    public GameObject[] trumpObjects; //Ǯ���� ������Ʈ
    public TrumpCard[] trumpCardScript; //Ǯ���� ������Ʈ�� ���� ��ũ��Ʈ

    //ä��Ĺ
    public GameObject cheshireCatGroup;
    public GameObject cheshireCatPrefab;
    public GameObject[] cheshireCatObjects;
    public CheshireCat[] cheshireCatScript;

    //�Ȼ��� ���� ����
    public GameObject firecrackerGroup;
    public GameObject firecrackerPrefab;
    public GameObject[] firecrackerObjects;
    public NonBirthdayFirecracker[] firecrackerScript;

    //���
    public GameObject appleGroup;
    public GameObject applePrefab;
    public GameObject[] appleObjects;
    public RollApple[] appleScript;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //Ʈ���� Ǯ��
        for (int i = 0; i < trumpObjects.Length; i++)
        {
            GameObject trumpObj = Instantiate(trumpPrefab);
            trumpObj.transform.parent = trumpGroup.transform;
            trumpObjects[i] = trumpObj;
            trumpCardScript[i] = trumpObjects[i].GetComponent<TrumpCard>();
            trumpObj.SetActive(false);
        }

        //ä��Ĺ Ǯ��
        for (int i = 0; i < trumpObjects.Length; i++)
        {
            GameObject cheshireCatObj = Instantiate(cheshireCatPrefab);
            cheshireCatObj.transform.parent = cheshireCatGroup.transform;
            cheshireCatObjects[i] = cheshireCatObj;
            cheshireCatScript[i] = cheshireCatObjects[i].GetComponent<CheshireCat>();
            cheshireCatScript[i].SetPlayer(player);
            //���ӿ�����Ʈ �÷��̾� �Ҵ�
            //�������� ��� ���� ��ġ�������� ���¿����� Find�� ���� �Ҵ��� �� ���� ����.
            //���� ���� ������ Ǯ�� �������� �Ҵ�.
            cheshireCatObj.SetActive(false);
        }

        //�Ȼ��� ���� ���� Ǯ��
        for (int i = 0; i < firecrackerObjects.Length; i++)
        {
            GameObject firecrackerObj = Instantiate(firecrackerPrefab);
            firecrackerObj.transform.parent = firecrackerGroup.transform;
            firecrackerObjects[i] = firecrackerObj;
            firecrackerScript[i] = firecrackerObjects[i].GetComponent<NonBirthdayFirecracker>();
            firecrackerScript[i].SetPlayer(player);
            firecrackerObj.SetActive(false);
        }

        //��� Ǯ��
        for (int i = 0; i < appleObjects.Length; i++)
        {
            GameObject appleObj = Instantiate(applePrefab);
            appleObj.transform.parent = appleGroup.transform;
            appleObjects[i] = appleObj;
            appleScript[i] = appleObjects[i].GetComponent<RollApple>();
            appleScript[i].SetPlayer(player);
            appleObj.SetActive(false);
        }
    }
}

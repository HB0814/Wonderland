using Unity.VisualScripting;
using UnityEngine;

public class WeaponPool : MonoBehaviour
{
    GameObject player;

    [System.Serializable]
    public class Pool
    {
        public GameObject group; //�θ� ������Ʈ
        public GameObject prefab; //������
        public GameObject[] objects; //Ǯ���� ������Ʈ ũ��
    }

    //Ʈ���� ī��
    public Pool trumpPool;
    public TrumpCard[] trumpCardScript; //Ǯ���� ������Ʈ�� ���� ��ũ��Ʈ �迭

    //ä��Ĺ
    public Pool cheshireCatPool;
    public CheshireCat[] cheshireCatScript;

    //�Ȼ��� ���� ����
    public Pool firecrackerPool;
    public NonBirthdayFirecracker[] firecrackerScript;

    //���
    public Pool applePool;
    public RollApple[] appleScript;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        InitializePool(trumpPool); //Ʈ����ī�� Ǯ��
        SetTrumpCard(); //��ũ��Ʈ �迭 ����

        InitializePool(cheshireCatPool);
        SetCheshireCat();

        InitializePool(firecrackerPool);
        SetFirecracker();

        InitializePool(applePool);
        SetApple();
    }

    //������Ʈ Ǯ��
    private void InitializePool(Pool pool)
    {
        int poolSize = pool.objects.Length; //Ǯ�� ũ�� �� ����
        pool.objects = new GameObject[poolSize]; //Ǯ�� ũ�� ����

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(pool.prefab, pool.group.transform); //Ǯ��(������, �θ� ������Ʈ)
            pool.objects[i] = obj; //������Ʈ �迭�� �ش� ���ӿ�����Ʈ �ֱ�
            obj.SetActive(false); //������Ʈ ��Ȱ��ȭ
        }
    }

    //Ʈ���� ī�� ��ũ��Ʈ �迭 ����
    void SetTrumpCard()
    {
        trumpCardScript = new TrumpCard[trumpPool.objects.Length]; //������Ʈ�� ���� ũ�� �Ҵ�

        for (int i = 0; i < trumpPool.objects.Length; i++)
        {
            trumpPool.objects[i].tag = "TrumpCard_" + i; //Ʈ���� ī�� �±� ����
            trumpCardScript[i] = trumpPool.objects[i].GetComponent<TrumpCard>();
        }
    }

    //ä��Ĺ ��ũ��Ʈ �迭 ����
    void SetCheshireCat()
    {
        cheshireCatScript = new CheshireCat[cheshireCatPool.objects.Length];

        for (int i = 0; i < cheshireCatPool.objects.Length; i++)
        {
            cheshireCatScript[i] = cheshireCatPool.objects[i].GetComponent<CheshireCat>();
            cheshireCatScript[i].SetPlayer(player); //ä��Ĺ ��ũ��Ʈ�� �÷��̾� ����
        }
    }

    //�Ȼ��� ���� ���� ��ũ��Ʈ �迭 ����
    void SetFirecracker()
    {
        firecrackerScript = new NonBirthdayFirecracker[firecrackerPool.objects.Length];

        for (int i = 0; i < firecrackerPool.objects.Length; i++)
        {
            firecrackerScript[i] = firecrackerPool.objects[i].GetComponent<NonBirthdayFirecracker>();
            firecrackerScript[i].SetPlayer(player);
        }
    }

    //��� ��ũ��Ʈ �迭 ����
    void SetApple()
    {
        appleScript = new RollApple[applePool.objects.Length];

        for (int i = 0; i < applePool.objects.Length; i++)
        {
            appleScript[i] = applePool.objects[i].GetComponent<RollApple>();
            appleScript[i].SetPlayer(player);
        }
    }
}
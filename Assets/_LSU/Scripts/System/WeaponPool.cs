using Unity.VisualScripting;
using UnityEngine;

public class WeaponPool : MonoBehaviour
{
    GameObject player;

    [System.Serializable]
    public class Pool
    {
        public GameObject group; //부모 오브젝트
        public GameObject prefab; //프립팹
        public GameObject[] objects; //풀링할 오브젝트 크기
    }

    //트럼프 카드
    public Pool trumpPool;
    public TrumpCard[] trumpCardScript; //풀링된 오브젝트의 무기 스크립트 배열

    //채셔캣
    public Pool cheshireCatPool;
    public CheshireCat[] cheshireCatScript;

    //안생일 축하 폭죽
    public Pool firecrackerPool;
    public NonBirthdayFirecracker[] firecrackerScript;

    //사과
    public Pool applePool;
    public RollApple[] appleScript;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        InitializePool(trumpPool); //트럼프카드 풀링
        SetTrumpCard(); //스크립트 배열 설정

        InitializePool(cheshireCatPool);
        SetCheshireCat();

        InitializePool(firecrackerPool);
        SetFirecracker();

        InitializePool(applePool);
        SetApple();
    }

    //오브젝트 풀링
    private void InitializePool(Pool pool)
    {
        int poolSize = pool.objects.Length; //풀링 크기 값 대입
        pool.objects = new GameObject[poolSize]; //풀링 크기 설정

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(pool.prefab, pool.group.transform); //풀링(프리팹, 부모 오브젝트)
            pool.objects[i] = obj; //오브젝트 배열에 해당 게임오브젝트 넣기
            obj.SetActive(false); //오브젝트 비활성화
        }
    }

    //트럼프 카드 스크립트 배열 설정
    void SetTrumpCard()
    {
        trumpCardScript = new TrumpCard[trumpPool.objects.Length]; //오브젝트와 같은 크기 할당

        for (int i = 0; i < trumpPool.objects.Length; i++)
        {
            trumpPool.objects[i].tag = "TrumpCard_" + i; //트럼프 카드 태그 설정
            trumpCardScript[i] = trumpPool.objects[i].GetComponent<TrumpCard>();
        }
    }

    //채셔캣 스크립트 배열 설정
    void SetCheshireCat()
    {
        cheshireCatScript = new CheshireCat[cheshireCatPool.objects.Length];

        for (int i = 0; i < cheshireCatPool.objects.Length; i++)
        {
            cheshireCatScript[i] = cheshireCatPool.objects[i].GetComponent<CheshireCat>();
            cheshireCatScript[i].SetPlayer(player); //채셔캣 스크립트의 플레이어 설정
        }
    }

    //안생일 축하 폭죽 스크립트 배열 설정
    void SetFirecracker()
    {
        firecrackerScript = new NonBirthdayFirecracker[firecrackerPool.objects.Length];

        for (int i = 0; i < firecrackerPool.objects.Length; i++)
        {
            firecrackerScript[i] = firecrackerPool.objects[i].GetComponent<NonBirthdayFirecracker>();
            firecrackerScript[i].SetPlayer(player);
        }
    }

    //사과 스크립트 배열 설정
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
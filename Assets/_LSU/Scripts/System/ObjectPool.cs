using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    GameObject player;

    //트럼프 카드
    public GameObject trumpGroup; //부모로 할 게임오브젝트
    public GameObject trumpPrefab; //프리팹
    public GameObject[] trumpObjects; //풀링된 오브젝트
    public TrumpCard[] trumpCardScript; //풀링된 오브젝트의 무기 스크립트

    //채셔캣
    public GameObject cheshireCatGroup;
    public GameObject cheshireCatPrefab;
    public GameObject[] cheshireCatObjects;
    public CheshireCat[] cheshireCatScript;

    //안생일 축하 폭죽
    public GameObject firecrackerGroup;
    public GameObject firecrackerPrefab;
    public GameObject[] firecrackerObjects;
    public NonBirthdayFirecracker[] firecrackerScript;

    //사과
    public GameObject appleGroup;
    public GameObject applePrefab;
    public GameObject[] appleObjects;
    public RollApple[] appleScript;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //트럼프 풀링
        for (int i = 0; i < trumpObjects.Length; i++)
        {
            GameObject trumpObj = Instantiate(trumpPrefab);
            trumpObj.transform.parent = trumpGroup.transform;
            trumpObjects[i] = trumpObj;
            trumpCardScript[i] = trumpObjects[i].GetComponent<TrumpCard>();
            trumpObj.SetActive(false);
        }

        //채셔캣 풀링
        for (int i = 0; i < trumpObjects.Length; i++)
        {
            GameObject cheshireCatObj = Instantiate(cheshireCatPrefab);
            cheshireCatObj.transform.parent = cheshireCatGroup.transform;
            cheshireCatObjects[i] = cheshireCatObj;
            cheshireCatScript[i] = cheshireCatObjects[i].GetComponent<CheshireCat>();
            cheshireCatScript[i].SetPlayer(player);
            //게임오브젝트 플레이어 할당
            //프리팹의 경우 씬에 배치되지않은 상태에서는 Find를 통해 할당을 할 수가 없음.
            //위와 같은 이유로 풀링 과정에서 할당.
            cheshireCatObj.SetActive(false);
        }

        //안생일 축하 폭죽 풀링
        for (int i = 0; i < firecrackerObjects.Length; i++)
        {
            GameObject firecrackerObj = Instantiate(firecrackerPrefab);
            firecrackerObj.transform.parent = firecrackerGroup.transform;
            firecrackerObjects[i] = firecrackerObj;
            firecrackerScript[i] = firecrackerObjects[i].GetComponent<NonBirthdayFirecracker>();
            firecrackerScript[i].SetPlayer(player);
            firecrackerObj.SetActive(false);
        }

        //사과 풀링
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

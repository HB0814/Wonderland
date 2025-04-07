using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LevelUpScript : MonoBehaviour
{
    //public PlayerScript playerScript; //플레이어 스크립트 > 능력치 변동 사항 적용하기 위함
    public GameObject fade;
    public Button pasue; //능력 선택창 활성화 시 버튼 기능 비활성화하기 위함

    [Header("Upgrade")] //무기와 무기 능력 선택 카드
    public GameObject upgradePanel; //업그레이드 판넬 게임오브젝트
    public GameObject[] tempUpgrades; //임시 무기
    public GameObject[] upgrades; //업그레이드 배열

    public GameObject trumpWeapon; //UI 업그레이드 부모로 할 게임오브젝트
    public GameObject cheshireWeapon;
    public GameObject jabberwockyWeapon;
    public GameObject firecrackerWeapon;
    public GameObject appleWeapon;

    public GameObject[] trumpUpgrades; //미사일 업그레이드 오브젝트들 > 레벨 별로 하나씩 꺼내서 사용
    public GameObject[] cheshireUpgrades; //아이콘, 텍스트를 따로 설정하여 대입 시 오브젝트 배열 사용 필요x. 다만, csv 파일이나 이미지 스프라이트 변경 등 필요
    public GameObject[] jabberwockyUpgrades;
    public GameObject[] firecrackerUpgrades;
    public GameObject[] appleUpgrades;
    public GameObject[] unlockUpgrades;

    [Header("Data")] //무기 데이터
    public WeaponData trumpData;
    public WeaponData cheshireData;
    public WeaponData jabberwockyData;
    public WeaponData firecrackerData;
    public WeaponData appleData;
    //public WeaponData ; //다른 무기 데이터 추가 작성
    //public WeaponData ;
    //public WeaponData ;
    //public WeaponData ;
    //public WeaponData ;
    //public WeaponData ;

    //무기 해금 여부
    public bool hasTrump = false;                     
    public bool hasCheshire = false;
    public bool hasJabberwocky = false;
    public bool hasFirecracker = false;
    public bool hasApple = false;
    //public bool has = false; //다른 무기 해금 여부 추가 작성
    //public bool has = false;
    //public bool has = false;
    //public bool has = false;
    //public bool has = false;
    //public bool has = false;


    //무기 별 레벨
    int trumpCard_Lv = 0; //트럼프 카드 레벨
    int cheshireCat_Lv = 0; //채셔캣 레벨
    int jabberwockyBreath_Lv = 0; //재버워키 레벨
    int nonBirthdayFirecracker_Lv = 0; //안생일 축하 폭죽 레벨
    int rollApple_Lv = 0; //굴러다니는 사과 레벨
    //int _Lv = 0; //다른 무기 레벨 추가 작성
    //int _Lv = 0;
    //int _Lv = 0; 
    //int _Lv = 0;
    //int _Lv = 0;
    //int _Lv = 0;

    int index; //업그레이드 배열 인덱스 크기
    int ran; //업그레이드 배열 랜덤 인덱스 값

    //임시로 무기 능력을 저장하는 오브젝트
    GameObject tempU_0;
    GameObject tempU_1;
    GameObject tempU_2;

    public GameObject weaponGroup; //무기 능력 오브젝트 그룹
    public Button reroll; //리롤버튼
    public Image rerollImage; //리롤버튼 이미지

    bool onUpgrade = false;

    float time = 0;

    private void Awake()
    {
        SetStat(); //스탯 재설정
        SetWeapon(); //무기 능력 설정
    }

    //스탯 재설정
    void SetStat()
    {
        
    }

    //무기 별로 자식 오브젝트에 있는 능력을 각 무기의 배열에 넣기
    void SetWeapon()
    {
        for (int i = 0; i < trumpWeapon.transform.childCount; i++) //트럼프 카드 오브젝트 그룹의 자식만큼
        {
            trumpUpgrades[i] = trumpWeapon.transform.GetChild(i).gameObject; //트럼프 업그레이드 배열에 트럼프 오브젝트 자식 별로 대입
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

    //무기 정렬 > 무기 + 스탯으로 할 예정이니 함수명과 내용 수정 필요 >> 선택지 4개도 좋을듯
    public void UpgradesArr()
    {
        index = 0;

        TempUpgradesArr();                                                           //임시 업그레이드 배열 값 설정

        upgrades = new GameObject[3];                                                //무기 능력 선택에 쓸 배열 크기 할당

        ran = Random.Range(0, index);                                                //배열 인덱스 n 랜덤값
        upgrades[0] = tempUpgrades[ran];                                             //무작위 무기 능력
        tempU_0 = upgrades[0].transform.parent.gameObject;                           //무작위 무기 능력의 부모를 임시로 tempU_0에 넣는다 -> 나중에 부모를 변경한 뒤에 다시 돌려놓기 위함
        upgrades[0].transform.SetParent(upgradePanel.transform);                     //해당 무기 능력의 부모 변경
        upgrades[0].transform.localPosition = new Vector3(-720, 0, 0);               //무기 능력 선택 카드의 위치 조정
        upgrades[0].SetActive(true);                                                 //무기 능력 카드 상태 활성화

        ran = Random.Range(0, index);
        upgrades[1] = tempUpgrades[ran];
        while (upgrades[0] == upgrades[1])                                           //첫번째와 두번째 능력이 같을 경우 다른 능력이 선택될 때까지 반복문 실행
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

    //임시 업그레이드 배열 값 설정
    void TempUpgradesArr()
    {
        //무작위 무기 배열의 최대 인덱스 값 초기화
        if (hasTrump)
        {
            switch (trumpCard_Lv) //각 무기 레벨 별로 업그레이드 오브젝트 넣기
            {
                case 0:
                    tempUpgrades[index] = trumpUpgrades[trumpCard_Lv]; //임시 웨폰 배열에 해당 무기 능력 선택 게임오브젝트 저장
                    index++; //배열 크기 증가
                    break;

                case 1:
                    tempUpgrades[index] = trumpUpgrades[trumpCard_Lv]; //임시 웨폰 배열에 해당 무기 능력 선택 게임오브젝트 저장
                    index++; //배열 크기 증가
                    break;

                case 2:
                    tempUpgrades[index] = trumpUpgrades[trumpCard_Lv]; //임시 웨폰 배열에 해당 무기 능력 선택 게임오브젝트 저장
                    index++; //배열 크기 증가
                    break;

                case 3:
                    tempUpgrades[index] = trumpUpgrades[trumpCard_Lv]; //임시 웨폰 배열에 해당 무기 능력 선택 게임오브젝트 저장
                    index++; //배열 크기 증가
                    break;

                case 4:
                    tempUpgrades[index] = trumpUpgrades[trumpCard_Lv]; //임시 웨폰 배열에 해당 무기 능력 선택 게임오브젝트 저장
                    index++; //배열 크기 증가
                    break;
            }
        }
        else if (!hasTrump) //미해금 시
        {
            tempUpgrades[index] = unlockUpgrades[0]; //해금 오브젝트 저장
            index++; //배열 크기 증가
        }
    }

    //부모가 변경된 무기 능력를 기존의 부모로 다시 변경, 능력 선택 시 실행
    public void RelocationUpgrades()
    {
        upgrades[0].transform.SetParent(tempU_0.transform);
        upgrades[1].transform.SetParent(tempU_1.transform);
        upgrades[2].transform.SetParent(tempU_2.transform);
    }

    //부모가 변경된 무기 능력를 기존의 부모로 다시 변경, 리롤 버튼을 눌렀을 시 실행
    public void UpgradesReroll()                                             
    {
        upgrades[0].transform.SetParent(tempU_0.transform);                         //임시로 넣어둔 부모를 다시 불러와 무기 능력의 부모를 원래대로 돌려놓기기
        upgrades[1].transform.SetParent(tempU_1.transform);
        upgrades[2].transform.SetParent(tempU_2.transform);

        reroll.interactable = false;                                                //리롤 버튼 기능 비활성화
        rerollImage.color = new Color32(52, 255, 0, 60);                            //리롤 비활성화 표시를 위한 버튼 이미지의 컬러 값 변경
        UpgradesArr();
    }

    //트럼프 카드 잠금해제
    public void UnlockUpgrades_0()
    {
        hasTrump = true;                                        //트럼프 카드 해금(보유) 여부
    }

    //트럼프 카드 업그레이드 기능 함수
    public void TrumpCardUpgrades()
    {
        switch (trumpCard_Lv) //각 무기 레벨 별 업그레이드
        {
            case 0:
                //트럼프 무기의 수치 변경
                //무기 공격력 증가, 속도 증가, 개수 증가 등등
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

    //무기 능력 선택 활성화 이벤트
    public void UpgradeSelectEvent_0()
    {
        onUpgrade = true;                                           //무기 능력 선택 이벤트 활성화 여부
        pasue.interactable = false;                                //일시정지 버튼 기능 비활성화
        Time.timeScale = 0.0f;                                     //타임스케일을 0으로 하여 정지상태
        //gameManager.joystick.SetActive(false);                     //조이스틱 비활성화
        UpgradesArr();                                              //무작위 무기 능력 정하기
        fade.SetActive(true);                                      //페이드 활성화
        upgradePanel.SetActive(true);                               //무기 능력 선택 판넬 활성화
        reroll.interactable = true;                                //리롤 버튼 기능 활성화
        rerollImage.color = new Color32(52, 255, 0, 255);          //리롤 버튼 이미지의 컬러 값 기존대로 변경
    }

    //무기 능력 선택 비활성화 이벤트
    public void UpgradeSelectEvent_1()
    {
        //gameManager.joystick.SetActive(true);                      //조이스틱 활성화
        TimeSlowUp();                                                //타임스케일을 1로 하는 함수
        //joystick.handle.anchoredPosition = Vector2.zero;           //조이스틱 핸들의 위치 초기화
        //joystick.input = Vector2.zero;                             //조이스틱의 입력값 초기화
        upgradePanel.SetActive(false);                              //무기 능력 선택 판넬 비활성화
        onUpgrade = false;                                          //무기 능력 선택 이벤트 활성화 여부
    }

    //무기 능력 선택 이벤트 후 천천히 정지 상태 풀리기
    public void TimeSlowUp()
    {
        if (Time.timeScale <= 1)                                   //타임스케일이 1과 같거나 작을 때 실행
        {
            if (fade.activeSelf == false)                          //페이드 비활성화
                fade.SetActive(true);

            pasue.interactable = true;                             //일시정지 버튼 기능 활성화

            time += 0.15f;                                         //가속적으로 일시정지가 해제하기 위한 연산
            Time.timeScale += time;                                //타임스케일의 값을 타임만큼 더하여 시간을 흐르게 하기

            if (Time.timeScale >= 1)                               //타임스케일의 값이 1과 같거나 1보다 클 경우 실행
            {
                fade.SetActive(false);                             //페이드 비활성화
                Time.timeScale = 1.0f;                             //정상적인 게임속도를 위해 타임스케일에 1 대입
                time = 0;                                          //타임 값 0으로 초기화
                upgradePanel.SetActive(false);                      //무기 능력 선택 판넬 비활성화
                return;                                            //리턴
            }
            else
                Invoke(nameof(TimeSlowUp), 0.06f);                       //인보크로 타임슬로우업을 0.06초의 딜레이를 가지고 실행
        }
    }

    //무기 업그레이드 레벨 별로 함수를 작성하거나 스위치 문을 활용하여 하나의 함수로 사용
    //public void trumpEvent_0()                                              //미사일 능력
    //{
    //    trumpData.damage += (trumpData.baseDamage / 100) * 30;            //미사일 데미지 증가
    //    mStar_0[trump_Lv_0].sprite = starSprite;                            //미사일 능력의 별 이미지 변경
    //    trump_Lv_0++;                                                       //미사일 레벨 증가
    //}
}
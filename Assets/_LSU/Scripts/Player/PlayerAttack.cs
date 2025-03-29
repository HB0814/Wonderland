using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public WeaponPool weaponPool; //오브젝트 풀링 스크립트

    //트럼프
    [Header("Trump")]
    public int trumpIndex = 0; //트럼트 배열 인덱스 값
    public int trumpCount = 1; //트럼프 개수
    public float trumpCooldown; //트럼프 쿨타임
    public float trumpTimer = 0.0f; //트럼프 쿨타임 타이머
    public bool hasTrump = true; //트럼프 무기 해금 여부
    public bool isTrump = false; //트럼프 공격 여부
    public float randomRot = 0.0f; //트럼프 회전 각도

    //채셔캣
    [Header("CheshireCat")]
    public int cheshireCatIndex = 0; //채셔캣 배열 인덱스 값
    public int cheshireCatCount = 1; //채셔캣 개수
    public float cheshireCatCooldown; //채셔캣 쿨타임
    public float cheshireCatTimer = 0.0f; //채셔캣 쿨타임 타이머
    public bool hasCheshireCat = true; //채셔캣 무기 해금 여부
    public bool isCheshireCat = false; //채셔캣 공격 여부

    //안생일 축하 폭죽
    [Header("NonBrithdayFirecracker")]
    public int firecrackerIndex = 0; //채셔캣 배열 인덱스 값
    public int firecrackerCount = 1; //채셔캣 개수
    public float firecrackerCooldown; //채셔캣 쿨타임
    public float firecrackerTimer = 0.0f; //채셔캣 쿨타임 타이머
    public bool hasFirecracker = true; //채셔캣 무기 해금 여부
    public bool isFirecracker = false; //채셔캣 공격 여부

    //사과
    [Header("Apple")]
    public int appleIndex = 0; //채셔캣 배열 인덱스 값
    public int appleCount = 1; //채셔캣 개수
    public float appleCooldown; //채셔캣 쿨타임
    public float appleTimer = 0.0f; //채셔캣 쿨타임 타이머
    public bool hasApple = true; //채셔캣 무기 해금 여부
    public bool isApple = false; //채셔캣 공격 여부

    private void Update()
    {
        //트럼프 공격
        if (hasTrump) //트럼프 무기 소유 여부 체크
        {
            trumpTimer += Time.deltaTime; //트럼프 쿨타임 타이머 시간 증가

            if (trumpTimer >= trumpCooldown) //쿨타임 종료
            {
                trumpTimer = 0.0f; //쿨타임 초기화
                for (int i = 0; i < trumpCount; i++) //트럼프 개수 체크
                {
                    weaponPool.trumpCardScript[trumpIndex].SetRotation(); //인덱스 값의 트럼프 카드 회전 함수 실행
                    weaponPool.trumpCardScript[trumpIndex].transform.position = transform.position;

                    weaponPool.trumpObjects[trumpIndex].SetActive(true); //인덱스 값의 트럼프 카드 활성화

                    trumpIndex++; //트럼프 인덱스 값 증가

                    if (trumpIndex >= weaponPool.trumpObjects.Length) //트럼프 인덱스 값 길이 초과 체크
                        trumpIndex = 0; //트럼프 인덱스 값 초기화
                }
            }
        }

        //채셔캣 공격
        if (hasCheshireCat) //채셔캣 무기 소유 여부 체크
        {
            cheshireCatTimer += Time.deltaTime; //채셔캣 쿨타임 타이머 시간 증가

            if (cheshireCatTimer >= cheshireCatCooldown) //쿨타임 종료
            {
                cheshireCatTimer = 0.0f; //쿨타임 초기화
                for (int i = 0; i < cheshireCatCount; i++) //채셔캣 개수 체크
                {
                    weaponPool.cheshireCatScript[cheshireCatIndex].SetRotation(); //채셔캣 회전
                    weaponPool.cheshireCatScript[cheshireCatIndex].SetAttackPos(); //채셔캣 공격 위치 설정
                    weaponPool.cheshireCatObjects[cheshireCatIndex].SetActive(true); //인덱스 값의 채셔캣 활성화
                    
                    cheshireCatIndex++; //채셔캣 인덱스 값 증가

                    if (cheshireCatIndex >= weaponPool.cheshireCatObjects.Length) //채셔캣 인덱스 값 길이 초과 체크
                        cheshireCatIndex = 0; //채셔캣 인덱스 값 초기화
                }
            }
        }

        //안생일 축하 폭죽 공격
        if (hasFirecracker) //폭죽 무기 소유 여부 체크
        {
            firecrackerTimer += Time.deltaTime; //폭죽 쿨타임 타이머 시간 증가

            if (firecrackerTimer >= firecrackerCooldown) //쿨타임 종료
            {
                firecrackerTimer = 0.0f; //쿨타임 초기화
                for (int i = 0; i < firecrackerCount; i++) //폭죽 개수 체크
                {
                    weaponPool.firecrackerScript[firecrackerIndex].SetAttackPos(); //폭죽 랜덤 위치
                    weaponPool.firecrackerObjects[firecrackerIndex].SetActive(true); //인덱스 값의 폭죽 활성화

                    firecrackerIndex++; //폭죽 인덱스 값 증가

                    if (firecrackerIndex >= weaponPool.firecrackerObjects.Length) //폭죽인덱스 값 길이 초과 체크
                        firecrackerIndex = 0; //폭죽 인덱스 값 초기화
                }
            }
        }

        //사과 공격
        if (hasApple) //사과 무기 소유 여부 체크
        {
            appleTimer += Time.deltaTime; //사과 쿨타임 타이머 시간 증가

            if (appleTimer >= appleCooldown) //쿨타임 종료
            {
                appleTimer = 0.0f; //쿨타임 초기화
                for (int i = 0; i < appleCount; i++) //사과 개수 체크
                {
                    weaponPool.appleScript[appleIndex].SetRotation(); //사과 회전
                    weaponPool.appleScript[appleIndex].SetRandomPos(); //사과 랜덤 위치 설정 함수
                    weaponPool.appleScript[appleIndex].SetAttackDir(); //사과의 공격방향 설정 함수
                    weaponPool.appleObjects[appleIndex].SetActive(true); //인덱스 값의 사과 활성화

                    appleIndex++; //사과 인덱스 값 증가

                    if (appleIndex >= weaponPool.appleObjects.Length) //사과 인덱스 값 길이 초과 체크
                        appleIndex = 0; //사과 인덱스 값 초기화
                }
            }
        }
    }
}

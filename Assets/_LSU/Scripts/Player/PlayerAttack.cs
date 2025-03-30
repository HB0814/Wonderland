using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public WeaponPool weaponPool; //오브젝트 풀링 스크립트

    [System.Serializable]
    public class Info
    {
        public int index = 0; //배열 인덱스 값
        public int count = 1; //공격 개수
        public float cooldown; //쿨타임
        public float timer = 0.0f; //쿨타임 타이머
        public bool hasWeapon= true; // 무기 해금 여부
        public bool isAttcak = false; //공격 여부
        public float randomRot = 0.0f; //공격 회전 각도
    }

    public Info trump;
    public Info cheshireCat;
    public Info firecracker;
    public Info apple;

    private void Update()
    {
        Attack_TrumpCard(); //트럼프 공격
        Attack_CheshireCat(); //채셔캣 공격
        Attack_Firecracker(); //안생일 축하 폭죽 공격
        Attack_Apple(); //사과 공격
    }

    //트럼프 카드 공격
    void Attack_TrumpCard()
    {
        //트럼프 공격
        if (trump.hasWeapon) //트럼프 무기 소유 여부 체크
        {
            trump.timer += Time.deltaTime; //트럼프 쿨타임 타이머 시간 증가

            if (trump.timer >= trump.cooldown) //쿨타임 종료
            {
                trump.timer = 0.0f; //쿨타임 초기화
                for (int i = 0; i < trump.count; i++) //트럼프 개수 체크
                {
                    weaponPool.trumpCardScript[trump.index].SetRotation(); //인덱스 값의 트럼프 카드 회전 함수 실행
                    weaponPool.trumpCardScript[trump.index].transform.position = transform.position;

                    weaponPool.trumpPool.objects[trump.index].SetActive(true); //인덱스 값의 트럼프 카드 활성화

                    trump.index++; //트럼프 인덱스 값 증가

                    if (trump.index >= weaponPool.trumpPool.objects.Length) //트럼프 인덱스 값 길이 초과 체크
                        trump.index = 0; //트럼프 인덱스 값 초기화
                }
            }
        }
    }

    //채셔캣 공격
    void Attack_CheshireCat()
    {
        //채셔캣 공격
        if (cheshireCat.hasWeapon) //채셔캣 무기 소유 여부 체크
        {
            cheshireCat.timer += Time.deltaTime; //채셔캣 쿨타임 타이머 시간 증가

            if (cheshireCat.timer >= cheshireCat.cooldown) //쿨타임 종료
            {
                cheshireCat.timer = 0.0f; //쿨타임 초기화
                for (int i = 0; i < cheshireCat.count; i++) //채셔캣 개수 체크
                {
                    weaponPool.cheshireCatScript[cheshireCat.index].SetRotation(); //채셔캣 회전
                    weaponPool.cheshireCatScript[cheshireCat.index].SetAttackPos(); //채셔캣 공격 위치 설정
                    weaponPool.cheshireCatPool.objects[cheshireCat.index].SetActive(true); //인덱스 값의 채셔캣 활성화

                    cheshireCat.index++; //채셔캣 인덱스 값 증가

                    if (cheshireCat.index >= weaponPool.cheshireCatPool.objects.Length) //채셔캣 인덱스 값 길이 초과 체크
                        cheshireCat.index = 0; //채셔캣 인덱스 값 초기화
                }
            }
        }
    }

    //안생일 축하 폭죽 공격
    void Attack_Firecracker()
    {
        //안생일 축하 폭죽 공격
        if (firecracker.hasWeapon) //폭죽 무기 소유 여부 체크
        {
            firecracker.timer += Time.deltaTime; //폭죽 쿨타임 타이머 시간 증가

            if (firecracker.timer >= firecracker.cooldown) //쿨타임 종료
            {
                firecracker.timer = 0.0f; //쿨타임 초기화
                for (int i = 0; i < firecracker.count; i++) //폭죽 개수 체크
                {
                    weaponPool.firecrackerScript[firecracker.index].SetAttackPos(); //폭죽 랜덤 위치
                    weaponPool.firecrackerPool.objects[firecracker.index].SetActive(true); //인덱스 값의 폭죽 활성화

                    firecracker.index++; //폭죽 인덱스 값 증가

                    if (firecracker.index >= weaponPool.firecrackerPool.objects.Length) //폭죽인덱스 값 길이 초과 체크
                        firecracker.index = 0; //폭죽 인덱스 값 초기화
                }
            }
        }
    }

    //사과 공격
    void Attack_Apple()
    {
        //사과 공격
        if (apple.hasWeapon) //사과 무기 소유 여부 체크
        {
            apple.timer += Time.deltaTime; //사과 쿨타임 타이머 시간 증가

            if (apple.timer >= apple.cooldown) //쿨타임 종료
            {
                apple.timer = 0.0f; //쿨타임 초기화
                for (int i = 0; i < apple.count; i++) //사과 개수 체크
                {
                    weaponPool.appleScript[apple.index].SetRotation(); //사과 회전
                    weaponPool.appleScript[apple.index].SetRandomPos(); //사과 랜덤 위치 설정 함수
                    weaponPool.appleScript[apple.index].SetAttackDir(); //사과의 공격방향 설정 함수
                    weaponPool.applePool.objects[apple.index].SetActive(true); //인덱스 값의 사과 활성화

                    apple.index++; //사과 인덱스 값 증가

                    if (apple.index >= weaponPool.applePool.objects.Length) //사과 인덱스 값 길이 초과 체크
                        apple.index = 0; //사과 인덱스 값 초기화
                }
            }
        }
    }
}

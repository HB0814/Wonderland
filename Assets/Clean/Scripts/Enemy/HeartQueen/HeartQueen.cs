using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HeartQueen : Enemy
{
    private enum BossPattern //보스 패턴
    {
        None, //아무것도 아님
        MovedGuillotine, //이동형 길로틴 패턴
        FixedGuillotine, //고정형 길로틴 패턴
        //OnGavel, //의사봉 패턴
    }
    private BossPattern currentPattern = BossPattern.None;
    private float patternphase1_Cooltimes = 5f;
    private float patternTimer = 0f;
    private bool isPatternExecuting = false;

    private Dictionary<BossPattern, float> phase1_phase1_Cooltimes = new();
    private Dictionary<BossPattern, float> phase2_phase1_Cooltimes = new();
    private Dictionary<BossPattern, float> patternTimers = new();

    [SerializeField] Bounds moveBounds; //이동 범위 > 이동 범위를 설정하기 위함
    Vector2 targetPos; //목표위치
    float moveTime = 0.0f; //이동 시간
    float moveTimer = 0.0f; //이동 타이머

    public bool canMove = true; //이동가능 여부 => 이동가능 여부체크를 애니메이션 bool CanMove로 체크

    [Header("보스 특수 속성")]
    public float phaseChangeHealth = 0.5f;  //페이즈 전환 체력 비율
    public float summonphase1_Cooltimes = 5f;      //소환 쿨다운
    public float specialAttackphase1_Cooltimes = 10f; //특수 공격 쿨다운
    public GameObject minionPrefab;         //소환할 미니언 프리팹

    [Header("길로틴")]
    public MovedGuillotione movedGuillotione; //이동형 길로틴 스크립트
    public FixedGuillotione[] fixedGuillotiones; //고정형 길로틴 스크립트
    public GameObject[] fixedWarn; //고정형 길로틴 경고 오브젝트

    Camera cam; //카메라

    public Gavel gavels; //의사봉 스크립트

    private bool isPhase2 = false; //두번째 페이즈 활성화 여부
    private float nextSummonTime; //다음 소환 시간
    private float nextSpecialAttackTime; //다음 특수 공격 시간

    WaitForSeconds patternDelay; //패턴 별 딜레이 시간

    protected override void Start()
    {
        base.Start();
        cam = Camera.main;

        SetTargetPosition(); //목표위치 설정
        nextSummonTime = Time.time + summonphase1_Cooltimes;
        nextSpecialAttackTime = Time.time + specialAttackphase1_Cooltimes;
        InitializePhase_phase1_Cooltimes();

    }

    //페이즈, 패턴 별 쿨다운
    private void InitializePhase_phase1_Cooltimes()
    {
        phase1_phase1_Cooltimes[BossPattern.MovedGuillotine] = 6f;
        phase1_phase1_Cooltimes[BossPattern.FixedGuillotine] = 8f;
        //phase1_phase1_Cooltimes[BossPattern.OnGavel] = 10f;

        phase2_phase1_Cooltimes[BossPattern.MovedGuillotine] = 4f;
        phase2_phase1_Cooltimes[BossPattern.FixedGuillotine] = 6f;
        //phase2_phase1_Cooltimes[BossPattern.OnGavel] = 7f;


        foreach (var pattern in phase1_phase1_Cooltimes.Keys)
        {
            patternTimers[pattern] = 0f;
        }
    }

    private new void Update()
    {
        if(Input.GetKeyDown(KeyCode.V)) //테스트용 v키 입력 시 >>> 보스 위치 초기화
            SetBoundsCenter(); //위치가 초기화, 바운즈 센터 값 설정

        moveTimer += Time.deltaTime; //이동 타이머 증가

        if (animator.GetBool("canMove")) //이동 가능할 때
        {
            MoveToTarget();
        }

        UpdateSpriteLayer(); //스프라이트 레이어 업데이트

        foreach (var key in patternTimers.Keys.ToList())
        {
            patternTimers[key] += Time.deltaTime;
        }

        if (!isPatternExecuting)
        {
            patternTimer += Time.deltaTime;
            if (patternTimer >= patternphase1_Cooltimes)
            {
                patternTimer = 0f;
                SelectRandomPattern();
            }
        }
    }
    private void SelectRandomPattern()
    {
        List<BossPattern> availablePatterns = new();

        foreach (var pattern in phase1_phase1_Cooltimes.Keys)
        {
            if (patternTimers[pattern] >= phase1_phase1_Cooltimes[pattern])
            {
                availablePatterns.Add(pattern);
            }
        }

        if (availablePatterns.Count == 0) 
            return; // 아직 쿨타임 지난 패턴이 없음

        BossPattern selected = availablePatterns[Random.Range(0, availablePatterns.Count)];
        currentPattern = selected;

        switch (selected)
        {
            case BossPattern.MovedGuillotine:
                StartCoroutine(PatternDelay("MovedGuillotine", 1.2f));
                break;

            case BossPattern.FixedGuillotine:
                StartCoroutine(PatternDelay("FixedGuillotine", 1.5f));
                break;

            //case BossPattern.OnGavel:
            //    StartCoroutine(PatternDelay("OnGavel", 2.0f));
            //    break;
        }

        patternTimers[selected] = 0f; // 쿨타임 초기화
        isPatternExecuting = true; //패턴
    }

    //패턴 별 딜레이
    private IEnumerator PatternDelay(string pattern, float delay) // 패턴 이름, 딜레이 시간
    {
        patternDelay = new WaitForSeconds(delay); //패턴 별 딜레이 시간 변경

        animator.SetTrigger(pattern); //패턴 별 트리거 활성화
        animator.SetBool("canMove", false); //걷기 애니메이션 비활성화

        switch(pattern)
        {
            case "MovedGuillotine":
                yield return patternDelay; //패턴 딜레이
                movedGuillotione.gameObject.SetActive(true); //길로틴 활성화
                break;

            case "FixedGuillotine":
                //활성화되어있는 길로틴 비활성화
                for (int i = 0; i < fixedGuillotiones.Length; i++)
                {
                    //fixedGuillotiones[i].Set();
                    fixedGuillotiones[i].gameObject.SetActive(false);
                }

                yield return patternDelay; //패턴 딜레이 -> 하트여왕의 전조 애니메이션 재생 중

                Camera cam = Camera.main; //메인카메라
                Vector2 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, 0)); //카메라 화면의 좌표값 - 좌측하단
                Vector2 topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)); //카메라 화면의 좌표값 - 우측 상단

                List<Vector2> placedPos = new List<Vector2>(); //길로틴 위치 저장 리스트
                float minDistance = 2.5f; //길로틴 생성 시 최소 간격

                for (int i = 0; i < fixedGuillotiones.Length; i++)
                {
                    Vector2 randomPos; //랜덤 위치
                    int attempts = 0; //시도 횟수
                    bool posValid; //유효한 위치 여부

                    do
                    {
                        float x = Random.Range(bottomLeft.x, topRight.x); //x 좌표
                        float y = Random.Range(bottomLeft.y, topRight.y); //y 좌표
                        randomPos = new Vector2(x, y);

                        posValid = true; //유효한 위치임

                        foreach (Vector2 placed in placedPos)
                        {
                            //랜덤 위치와 최소 간격 비교, 최소 간격보다 좁을 시 실행
                            if (Vector2.Distance(randomPos, placed) < minDistance)
                            {
                                posValid = false; //유효한 위치가 아님
                                break;
                            }
                        }

                        attempts++; //시도 횟수 증가
                        //길로틴 생성을 너무 많이 시도하지 않도록 하기 위한 조건문
                        if (attempts > 100)
                        {
                            break;
                        }

                    } while (!posValid); //유효한 위치가 아닐 동안 반복

                    placedPos.Add(randomPos); //길로틴 위치 리스트에 해당 랜덤 위치 값 추가

                    //경고 위치 및 활성화
                    fixedWarn[i].transform.position = randomPos;
                    fixedWarn[i].SetActive(true);

                    //길로틴 위치 설정
                    fixedGuillotiones[i].transform.position = randomPos;
                }

                yield return patternDelay; //딜레이 -> 경고 표시 애니메이션 종료 후 길로틴 생성

                for (int i = 0; i < fixedGuillotiones.Length; i++)
                {
                    fixedWarn[i].SetActive(false); //경고 비활성화
                    fixedGuillotiones[i].gameObject.SetActive(true); //길로틴 활성화
                }

                break;

            case "OnGavel":
                yield return patternDelay;
                gavels.Init(_player, 3.0f);
                break;
        }

        isPatternExecuting = false;
        currentPattern = BossPattern.None;
        ReturnToWalk(); //걷기 상태로 전환
    }

    //다시 걷기 애니메이션 실행
    public void ReturnToWalk()
    {
        animator.SetBool("canMove", true);
    }

    //바운즈 센터 값 설정 -> 보스 몬스터 스폰 시 실행되게 끔.
    private void SetBoundsCenter()
    {
        //보스의 위치를 플레이어 위에서 나타나게 설정
        transform.position = new Vector3(player.transform.position.x,
                                        player.transform.position.y + 4.0f,
                                        transform.position.z);

        //바운즈의 센터를 플레이어 위치로 설정
        moveBounds.center = new Vector3(player.transform.position.x, 
                                        player.transform.position.y,
                                        player.transform.position.z);
    }

    //보스가 이동할 목표지점 설정
    private void SetTargetPosition()
    {
        //목표 위치를 랜덤하게 설정
        targetPos = new Vector2(Random.Range(moveBounds.min.x, moveBounds.max.x),
                                Random.Range(moveBounds.min.y, moveBounds.max.y));
        //플립
        bool shouldFlip = targetPos.x > transform.position.x;
        if (shouldFlip != spriteRenderer.flipX)
        {
            spriteRenderer.flipX = shouldFlip;
        }
    }

    //목표지점으로 이동
    private void MoveToTarget()
    {
        if (!animator.GetBool("canMove"))
            return;

        Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPos, moveSpeed * Time.fixedDeltaTime); //현재 위치에서 목표 위치로 속도에 따라 이동
        rb.MovePosition(newPosition); //리지드바디 이동

        if (Vector2.Distance(rb.position, targetPos) < 0.1f || moveTimer > moveTime) //목표 위치에 도달하면 새로운 목표 설정
        {
            moveTimer = 0.0f;
            moveTime = Random.Range(2.0f, 5.0f);
            SetTargetPosition(); //새로운 목표위치 설정
        }
    }

    private void EnterPhase2()
    {
        isPhase2 = true;

        // 페이즈2 쿨타임 덮어쓰기
        foreach (var pattern in phase2_phase1_Cooltimes.Keys)
        {
            if (phase1_phase1_Cooltimes.ContainsKey(pattern))
            {
                phase1_phase1_Cooltimes[pattern] = phase2_phase1_Cooltimes[pattern];
            }
        }

    }

    private void SummonMinions()
    {
        if (minionPrefab != null)
        {
            // 미니언 소환 로직 구현
            for (int i = 0; i < 3; i++)
            {
                Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * 2f;
                Instantiate(minionPrefab, spawnPos, Quaternion.identity);
            }
        }
    }
}

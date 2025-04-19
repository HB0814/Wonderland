using System.Collections;
using UnityEngine;
using static EnemyManager;
using static UnityEditor.Experimental.GraphView.GraphView;


public abstract class Enemy : MonoBehaviour
{
    WaitForSeconds hitCool = new(0.1f);
    WaitForSeconds knockbackCool = new(0.1f);

    [Header("기본 속성")]
    public float maxHealth = 100.0f;
    public float moveSpeed = 2.0f;
    public float deffense = 0.0f;
    public float attackDamage = 10.0f;
    public float attackRange = 0.1f;
    public float attackCooldown = 1.0f;
    public float knockbackResistance = 0.5f;

    [Header("드롭 아이템")]
    protected GameObject[] dropItems;
    protected float dropChance = 0.3f;
    protected GameObject expGemPrefab;

    public float currentHealth;
    public float nextAttackTime;
    public bool isKnockback;
    public float knockbackRecoveryTimer;
    public bool isSlowed;
    public float slowEffectTimer;
    public float currentSlowAmount;
    public float originalMoveSpeed;

    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    protected Color originalColor;
    protected GameObject player;
    protected Player _player;
    protected Animator animator;

    // 스프라이트 업데이트 관련 변수
    protected float lastUpdateTime = 0.0f;
    protected bool isVisible = true;
    protected float marginArea = 0.1f;

    //초기화
    public virtual void Initialize()
    {
        currentHealth = maxHealth;
        originalMoveSpeed = moveSpeed;
        nextAttackTime = 0.0f;
        isKnockback = false;
        knockbackRecoveryTimer = 0.0f;
        isSlowed = false;
        slowEffectTimer = 0.0f;
        currentSlowAmount = 0.0f;

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        _player = player.GetComponent<Player>(); //최적화를 위해 플레이어 스크립트를 해당 부분에서 가져오기

        if (rb != null)
        {
            rb.gravityScale = 0.0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    protected virtual void Start()
    {
        Initialize(); //초기화
    }

    protected virtual void Update()
    {
        if (isKnockback)
        {

        }
        else //넉백 시 이동x
        {
            float dis = (transform.position - player.transform.position).sqrMagnitude; //플레이어와의 거리 계산

            if (dis > attackRange * attackRange) //추격 범위와의 비교를 통해 이동 여부
            {
                MoveTowardsPlayer();
            }
            else
            {
                rb.linearVelocity = Vector2.zero; //이동 정지
            }
        }

        UpdateSprite();
    }

    //스프라이트 관련 함수
    protected virtual void UpdateSprite()
    {
        UpdateSpriteLayer(); // 스프라이트 레이어 업데이트
        if (Time.time - lastUpdateTime >= 0.1f) //0.1초 정도의 딜레이
        {
            lastUpdateTime = Time.time;
            UpdateSpriteFlip();
            spriteRenderer.enabled = IsVisible(); //렌더링
        }
    }
    //스프라이트 렌더러 레이어 업데이트
    protected virtual void UpdateSpriteLayer()
    {
        if (spriteRenderer != null)
        {
            // Y 좌표가 낮을수록(화면 아래) 더 앞에 표시
            spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
        }
    }
    //스프라이트 플립 업데이트
    protected virtual void UpdateSpriteFlip()
    {
        if (spriteRenderer != null && player != null)
        {
            bool shouldFlip = player.transform.position.x < transform.position.x;
            if (shouldFlip != spriteRenderer.flipX)
            {
                spriteRenderer.flipX = shouldFlip;
            }
        }
    }
     //스파라이트 렌더링 기능
    protected virtual bool IsVisible()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position); //월드 좌표를 뷰포트로 변환하여 범위 내 오브젝트 위치 확인
        bool newVisibility = screenPoint.x > -marginArea && screenPoint.x < 1 + marginArea &&
                             screenPoint.y > -marginArea && screenPoint.y < 1 + marginArea;
        //화면 내에 있는지 여부 확인

        if (isVisible != newVisibility) //화면 내 여부 변경 확인
        {
            isVisible = newVisibility; //상태 업데이트
            spriteRenderer.enabled = isVisible; //렌더러 상태 변경
        }

        return isVisible; //상태 반환
    }

    //플레이어 공격
    public virtual void Attack()
    { 
        if (_player != null)
        {
            _player.TakeDamage(attackDamage);
            Debug.Log("플레이어가 피해를 입음!");
        }
    }

    //플레이어 방향으로 이동
    protected virtual void MoveTowardsPlayer()
    {
        if (player != null && !isKnockback)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
    }

    //무기 피격
    public virtual void TakeDamage(float damage, float knockbackForce, float slowForce, float slowDuration)
    {
        damage -= deffense; //damage = (damage * 플레이어 공격력 * 0.01) - 적 방어력
        float totalDamage = Mathf.Max(1, damage);
        currentHealth -= totalDamage;

        Debug.Log($"몬스터가 {totalDamage}의 데미지를 입음");

        knockbackForce -= knockbackResistance;
        if(knockbackForce > 0 && !isKnockback)
        {
            StartCoroutine(ApplyKnockback(knockbackForce));
        }

        if(gameObject.activeSelf)
            StartCoroutine(HitColor());

        if (slowForce > 0)
        {
            StartCoroutine(ApplySlow(slowForce, slowDuration));
        }

        if(currentHealth <= 0.0f)
        {
            Die();
        }
    }

    private IEnumerator ApplyKnockback(float knockbackForce)
    {
        isKnockback = true;
        Vector3 knockbackDir = transform.position - player.transform.position;
        rb.AddForce(knockbackDir.normalized * knockbackForce, ForceMode2D.Impulse);
        yield return knockbackCool;
        rb.linearVelocity = Vector2.zero;
        isKnockback = false;
    }

    private IEnumerator ApplySlow(float slowForce, float slowDuration)
    {
        float baseSpeed = moveSpeed;
        moveSpeed *= (1 - slowForce);
        yield return slowDuration;

        moveSpeed = baseSpeed;
    }

    //무기 피격 효과
    protected virtual System.Collections.IEnumerator HitColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return hitCool;
            spriteRenderer.color = originalColor;
        }
    }

    //죽음    //풀링 변경 필요
    protected virtual void Die()
    {
        StopAllCoroutines();

        //if (expGemPrefab != null)
        //{
        //    Instantiate(expGemPrefab, transform.position, Quaternion.identity);
        //}

        if (dropItems != null && dropItems.Length > 0 && Random.value <= dropChance)
        {
            int randomIndex = Random.Range(0, dropItems.Length);
            Instantiate(dropItems[randomIndex], transform.position, Quaternion.identity);
        }

        ParticleSystem deathEffect = GetComponent<ParticleSystem>();
        if (deathEffect != null)
        {
            deathEffect.Play();
        }

        CreateExpgem();

        spriteRenderer.color = originalColor;
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    protected virtual void CreateExpgem()
    {
        string rate;
        
        GameObject expgemToSpawn = ObjectPool.Instance.SpawnFromPool_Expgem("Common", transform.position);

        if (expgemToSpawn != null)
        {
            // 초기화 및 활성화
            expgemToSpawn.SetActive(true);
            Debug.Log("몬스터 활성화");
        }
    }

    protected virtual void OnEnable()
    {
        Initialize();
        Debug.Log("몬스터 스탯 재설정");
    }

    //공격 범위 표시
    //protected virtual void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, attackRange);
    //}

} 
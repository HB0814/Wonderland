using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("기본 속성")]
    private float projectileDamage = 10f;
    private float projectileLifeTime = 1f;
    private float projectileSize = 1f;
    private float projectileSpeed = 10f;

    [Header("방향")]
    private Vector3 direction;

    [Header("부가 효과")]
    private float knockbackForce = 1; // 넉백 강도
    private float slowForce = 3; // 슬로우 강도
    private float slowDuration = 1; // 슬로우 지속시간

    [Header("관통 속성")]
    public int maxPierceCount = 0; // 최대 관통 수
    public int currentPierceCount = 0; // 현재 관통 수

    [Header("모자 부메랑 속성")]
    private float returnSpeed = 12f;
    private float maxDistance = 4f;
    private bool isReturning = false;
    private Transform playerTransform;
    private Transform playerCenter;
    private HatBoomerangWeapon hatWeapon;
    private Vector3 initialPosition; // 모자의 초기 위치 저장

    public WeaponType weaponType;
    public WeaponData WeaponData { get; set; }
    
    private float currentLifeTime;
    private bool isActive = false;
    private Rigidbody2D rb;

    private void Start()
    {
        WeaponData = WeaponDataManager.Instance.GetWeaponData(weaponType);
        rb = GetComponent<Rigidbody2D>();
        if(weaponType == WeaponType.Hat)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            hatWeapon = playerTransform.GetComponentInChildren<HatBoomerangWeapon>();
        }
        playerCenter = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform;

        if (weaponType != WeaponType.Apple) 
            transform.position = playerCenter.position;
    }

    public virtual void BaseInitialize(float damage, float size, float lifeTime, float speed)
    {
        this.projectileDamage = damage;
        this.projectileSize = size;
        this.projectileLifeTime = lifeTime;
        this.projectileSpeed = speed;

        currentLifeTime = lifeTime;
        isActive = true;
        initialPosition = transform.position; // 초기 위치 저장

        transform.localScale = new Vector3(projectileSize, projectileSize, projectileSize);
    }

    public virtual void DebuffInitialize(float knockbackForce, float slowForce, float slowDuration)
    {
        this.knockbackForce = knockbackForce;
        this.slowForce = slowForce;
        this.slowDuration = slowDuration;
    }

    public virtual void PierceInitialize(int pierceCount)
    {
        this.maxPierceCount = pierceCount;
        this.currentPierceCount = 0;
    }

    protected virtual void Update()
    {
        if (!isActive) return;

        currentLifeTime -= Time.deltaTime;

        if (IsOutOfCamera())
        {
            Deactivate();
            return;
        }

        if (currentLifeTime <= 0 || currentPierceCount > maxPierceCount)
        {
            Deactivate();
        }
        else 
        {
            Move(direction);
        }
    }

    protected virtual void Deactivate()
    {
        isReturning=false;
        isActive = false;
        gameObject.SetActive(false);
    }
    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public void Move(Vector3 dir)
    {
        switch(weaponType)
        {
            case WeaponType.Apple:
                // 사과의 굴러가는 이동 (글로벌 좌표계 사용)
                transform.position += dir * projectileSpeed * Time.deltaTime;
                break;

            case WeaponType.Book:
                // 책의 직선 이동 (글로벌 좌표계 사용)
                transform.position += dir * projectileSpeed * Time.deltaTime;
                break;

            case WeaponType.Hat:
                if (!isReturning)
                {
                    // 앞으로 날아가는 중
                    transform.position += dir * projectileSpeed * Time.deltaTime;
                    
                    // 최대 거리 도달 체크
                    if (Vector3.Distance(transform.position, playerCenter.position) >= maxDistance)
                    {
                        isReturning = true;
                    }
                }
                else
                {
                    transform.position += (-dir) * returnSpeed * Time.deltaTime;

                    // 플레이어와의 거리 체크
                    float distanceToPlayer = Vector3.Distance(transform.position, playerCenter.transform.position);
                    if (distanceToPlayer < 0.5f)
                    {
                        // 쿨타임 감소 적용
                        if (hatWeapon != null)
                        {
                            hatWeapon.ReduceCooldown();
                        }
                        Deactivate();
                    }
                }
                break;

            default:
                // 일반 투사체 이동 (글로벌 좌표계 사용)
                transform.position += dir * projectileSpeed * Time.deltaTime;
                break;
        }
    }

    //화면 밖 판별 함수
    private bool IsOutOfCamera()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        return viewportPos.x < -0.15f || viewportPos.x > 1.15f ||
               viewportPos.y < -0.15f || viewportPos.y > 1.15f;
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if(weaponType != WeaponType.Book) return;
    //    if (other.CompareTag("Enemy"))
    //    {
    //            currentPierceCount++;
    //    }
    //}
} 
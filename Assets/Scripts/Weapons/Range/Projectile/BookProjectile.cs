using UnityEngine;

public class BookProjectile : Projectile
{
    [Header("책 특수 속성")]
    [SerializeField]public float rotationSpeed = 720f; // 초당 회전 속도 (도)
    [SerializeField]public float maxRotationTime = 0.5f; // 최대 회전 시간
    [SerializeField]public float minRotationSpeed = 180f; // 최소 회전 속도
    private float currentRotationTime = 0f;
    private float initialRotationSpeed;

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        currentRotationTime = 0f;
        rotationSpeed = initialRotationSpeed;
        poolTag = "BookProjectile";
    }

    protected override void Update()
    {
        base.Update();
        
        // 회전 속도 점진적 감소
        if (currentRotationTime < maxRotationTime)
        {
            currentRotationTime += Time.deltaTime;
            float t = currentRotationTime / maxRotationTime;
            rotationSpeed = Mathf.Lerp(initialRotationSpeed, minRotationSpeed, t);
        }
        
        // 회전 적용
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 적과 충돌 시 추가 효과
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                gameObject.SetActive(false);
            }
        }
    }
} 
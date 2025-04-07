using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class RollAppleProjectile : MonoBehaviour
{
    // 이동 관련 변수
    private Vector3 direction;
    private float projectileSpeed = 8.0f;

    // 회전 관련 변수
    private float randomRotation;

    // 무기 속성
    private float damage;
    private float knockbackForce;

    private void Update()
    {
        Move();
    }

    // 사과 이동 함수
    private void Move()
    {
        transform.position += direction * projectileSpeed * Time.deltaTime;
    }

    // 회전 각도 설정 함수
    public void SetRotation()
    {
        randomRotation = math.floor(Random.Range(-180.0f, 180.0f) * 10) * 0.1f;
        transform.rotation = Quaternion.Euler(0, 0, randomRotation);
    }

    // 공격 방향 설정
    public void SetDirection(Vector3 targetPosition)
    {
        Vector3 distance = targetPosition - transform.position;
        direction = distance.normalized;
    }

    // 무기 속성 설정
    public void SetWeaponProperties(float _damage, float _knockbackForce)
    {
        damage = _damage;
        knockbackForce = _knockbackForce;
    }

    // 화면에서 벗어날 때 호출되는 함수
    private void OnBecameInvisible()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        gameObject.SetActive(false);
    }

    // 충돌 처리
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
               // enemy.TakeDamage(damage);
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                //enemy.ApplyKnockback(knockbackDirection, knockbackForce);
            }
        }
    }
} 
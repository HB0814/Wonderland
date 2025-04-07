using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    [Header("기본 속성")]
    public float damage = 10f;
    public float duration = 1f;
    public float radius = 1f;
    public LayerMask targetLayer;
    public GameObject hitEffect;

    [SerializeField]protected float currentDuration;
    protected bool isActive = false;

    public virtual void Initialize(float damage, float duration, float radius)
    {
        this.damage = damage;
        this.duration = duration;
        this.radius = radius;
        currentDuration = duration;
        isActive = true;
    }

    protected virtual void Update()
    {
        if (!isActive) return;

        currentDuration -= Time.deltaTime;
        if (currentDuration <= 0)
        {
            Deactivate();
        }
    }

    protected virtual void Deactivate()
    {
        isActive = false;
        gameObject.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;

        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                //enemy.TakeDamage(damage);
                SpawnHitEffect();
            }
        }
    }

    protected virtual void SpawnHitEffect()
    {
        if (hitEffect != null)
        {
            GameObject effect = WeaponManager.Instance.SpawnProjectile("HitEffect", transform.position, Quaternion.identity);
            if (effect != null)
            {
                effect.transform.localScale = Vector3.one * radius;
            }
        }
    }
} 
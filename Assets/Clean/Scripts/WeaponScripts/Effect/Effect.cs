using UnityEngine;

public class Effect : MonoBehaviour
{    
    [Header("기본 속성")]
    public float effectDamage = 10f;
    public float effectLifeTime = 1f;
    public float effectSize = 1f;

    [Header("부가 효과")]
    public float knockbackForce = 1; // 넉백 강도
    public float slowForce = 3; // 슬로우 강도
    public float slowDuration = 1; // 슬로우 지속시간

    public WeaponType weaponType;
    public WeaponData WeaponData { get; set; }
    
    private float currentLifeTime;
    private bool isActive = false;

    private void Start()
    {
        WeaponData = WeaponDataManager.Instance.GetWeaponData(weaponType);
    }

    public virtual void BaseInitialize(float damage, float size, float lifeTime)
    {
        this.effectDamage = damage;
        this.effectSize = size;
        this.effectLifeTime = lifeTime;

        currentLifeTime = lifeTime;
        isActive = true;

        transform.localScale = new Vector3(effectSize, effectSize, effectSize);
    }
    public virtual void DebuffInitialize(float knockbackForce, float slowForce, float slowDuration)
    {
        this.knockbackForce = knockbackForce;
        this.slowForce = slowForce;
        this.slowDuration = slowDuration;
    }

    protected virtual void Update()
    {
        if (!isActive) return;

        currentLifeTime -= Time.deltaTime;
        if (currentLifeTime <= 0)
        {
            Deactivate();
        }
    }

    protected virtual void Deactivate()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
}
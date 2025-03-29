using UnityEngine;
using System.Collections;

public class VorpalSword : MeleeWeaponBase
{
    [Header("보팔검 특수 효과")]
    public GameObject slashEffectPrefab;
    public float effectDuration = 0.5f;
    
    private const string SLASH_EFFECT_TAG = "SlashEffect";

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnHitEffect(Vector3 hitPosition)
    {
        if (slashEffectPrefab != null)
        {
            GameObject effect = ObjectPool.Instance.SpawnFromPool(SLASH_EFFECT_TAG, hitPosition, Quaternion.identity);
            if (effect != null)
            {
                effect.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                StartCoroutine(ReturnToPool(effect, effectDuration));
            }
        }
    }

    private System.Collections.IEnumerator ReturnToPool(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        ObjectPool.Instance.ReturnToPool(SLASH_EFFECT_TAG, obj);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
    }
} 
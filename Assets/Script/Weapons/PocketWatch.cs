using UnityEngine;

public class PocketWatch : MeleeWeaponBase
{
    [SerializeField] private float rotationSpeed = 360f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        // 시계 회전
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
    }
} 
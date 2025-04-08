using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField]
    private Dictionary<Collider2D, WeaponDataHolder> weaponCache = new();
    [SerializeField]
    private Dictionary<string, WeaponData> weaponDataMap => weaponDataManager.weaponDataMap;

    [SerializeField]
    Enemy enemy;
    [SerializeField]
    WeaponDataManager weaponDataManager;

    private Collider2D playerCol;

    private float attackCooldown = 1.0f;
    private float attackTimer;
    private bool canAttack = false;

    private float firecrackerTimer = 0f;
    private float f_hitCooldown = 0.1f;

    private float pipeTimer = 0f;
    private float p_hitCooldown = 0.2f;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        weaponDataManager = FindAnyObjectByType<WeaponDataManager>();
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;
        firecrackerTimer += Time.deltaTime;
        pipeTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            canAttack = true;
        }

        if (canAttack && playerCol != null)
        {
            attackTimer = 0f;
            canAttack = false;
            enemy.Attack();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                playerCol = other;
                break;

            case "Weapon":
                HandleWeaponHit(other);
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                playerCol = other;
                break;

            case "NonBirthdayFirecracker":
                if (firecrackerTimer > f_hitCooldown)
                    firecrackerTimer = 0f;
                break;

            case "Pipe":
                if (pipeTimer > p_hitCooldown)
                    pipeTimer = 0f;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCol = null;
        }

        //weaponCache.Remove(other); //캐싱 해제
    }

    private void HandleWeaponHit(Collider2D other)
    {
        if (!weaponCache.TryGetValue(other, out WeaponDataHolder holder))
        {
            holder = other.GetComponent<WeaponDataHolder>();
            if (holder == null) return;

            weaponCache[other] = holder; //최초 캐싱
        }

        WeaponData weaponData = holder.weaponData;
        if (weaponData == null) return;

        string tagKey = weaponData.weaponTag;

        //WeaponData 가져오기
        if (!weaponDataMap.TryGetValue(tagKey, out var data)) return;

        int index = data.currentLevel;

        enemy.TakeDamage(
            data.levelStats.damage[index],
            data.levelStats.knockbackForce[index],
            data.levelStats.slowForce[index],
            data.levelStats.slowDuration[index]
        );
    }
}
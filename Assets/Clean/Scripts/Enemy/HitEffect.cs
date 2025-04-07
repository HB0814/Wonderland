using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField]
    Enemy enemy;
    [SerializeField]
    WeaponDataManager weaponDataManager;
    [SerializeField]
    WeaponData weaponData;

    float attackCooldown = 1.0f;
    float attackTimer; //플레이어 공격 쿨타임 타이머

    bool canAttack = false; //공격 가능 여부
    Collider2D playerCol; //플레이어 콜라이더

    float firecrackerTimer = 0.0f; //폭죽 피격 쿨타임 타이머
    float f_hitCooldown = 0.1f; //폭죽 피격 쿨타임

    float pipeTimer = 0.0f; //담배 파이프 피격 쿨타임 타이머
    float p_hitCooldown = 0.2f; //담배 파이프 피격 쿨타임

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        weaponDataManager = FindAnyObjectByType<WeaponDataManager>();
    }

    private void Update()
    {
        attackTimer += Time.deltaTime; //타이머 증가
        firecrackerTimer += Time.deltaTime; //안생일 축하폭죽 타이머 증가
        pipeTimer += Time.deltaTime; //파이프 타이머 증가

        if (attackTimer >= attackCooldown) //쿨타임 완료
        {
            canAttack = true; //공격 가능
        }
        if (canAttack && playerCol != null) //공격 가능하고 플레이어 콜라이더가 있다면
        {
            attackTimer = 0.0f; //타이머 쿨타임 초기화
            canAttack = false; //공격 가능 여부 초기화
            enemy.Attack(); //공격
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //플레이어
        string tag = other.tag;
        if(tag == "Player")
        {
            playerCol = other;
        }

        //무기
        weaponData = weaponDataManager.GetWeaponData(other.tag); //무기 데이터 가져오기
                                                                                                              // 태그랑 weaponData의 weaponTag가 일치해야함
        if (weaponData != null)
        {
            int index = weaponData.currentLevel; //무기 데이터의 레벨 가져오기

            float damage = weaponData.levelStats.damage[index]; //데미지
            float knockback = weaponData.levelStats.knockbackForce[index]; //넉백 크기
            float slow = weaponData.levelStats.slowForce[index]; //슬로우 크기
            float slowDuration = weaponData.levelStats.slowDuration[index]; //슬로우 지속시간

            enemy.TakeDamage(damage, knockback, slow, slowDuration); //데미지, 넉백, 슬로우 계산
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        string tag = other.tag;

        switch(tag)
        {
            case "Player":
                playerCol = other;
                break;

            case "NonBirthdayFirecracker":
                if(firecrackerTimer > f_hitCooldown)
                {
                    firecrackerTimer = 0.0f;
                }
                break;

            case "Pipe":
                if(pipeTimer > p_hitCooldown)
                {
                    pipeTimer = 0.0f;
                }
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCol = null;
        }
    }
}

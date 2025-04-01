using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string type;

    public float damage_Default; //데미지
    public float duration_Default; //지속시간
    public float cooldown_Default; //쿨타임
    //public float timer = 0.0f; //쿨타임 타이머
    public float attackRange_Default; //공격 범위 => 기존 스케일 값에 곱연산으로 들어가지 않을까 싶음.

    public bool hasCritical_Default; //치명타 여부
    public float criticalRate_Default; //치명타 확률

    public float knockbackForce_Default; //넉백 크기
    public int strike_Default; //관통 카운트

    public float projectileSpeed_Default; //투사체 속도
    public int projectileCount_Default; //투사체 개수

    public bool hasDefenseDecrease_Default; //방어력 감소 여부
    public float defenseDecrease_Default; //방어력 감소

    public bool hasIgnoreDefense_Default; //방어력 무시 여부
    public float ignoreDefense_Default; //방어력 무시

    public bool hasSlow_Default; //슬로우 여부
    public float slowForce_Default;  //슬로우 크기
}

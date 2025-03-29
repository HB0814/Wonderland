using UnityEngine;

public class DataManager : MonoBehaviour
{
    [Header("무기 스탯")]
    public float damage; //데미지
    public float duration; //지속시간
    public float cooldown; //쿨타임
    public float timer; //쿨타임 타이머
    public float attackRange; //공격 범위 => 기존 스케일 값에 곱연산으로 들어가지 않을까 싶음.
    public bool hasCritical; //치명타 여부
    public float criticalRate; //치명타 확률
    public float knockbackForce; //넉백 크기
    public int strike; //관통 카운트
    public float projectileSpeed; //투사체 속도
    public int projectileCount; //투사체 개수

    public bool hasDefenseDecrease; //방어력 감소 여부
    public float defenseDecrease; //방어력 감소

    public bool hasIgnoreDefense; //방어력 무시 여부
    public float ignoreDefense; //방어력 무시

    public bool hasSlow; //슬로우 여부
    public float slowForce;  //슬로우 크기
}

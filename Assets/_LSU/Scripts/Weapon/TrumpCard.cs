using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

using Random = UnityEngine.Random;

public class TrumpCard : MonoBehaviour
{
    static ScriptableObject trump;

    //[Header("무기 스탯")]
    float damage; //데미지
    //float duration; //지속시간
    //public float cooldown = 2f; //쿨타임
    //public float timer = 0.0f; //쿨타임 타이머
    float attackRange; //공격 범위 => 기존 스케일 값에 곱연산으로 들어가지 않을까 싶음.
    bool hasCritical; //치명타 여부
    float criticalRate; //치명타 확률
    float knockbackForce; //넉백 크기
    //int strike; //관통 카운트
    float projectileSpeed = 10.0f; //투사체 속도
    int projectileCount; //투사체 개수

    //bool hasDefenseDecrease; //방어력 감소 여부
    //float defenseDecrease; //방어력 감소

    //bool hasIgnoreDefense; //방어력 무시 여부
    //float ignoreDefense; //방어력 무시

    //bool hasSlow; //슬로우 여부
    //float slowForce;  //슬로우 크기

    float randomRot; //카드의 랜덤 각도 변수

    private void Update()
    {
        ShootTrump(); //카드 이동 함수 실행
    }

    //스크립터블 데이터 가져오기
    void GetData()
    {
        
    }

    //카드 이동 함수
    public void ShootTrump() 
    {
        transform.Translate(Vector3.up * projectileSpeed * Time.deltaTime); //카드 이동
    }

    //카드의 랜덤 각도 함수 
    public void SetRotation() 
    {
        randomRot = math.floor(Random.Range(-180.0f, 180.0f) * 10) * 0.1f; //-180.0~180.0도 값의 소수점 일의 자리 버리기
        transform.rotation = Quaternion.Euler(0, 0, randomRot); //카드의 각도 변경
    }

    //화면 밖으로 나갈 시
    private void OnBecameInvisible()
    {
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }
}

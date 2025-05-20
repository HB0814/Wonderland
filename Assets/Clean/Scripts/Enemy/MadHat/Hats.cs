using UnityEngine;

public class Hats : MonoBehaviour
{
    //모자 종류
    public enum HatType
    {
        Red, 
        Blue, 
        Yellow,
        Green
    }

    public HatType hatType;

    [Header("공통")]
    public float damage; //피해량
    public float speed; //속도
    Player player; //플레이어 스크립트
    Rigidbody2D rb; //리지드바디2D


    [Header("빨강 - 폭발")] 
    public bool isReady = false; //폭발 준비 여부
    public float explosionReadyRange = 6.0f; //폭발 준비 시작 범위
    public float explosionMoveSpeed = 3.0f; //폭발 준비 시 이동속도
    public float explosionRange = 1.0f; //폭발  범위
    public float explosionDamage = 50.0f; //폭발 데미지
    public GameObject explosionEffect; //폭발 이펙트 -> 게임오브젝트 대신 파티클시스템 사용할수도
    public GameObject center; //중앙에 위치한 게임오브젝트

    [Header("파랑 - 슬로우")]
    public float slowForce;
    public float slowDuration;

    [Header("노랑 - 고속")]
    public float maxSpeed;

    [Header("초록 - 분신")]
    public int count; //모자 개수
}

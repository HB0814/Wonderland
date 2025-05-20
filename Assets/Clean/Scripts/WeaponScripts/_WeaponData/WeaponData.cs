using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponData", menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    private const int MAX_LEVEL = 5;  // 최대 레벨

    [Header("무기 기본 정보")]
    public string weaponName; // 에셋 이름과 동일하게 설정
    public string[] description; // 무기 설명
    public int currentLevel = 0; // 현재 레벨
    public Sprite icon; // 무기 아이콘
    public string[] UpgradeDetails = new string[MAX_LEVEL]; // 레벨업 상세 정보

    [Header("무기 타입")]
    public WeaponType weaponType;

    [Header("레벨별 스탯 정보")]
    public LevelStats levelStats;

    [System.Serializable]
    public class LevelStats
    {
        [Header("기본 스탯")]
        public float[] damage = new float[MAX_LEVEL]; // 데미지
        public float[] attackCooldown = new float[MAX_LEVEL]; // 공격 쿨다운
        public float[] lifeTime = new float[MAX_LEVEL]; // 지속시간
        public float[] size = new float[MAX_LEVEL]; // 크기

        [Header("부가 효과")]
        public float[] knockbackForce = new float[MAX_LEVEL]; // 넉백 강도
        public float[] slowForce = new float[MAX_LEVEL]; // 슬로우 강도
        public float[] slowDuration = new float[MAX_LEVEL]; // 슬로우 지속시간

        [Header("투사체")]
        public float[] detectionRange = new float[MAX_LEVEL]; // 탐지 범위
        public float[] speed = new float[MAX_LEVEL]; // 속도

        [Header("카드, 화살, 폭죽")]
        public int[] count = new int[MAX_LEVEL]; // 발사체 개수

        [Header("검, 사과, 모자")]
        public float attackRangeX; // 공격 범위 X
        public float attackRangeY; // 공격 범위 Y
        
        [Header("책")]
        public int[] pierceCount; // 관통 수
        
        [Header("모자")]
        public float[] coolDownDecrease; // 쿨타임 감소량
    }

    private void OnValidate()
    {
        weaponName = this.name;
    }
}

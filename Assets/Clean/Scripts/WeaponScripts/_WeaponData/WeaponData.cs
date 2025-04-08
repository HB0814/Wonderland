using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("무기 정보")]
    public string weaponTag; //무기 타입(이름)
    [HideInInspector]
    public int level = 0;
    public int currentLevel; //무기 레벨
    public Sprite icon; //무기 아이콘
    public string[] UpgradeDetails = new string[5]; //무기 업그레이드 내용

    [Header("레벨 별 무기 정보")]
    public LevelStats levelStats;

    [System.Serializable]
    public class LevelStats
    {
        [Header("공통")]
        public float[] damage = new float[5]; //무기 데미지
        public float[] attackCooldown = new float[5]; //무기 쿨다운

        [Header("무기 효과")]
        public float[] knockbackForce = new float[5]; //무기 넉백 크기
        public float[] slowForce = new float[5]; //무기 슬로우 크기
        public float[] slowDuration = new float[5]; //무기 슬로우 지속시간

        [Header("투사체")]
        public float detectionRange; //무기 탐지범위(공격사거리)
        public float[] speed = new float[5]; //무기 속도

        [Header("사과, 카드, 책")]
        public int[] count = new int[5]; //무기 투사체의 개수, 관통 수치

        [Header("사과, 폭죽, 채셔캣")]
        public float attackRangeX; //무기 공격 범위: 사과, 폭죽, 채셔캣
        public float attackRangeY; //무기 공격 범위: 사과, 폭죽, 채셔캣
    }

    public void ResetLevel()
    {
        currentLevel = level;
    }
}

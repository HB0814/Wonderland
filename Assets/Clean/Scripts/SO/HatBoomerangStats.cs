using UnityEngine;

[CreateAssetMenu(fileName = "HatBoomerangStats", menuName = "Weapons/HatBoomerangStats")]
public class HatBoomerangStats : ScriptableObject
{
    [System.Serializable]
    public class LevelStats
    {
        public int currentLevel;
        public float damage;
        public float attackCooldown;
        public float hatSpeed;
        public float maxDistance;
        public float returnSpeed;
        public float cooldownReduction;
    }

    public LevelStats[] levelStats;
} 
using UnityEngine;

[CreateAssetMenu(fileName = "RollAppleStats", menuName = "Weapons/RollAppleStats")]
public class RollAppleStats : ScriptableObject
{
    public LevelStats[] levelStats;

    [System.Serializable]
    public class LevelStats
    {
        public int currentLevel;
        public float damage;
        public float knockbackForce;
        public float attackRangeX;
        public float attackRangeY;
        public float attackCooldown;
    }
}          

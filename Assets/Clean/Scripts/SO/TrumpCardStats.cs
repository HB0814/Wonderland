using UnityEngine;

[CreateAssetMenu(fileName = "TrumpCardStats", menuName = "Weapons/TrumpCardStats")]
public class TrumpCardStats : ScriptableObject
{
    [System.Serializable]
    public class LevelStats
    {
        public int currentLevel;
        public int cardCount;
        public float cardSpeed;
        public float damage;
        public float attackCooldown;
        public float cardLifetime;
    }

    public LevelStats[] levelStats;
}

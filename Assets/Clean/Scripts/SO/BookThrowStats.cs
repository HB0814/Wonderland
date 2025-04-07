using UnityEngine;
[CreateAssetMenu(fileName = "BookThrowStats", menuName = "Weapons/BookThrowStats")]
public class BookThrowStats : ScriptableObject
{
    [System.Serializable]
    public class LevelStats
    {
        public int currentLevel;
        public float damage;
        public float attackCooldown;
        public float bookDetectionRange;
    }

    public LevelStats[] levelStats;
}

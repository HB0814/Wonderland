using UnityEngine;

[CreateAssetMenu(fileName = "CheshireCatWeaponStats", menuName = "Weapons/CheshireCatWeaponStats")]
public class CheshireCatWeaponStats : ScriptableObject
{
    [System.Serializable]
    public class LevelStats
    {
        public int currentLevel;
        public float damage;
        public float effectRadius;
        public float effectDuration;
        public float detectionRange;
    }

    public LevelStats[] levelStats = new LevelStats[]
    {
        new LevelStats { currentLevel = 1, damage = 15f, effectRadius = 3f, effectDuration = 1f, detectionRange = 10f },
        new LevelStats { currentLevel = 2, damage = 20f, effectRadius = 3.5f, effectDuration = 1.2f, detectionRange = 12f },
        new LevelStats { currentLevel = 3, damage = 25f, effectRadius = 4f, effectDuration = 1.4f, detectionRange = 14f },
        new LevelStats { currentLevel = 4, damage = 30f, effectRadius = 4.5f, effectDuration = 1.6f, detectionRange = 16f },
        new LevelStats { currentLevel = 5, damage = 35f, effectRadius = 5f, effectDuration = 1.8f, detectionRange = 18f }
    };

    private int currentLevel = 1;

    public void LevelUp()
    {
        if (currentLevel < levelStats.Length)
        {
            currentLevel++;
        }
    }

    public LevelStats GetCurrentLevelStats()
    {
        return levelStats[currentLevel - 1];
    }
} 
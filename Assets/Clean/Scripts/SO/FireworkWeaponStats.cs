using UnityEngine;

[CreateAssetMenu(fileName = "FireworkWeaponStats", menuName = "Weapons/FireworkWeaponStats")]
public class FireworkWeaponStats : ScriptableObject
{
    [System.Serializable]
    public class LevelStats
    {
        public int currentLevel;
        public float damage;
        public float effectRadius;
        public float effectScale;
        public float damageInterval;
        public int effectCount;
        public float effectDuration;
        public float spawnRange;
    }

    public LevelStats[] levelStats = new LevelStats[]
    {
        new LevelStats { currentLevel = 1, damage = 8f, effectRadius = 1.5f, effectScale = 1.0f, damageInterval = 1.0f, effectCount = 2, effectDuration = 5f, spawnRange = 3f },
        new LevelStats { currentLevel = 2, damage = 10f, effectRadius = 1.7f, effectScale = 1.2f, damageInterval = 0.9f, effectCount = 3, effectDuration = 5.5f, spawnRange = 3.5f },
        new LevelStats { currentLevel = 3, damage = 12f, effectRadius = 1.9f, effectScale = 1.4f, damageInterval = 0.8f, effectCount = 4, effectDuration = 6f, spawnRange = 4f },
        new LevelStats { currentLevel = 4, damage = 15f, effectRadius = 2.1f, effectScale = 1.6f, damageInterval = 0.7f, effectCount = 5, effectDuration = 6.5f, spawnRange = 4.5f },
        new LevelStats { currentLevel = 5, damage = 18f, effectRadius = 2.3f, effectScale = 1.8f, damageInterval = 0.6f, effectCount = 6, effectDuration = 7f, spawnRange = 5f }
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
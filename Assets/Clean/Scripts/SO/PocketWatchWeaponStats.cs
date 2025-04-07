using UnityEngine;

[CreateAssetMenu(fileName = "PocketWatchWeaponStats", menuName = "Weapons/PocketWatchWeaponStats")]
public class PocketWatchWeaponStats : ScriptableObject
{
    [System.Serializable]
    public class LevelStats
    {
        public int currentLevel;
        public float damage;
        public float effectRadius;
        public float effectDuration;
        public float attackCooldown;
    }

    public LevelStats[] levelStats = new LevelStats[]
    {
        new LevelStats { 
            currentLevel = 1, 
            damage = 10f, 
            effectRadius = 5f, 
            effectDuration = 3f,
            attackCooldown = 3f
        },
        new LevelStats { 
            currentLevel = 2, 
            damage = 15f, 
            effectRadius = 6f, 
            effectDuration = 3.5f,
            attackCooldown = 2.8f
        },
        new LevelStats { 
            currentLevel = 3, 
            damage = 20f, 
            effectRadius = 7f, 
            effectDuration = 4f,
            attackCooldown = 2.6f
        },
        new LevelStats { 
            currentLevel = 4, 
            damage = 25f, 
            effectRadius = 8f, 
            effectDuration = 4.5f,
            attackCooldown = 2.4f
        },
        new LevelStats { 
            currentLevel = 5, 
            damage = 30f, 
            effectRadius = 9f, 
            effectDuration = 5f,
            attackCooldown = 2.2f
        }
    };

    public LevelStats GetCurrentLevelStats()
    {
        foreach (LevelStats stats in levelStats)
        {
            if (stats.currentLevel == currentLevel)
            {
                return stats;
            }
        }
        return levelStats[0];
    }

    public void LevelUp()
    {
        currentLevel = Mathf.Min(currentLevel + 1, levelStats.Length);
    }

    [HideInInspector]
    public int currentLevel = 1;
} 
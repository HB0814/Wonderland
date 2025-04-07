using UnityEngine;

[CreateAssetMenu(fileName = "VorpalSwordWeaponStats", menuName = "Weapons/VorpalSwordWeaponStats")]
public class VorpalSwordWeaponStats : ScriptableObject
{
    [System.Serializable]
    public class LevelStats
    {
        public int currentLevel;
        public float damage;
        public float knockbackForce;
        public float effectDuration;
    }

    public LevelStats[] levelStats = new LevelStats[]
    {
        new LevelStats { currentLevel = 1, damage = 15f, knockbackForce = 5f, effectDuration = 0.3f },
        new LevelStats { currentLevel = 2, damage = 20f, knockbackForce = 6f, effectDuration = 0.35f },
        new LevelStats { currentLevel = 3, damage = 25f, knockbackForce = 7f, effectDuration = 0.4f },
        new LevelStats { currentLevel = 4, damage = 30f, knockbackForce = 8f, effectDuration = 0.45f },
        new LevelStats { currentLevel = 5, damage = 35f, knockbackForce = 9f, effectDuration = 0.5f }
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
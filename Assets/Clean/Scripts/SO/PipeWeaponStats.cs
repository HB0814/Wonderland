using UnityEngine;

[CreateAssetMenu(fileName = "PipeWeaponStats", menuName = "Weapons/PipeWeaponStats")]
public class PipeWeaponStats : ScriptableObject
{
    [System.Serializable]
    public class LevelStats
    {
        public int currentLevel;
        public float damage;
        public float smokeRadius;
        public float smokeExpandSpeed;
        public float smokeMaxRadius;
        public float smokeDamageInterval;
    }

    public LevelStats[] levelStats = new LevelStats[]
    {
        new LevelStats { 
            currentLevel = 1, 
            damage = 5f, 
            smokeRadius = 1f, 
            smokeExpandSpeed = 2f, 
            smokeMaxRadius = 3f, 
            smokeDamageInterval = 0.5f 
        },
        new LevelStats { 
            currentLevel = 2, 
            damage = 7f, 
            smokeRadius = 1.2f, 
            smokeExpandSpeed = 2.2f, 
            smokeMaxRadius = 3.5f, 
            smokeDamageInterval = 0.45f 
        },
        new LevelStats { 
            currentLevel = 3, 
            damage = 10f, 
            smokeRadius = 1.4f, 
            smokeExpandSpeed = 2.4f, 
            smokeMaxRadius = 4f, 
            smokeDamageInterval = 0.4f 
        },
        new LevelStats { 
            currentLevel = 4, 
            damage = 12f, 
            smokeRadius = 1.6f, 
            smokeExpandSpeed = 2.6f, 
            smokeMaxRadius = 4.5f, 
            smokeDamageInterval = 0.35f 
        },
        new LevelStats { 
            currentLevel = 5, 
            damage = 15f, 
            smokeRadius = 1.8f, 
            smokeExpandSpeed = 2.8f, 
            smokeMaxRadius = 5f, 
            smokeDamageInterval = 0.3f 
        }
    };
} 
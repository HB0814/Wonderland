using UnityEngine;

[CreateAssetMenu(fileName = "TeaSplashStats", menuName = "Weapons/TeaSplashStats")]
public class TeaSplashStats : ScriptableObject
{
    [System.Serializable]
    public class LevelStats
    {
        public int currentLevel;
        public float damage;
        public float attackCooldown;
        public float teaSpeed;
        public float spreadAngle;
        public int projectileCount;
        public float knockbackForce;
    }

    public LevelStats[] levelStats = new LevelStats[]
    {
        new LevelStats { currentLevel = 1, damage = 10f, attackCooldown = 1f, teaSpeed = 12f, spreadAngle = 30f, projectileCount = 3, knockbackForce = 5f },
        new LevelStats { currentLevel = 2, damage = 15f, attackCooldown = 0.9f, teaSpeed = 14f, spreadAngle = 35f, projectileCount = 4, knockbackForce = 6f },
        new LevelStats { currentLevel = 3, damage = 20f, attackCooldown = 0.8f, teaSpeed = 16f, spreadAngle = 40f, projectileCount = 5, knockbackForce = 7f },
        new LevelStats { currentLevel = 4, damage = 25f, attackCooldown = 0.7f, teaSpeed = 18f, spreadAngle = 45f, projectileCount = 6, knockbackForce = 8f },
        new LevelStats { currentLevel = 5, damage = 30f, attackCooldown = 0.6f, teaSpeed = 20f, spreadAngle = 50f, projectileCount = 7, knockbackForce = 10f }
    };
} 
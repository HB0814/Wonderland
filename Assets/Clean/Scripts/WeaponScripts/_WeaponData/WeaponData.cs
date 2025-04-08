using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("���� ����")]
    public string weaponTag; //���� Ÿ��(�̸�)
    [HideInInspector]
    public int level = 0;
    public int currentLevel; //���� ����
    public Sprite icon; //���� ������
    public string[] UpgradeDetails = new string[5]; //���� ���׷��̵� ����

    [Header("���� �� ���� ����")]
    public LevelStats levelStats;

    [System.Serializable]
    public class LevelStats
    {
        [Header("����")]
        public float[] damage = new float[5]; //���� ������
        public float[] attackCooldown = new float[5]; //���� ��ٿ�

        [Header("���� ȿ��")]
        public float[] knockbackForce = new float[5]; //���� �˹� ũ��
        public float[] slowForce = new float[5]; //���� ���ο� ũ��
        public float[] slowDuration = new float[5]; //���� ���ο� ���ӽð�

        [Header("����ü")]
        public float detectionRange; //���� Ž������(���ݻ�Ÿ�)
        public float[] speed = new float[5]; //���� �ӵ�

        [Header("���, ī��, å")]
        public int[] count = new int[5]; //���� ����ü�� ����, ���� ��ġ

        [Header("���, ����, ä��Ĺ")]
        public float attackRangeX; //���� ���� ����: ���, ����, ä��Ĺ
        public float attackRangeY; //���� ���� ����: ���, ����, ä��Ĺ
    }

    public void ResetLevel()
    {
        currentLevel = level;
    }
}

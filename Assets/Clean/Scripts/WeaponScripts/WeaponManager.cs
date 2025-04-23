using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    [Header("무기 설정")]
    public int maxWeapons = 4; // 최대 보유 가능한 무기 수
    public List<WeaponBase> equippedWeapons = new List<WeaponBase>();

    [Header("무기 프리팹")]
    public GameObject[] weaponPrefabs; // 사용 가능한 모든 무기 프리팹

    private GameObject player;
    private ObjectPool objectPool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        objectPool = ObjectPool.Instance;

        AddWeapon(WeaponManager.Instance.CreateWeapon((int)WeaponType.Tea));
    }

    public bool AddWeapon(WeaponBase weapon)
    {
        if (weapon == null) return false;

        // 이미 같은 타입의 무기가 있는지 확인
        foreach (var equippedWeapon in equippedWeapons)
        {
            if (equippedWeapon.WeaponType == weapon.WeaponType)
            {
                Debug.Log($"이미 {weapon.WeaponType} 타입의 무기가 장착되어 있습니다!");
                return false;
            }
        }

        if (equippedWeapons.Count >= maxWeapons)
        {
            Debug.Log("최대 무기 수에 도달했습니다!");
            return false;
        }

        equippedWeapons.Add(weapon);
        // 무기를 플레이어의 자식으로 설정
        weapon.transform.SetParent(player.transform);
        weapon.transform.localPosition = Vector3.zero;
        return true;
    }

    public void RemoveWeapon(WeaponBase weapon)
    {
        if (equippedWeapons.Contains(weapon))
        {
            equippedWeapons.Remove(weapon);
            Destroy(weapon.gameObject);
        }
    }

    public void LevelUpWeapon(WeaponBase weapon)
    {
        if (equippedWeapons.Contains(weapon))
        {
            weapon.LevelUpLogic();
        }
    }

    public void InitializeWeapons()
    {
        foreach (var weapon in equippedWeapons)
        {
            weapon.Initialize();
        }
    }

    // 무기 프리팹에서 새로운 무기 생성
    public WeaponBase CreateWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= weaponPrefabs.Length)
        {
            Debug.LogError("잘못된 무기 인덱스입니다!");
            return null;
        }

        // 이미 같은 타입의 무기가 있는지 확인
        WeaponType weaponType = (WeaponType)weaponIndex;
        foreach (var equippedWeapon in equippedWeapons)
        {
            if (equippedWeapon.WeaponType == weaponType)
            {
                Debug.Log($"이미 {weaponType} 타입의 무기가 장착되어 있습니다!");
                return null;
            }
        }

        GameObject weaponObj = Instantiate(weaponPrefabs[weaponIndex]);
        WeaponBase weapon = weaponObj.GetComponent<WeaponBase>();
        
        if (weapon != null)
        {
            if (AddWeapon(weapon))
            {
                return weapon;
            }
            else
            {
                Destroy(weaponObj);
                return null;
            }
        }
        else
        {
            Destroy(weaponObj);
            return null;
        }
    }

    // ObjectPool을 통해 투사체 생성
    public GameObject SpawnProjectile(string projectileTag, Vector3 position, Quaternion rotation)
    {
        return objectPool.SpawnFromPool(projectileTag, position, rotation);
    }
} 
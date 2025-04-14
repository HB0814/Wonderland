using System.Collections.Generic;
using UnityEngine;

public class WeaponDataManager : MonoBehaviour
{
    private static WeaponDataManager instance;
    public static WeaponDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<WeaponDataManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("WeaponDataManager");
                    instance = obj.AddComponent<WeaponDataManager>();
                }
            }
            return instance;
        }
    }

    public List<WeaponData> weaponDataList; // 모든 무기 데이터 리스트
    private Dictionary<string, WeaponData> weaponDataMap = new Dictionary<string, WeaponData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 무기 이름을 기반으로 무기 데이터를 매핑하는 초기화 작업
        foreach (var weapon in weaponDataList)
        {
            if (!weaponDataMap.ContainsKey(weapon.weaponName))
            {
                weaponDataMap.Add(weapon.weaponName, weapon);
            }
            weapon.currentLevel = 0;
        }
    }

    // 무기 타입으로 특정 무기 데이터를 가져오는 메서드
    public WeaponData GetWeaponData(WeaponType weaponType)
    {
        foreach (var weapon in weaponDataList)
        {
            if (weapon.weaponType == weaponType)
            {
                return weapon;
            }
        }
        return null;
    }

    // 모든 무기 데이터를 가져오는 메서드
    public List<WeaponData> GetAllWeaponData()
    {
        return weaponDataList;
    }

    // 특정 무기 데이터를 추가하는 메서드
    public void AddWeaponData(WeaponData weaponData)
    {
        if (!weaponDataList.Contains(weaponData))
        {
            weaponDataList.Add(weaponData);
            if (!weaponDataMap.ContainsKey(weaponData.weaponName))
            {
                weaponDataMap.Add(weaponData.weaponName, weaponData);
            }
        }
    }

    // 특정 무기 데이터를 제거하는 메서드
    public void RemoveWeaponData(WeaponData weaponData)
    {
        if (weaponDataList.Contains(weaponData))
        {
            weaponDataList.Remove(weaponData);
            weaponDataMap.Remove(weaponData.weaponName);
        }
    }
}

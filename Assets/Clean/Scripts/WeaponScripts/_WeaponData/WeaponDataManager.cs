using System.Collections.Generic;
using UnityEngine;

public class WeaponDataManager : MonoBehaviour
{
    public List<WeaponData> weaponDataList; // 등록된 모든 무기
    public Dictionary<string, WeaponData> weaponDataMap = new Dictionary<string, WeaponData>();

    private void Awake()
    {
        // 태그를 기반으로 무기 데이터를 저장하는 딕셔너리 생성
        foreach (var weapon in weaponDataList)
        {
            if (!weaponDataMap.ContainsKey(weapon.weaponTag))
            {
                weaponDataMap.Add(weapon.weaponTag, weapon);
            }
            weapon.ResetLevel();
        }
    }

    // 태그를 통해 해당 무기 데이터 가져오기
    public WeaponData GetWeaponData(string weaponTag)
    {
        weaponDataMap.TryGetValue(weaponTag, out WeaponData weapon);
        return weapon;
    }
}

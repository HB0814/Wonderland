using System.Collections.Generic;
using UnityEngine;

public class WeaponDataManager : MonoBehaviour
{
    public List<WeaponData> weaponDataList; // ��ϵ� ��� ����
    private Dictionary<string, WeaponData> weaponDataMap = new Dictionary<string, WeaponData>();

    private void Awake()
    {
        // �±׸� ������� ���� �����͸� �����ϴ� ��ųʸ� ����
        foreach (var weapon in weaponDataList)
        {
            if (!weaponDataMap.ContainsKey(weapon.weaponTag))
            {
                weaponDataMap.Add(weapon.weaponTag, weapon);
            }
        }
    }

    // �±׸� ���� �ش� ���� ������ ��������
    public WeaponData GetWeaponData(string weaponTag)
    {
        weaponDataMap.TryGetValue(weaponTag, out WeaponData weapon);
        return weapon;
    }
}

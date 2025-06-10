using UnityEngine;
using System.Collections.Generic;

public enum ItemEffectType
{
    Damage,
    Speed,
    Health,
    // 추가 효과 타입들...
}

[System.Serializable]
public class ItemEffect
{
    public ItemEffectType effectType;
    public float effectValue;
}

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public string description;
    public Sprite icon;

    [Header("Effects")]
    public List<ItemEffect> effects; // 여러 효과를 저장하는 리스트

    [Header("Rarity")]
    public int rarity; // 1: 일반, 2: 희귀, 3: 전설
    
    private void OnValidate()
    {
        itemName = this.name;
    } 
}


using UnityEngine;

public enum ItemEffectType
{
    Damage,
    Speed,
    Health,
    // 추가 효과 타입들...
}

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public string description;
    public Sprite icon;

    [Header("Effect")]
    public ItemEffectType effectType;
    public float effectValue; // 효과 값 (퍼센트 또는 절대값)

    [Header("Rarity")]
    public int rarity; // 1: 일반, 2: 희귀, 3: 전설
    
    private void OnValidate()
    {
        itemName = this.name;
    } 
}


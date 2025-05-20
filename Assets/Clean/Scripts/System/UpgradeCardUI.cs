using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeCardUI : MonoBehaviour
{
    [Header("UI Components")]
    public Image iconImage;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI levelText;
    public GameObject weaponStatsPanel;
    public GameObject itemStatsPanel;

    [Header("Weapon Stats")]
    public TextMeshProUGUI status1Text;
    public TextMeshProUGUI status2Text;
    public TextMeshProUGUI status3Text;

    [Header("Item Stats")]
    public TextMeshProUGUI effectValueText;

    // 데이터
    public bool isWeapon;
    public bool isUpgrade; // true면 업그레이드, false면 신규 무기
    public WeaponData weaponData;
    public ItemData itemData;

    // 무기 카드 설정
    public void SetupWeaponCard(WeaponData weapon, bool isUpgrade)
    {
        isWeapon = true;
        this.isUpgrade = isUpgrade;
        weaponData = weapon;

        // UI 업데이트
        iconImage.sprite = weapon.icon;
        titleText.text = weapon.weaponName;
        
        if (isUpgrade)
        {
            descriptionText.text = weapon.description[weapon.currentLevel];
            levelText.text = $"Lv.{weapon.currentLevel}";
        }
        else
        {
            descriptionText.text = "신규 무기 획득!";
            levelText.text = "NEW";
        }

        // 무기 스탯 표시
        weaponStatsPanel.SetActive(true);
        itemStatsPanel.SetActive(false);

        // 스탯 값 업데이트
        if (isUpgrade)
        {
            // 업그레이드 시 현재 레벨의 스탯 표시
            UpdateWeaponStats(weapon.currentLevel);
        }
        else
        {
            // 신규 무기 시 레벨 1의 스탯 표시
            UpdateWeaponStats(1);
        }
    }

    private void UpdateWeaponStats(int level)
    {
        switch (weaponData.weaponType)
        {
            case WeaponType.Firecracker:
                status1Text.text = $"데미지: {weaponData.levelStats.damage[level]}";
                status2Text.text = $"개수: {weaponData.levelStats.count[level]}";
                status3Text.text = $"크기: {weaponData.levelStats.size[level]}";
                break;
            case WeaponType.Sword:
                status1Text.text = $"데미지: {weaponData.levelStats.damage[level]}";
                status2Text.text = $"공격속도: {weaponData.levelStats.attackCooldown[level]}";
                status3Text.text = $" ";
                break;
            case WeaponType.Hat:
                status1Text.text = $"데미지: {weaponData.levelStats.damage[level]}";
                status2Text.text = $"공격속도: {weaponData.levelStats.attackCooldown[level]}";
                status3Text.text = $"쿨타임감소량: {weaponData.levelStats.coolDownDecrease[level]}";
                break;
            case WeaponType.Book:
                status1Text.text = $"데미지: {weaponData.levelStats.damage[level]}";
                status2Text.text = $"관통 수: {weaponData.levelStats.pierceCount[level]}";
                status3Text.text = $"공격속도: {weaponData.levelStats.attackCooldown[level]}";
                break;
            case WeaponType.Apple:
                status1Text.text = $"데미지: {weaponData.levelStats.damage[level]}";
                status2Text.text = $"생성속도: {weaponData.levelStats.attackCooldown[level]}";
                status3Text.text = $"사이즈: {weaponData.levelStats.size[level]}";
                break;
            case WeaponType.Tea:
                status1Text.text = $"데미지: {weaponData.levelStats.damage[level]}";
                status2Text.text = $"넉백: {weaponData.levelStats.knockbackForce[level]}";
                status3Text.text = $"공격속도: {weaponData.levelStats.attackCooldown[level]}";
                break;
            case WeaponType.Card:
                status1Text.text = $"데미지: {weaponData.levelStats.damage[level]}";
                status2Text.text = $"공격속도: {weaponData.levelStats.attackCooldown[level]}";
                status3Text.text = $"개수: {weaponData.levelStats.count[level]}";
                break;
            case WeaponType.Pipe:
                status1Text.text = $"데미지: {weaponData.levelStats.damage[level]}";
                status2Text.text = $"사이즈: {weaponData.levelStats.size[level]}";
                status3Text.text = $" ";
                break;
            case WeaponType.Cat:
                status1Text.text = $"데미지: {weaponData.levelStats.damage[level]}";
                status2Text.text = $"공격속도: {weaponData.levelStats.attackCooldown[level]}";
                status3Text.text = $"크기: {weaponData.levelStats.size[level]}";
                break;
            case WeaponType.Watch:
                status1Text.text = $"데미지: {weaponData.levelStats.damage[level]}";
                status2Text.text = $"공격속도: {weaponData.levelStats.attackCooldown[level]}";
                status3Text.text = $" ";
                break;
                
        }
    }

    // 아이템 카드 설정
    public void SetupItemCard(ItemData item)
    {
        isWeapon = false;
        itemData = item;

        // UI 업데이트
        iconImage.sprite = item.icon;
        titleText.text = item.itemName;
        descriptionText.text = item.description;

        // 아이템 스탯 표시
        weaponStatsPanel.SetActive(false);
        itemStatsPanel.SetActive(true);

        // 효과 값 업데이트
        effectValueText.text = GetEffectValueText(item);
    }

    // 아이템 효과 값 텍스트 생성
    private string GetEffectValueText(ItemData item)
    {
        switch (item.effectType)
        {
            case ItemEffectType.Damage:
                return $"+{item.effectValue}% Damage";
            case ItemEffectType.Speed:
                return $"+{item.effectValue}% Speed";
            case ItemEffectType.Health:
                return $"+{item.effectValue}% Health";
            default:
                return "";
        }
    }
} 
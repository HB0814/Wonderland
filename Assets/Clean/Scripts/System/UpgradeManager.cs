using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static UpgradeManager Instance { get; private set; }

    [Header("UI Components")]
    public GameObject upgradePanel; // 업그레이드 선택 패널
    public GameObject[] upgradeCards; // 4개의 업그레이드 카드
    public Button[] cardButtons; // 카드 선택 버튼
    public GameObject fadePanel; // 페이드 효과 패널

    [Header("Weapon References")]
    public WeaponDataManager weaponDataManager; // 무기 데이터 매니저 참조
    public List<ItemData> availableItems; // 사용 가능한 아이템들

    [Header("Upgrade Settings")]
    public int weaponUpgradeCount = 3; // 무기 업그레이드 카드 수
    public int itemUpgradeCount = 1; // 아이템 업그레이드 카드 수

    private bool isUpgrading = false;

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // 버튼 이벤트 연결
        for (int i = 0; i < cardButtons.Length; i++)
        {
            int index = i; // 클로저 문제 방지
            cardButtons[i].onClick.AddListener(() => OnCardSelected(index));
        }
    }

    // 레벨업 시 호출되는 함수
    public void ShowUpgradeOptions()
    {
        if (isUpgrading) return;

        SoundManager.Instance.PlaySFX("levelUp");
        isUpgrading = true;
        Time.timeScale = 0f; // 게임 일시정지
        fadePanel.SetActive(true);
        upgradePanel.SetActive(true);

        // 업그레이드 카드 생성
        GenerateUpgradeCards();
    }

    // 업그레이드 카드 생성
    private void GenerateUpgradeCards()
    {
        // 사용 가능한 무기와 아이템 목록 초기화
        List<WeaponData> availableWeaponUpgrades = new List<WeaponData>();
        List<WeaponData> availableNewWeapons = new List<WeaponData>();
        List<ItemData> availableItemUpgrades = new List<ItemData>();

        // 모든 무기 데이터 가져오기
        var allWeapons = weaponDataManager.GetAllWeaponData();
        
        // 무기 매니저에서 현재 장착된 무기 목록 가져오기
        var equippedWeapons = WeaponManager.Instance.equippedWeapons;
        var equippedWeaponTypes = new HashSet<WeaponType>();
        foreach (var weapon in equippedWeapons)
        {
            equippedWeaponTypes.Add(weapon.WeaponType);
        }

        // 무기 분류 (업그레이드 가능한 무기와 신규 무기)
        foreach (var weapon in allWeapons)
        {
            if (weapon.currentLevel > 0 && weapon.currentLevel < weapon.UpgradeDetails.Length)
            {
                // 업그레이드 가능한 무기 추가
                availableWeaponUpgrades.Add(weapon);
            }
            else if (!equippedWeaponTypes.Contains(weapon.weaponType) && 
                     equippedWeapons.Count < WeaponManager.Instance.maxWeapons)
            {
                // 신규 무기 추가
                availableNewWeapons.Add(weapon);
            }
        }

        // 사용 가능한 아이템 옵션 추가
        availableItemUpgrades.AddRange(availableItems);

        // 카드 섞기
        ShuffleList(availableWeaponUpgrades);
        ShuffleList(availableNewWeapons);
        ShuffleList(availableItemUpgrades);

        // 카드 배치
        int weaponUpgradeIndex = 0;
        int newWeaponIndex = 0;
        int itemIndex = 0;

        for (int i = 0; i < upgradeCards.Length; i++)
        {
            // 무기 업그레이드와 신규 무기의 확률 계산
            float upgradeChance = 0.6f; // 업그레이드 확률 60%
            float newWeaponChance = 0.3f; // 신규 무기 확률 30%
            float itemChance = 0.1f; // 아이템 확률 10%

            // 확률에 따른 카드 타입 결정
            float randomValue = Random.value;
            if (randomValue < upgradeChance && weaponUpgradeIndex < availableWeaponUpgrades.Count)
            {
                // 무기 업그레이드 카드 설정
                SetupWeaponCard(upgradeCards[i], availableWeaponUpgrades[weaponUpgradeIndex], true);
                weaponUpgradeIndex++;
            }
            else if (randomValue < upgradeChance + newWeaponChance && newWeaponIndex < availableNewWeapons.Count)
            {
                // 신규 무기 카드 설정
                SetupWeaponCard(upgradeCards[i], availableNewWeapons[newWeaponIndex], false);
                newWeaponIndex++;
            }
            else if (itemIndex < availableItemUpgrades.Count)
            {
                // 아이템 카드 설정
                SetupItemCard(upgradeCards[i], availableItemUpgrades[itemIndex]);
                itemIndex++;
            }
            else
            {
                // 남은 카드는 비활성화
                upgradeCards[i].SetActive(false);
            }
        }
    }

    // 무기 카드 설정 (isUpgrade: true면 업그레이드, false면 신규 무기)
    private void SetupWeaponCard(GameObject card, WeaponData weapon, bool isUpgrade)
    {
        card.SetActive(true);
        var cardUI = card.GetComponent<UpgradeCardUI>();
        if (cardUI != null)
        {
            cardUI.SetupWeaponCard(weapon, isUpgrade);
        }
    }

    // 아이템 카드 설정
    private void SetupItemCard(GameObject card, ItemData item)
    {
        card.SetActive(true);
        // 카드 UI 업데이트
        var cardUI = card.GetComponent<UpgradeCardUI>();
        if (cardUI != null)
        {
            cardUI.SetupItemCard(item);
        }
    }

    // 카드 선택 시 호출
    private void OnCardSelected(int cardIndex)
    {
        var cardUI = upgradeCards[cardIndex].GetComponent<UpgradeCardUI>();
        if (cardUI != null)
        {
            if (cardUI.isWeapon)
            {
                if (cardUI.isUpgrade)
                {
                    // 무기 업그레이드 적용
                    ApplyWeaponUpgrade(cardUI.weaponData);
                }
                else
                {
                    // 신규 무기 장착
                    EquipNewWeapon(cardUI.weaponData);
                }
            }
            else
            {
                // 아이템 효과 적용
                ApplyItemEffect(cardUI.itemData);
            }
        }

        // 업그레이드 완료
        CompleteUpgrade();
    }

    // 무기 업그레이드 적용
    private void ApplyWeaponUpgrade(WeaponData weapon)
    {
        if (weapon != null)
        {
            // 무기 데이터 레벨 업
            weapon.currentLevel++;
            
            // WeaponManager를 통해 해당 무기 찾기
            var weaponManager = WeaponManager.Instance;
            foreach (var equippedWeapon in weaponManager.equippedWeapons)
            {
                Debug.Log("equippedWeapon.WeaponType: " + equippedWeapon.WeaponType);
                Debug.Log("weapon.weaponType: " + weapon.weaponType);
                if (equippedWeapon.WeaponType == weapon.weaponType)
                {
                    Debug.Log("UpGrade!");
                    // 무기 업그레이드 적용
                    equippedWeapon.LevelUpLogic();
                    break;
                }
            }
        }
    }

    // 아이템 효과 적용
    private void ApplyItemEffect(ItemData item)
    {
        // 아이템 효과 적용
        switch (item.effectType)
        {
            case ItemEffectType.Damage:
                // 데미지 증가
                break;
            case ItemEffectType.Speed:
                // 속도 증가
                break;
            case ItemEffectType.Health:
                // 체력 증가
                break;
            // 기타 효과들...
        }
    }

    // 신규 무기 장착
    private void EquipNewWeapon(WeaponData weapon)
    {
        if (weapon != null)
        {
            // 무기 데이터의 현재 레벨을 1로 설정
            weapon.currentLevel = 1;
            
            // WeaponManager를 통해 새로운 무기 생성 및 장착
            var weaponManager = WeaponManager.Instance;
            var newWeapon = weaponManager.CreateWeapon((int)weapon.weaponType);
            if (newWeapon != null)
            {
                weaponManager.AddWeapon(newWeapon);
            }
        }
    }

    // 업그레이드 완료
    private void CompleteUpgrade()
    {
        isUpgrading = false;
        Time.timeScale = 1f; // 게임 재개
        fadePanel.SetActive(false);
        upgradePanel.SetActive(false);
    }

    // 리스트 섞기
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
} 
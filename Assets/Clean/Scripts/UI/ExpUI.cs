using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Player player;

    private void Awake()
    {
        // 컴포넌트 참조 확인
        if (expSlider == null)
        {
            Debug.LogError($"[{nameof(ExpUI)}] expSlider reference is missing!");
            enabled = false;
            return;
        }
        if (levelText == null)
        {
            Debug.LogError($"[{nameof(ExpUI)}] levelText reference is missing!");
            enabled = false;
            return;
        }
        if (player == null)
        {
            Debug.LogError($"[{nameof(ExpUI)}] player reference is missing!");
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        // 이벤트 구독
        player.onExpChanged += UpdateExpUI;
        player.onLevelChanged += UpdateLevelUI;
        
        // 초기 UI 업데이트
        UpdateExpUI(player.CurrentExp, player.MaxExp);
        UpdateLevelUI(player.Level);
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        if (player != null)
        {
            player.onExpChanged -= UpdateExpUI;
            player.onLevelChanged -= UpdateLevelUI;
        }
    }

    private void UpdateExpUI(float currentExp, float maxExp)
    {
        expSlider.value = currentExp / maxExp;
    }

    private void UpdateLevelUI(int level)
    {
        levelText.text = $"{level}";
    }
} 
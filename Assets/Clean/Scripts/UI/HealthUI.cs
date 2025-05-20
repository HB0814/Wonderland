using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Player player;

    private void Awake()
    {
        // 컴포넌트 참조 확인
        if (healthSlider == null)
        {
            Debug.LogError($"[{nameof(HealthUI)}] healthSlider reference is missing!");
            enabled = false;
            return;
        }
        if (player == null)
        {
            Debug.LogError($"[{nameof(HealthUI)}] player reference is missing!");
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        // 이벤트 구독
        player.onHealthChanged += UpdateHealthUI;
        
        // 초기 UI 업데이트
        UpdateHealthUI(player.CurrentHealth, player.MaxHealth);
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.onHealthChanged -= UpdateHealthUI;
        }
    }

    private void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        healthSlider.value = currentHealth / maxHealth;
    }
}
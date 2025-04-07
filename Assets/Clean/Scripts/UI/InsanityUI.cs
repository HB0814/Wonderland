using UnityEngine;
using UnityEngine.UI;

public class InsanityUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider insanitySlider;
    [SerializeField] private InsanitySystem insanitySystem;

    private void Awake()
    {
        // 컴포넌트 참조 확인
        if (insanitySlider == null)
        {
            Debug.LogError($"[{nameof(InsanityUI)}] insanitySlider reference is missing!");
            enabled = false;
            return;
        }
        if (insanitySystem == null)
        {
            Debug.LogError($"[{nameof(InsanityUI)}] insanitySystem reference is missing!");
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        // 이벤트 구독
        insanitySystem.onInsanityChanged += UpdateInsanityUI;
        
        // 초기 UI 업데이트
        UpdateInsanityUI(insanitySystem.CurrentInsanity);
    }

    private void OnDestroy()
    {
        if (insanitySystem != null)
        {
            insanitySystem.onInsanityChanged -= UpdateInsanityUI;
        }
    }

    private void UpdateInsanityUI(float currentInsanity)
    {
        insanitySlider.value = currentInsanity / 100f;
    }
} 
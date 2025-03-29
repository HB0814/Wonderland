using UnityEngine;

public class InsanitySystem : MonoBehaviour
{
    [Header("광기 설정")]
    [SerializeField] private float currentInsanity = 0f;
    [SerializeField] private float baseMinInsanity = 25f;  // 기본 최소 광기
    [SerializeField] private float maxInsanity = 100f;
    
    [Header("회복 설정")]
    [SerializeField] private float recoveryRate = 1f;      // 회복량
    [SerializeField] private float recoveryInterval = 2f;  // 회복 간격
    private float recoveryTimer = 0f;
    private float currentMinInsanity;  // 현재 최소 광기 제한
    
    [Header("광기 효과")]
    [SerializeField] private float speedBonus = 3f;
    [SerializeField] private float damageMultiplier25 = 1.5f;    // 25이상 데미지 배율
    [SerializeField] private float damageMultiplier50 = 2f;      // 50이상 데미지 배율
    
    public System.Action<float> onInsanityChanged;
    private Player player;
    private bool isControlsInverted = false;

    // 프로퍼티
    public float CurrentInsanity => currentInsanity;
    public float MaxInsanity => maxInsanity;
    public float CurrentMinInsanity => currentMinInsanity;

    // 광기 단계 상수
    private const float INSANITY_LEVEL_25 = 25f;
    private const float INSANITY_LEVEL_50 = 50f;
    private const float INSANITY_LEVEL_75 = 75f;

    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        player = GetComponent<Player>();
        currentMinInsanity = baseMinInsanity;

        if (player == null)
        {
            Debug.LogError($"[{nameof(InsanitySystem)}] Player component not found!");
        }
    }

    private void Update()
    {
        // 3초마다 광기 회복
        recoveryTimer += Time.deltaTime;
        if (recoveryTimer >= recoveryInterval)
        {
            recoveryTimer = 0f;
            RecoverInsanity();
        }
    }

    private void RecoverInsanity()
    {
        if (currentInsanity <= currentMinInsanity) return;

        float previousInsanity = currentInsanity;
        currentInsanity = Mathf.Max(currentMinInsanity, currentInsanity - recoveryRate);
        
        if (currentInsanity != previousInsanity)
        {
            onInsanityChanged?.Invoke(currentInsanity);
            CheckInsanityEffects(previousInsanity);
        }
    }

    public void IncreaseMinInsanity()
    {
        float previousMinInsanity = currentMinInsanity;
        currentMinInsanity = Mathf.Min(maxInsanity, currentMinInsanity + 25f);

        // 현재 광기가 새로운 최소값보다 낮다면 바로 증가
        if (currentInsanity < currentMinInsanity)
        {
            float previousInsanity = currentInsanity;
            currentInsanity = currentMinInsanity;
            onInsanityChanged?.Invoke(currentInsanity);
            CheckInsanityEffects(previousInsanity);
        }
    }

    public void AddInsanity(float amount)
    {
        if (amount <= 0) return;

        float previousInsanity = currentInsanity;
        currentInsanity = Mathf.Clamp(currentInsanity + amount, 0, maxInsanity);

        if (currentInsanity != previousInsanity)
        {
            onInsanityChanged?.Invoke(currentInsanity);
            CheckInsanityEffects(previousInsanity);
        }
    }

    public void ReduceInsanity(float amount)
    {
        if (amount <= 0) return;

        float previousInsanity = currentInsanity;
        currentInsanity = Mathf.Max(currentMinInsanity, currentInsanity - amount);

        if (currentInsanity != previousInsanity)
        {
            onInsanityChanged?.Invoke(currentInsanity);
            CheckInsanityEffects(previousInsanity);
        }
    }

    private void CheckInsanityEffects(float previousInsanity)
    {
        // 25 단계 체크
        CheckInsanityLevel(previousInsanity, INSANITY_LEVEL_25, ApplyLevel25Effects);
        
        // 50 단계 체크
        CheckInsanityLevel(previousInsanity, INSANITY_LEVEL_50, ApplyLevel50Effects);
        
        // 75 단계 체크
        CheckInsanityLevel(previousInsanity, INSANITY_LEVEL_75, ApplyLevel75Effects);
    }

    private void CheckInsanityLevel(float previousInsanity, float threshold, System.Action<bool> effectHandler)
    {
        bool crossedThreshold = currentInsanity >= threshold && previousInsanity < threshold;
        bool crossedBack = currentInsanity < threshold && previousInsanity >= threshold;

        if (crossedThreshold || crossedBack)
        {
            effectHandler?.Invoke(crossedThreshold);
        }
    }

    private void ApplyLevel25Effects(bool enable)
    {
        if (player == null) return;

        if (enable)
        {
            player.AddSpeedBonus(speedBonus);
            player.SetDamageMultiplier(damageMultiplier25);
        }
        else
        {
            player.AddSpeedBonus(-speedBonus);
            player.SetDamageMultiplier(1f);
        }
    }

    private void ApplyLevel50Effects(bool enable)
    {
        if (player == null) return;

        if (enable)
        {
            player.SetDamageMultiplier(damageMultiplier50);
        }
        else
        {
            player.SetDamageMultiplier(damageMultiplier25);
        }
    }

    private void ApplyLevel75Effects(bool enable)
    {
        isControlsInverted = enable;
    }

    public float GetDamageMultiplier()
    {
        if (currentInsanity >= INSANITY_LEVEL_50) return damageMultiplier50;
        if (currentInsanity >= INSANITY_LEVEL_25) return damageMultiplier25;
        return 1f;
    }

    public bool AreControlsInverted() => isControlsInverted;
    public float GetInsanity() => currentInsanity;
} 
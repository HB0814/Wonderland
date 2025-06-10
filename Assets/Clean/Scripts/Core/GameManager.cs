using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 게임 전체를 관리하는 매니저 클래스
/// </summary>
public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static GameManager Instance { get; private set; }

    [Header("매니저")]
    public SettingsManager settingsManager;  // 설정 매니저

    // 게임 상태 변경 이벤트
    public event System.Action OnGamePausedEvent;
    public event System.Action OnGameResumedEvent;

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 씬 전환시에도 유지
        }
        else
        {
            Destroy(gameObject);  // 이미 인스턴스가 있다면 중복 생성 방지
        }
    }

    /// <summary>
    /// 게임 일시정지 시 호출
    /// </summary>
    public void OnGamePaused()
    {
        OnGamePausedEvent?.Invoke();
    }

    /// <summary>
    /// 게임 재개 시 호출
    /// </summary>
    public void OnGameResumed()
    {
        OnGameResumedEvent?.Invoke();
    }

    /// <summary>
    /// 메인 메뉴로 이동
    /// </summary>
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainScene");
        Debug.Log("Loading main menu...");
    }

    /// <summary>
    /// 설정 메뉴 열기
    /// </summary>
    public void OpenSettings()
    {
        if (settingsManager != null)
        {
            settingsManager.OpenSettings();
        }
    }

    /// <summary>
    /// 설정 메뉴 닫기
    /// </summary>
    public void CloseSettings()
    {
        if (settingsManager != null)
        {
            settingsManager.CloseSettings();
        }
    }
} 
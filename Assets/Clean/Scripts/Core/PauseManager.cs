using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 게임 일시정지 관리 클래스
/// </summary>
public class PauseManager : MonoBehaviour
{
    [Header("일시정지 UI")]
    public GameObject pausePanel;        // 일시정지 패널
    public Button resumeButton;          // 재개 버튼
    public Button mainMenuButton;        // 메인 메뉴 버튼
    public Button settingsButton;        // 설정 버튼

    [Header("일시정지 효과")]
    public float pauseTransitionTime = 0.3f;  // 일시정지 전환 시간
    public float timeScaleWhenPaused = 0.1f;  // 일시정지 시 시간 스케일

    private bool isPaused = false;
    private float previousTimeScale = 1f;
    private CanvasGroup pausePanelCanvasGroup;

    private void Start()
    {
        // 초기화
        if (pausePanel != null)
        {
            pausePanelCanvasGroup = pausePanel.GetComponent<CanvasGroup>();
            if (pausePanelCanvasGroup == null)
            {
                pausePanelCanvasGroup = pausePanel.AddComponent<CanvasGroup>();
            }
            pausePanel.SetActive(false);
        }

        // 버튼 이벤트 설정
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame);
        }
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(LoadMainMenu);
        }
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OpenSettings);
        }
    }

    private void Update()
    {
        // ESC 키로 일시정지 토글
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    /// <summary>
    /// 일시정지 상태 토글
    /// </summary>
    public void TogglePause()
    {
        if (!isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    /// <summary>
    /// 게임 일시정지
    /// </summary>
    public void PauseGame()
    {
        if (isPaused) return;

        isPaused = true;
        previousTimeScale = Time.timeScale;
        Time.timeScale = timeScaleWhenPaused;

        // 일시정지 패널 표시
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
            StartCoroutine(FadeInPausePanel());
        }

        // 게임 상태 변경 이벤트 발생
        GameManager.Instance.OnGamePaused();
    }

    /// <summary>
    /// 게임 재개
    /// </summary>
    public void ResumeGame()
    {
        if (!isPaused) return;

        isPaused = false;
        Time.timeScale = previousTimeScale;

        // 일시정지 패널 숨김
        if (pausePanel != null)
        {
            StartCoroutine(FadeOutPausePanel());
        }

        // 게임 상태 변경 이벤트 발생
        GameManager.Instance.OnGameResumed();
    }

    /// <summary>
    /// 메인 메뉴로 이동
    /// </summary>
    private void LoadMainMenu()
    {
        Time.timeScale = 1f;  // 시간 스케일 초기화
        GameManager.Instance.LoadMainMenu();
    }

    /// <summary>
    /// 설정 메뉴 열기
    /// </summary>
    private void OpenSettings()
    {
        pausePanel.SetActive(false);
    }

    /// <summary>
    /// 일시정지 패널 페이드 인
    /// </summary>
    private IEnumerator FadeInPausePanel()
    {
        float elapsedTime = 0f;
        pausePanelCanvasGroup.alpha = 0f;

        while (elapsedTime < pauseTransitionTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            pausePanelCanvasGroup.alpha = elapsedTime / pauseTransitionTime;
            yield return null;
        }

        pausePanelCanvasGroup.alpha = 1f;
    }

    /// <summary>
    /// 일시정지 패널 페이드 아웃
    /// </summary>
    private IEnumerator FadeOutPausePanel()
    {
        float elapsedTime = 0f;
        pausePanelCanvasGroup.alpha = 1f;

        while (elapsedTime < pauseTransitionTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            pausePanelCanvasGroup.alpha = 1f - (elapsedTime / pauseTransitionTime);
            yield return null;
        }

        pausePanelCanvasGroup.alpha = 0f;
        pausePanel.SetActive(false);
    }
} 
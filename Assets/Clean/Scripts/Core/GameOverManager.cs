using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;

    [Header("Game Clear UI")]
    [SerializeField] private GameObject gameClearPanel;
    [SerializeField] private TextMeshProUGUI clearTimeText;
    [SerializeField] private Button gameClearMainMenuButton;

    private float gameStartTime;

    private void Start()
    {
        // 초기화
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        if (gameClearPanel != null)
        {
            gameClearPanel.SetActive(false);
        }

        // 버튼 이벤트 등록
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(OnRetryButtonClick);
        }
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
        }
        if (gameClearMainMenuButton != null)
        {
            gameClearMainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
        }

        gameStartTime = Time.time;
    }

    /// <summary>
    /// 게임 오버 UI를 표시합니다.
    /// </summary>
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f; // 게임 일시정지
        }
    }

    /// <summary>
    /// 게임 클리어 UI를 표시합니다.
    /// </summary>
    public void ShowGameClear()
    {
        if (gameClearPanel != null)
        {
            gameClearPanel.SetActive(true);
            Time.timeScale = 0f; // 게임 일시정지

            // 클리어 시간 계산 및 표시
            float clearTime = Time.time - gameStartTime;
            int minutes = Mathf.FloorToInt(clearTime / 60);
            int seconds = Mathf.FloorToInt(clearTime % 60);
            
            if (clearTimeText != null)
            {
                clearTimeText.text = string.Format("클리어 시간: {0:00}:{1:00}", minutes, seconds);
            }
        }
    }

    /// <summary>
    /// 다시하기 버튼 클릭 시 호출됩니다.
    /// </summary>
    private void OnRetryButtonClick()
    {
        Time.timeScale = 1f; // 게임 시간 복원
        WeaponManager.Instance.RemoveAllWeapons();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬 다시 로드
    }

    /// <summary>
    /// 메인 메뉴 버튼 클릭 시 호출됩니다.
    /// </summary>
    private void OnMainMenuButtonClick()
    {
        Time.timeScale = 1f; // 게임 시간 복원
        SceneManager.LoadScene("MainMenu"); // 메인 메뉴 씬으로 이동
    }
} 
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button achievementButton;

    private void Start()
    {
        // 초기화: 난이도 패널은 비활성화
        if (difficultyPanel != null)
            difficultyPanel.SetActive(false);

        // 버튼 이벤트 등록
        if (startButton != null)
            startButton.onClick.AddListener(OnStartButtonClick);
        
        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitButtonClick);

        if (achievementButton != null)
            achievementButton.onClick.AddListener(OnAchievementButtonClick);
    }

    private void OnStartButtonClick()
    {
        // 메인 패널 비활성화
        gameObject.SetActive(false);
        // 난이도 패널 활성화
        if (difficultyPanel != null)
            difficultyPanel.SetActive(true);
    }

    private void OnQuitButtonClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void OnAchievementButtonClick()
    {
        // 여기에 업적 패널 활성화 로직 추가
        Debug.Log("업적 패널 활성화");
    }
} 
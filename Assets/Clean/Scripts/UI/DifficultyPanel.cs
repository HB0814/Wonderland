using UnityEngine;
using UnityEngine.UI;

public class DifficultyPanel : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button easyButton;
    [SerializeField] private Button normalButton;
    [SerializeField] private Button hardButton;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject easyDescription;
    [SerializeField] private GameObject normalDescription;
    [SerializeField] private GameObject hardDescription;

    private bool easySelected = false;
    private bool normalSelected = false;
    private bool hardSelected = false;

    private void Start()
    {
        // 버튼 이벤트 등록
        if (backButton != null)
            backButton.onClick.AddListener(OnBackButtonClick);
        
        if (easyDescription != null) easyDescription.SetActive(false);
        if (normalDescription != null) normalDescription.SetActive(false);
        if (hardDescription != null) hardDescription.SetActive(false);

        if (easyButton != null)
            easyButton.onClick.AddListener(OnEasyButtonClick);
        
        if (normalButton != null)
            normalButton.onClick.AddListener(OnNormalButtonClick);
        
        if (hardButton != null)
            hardButton.onClick.AddListener(OnHardButtonClick);
    }

    private void OnBackButtonClick()
    {
        // 난이도 패널 비활성화
        gameObject.SetActive(false);
        // 메인 패널 활성화
        if (mainPanel != null)
            mainPanel.SetActive(true);

        // 설명창 모두 비활성화
        if (easyDescription != null) easyDescription.SetActive(false);
        if (normalDescription != null) normalDescription.SetActive(false);
        if (hardDescription != null) hardDescription.SetActive(false);

        // 선택 상태 초기화
        easySelected = false;
        normalSelected = false;
        hardSelected = false;
    }

    private void OnEasyButtonClick()
    {
        if (!easySelected)
        {
            easyDescription.SetActive(true);
            normalDescription.SetActive(false);
            hardDescription.SetActive(false);
            easySelected = true;
            normalSelected = false;
            hardSelected = false;
        }
        else
        {
            GameSceneManager.Instance.LoadNextScene();
        }
    }

    private void OnNormalButtonClick()
    {
        if (!normalSelected)
        {
            easyDescription.SetActive(false);
            normalDescription.SetActive(true);
            hardDescription.SetActive(false);
            easySelected = false;
            normalSelected = true;
            hardSelected = false;
        }
        else
        {
            GameSceneManager.Instance.LoadNextScene();
        }
    }

    private void OnHardButtonClick()
    {
        if (!hardSelected)
        {
            easyDescription.SetActive(false);
            normalDescription.SetActive(false);
            hardDescription.SetActive(true);
            easySelected = false;
            normalSelected = false;
            hardSelected = true;
        }
        else
        {
            GameSceneManager.Instance.LoadNextScene();
        }
    }
}
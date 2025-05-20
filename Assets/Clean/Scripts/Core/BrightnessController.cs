using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 게임 화면의 밝기를 조절하는 클래스
/// </summary>
public class BrightnessController : MonoBehaviour
{
    [Header("UI 설정")]
    [SerializeField] private Canvas brightnessCanvas;
    [SerializeField] private Image brightnessOverlay;
    [SerializeField] private CanvasScaler canvasScaler;

    private void Awake()
    {
        // Canvas가 없으면 생성
        if (brightnessCanvas == null)
        {
            CreateBrightnessCanvas();
        }

        // CanvasScaler가 없으면 추가
        if (canvasScaler == null)
        {
            canvasScaler = brightnessCanvas.GetComponent<CanvasScaler>();
        }

        // Image가 없으면 생성
        if (brightnessOverlay == null)
        {
            CreateBrightnessOverlay();
        }
    }

    private void CreateBrightnessCanvas()
    {
        // Canvas 생성
        GameObject canvasObj = new GameObject("BrightnessCanvas");
        brightnessCanvas = canvasObj.AddComponent<Canvas>();
        brightnessCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        brightnessCanvas.sortingOrder = 100; // 다른 UI 요소들보다 낮은 순서

        // CanvasScaler 추가
        canvasScaler = canvasObj.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);

        // Canvas가 씬이 변경되어도 파괴되지 않도록 설정
        DontDestroyOnLoad(canvasObj);
    }

    private void CreateBrightnessOverlay()
    {
        // Image 생성
        GameObject overlayObj = new GameObject("BrightnessOverlay");
        overlayObj.transform.SetParent(brightnessCanvas.transform, false);
        
        brightnessOverlay = overlayObj.AddComponent<Image>();
        brightnessOverlay.color = new Color(0, 0, 0, 0.5f);
        brightnessOverlay.raycastTarget = false;

        // RectTransform 설정
        RectTransform rectTransform = brightnessOverlay.rectTransform;
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;
    }

    /// <summary>
    /// 밝기 설정
    /// </summary>
    public void SetBrightness(float brightness)
    {
        if (brightnessOverlay != null)
        {
            // 밝기 값을 0-200 범위로 변환 (0이 가장 밝음, 200이 가장 어두움)
            float alphaValue = 200f * (1f - brightness);
            Color color = brightnessOverlay.color;
            color.a = alphaValue / 255f;
            brightnessOverlay.color = color;
            Debug.Log($"Brightness set to: {brightness}, Alpha: {alphaValue}");
        }
    }

    /// <summary>
    /// 현재 밝기 값 가져오기
    /// </summary>
    public float GetCurrentBrightness()
    {
        if (brightnessOverlay != null)
        {
            return 1f - (brightnessOverlay.color.a * 255f / 200f);
        }
        return 0.5f;
    }

    private void OnDestroy()
    {
        if (brightnessCanvas != null)
        {
            Destroy(brightnessCanvas.gameObject);
        }
    }
} 
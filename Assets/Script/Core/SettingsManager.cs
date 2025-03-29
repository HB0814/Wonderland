using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 게임 설정을 관리하는 클래스
/// </summary>
public class SettingsManager : MonoBehaviour
{
    [Header("오디오 믹서")]
    [SerializeField] private AudioMixer audioMixer;  // 오디오 믹서

    [Header("설정 UI")]
    [SerializeField] private GameObject audioPanel;        // 사운드 설정 패널
    [SerializeField] private GameObject graphicsPanel;     // 그래픽 설정 패널
    [SerializeField] private GameObject pausePanel;        // 일시정지 패널

    [Header("오디오 설정")]
    [SerializeField] private Slider masterVolumeSlider;            // 전체 볼륨 슬라이더
    [SerializeField] private Slider bgmVolumeSlider;               // BGM 볼륨 슬라이더
    [SerializeField] private Slider sfxVolumeSlider;               // 효과음 볼륨 슬라이더

    [Header("그래픽 설정")]
    [SerializeField] private Slider brightnessSlider;              // 밝기 슬라이더
    [SerializeField] private Toggle fullscreenToggle;              // 전체화면 토글
    [SerializeField] private Dropdown qualityDropdown;             // 품질 드롭다운
    [SerializeField] private Dropdown resolutionDropdown;          // 해상도 드롭다운
    [SerializeField] private BrightnessController brightnessController;  // 밝기 컨트롤러

    // 해상도 설정
    private readonly Vector2[] customResolutions = new Vector2[]
    {
        new Vector2(1920, 1080),  // Full HD
        new Vector2(1280, 720),   // HD
        new Vector2(800, 600)     // SVGA
    };

    // 설정값
    private float masterVolume = 1f;
    private float bgmVolume = 1f;
    private float sfxVolume = 1f;
    private float screenBrightness = 1f;
    private bool fullScreen = true;
    private int qualityLevel = 2;  // 기본값: High
    private int currentResolutionIndex = 0;

    // 설정 변경 이벤트
    public event Action<float> OnMasterVolumeChanged;
    public event Action<float> OnBGMVolumeChanged;
    public event Action<float> OnSFXVolumeChanged;
    public event Action<float> OnBrightnessChanged;
    public event Action<bool> OnFullScreenChanged;
    public event Action<int> OnQualityLevelChanged;
    public event Action<Vector2> OnResolutionChanged;

    // PlayerPrefs 키
    private const string RESOLUTION_INDEX_KEY = "ResolutionIndex";
    private const string FULLSCREEN_KEY = "Fullscreen";
    private const string BRIGHTNESS_KEY = "Brightness";
    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string BGM_VOLUME_KEY = "BGMVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const string QUALITY_LEVEL_KEY = "QualityLevel";

    private void Awake()
    {
        // 이벤트 리스너 등록
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        if (bgmVolumeSlider != null)
            bgmVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        if (brightnessSlider != null)
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.AddListener(SetFullScreen);
        if (qualityDropdown != null)
            qualityDropdown.onValueChanged.AddListener(SetQualityLevel);
        if (resolutionDropdown != null)
            resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    private void Start()
    {
        // 저장된 설정 불러오기
        LoadSettings();
        
        // 초기 UI 상태 설정
        if (audioPanel != null) audioPanel.SetActive(false);
        if (graphicsPanel != null) graphicsPanel.SetActive(false);
    }

    /// <summary>
    /// 사용 가능한 해상도 목록 가져오기
    /// </summary>
    public List<string> GetResolutionOptions()
    {
        List<string> options = new List<string>(customResolutions.Length);
        foreach (Vector2 resolution in customResolutions)
        {
            options.Add($"{resolution.x}x{resolution.y}");
        }
        return options;
    }

    /// <summary>
    /// 해상도 설정
    /// </summary>
    public void SetResolution(int index)
    {
        if (index >= 0 && index < customResolutions.Length)
        {
            currentResolutionIndex = index;
            Vector2 resolution = customResolutions[index];
            
            // 전체화면이 켜져있으면 끄기
            if (fullScreen)
            {
                SetFullScreen(false);
            }
            
            // 해상도 변경
            Screen.SetResolution((int)resolution.x, (int)resolution.y, fullScreen);
            PlayerPrefs.SetInt(RESOLUTION_INDEX_KEY, currentResolutionIndex);
            OnResolutionChanged?.Invoke(resolution);
        }
    }

    /// <summary>
    /// 전체화면 설정
    /// </summary>
    public void SetFullScreen(bool isFullScreen)
    {
        if (fullScreen != isFullScreen)
        {
            fullScreen = isFullScreen;
            Screen.fullScreen = fullScreen;
            
            // Toggle의 값을 변경할 때 이벤트를 일시적으로 제거
            if (fullscreenToggle != null)
            {
                fullscreenToggle.onValueChanged.RemoveListener(SetFullScreen);
                fullscreenToggle.isOn = fullScreen;
                fullscreenToggle.onValueChanged.AddListener(SetFullScreen);
            }
            
            PlayerPrefs.SetInt(FULLSCREEN_KEY, fullScreen ? 1 : 0);
            OnFullScreenChanged?.Invoke(fullScreen);
        }
    }

    /// <summary>
    /// 밝기 설정
    /// </summary>
    public void SetBrightness(float brightness)
    {
        if (brightnessController != null)
        {
            screenBrightness = Mathf.Clamp01(brightness);
            brightnessController.SetBrightness(screenBrightness);
            PlayerPrefs.SetFloat(BRIGHTNESS_KEY, screenBrightness);
            OnBrightnessChanged?.Invoke(screenBrightness);
        }
    }

    /// <summary>
    /// 마스터 볼륨 설정
    /// </summary>
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
        }
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, masterVolume);
        OnMasterVolumeChanged?.Invoke(masterVolume);
    }

    /// <summary>
    /// BGM 볼륨 설정
    /// </summary>
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        if (audioMixer != null)
        {
            audioMixer.SetFloat("BGMVolume", Mathf.Log10(bgmVolume) * 20);
        }
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, bgmVolume);
        OnBGMVolumeChanged?.Invoke(bgmVolume);
    }

    /// <summary>
    /// 효과음 볼륨 설정
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (audioMixer != null)
        {
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
        }
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxVolume);
        OnSFXVolumeChanged?.Invoke(sfxVolume);
    }

    /// <summary>
    /// 그래픽 품질 설정
    /// </summary>
    public void SetQualityLevel(int level)
    {
        qualityLevel = Mathf.Clamp(level, 0, QualitySettings.names.Length - 1);
        QualitySettings.SetQualityLevel(qualityLevel);
        PlayerPrefs.SetInt(QUALITY_LEVEL_KEY, qualityLevel);
        OnQualityLevelChanged?.Invoke(qualityLevel);
    }

    /// <summary>
    /// 설정 불러오기
    /// </summary>
    private void LoadSettings()
    {
        masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 1f);
        bgmVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
        screenBrightness = PlayerPrefs.GetFloat(BRIGHTNESS_KEY, 0.5f);
        fullScreen = PlayerPrefs.GetInt(FULLSCREEN_KEY, 1) == 1;
        qualityLevel = PlayerPrefs.GetInt(QUALITY_LEVEL_KEY, 2);
        currentResolutionIndex = PlayerPrefs.GetInt(RESOLUTION_INDEX_KEY, 0);

        // 설정 적용
        SetMasterVolume(masterVolume);
        SetBGMVolume(bgmVolume);
        SetSFXVolume(sfxVolume);
        SetBrightness(screenBrightness);
        SetFullScreen(fullScreen);
        SetQualityLevel(qualityLevel);
        SetResolution(currentResolutionIndex);
    }

    // 현재 설정값 가져오기
    public float GetMasterVolume() => masterVolume;
    public float GetBGMVolume() => bgmVolume;
    public float GetSFXVolume() => sfxVolume;
    public float GetBrightness() => screenBrightness;
    public bool GetFullScreen() => fullScreen;
    public int GetQualityLevel() => qualityLevel;
    public int GetCurrentResolutionIndex() => currentResolutionIndex;
    public Vector2 GetCurrentResolution() => customResolutions[currentResolutionIndex];

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 설정 패널을 엽니다.
    /// </summary>
    public void OpenSettings()
    {
        if (audioPanel != null) audioPanel.SetActive(true);
        if (graphicsPanel != null) graphicsPanel.SetActive(true);
        if (pausePanel != null) pausePanel.SetActive(true);
    }

    /// <summary>
    /// 설정 패널을 닫습니다.
    /// </summary>
    public void CloseSettings()
    {
        if (audioPanel != null) audioPanel.SetActive(false);
        if (graphicsPanel != null) graphicsPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);

        //게임 재개
        Time.timeScale = 1f;
    }

    /// <summary>
    /// 오디오 설정 패널을 표시합니다.
    /// </summary>
    public void ShowAudioPanel()
    {
        if (audioPanel != null) audioPanel.SetActive(true);
        if (graphicsPanel != null) graphicsPanel.SetActive(false);
    }

    /// <summary>
    /// 그래픽 설정 패널을 표시합니다.
    /// </summary>
    public void ShowGraphicsPanel()
    {
        if (audioPanel != null) audioPanel.SetActive(false);
        if (graphicsPanel != null) graphicsPanel.SetActive(true);
    }
} 
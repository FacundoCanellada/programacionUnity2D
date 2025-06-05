using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro;

public class GameSettingsManager : MonoBehaviour
{
    public static GameSettingsManager Instance { get; private set; }

    public int currentResolutionWidth;
    public int currentResolutionHeight;
    public int currentQualityLevel;
    public bool isFullScreen;

    // --- Claves para PlayerPrefs ---
    private const string RESOLUTION_WIDTH_KEY = "ResolutionWidth";
    private const string RESOLUTION_HEIGHT_KEY = "ResolutionHeight";
    private const string QUALITY_LEVEL_KEY = "QualityLevel";
    private const string FULLSCREEN_KEY = "FullScreen";

    private Resolution[] availableResolutions;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            availableResolutions = Screen.resolutions;

            LoadSettings();
            ApplySettingsToUnity();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void LoadSettings()
    {
        currentResolutionWidth = PlayerPrefs.GetInt(RESOLUTION_WIDTH_KEY, Screen.currentResolution.width);
        currentResolutionHeight = PlayerPrefs.GetInt(RESOLUTION_HEIGHT_KEY, Screen.currentResolution.height);

        currentQualityLevel = PlayerPrefs.GetInt(QUALITY_LEVEL_KEY, QualitySettings.GetQualityLevel());

        isFullScreen = PlayerPrefs.GetInt(FULLSCREEN_KEY, Screen.fullScreen ? 1 : 0) == 1;

        Debug.Log($"Settings Loaded: Resolution={currentResolutionWidth}x{currentResolutionHeight}, Quality={currentQualityLevel}, FullScreen={isFullScreen}");
    }
    public void ApplySettingsToUnity()
    {
        Resolution targetResolution = Screen.currentResolution;
        bool foundResolution = false;
        for (int i = 0; i < availableResolutions.Length; i++)
        {
            if (availableResolutions[i].width == currentResolutionWidth &&
                availableResolutions[i].height == currentResolutionHeight)
            {
                targetResolution = availableResolutions[i];
                foundResolution = true;
                break;
            }
        }
        if (!foundResolution)
        {
            targetResolution = Screen.currentResolution;
            currentResolutionWidth = targetResolution.width; 
            currentResolutionHeight = targetResolution.height;
        }

        Screen.SetResolution(targetResolution.width, targetResolution.height, isFullScreen);

        QualitySettings.SetQualityLevel(currentQualityLevel);

        Screen.fullScreen = isFullScreen;

        Debug.Log($"Settings Applied: Resolution={Screen.width}x{Screen.height} ({Screen.fullScreen}), Quality={QualitySettings.GetQualityLevel()}");
    }
    public void SaveSettings()
    {
        PlayerPrefs.SetInt(RESOLUTION_WIDTH_KEY, currentResolutionWidth);
        PlayerPrefs.SetInt(RESOLUTION_HEIGHT_KEY, currentResolutionHeight);
        PlayerPrefs.SetInt(QUALITY_LEVEL_KEY, currentQualityLevel);
        PlayerPrefs.SetInt(FULLSCREEN_KEY, isFullScreen ? 1 : 0);
        PlayerPrefs.Save(); 
        Debug.Log("Settings Saved to PlayerPrefs.");
    }
    public void SetAndSaveResolution(int dropdownIndex)
    {
        if (dropdownIndex >= 0 && dropdownIndex < availableResolutions.Length)
        {
            Resolution selectedRes = availableResolutions[dropdownIndex];
            currentResolutionWidth = selectedRes.width;
            currentResolutionHeight = selectedRes.height;
            ApplySettingsToUnity();
            SaveSettings();
        }
        else
        {
            Debug.LogError("Índice de resolución fuera de rango: " + dropdownIndex);
            currentResolutionWidth = Screen.currentResolution.width;
            currentResolutionHeight = Screen.currentResolution.height;
            ApplySettingsToUnity();
            SaveSettings();
        }
    }

    public void SetAndSaveQuality(int qualityIndex)
    {
        currentQualityLevel = qualityIndex;
        ApplySettingsToUnity();
        SaveSettings();
    }

    public void SetAndSaveFullScreen(bool fullScreen)
    {
        isFullScreen = fullScreen;
        ApplySettingsToUnity();
        SaveSettings();
    }
    public Resolution[] GetAvailableResolutions()
    {
        return availableResolutions;
    }
}
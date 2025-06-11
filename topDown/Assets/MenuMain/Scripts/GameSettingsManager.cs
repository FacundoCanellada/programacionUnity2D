using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameSettingsManager : MonoBehaviour
{
    public static GameSettingsManager Instance { get; private set; }

    public int currentResolutionWidth;
    public int currentResolutionHeight;
    public int currentQualityLevel;
    public bool isFullScreen;

    private const string RESOLUTION_WIDTH_KEY = "ResolutionWidth";
    private const string RESOLUTION_HEIGHT_KEY = "ResolutionHeight";
    private const string QUALITY_LEVEL_KEY = "QualityLevel";
    private const string FULLSCREEN_KEY = "FullScreen";

    private Resolution[] availableResolutions;
    private List<Resolution> uniqueResolutions = new List<Resolution>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            availableResolutions = Screen.resolutions;
            GenerateUniqueResolutions();

            LoadSettings();
            ApplySettingsToUnity();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void GenerateUniqueResolutions()
    {
        HashSet<string> added = new HashSet<string>();
        uniqueResolutions.Clear();

        foreach (var res in availableResolutions)
        {
            string key = res.width + "x" + res.height;
            if (!added.Contains(key))
            {
                added.Add(key);
                uniqueResolutions.Add(res);
            }
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
        Resolution targetResolution = new Resolution();
        bool found = false;

        foreach (var res in uniqueResolutions)
        {
            if (res.width == currentResolutionWidth && res.height == currentResolutionHeight)
            {
                targetResolution = res;
                found = true;
                break;
            }
        }

        if (!found)
        {
            targetResolution = Screen.currentResolution;
            currentResolutionWidth = targetResolution.width;
            currentResolutionHeight = targetResolution.height;
        }

        Screen.SetResolution(targetResolution.width, targetResolution.height, isFullScreen);
        QualitySettings.SetQualityLevel(currentQualityLevel);
        Screen.fullScreen = isFullScreen;

        Debug.Log($"Settings Applied: {Screen.width}x{Screen.height}, FullScreen={Screen.fullScreen}");
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt(RESOLUTION_WIDTH_KEY, currentResolutionWidth);
        PlayerPrefs.SetInt(RESOLUTION_HEIGHT_KEY, currentResolutionHeight);
        PlayerPrefs.SetInt(QUALITY_LEVEL_KEY, currentQualityLevel);
        PlayerPrefs.SetInt(FULLSCREEN_KEY, isFullScreen ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Settings Saved.");
    }

    public void SetAndSaveResolution(int dropdownIndex)
    {
        if (dropdownIndex >= 0 && dropdownIndex < uniqueResolutions.Count)
        {
            Resolution selected = uniqueResolutions[dropdownIndex];
            currentResolutionWidth = selected.width;
            currentResolutionHeight = selected.height;
            ApplySettingsToUnity();
            SaveSettings();
        }
        else
        {
            Debug.LogError("Dropdown index fuera de rango.");
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

    public List<Resolution> GetFilteredResolutions()
    {
        return uniqueResolutions;
    }
}

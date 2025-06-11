using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] public AudioMixer audioMixer;
    [SerializeField] public Slider musicSilder;

    [SerializeField] public TMP_Dropdown resolutionDropdown;
    [SerializeField] public TMP_Dropdown qualityDropdown;
    [SerializeField] public Toggle fullScreenToggle;

    private List<string> resolutionOptions = new List<string>();

    void Start()
    {
        if (GameSettingsManager.Instance == null)
        {
            Debug.LogError("GameSettingsManager no encontrado.");
            return;
        }

        InitializeResolutionDropdown();
        UpdateUIFromGameSettingsManager();
        SetupUISetListeners();
    }

    void InitializeResolutionDropdown()
    {
        List<Resolution> availableResolutions = GameSettingsManager.Instance.GetFilteredResolutions();
        resolutionDropdown.ClearOptions();
        resolutionOptions.Clear();

        foreach (var res in availableResolutions)
        {
            string option = res.width + " x " + res.height;
            resolutionOptions.Add(option);
        }

        resolutionDropdown.AddOptions(resolutionOptions);
    }

    void UpdateUIFromGameSettingsManager()
    {
        string currentRes = GameSettingsManager.Instance.currentResolutionWidth + " x " + GameSettingsManager.Instance.currentResolutionHeight;
        int selectedIndex = resolutionOptions.FindIndex(option => option == currentRes);
        resolutionDropdown.value = Mathf.Max(selectedIndex, 0);
        resolutionDropdown.RefreshShownValue();

        qualityDropdown.value = GameSettingsManager.Instance.currentQualityLevel;
        qualityDropdown.RefreshShownValue();

        fullScreenToggle.isOn = GameSettingsManager.Instance.isFullScreen;

        if (audioMixer != null && audioMixer.GetFloat("volume", out float currentVolume))
        {
            musicSilder.value = Mathf.Pow(10, currentVolume / 20);
        }
    }

    void SetupUISetListeners()
    {
        resolutionDropdown.onValueChanged.RemoveAllListeners();
        qualityDropdown.onValueChanged.RemoveAllListeners();
        fullScreenToggle.onValueChanged.RemoveAllListeners();
        musicSilder.onValueChanged.RemoveAllListeners();

        resolutionDropdown.onValueChanged.AddListener(GameSettingsManager.Instance.SetAndSaveResolution);
        qualityDropdown.onValueChanged.AddListener(GameSettingsManager.Instance.SetAndSaveQuality);
        fullScreenToggle.onValueChanged.AddListener(GameSettingsManager.Instance.SetAndSaveFullScreen);
        musicSilder.onValueChanged.AddListener((float value) => SetMusicVolume());
    }

    public void SetMusicVolume()
    {
        float volume = musicSilder.value;
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
    }

    public void ApplySettings()
    {
        GameSettingsManager.Instance.SaveSettings();
        Debug.Log("Opciones aplicadas y guardadas.");
    }
}

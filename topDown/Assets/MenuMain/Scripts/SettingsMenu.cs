using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] public AudioMixer audioMixer;
    [SerializeField] public Slider musicSilder;

    // Propiedades de Gráficos (ahora interactúan con GameSettingsManager)
    [SerializeField] public TMPro.TMP_Dropdown resolutionDropdown;
    [SerializeField] public TMPro.TMP_Dropdown qualityDropdown;
    [SerializeField] public Toggle fullScreenToggle;

    private List<string> resolutionOptions = new List<string>();

    void Start()
    {
        if (GameSettingsManager.Instance == null)
        {
            Debug.LogError("GameSettingsManager no encontrado. Asegúrate de que un GameObject con GameSettingsManager esté en tu escena de inicio y use DontDestroyOnLoad().");
            return;
        }

        InitializeResolutionDropdown(); 
        UpdateUIFromGameSettingsManager(); 
        SetupUISetListeners(); 
    }
    void InitializeResolutionDropdown()
    {
        Resolution[] availableResolutions = GameSettingsManager.Instance.GetAvailableResolutions();
        resolutionDropdown.ClearOptions();
        resolutionOptions.Clear(); // Limpia la lista interna de strings también

        HashSet<string> addedResolutions = new HashSet<string>();

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            string option = availableResolutions[i].width + " x " + availableResolutions[i].height;
            if (!addedResolutions.Contains(option))
            {
                addedResolutions.Add(option);
                resolutionOptions.Add(option); // Guarda los strings de las opciones
            }
        }
        resolutionDropdown.AddOptions(resolutionOptions);
    }
    void UpdateUIFromGameSettingsManager()
    {
        string savedResolutionString = GameSettingsManager.Instance.currentResolutionWidth + " x " + GameSettingsManager.Instance.currentResolutionHeight;
        int targetDropdownIndex = 0;

        for (int i = 0; i < resolutionOptions.Count; i++) 
        {
            if (resolutionOptions[i] == savedResolutionString)
            {
                targetDropdownIndex = i;
                break;
            }
        }
        resolutionDropdown.value = targetDropdownIndex;
        resolutionDropdown.RefreshShownValue();

        // Actualizar Dropdown de Calidad
        qualityDropdown.value = GameSettingsManager.Instance.currentQualityLevel;
        qualityDropdown.RefreshShownValue();

        // Actualizar Toggle de Pantalla Completa
        fullScreenToggle.isOn = GameSettingsManager.Instance.isFullScreen;

        float currentVolume;
        if (audioMixer != null && audioMixer.GetFloat("volume", out currentVolume))
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
    public void SetResolution(int resolutionIndex)
    {
        GameSettingsManager.Instance.SetAndSaveResolution(resolutionIndex);
    }

    public void SetMusicVolume()
    {
        float volume = musicSilder.value;
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
    }

    public void SetQuality(int qualityIndex)
    {
        GameSettingsManager.Instance.SetAndSaveQuality(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        GameSettingsManager.Instance.SetAndSaveFullScreen(isFullScreen);
    }

    public void ApplySettings()
    {
        GameSettingsManager.Instance.SaveSettings();
        Debug.Log("Opciones de pantalla y calidad guardadas y aplicadas.");
    }
}
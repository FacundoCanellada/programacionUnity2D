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

            // Inicializa las resoluciones disponibles una vez al inicio del juego
            availableResolutions = Screen.resolutions;

            // Carga las configuraciones al inicio del juego
            LoadSettings();
            // Aplica las configuraciones inmediatamente al motor de Unity
            ApplySettingsToUnity();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void LoadSettings()
    {
        // Cargar resoluci�n. Usamos la resoluci�n actual del sistema como valor por defecto.
        currentResolutionWidth = PlayerPrefs.GetInt(RESOLUTION_WIDTH_KEY, Screen.currentResolution.width);
        currentResolutionHeight = PlayerPrefs.GetInt(RESOLUTION_HEIGHT_KEY, Screen.currentResolution.height);

        // Cargar nivel de calidad. Por defecto, el nivel actual de Unity.
        currentQualityLevel = PlayerPrefs.GetInt(QUALITY_LEVEL_KEY, QualitySettings.GetQualityLevel());

        // Cargar estado de pantalla completa. Por defecto, el estado actual de Unity.
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
        PlayerPrefs.Save(); // Forzar el guardado inmediato en disco
        Debug.Log("Settings Saved to PlayerPrefs.");
    }
    /// Establece y guarda la resoluci�n basada en el �ndice seleccionado en el Dropdown.
    /// <param name="dropdownIndex">El �ndice seleccionado en el Dropdown de resoluci�n.</param>
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
            Debug.LogError("�ndice de resoluci�n fuera de rango: " + dropdownIndex);
            // Si el �ndice es inv�lido, podr�as cargar la resoluci�n actual del sistema
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

    // M�todo para obtener las resoluciones para el dropdown del men�
    public Resolution[] GetAvailableResolutions()
    {
        return availableResolutions;
    }
}
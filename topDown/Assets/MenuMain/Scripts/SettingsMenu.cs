using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    // Propiedades de Audio (sin cambios, gestionadas por tu AudioManager)
    [SerializeField] public AudioMixer audioMixer;
    [SerializeField] public Slider musicSilder;

    // Propiedades de Gr�ficos (ahora interact�an con GameSettingsManager)
    [SerializeField] public TMPro.TMP_Dropdown resolutionDropdown;
    [SerializeField] public TMPro.TMP_Dropdown qualityDropdown;
    [SerializeField] public Toggle fullScreenToggle;

    // Almacenar� las opciones de resoluci�n (strings) para el dropdown
    private List<string> resolutionOptions = new List<string>();

    void Start()
    {
        if (GameSettingsManager.Instance == null)
        {
            Debug.LogError("GameSettingsManager no encontrado. Aseg�rate de que un GameObject con GameSettingsManager est� en tu escena de inicio y use DontDestroyOnLoad().");
            return;
        }

        InitializeResolutionDropdown(); // Primero llena el dropdown
        UpdateUIFromGameSettingsManager(); // Luego sincroniza la UI con los valores del Manager
        SetupUISetListeners(); // Finalmente, configura los listeners
    }

    /// <summary>
    /// Llena el Dropdown de resoluciones con las opciones disponibles del sistema.
    /// </summary>
    void InitializeResolutionDropdown()
    {
        Resolution[] availableResolutions = GameSettingsManager.Instance.GetAvailableResolutions();
        resolutionDropdown.ClearOptions();
        resolutionOptions.Clear(); // Limpia la lista interna de strings tambi�n

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

    /// <summary>
    /// Actualiza los elementos de la UI (Dropdowns, Toggles, Sliders)
    /// para reflejar los valores actuales almacenados en el GameSettingsManager.
    /// </summary>
    void UpdateUIFromGameSettingsManager()
    {
        // Actualizar Dropdown de Resoluci�n
        string savedResolutionString = GameSettingsManager.Instance.currentResolutionWidth + " x " + GameSettingsManager.Instance.currentResolutionHeight;
        int targetDropdownIndex = 0;

        // Busca el string de la resoluci�n guardada en las opciones del dropdown
        for (int i = 0; i < resolutionOptions.Count; i++) // Usa resolutionOptions que tiene los strings �nicos
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

        // Mantener la l�gica de volumen si es necesaria
        float currentVolume;
        if (audioMixer != null && audioMixer.GetFloat("volume", out currentVolume))
        {
            musicSilder.value = Mathf.Pow(10, currentVolume / 20);
        }
    }

    /// <summary>
    /// Configura los listeners de los elementos de UI para que llamen a los m�todos
    /// del GameSettingsManager cuando sus valores cambien.
    /// </summary>
    void SetupUISetListeners()
    {
        resolutionDropdown.onValueChanged.RemoveAllListeners();
        qualityDropdown.onValueChanged.RemoveAllListeners();
        fullScreenToggle.onValueChanged.RemoveAllListeners();
        musicSilder.onValueChanged.RemoveAllListeners();

        // Ahora el listener de resoluci�n necesita convertir el �ndice del dropdown
        // a los valores de ancho y alto y pas�rselos al manager.
        // O m�s directo: el manager recibe el �ndice del dropdown y hace la b�squeda.
        // Optamos por que el manager reciba el �ndice del dropdown.
        resolutionDropdown.onValueChanged.AddListener(GameSettingsManager.Instance.SetAndSaveResolution);
        qualityDropdown.onValueChanged.AddListener(GameSettingsManager.Instance.SetAndSaveQuality);
        fullScreenToggle.onValueChanged.AddListener(GameSettingsManager.Instance.SetAndSaveFullScreen);
        musicSilder.onValueChanged.AddListener((float value) => SetMusicVolume());
    }

    // --- M�todos p�blicos de tu men� (llamados desde la UI) ---
    // Estos m�todos ahora simplemente "pasan" la llamada al GameSettingsManager

    public void SetResolution(int resolutionIndex)
    {
        // El dropdownIndex se pasa directamente al manager
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
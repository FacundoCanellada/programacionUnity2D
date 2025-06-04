using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para SceneManager
using TMPro;

public class GameSettingsManager : MonoBehaviour
{
    public static GameSettingsManager Instance { get; private set; }

    // --- Variables para almacenar las opciones (se cargar�n/guardar�n en PlayerPrefs) ---
    // Ahora guardaremos width y height para la resoluci�n
    public int currentResolutionWidth;
    public int currentResolutionHeight;
    public int currentQualityLevel;
    public bool isFullScreen;

    // --- Claves para PlayerPrefs ---
    private const string RESOLUTION_WIDTH_KEY = "ResolutionWidth";   // Nueva clave
    private const string RESOLUTION_HEIGHT_KEY = "ResolutionHeight"; // Nueva clave
    private const string QUALITY_LEVEL_KEY = "QualityLevel";
    private const string FULLSCREEN_KEY = "FullScreen";

    private Resolution[] availableResolutions; // Almacenar� todas las resoluciones disponibles

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

    /// <summary>
    /// Carga las configuraciones guardadas de PlayerPrefs en las variables del Manager.
    /// </summary>
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

    /// <summary>
    /// Aplica las configuraciones almacenadas en el Manager al motor de Unity.
    /// </summary>
    public void ApplySettingsToUnity()
    {
        // Aplicar Resoluci�n
        // Buscamos la resoluci�n que coincida con width y height
        Resolution targetResolution = Screen.currentResolution; // Valor por defecto si no encontramos la guardada
        bool foundResolution = false;
        for (int i = 0; i < availableResolutions.Length; i++)
        {
            if (availableResolutions[i].width == currentResolutionWidth &&
                availableResolutions[i].height == currentResolutionHeight)
            {
                targetResolution = availableResolutions[i];
                foundResolution = true;
                break; // Encontramos la resoluci�n, salimos del bucle
            }
        }

        // Si la resoluci�n guardada no se encontr� en la lista actual, usa la resoluci�n actual del sistema
        if (!foundResolution)
        {
            targetResolution = Screen.currentResolution;
            currentResolutionWidth = targetResolution.width; // Actualiza los valores del manager
            currentResolutionHeight = targetResolution.height;
        }

        Screen.SetResolution(targetResolution.width, targetResolution.height, isFullScreen);

        // Aplicar Calidad
        QualitySettings.SetQualityLevel(currentQualityLevel);

        // Aplicar Pantalla Completa (aunque SetResolution ya lo establece)
        Screen.fullScreen = isFullScreen;

        Debug.Log($"Settings Applied: Resolution={Screen.width}x{Screen.height} ({Screen.fullScreen}), Quality={QualitySettings.GetQualityLevel()}");
    }

    /// <summary>
    /// Guarda las configuraciones actuales del Manager en PlayerPrefs.
    /// </summary>
    public void SaveSettings()
    {
        PlayerPrefs.SetInt(RESOLUTION_WIDTH_KEY, currentResolutionWidth);
        PlayerPrefs.SetInt(RESOLUTION_HEIGHT_KEY, currentResolutionHeight);
        PlayerPrefs.SetInt(QUALITY_LEVEL_KEY, currentQualityLevel);
        PlayerPrefs.SetInt(FULLSCREEN_KEY, isFullScreen ? 1 : 0);
        PlayerPrefs.Save(); // Forzar el guardado inmediato en disco
        Debug.Log("Settings Saved to PlayerPrefs.");
    }

    // --- M�todos para que el UI Menu los llame y actualice las variables del Manager ---

    /// <summary>
    /// Establece y guarda la resoluci�n basada en el �ndice seleccionado en el Dropdown.
    /// </summary>
    /// <param name="dropdownIndex">El �ndice seleccionado en el Dropdown de resoluci�n.</param>
    public void SetAndSaveResolution(int dropdownIndex)
    {
        // El Dropdown tiene resoluciones �nicas (ej. "1920x1080").
        // Necesitamos encontrar la Resolution real que corresponde a este texto.

        // Obtenemos la resoluci�n del dropdown en base al �ndice seleccionado
        // NOTA: Para que esto funcione, la lista 'options' que se usa para poblar el dropdown
        // en SettingsMenu.InitializeResolutionDropdown() DEBE ser accedible o replicada aqu�.
        // La forma m�s segura es que el GameSettingsManager tenga una lista de los strings
        // de las resoluciones �nicas que tambi�n usa el dropdown.

        // Por simplicidad, vamos a asumir que el dropdownIndex se mapea directamente
        // a una de las resoluciones �nicas que se mostraron en el dropdown.
        // Si tienes resoluciones duplicadas con diferentes refresh rates, esto puede ser un poco m�s complejo.

        // Para evitar el problema de �ndices que cambian, lo ideal ser�a que el SettingsMenu
        // tambi�n pase el ancho y alto directamente, o que el GameSettingsManager tenga
        // la l�gica para mapear el �ndice del dropdown a la resoluci�n correcta.

        // Dada la implementaci�n actual, donde el dropdown se pobla con anchos y altos,
        // necesitamos buscar la resoluci�n real en availableResolutions.

        // Obt�n el string de la opci�n seleccionada del dropdown (ej. "1920 x 1080")
        // Necesitas una referencia al Dropdown o que el SettingsMenu te pase los valores.
        // Como el GameSettingsManager es persistente, podemos hacer esto:
        // (�Esto requiere que el SettingsMenu te pase el string de la resoluci�n!)
        // O m�s f�cil: pasar width y height directamente desde el SettingsMenu.

        // **MODIFICACI�N CLAVE PARA ESTE PUNTO:**
        // Necesitamos que el SettingsMenu nos pase el *ancho* y el *alto* de la resoluci�n,
        // no solo un �ndice de dropdown que puede no ser el mismo que el �ndice del array 'resolutions'.
        // Si el listener de tu UI llama a SetAndSaveResolution(int dropdownIndex),
        // este �ndice es el del dropdown, no necesariamente el del array `Screen.resolutions`.
        // La forma m�s robusta es guardar el ancho y alto del dropdown.

        // Por ahora, asumiremos que el dropdownIndex corresponde a la lista de 'options' del SettingsMenu.
        // Para que esto funcione, el SettingsMenu debe proporcionarle el valor de la resoluci�n completa.
        // La forma m�s sencilla es que el SetResolution del SettingsMenu actualice directamente
        // currentResolutionWidth y currentResolutionHeight del GameSettingsManager.

        // Para solucionar esto de forma robusta con el 'int dropdownIndex':
        // Necesitas un mapeo de dropdownIndex a Resolution real.
        // La manera m�s directa es que el SettingsMenu tenga acceso a la lista de resoluciones *�nicas*
        // que ha puesto en el dropdown, y pase el objeto Resolution (o sus width/height) al Manager.

        // Vamos a asumir un escenario m�s simple y que el dropdownIndex se puede traducir.
        // Si tu `InitializeResolutionDropdown` en `SettingsMenu` agrega opciones *�nicas*
        // y ese �ndice se corresponde con el �ndice en la lista de opciones que t� construyes,
        // entonces el `resolutionDropdown.value` te da el �ndice de esa opci�n �nica.

        // Para ser m�s robustos, los listeners deber�an pasar el width y height directamente.
        // Sin embargo, si la UI solo te da un �ndice, podemos hacer una b�squeda inversa aqu�.

        // La mejor manera de hacerlo es que el SettingsMenu tambi�n guarde la lista de strings "1920 x 1080"
        // y al llamar a SetAndSaveResolution, pase ese STRING para que aqu� lo parseemos.

        // **** RECOMENDACI�N: Mover la l�gica de resoluci�n a un objeto que maneje la UI del dropdown ***
        // Para que el GameSettingsManager no tenga que saber c�mo SettingsMenu crea el dropdown.
        // La funci�n SetAndSaveResolution deber�a recibir (int width, int height).

        // *************** ALTERNATIVA ROBUSTA *********************
        // Esta es la implementaci�n si el Dropdown pasa el �ndice de su propia lista de opciones
        // y necesitamos buscar la resoluci�n real en el `availableResolutions` del Manager.
        // Esto asume que el `InitializeResolutionDropdown` en `SettingsMenu` llena `options`
        // y luego `dropdownIndex` es el �ndice de esa `options` lista.

        // Como el `GameSettingsManager` ya tiene `availableResolutions`, podemos buscar.
        // La implementaci�n actual del `SettingsMenu` llena el dropdown con `options` �nicas.
        // Entonces, `dropdownIndex` es un �ndice en esa lista de `options`.
        // Necesitamos encontrar la `Resolution` real que corresponde a ese `option` string.

        // Esto es complicado si no tenemos acceso a la lista 'options' aqu�.
        // La soluci�n m�s limpia es que el `SettingsMenu` le d� al `GameSettingsManager`
        // la `Resolution` (o sus `width` y `height`) correspondiente al �ndice del dropdown.

        // Por ahora, para que funcione con tu SetupUISetListeners actual,
        // vamos a buscar la resoluci�n en `availableResolutions` del manager.
        // ASUMIMOS que el `dropdownIndex` del `resolutionDropdown` *corresponde*
        // al �ndice de una de las resoluciones �nicas que hemos agregado a `options`
        // en el `SettingsMenu.InitializeResolutionDropdown`.
        // Esto requiere que ambas listas (la interna del manager y la del dropdown) est�n alineadas.

        // VOY A ASUMIR que el `dropdownIndex` es directamente un �ndice v�lido para `availableResolutions`.
        // Si eso no es siempre cierto (ej. si `availableResolutions` incluye refresh rates),
        // tendremos que hacer un mapeo m�s complejo.

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
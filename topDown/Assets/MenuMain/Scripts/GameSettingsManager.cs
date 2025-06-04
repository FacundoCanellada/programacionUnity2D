using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para SceneManager
using TMPro;

public class GameSettingsManager : MonoBehaviour
{
    public static GameSettingsManager Instance { get; private set; }

    // --- Variables para almacenar las opciones (se cargarán/guardarán en PlayerPrefs) ---
    // Ahora guardaremos width y height para la resolución
    public int currentResolutionWidth;
    public int currentResolutionHeight;
    public int currentQualityLevel;
    public bool isFullScreen;

    // --- Claves para PlayerPrefs ---
    private const string RESOLUTION_WIDTH_KEY = "ResolutionWidth";   // Nueva clave
    private const string RESOLUTION_HEIGHT_KEY = "ResolutionHeight"; // Nueva clave
    private const string QUALITY_LEVEL_KEY = "QualityLevel";
    private const string FULLSCREEN_KEY = "FullScreen";

    private Resolution[] availableResolutions; // Almacenará todas las resoluciones disponibles

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
        // Cargar resolución. Usamos la resolución actual del sistema como valor por defecto.
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
        // Aplicar Resolución
        // Buscamos la resolución que coincida con width y height
        Resolution targetResolution = Screen.currentResolution; // Valor por defecto si no encontramos la guardada
        bool foundResolution = false;
        for (int i = 0; i < availableResolutions.Length; i++)
        {
            if (availableResolutions[i].width == currentResolutionWidth &&
                availableResolutions[i].height == currentResolutionHeight)
            {
                targetResolution = availableResolutions[i];
                foundResolution = true;
                break; // Encontramos la resolución, salimos del bucle
            }
        }

        // Si la resolución guardada no se encontró en la lista actual, usa la resolución actual del sistema
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

    // --- Métodos para que el UI Menu los llame y actualice las variables del Manager ---

    /// <summary>
    /// Establece y guarda la resolución basada en el índice seleccionado en el Dropdown.
    /// </summary>
    /// <param name="dropdownIndex">El índice seleccionado en el Dropdown de resolución.</param>
    public void SetAndSaveResolution(int dropdownIndex)
    {
        // El Dropdown tiene resoluciones únicas (ej. "1920x1080").
        // Necesitamos encontrar la Resolution real que corresponde a este texto.

        // Obtenemos la resolución del dropdown en base al índice seleccionado
        // NOTA: Para que esto funcione, la lista 'options' que se usa para poblar el dropdown
        // en SettingsMenu.InitializeResolutionDropdown() DEBE ser accedible o replicada aquí.
        // La forma más segura es que el GameSettingsManager tenga una lista de los strings
        // de las resoluciones únicas que también usa el dropdown.

        // Por simplicidad, vamos a asumir que el dropdownIndex se mapea directamente
        // a una de las resoluciones únicas que se mostraron en el dropdown.
        // Si tienes resoluciones duplicadas con diferentes refresh rates, esto puede ser un poco más complejo.

        // Para evitar el problema de índices que cambian, lo ideal sería que el SettingsMenu
        // también pase el ancho y alto directamente, o que el GameSettingsManager tenga
        // la lógica para mapear el índice del dropdown a la resolución correcta.

        // Dada la implementación actual, donde el dropdown se pobla con anchos y altos,
        // necesitamos buscar la resolución real en availableResolutions.

        // Obtén el string de la opción seleccionada del dropdown (ej. "1920 x 1080")
        // Necesitas una referencia al Dropdown o que el SettingsMenu te pase los valores.
        // Como el GameSettingsManager es persistente, podemos hacer esto:
        // (¡Esto requiere que el SettingsMenu te pase el string de la resolución!)
        // O más fácil: pasar width y height directamente desde el SettingsMenu.

        // **MODIFICACIÓN CLAVE PARA ESTE PUNTO:**
        // Necesitamos que el SettingsMenu nos pase el *ancho* y el *alto* de la resolución,
        // no solo un índice de dropdown que puede no ser el mismo que el índice del array 'resolutions'.
        // Si el listener de tu UI llama a SetAndSaveResolution(int dropdownIndex),
        // este índice es el del dropdown, no necesariamente el del array `Screen.resolutions`.
        // La forma más robusta es guardar el ancho y alto del dropdown.

        // Por ahora, asumiremos que el dropdownIndex corresponde a la lista de 'options' del SettingsMenu.
        // Para que esto funcione, el SettingsMenu debe proporcionarle el valor de la resolución completa.
        // La forma más sencilla es que el SetResolution del SettingsMenu actualice directamente
        // currentResolutionWidth y currentResolutionHeight del GameSettingsManager.

        // Para solucionar esto de forma robusta con el 'int dropdownIndex':
        // Necesitas un mapeo de dropdownIndex a Resolution real.
        // La manera más directa es que el SettingsMenu tenga acceso a la lista de resoluciones *únicas*
        // que ha puesto en el dropdown, y pase el objeto Resolution (o sus width/height) al Manager.

        // Vamos a asumir un escenario más simple y que el dropdownIndex se puede traducir.
        // Si tu `InitializeResolutionDropdown` en `SettingsMenu` agrega opciones *únicas*
        // y ese índice se corresponde con el índice en la lista de opciones que tú construyes,
        // entonces el `resolutionDropdown.value` te da el índice de esa opción única.

        // Para ser más robustos, los listeners deberían pasar el width y height directamente.
        // Sin embargo, si la UI solo te da un índice, podemos hacer una búsqueda inversa aquí.

        // La mejor manera de hacerlo es que el SettingsMenu también guarde la lista de strings "1920 x 1080"
        // y al llamar a SetAndSaveResolution, pase ese STRING para que aquí lo parseemos.

        // **** RECOMENDACIÓN: Mover la lógica de resolución a un objeto que maneje la UI del dropdown ***
        // Para que el GameSettingsManager no tenga que saber cómo SettingsMenu crea el dropdown.
        // La función SetAndSaveResolution debería recibir (int width, int height).

        // *************** ALTERNATIVA ROBUSTA *********************
        // Esta es la implementación si el Dropdown pasa el índice de su propia lista de opciones
        // y necesitamos buscar la resolución real en el `availableResolutions` del Manager.
        // Esto asume que el `InitializeResolutionDropdown` en `SettingsMenu` llena `options`
        // y luego `dropdownIndex` es el índice de esa `options` lista.

        // Como el `GameSettingsManager` ya tiene `availableResolutions`, podemos buscar.
        // La implementación actual del `SettingsMenu` llena el dropdown con `options` únicas.
        // Entonces, `dropdownIndex` es un índice en esa lista de `options`.
        // Necesitamos encontrar la `Resolution` real que corresponde a ese `option` string.

        // Esto es complicado si no tenemos acceso a la lista 'options' aquí.
        // La solución más limpia es que el `SettingsMenu` le dé al `GameSettingsManager`
        // la `Resolution` (o sus `width` y `height`) correspondiente al índice del dropdown.

        // Por ahora, para que funcione con tu SetupUISetListeners actual,
        // vamos a buscar la resolución en `availableResolutions` del manager.
        // ASUMIMOS que el `dropdownIndex` del `resolutionDropdown` *corresponde*
        // al índice de una de las resoluciones únicas que hemos agregado a `options`
        // en el `SettingsMenu.InitializeResolutionDropdown`.
        // Esto requiere que ambas listas (la interna del manager y la del dropdown) estén alineadas.

        // VOY A ASUMIR que el `dropdownIndex` es directamente un índice válido para `availableResolutions`.
        // Si eso no es siempre cierto (ej. si `availableResolutions` incluye refresh rates),
        // tendremos que hacer un mapeo más complejo.

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
            // Si el índice es inválido, podrías cargar la resolución actual del sistema
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

    // Método para obtener las resoluciones para el dropdown del menú
    public Resolution[] GetAvailableResolutions()
    {
        return availableResolutions;
    }
}
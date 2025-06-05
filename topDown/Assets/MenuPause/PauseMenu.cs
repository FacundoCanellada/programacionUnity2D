using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }

    public GameObject pauseMenuUI; // El GameObject raíz de tu UI de menú de pausa
    public AudioMixer masterMixer;
    private bool isMuted = false;

    private float previousVolume = 0.75f; // valor por defecto

    private int pauseRequestCount = 0; // Contador de solicitudes de pausa (de TutorialManager y de este menú)
    public static bool GameIsPaused = false; // Estado público para saber si el juego está pausado

    private bool inputEnabledForPauseMenu = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            // Opcional: Si quieres que el PauseMenu persista entre escenas
            // DontDestroyOnLoad(gameObject);
        }

        // Asegura que el juego no quede pausado al iniciar o volver a esta escena
        Time.timeScale = 1f;
        GameIsPaused = false;
        pauseRequestCount = 0; // Resetear el contador al cargar la escena

        // Asegura que el UI del menú de pausa esté inicialmente oculto
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        Debug.Log("PauseMenu: Awake completado. Time.timeScale = " + Time.timeScale);
    }

    void Update()
    {
        if (inputEnabledForPauseMenu && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("PauseMenu: Tecla Escape presionada.");
            TogglePauseMenu();
        }
    }

    public void SetPauseMenuInputEnabled(bool isEnabled)
    {
        inputEnabledForPauseMenu = isEnabled;
        Debug.Log($"PauseMenu: Input para menú de pausa {(isEnabled ? "habilitado" : "deshabilitado")}.");
    }

    // --- NUEVO/MODIFICADO: Este es el método central para activar/desactivar el menú de pausa UI ---
    // Este método se llamará desde la tecla Escape Y desde el botón "Resume"
    public void TogglePauseMenu()
    {
        if (pauseMenuUI == null)
        {
            Debug.LogWarning("PauseMenu: pauseMenuUI no está asignado.");
            return;
        }

        Debug.Log("PauseMenu: TogglePauseMenu llamado. Current activeSelf: " + pauseMenuUI.activeSelf);

        if (pauseMenuUI.activeSelf) // Si el menú de pausa ESTÁ actualmente visible
        {
            pauseMenuUI.SetActive(false); // Ocultarlo
            RequestPause(false); // Levanta la solicitud de pausa del MENÚ DE PAUSA
            Debug.Log("PauseMenu: Menú ocultado y RequestPause(false) (por Escape/Resume button).");
        }
        else // Si el menú de pausa NO ESTÁ actualmente visible
        {
            pauseMenuUI.SetActive(true); // Mostrarlo
            RequestPause(true); // Solicita una pausa (incrementa el contador) por el MENÚ DE PAUSA
            Debug.Log("PauseMenu: Menú mostrado y RequestPause(true) (por Escape).");
        }
    }
    // --- FIN NUEVO/MODIFICADO ---


    // --- MODIFICADO: RequestPause solo gestiona Time.timeScale, NO el UI ---
    // Este método es llamado por TogglePauseMenu() y por TutorialManager
    public void RequestPause(bool pause)
    {
        if (pause)
        {
            pauseRequestCount++;
        }
        else
        {
            pauseRequestCount = Mathf.Max(0, pauseRequestCount - 1); // Asegura que no sea negativo
        }

        Debug.Log($"PauseMenu: RequestPause({pause}) llamado. pauseRequestCount: {pauseRequestCount}.");

        // Aplica el timeScale solo si hay al menos una solicitud de pausa
        // O si no hay solicitudes, reanuda.
        if (pauseRequestCount > 0)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        GameIsPaused = (Time.timeScale == 0f); // Sincroniza la variable GameIsPaused
        Debug.Log("PauseMenu: Nuevo Time.timeScale = " + Time.timeScale);
    }


    // --- Métodos de Botones de UI ---
    // Asegúrate de que el botón "Resume" en tu UI llame a PauseMenu.Instance.TogglePauseMenu()
    // No uses 'Resume()' ni 'Pause()' directamente como eventos de botón, usa TogglePauseMenu()

    public void LoadMenu()
    {
        Debug.Log("PauseMenu: Botón Load Menu clickeado.");
        Time.timeScale = 1f; // Asegura que el tiempo se reanude antes de cargar
        if (AudioMenu.Instance != null)
        {
            AudioMenu.Instance.PlayMenuMusic();
        }
        SceneManager.LoadScene("MainMenu");
    }
    public void MuteGame()
    {
        Debug.Log("PauseMenu: Botón Mute Game clickeado.");
        if (masterMixer != null)
        {
            float currentVolume;
            masterMixer.GetFloat("master", out currentVolume);

            if (currentVolume > -79f) // Si el volumen actual no es "muteado"
            {
                // Guarda el volumen actual antes de mutearlo
                masterMixer.GetFloat("master", out previousVolume);
                masterMixer.SetFloat("master", -80f); // Mute (un valor muy bajo)
                isMuted = true;
            }
            else // Si ya está muteado
            {
                masterMixer.SetFloat("master", previousVolume); // Restaura al volumen anterior
                isMuted = false;
            }
        }
    }
}
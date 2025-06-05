using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }

    public GameObject pauseMenuUI; 
    public AudioMixer masterMixer;
    private bool isMuted = false;

    private float previousVolume = 0.75f;

    private int pauseRequestCount = 0; 
    public static bool GameIsPaused = false; 

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
        }
        Time.timeScale = 1f;
        GameIsPaused = false;
        pauseRequestCount = 0; 

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
    public void TogglePauseMenu()
    {
        if (pauseMenuUI == null)
        {
            Debug.LogWarning("PauseMenu: pauseMenuUI no está asignado.");
            return;
        }

        Debug.Log("PauseMenu: TogglePauseMenu llamado. Current activeSelf: " + pauseMenuUI.activeSelf);

        if (pauseMenuUI.activeSelf)
        {
            pauseMenuUI.SetActive(false);
            RequestPause(false); 
            Debug.Log("PauseMenu: Menú ocultado y RequestPause(false) (por Escape/Resume button).");
        }
        else 
        {
            pauseMenuUI.SetActive(true); 
            RequestPause(true); 
            Debug.Log("PauseMenu: Menú mostrado y RequestPause(true) (por Escape).");
        }
    }
    public void RequestPause(bool pause)
    {
        if (pause)
        {
            pauseRequestCount++;
        }
        else
        {
            pauseRequestCount = Mathf.Max(0, pauseRequestCount - 1); 
        }

        Debug.Log($"PauseMenu: RequestPause({pause}) llamado. pauseRequestCount: {pauseRequestCount}.");

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
            else 
            {
                masterMixer.SetFloat("master", previousVolume); // Restaura al volumen anterior
                isMuted = false;
            }
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio; // ? Asegúrate de incluir esto

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    private bool isMuted = false;
    private float previousVolume = 0.75f; // valor por defecto

    // Referencia al AudioMixer
    public AudioMixer masterMixer;

    void Start()
    {
        // Asegura que el juego no quede pausado al volver a esta escena
        Time.timeScale = 1f;
        GameIsPaused = false;

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false); // Asegura que no se vea
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        if (AudioMenu.Instance != null)
        {
            AudioMenu.Instance.PlayMenuMusic();
        }

        SceneManager.LoadScene("MainMenu");
    }

    public void MuteGame()
    {
        if (AudioMenu.Instance != null)
        {
            AudioMenu.Instance.ToggleMute();
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreenManager : MonoBehaviour
{
    public GameObject victoryScreenUI; // Asignar tu Canvas (o Panel) de victoria desde el Inspector
    public string mainMenuSceneName = "MainMenu"; // El nombre de tu escena del men� principal
    public static bool isGamePaused = false;

    void Start()
    {
        // Asegurate de que la pantalla de victoria est� deshabilitada al inicio
        if (victoryScreenUI != null)
        {
            victoryScreenUI.SetActive(false);
        }
        isGamePaused = false;
    }

    // Este m�todo lo llamar�s cuando el boss muera
    public void OnBossDeath()
    {
        Debug.Log("Boss ha muerto. Mostrando pantalla de victoria.");
        if (victoryScreenUI != null)
        {
            victoryScreenUI.SetActive(true);
            Time.timeScale = 0f; // Pausa el tiempo del juego
            isGamePaused = true;
        }
        else
        {
            Debug.LogError("No se ha asignado la UI de la pantalla de victoria en el Inspector.");
        }
    }

    // Este m�todo se asignar� al bot�n "Volver al Men� Principal"
    public void GoToMainMenu()
    {
        Debug.Log("Volviendo al men� principal...");
        Time.timeScale = 1f; // Reanuda el tiempo del juego
        isGamePaused = false; // Restablece el estado del juego

        if (gameManager.Instance != null)
        {
            gameManager.Instance.ResetGame();
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }
    void OnDisable()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
    }
}

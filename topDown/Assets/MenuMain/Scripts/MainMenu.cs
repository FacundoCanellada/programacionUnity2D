using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        if (AudioMenu.Instance != null) // Asegúrate de que tu AudioMenu exista y sea un Singleton
        {
            AudioMenu.Instance.PlayGameplayMusic();
        }

        SceneManager.LoadScene("Game");
    }

    public void QuitGame(){

        Debug.Log("Quit");
        Application.Quit();
    }
}

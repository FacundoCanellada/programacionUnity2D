using UnityEngine;
using UnityEngine.SceneManagement;

public class defeatMenu : MonoBehaviour
{
    public void Restart()
    {
        AudioListener.pause = false;

        // Reinicia la música si existe un objeto con el nombre o etiqueta
        GameObject musicPlayer = GameObject.Find("Music"); // Cambia el nombre si es necesario
        if (musicPlayer != null)
        {
            AudioSource musicSource = musicPlayer.GetComponent<AudioSource>();
            if (musicSource != null)
            {
                musicSource.Stop();
                musicSource.Play();
            }
        }

        if (gameManager.Instance != null)
        {
            gameManager.Instance.ResetGame();
        }

        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        AudioListener.pause = false;

        Application.Quit();
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class defeatMenu : MonoBehaviour
{
    public void Restart()
    {
        if (gameManager.Instance != null)
        {
            gameManager.Instance.ResetGame();
        }

        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        Application.Quit();
    }
}

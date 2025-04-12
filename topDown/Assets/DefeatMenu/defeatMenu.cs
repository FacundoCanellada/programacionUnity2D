using UnityEngine;
using UnityEngine.SceneManagement;

public class defeatMenu : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        Application.Quit();
    }
}

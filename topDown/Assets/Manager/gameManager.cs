using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    private float timeToWaitBeforeExit;

    public void onPlayerDied()
    {
        Invoke(nameof(endGame), timeToWaitBeforeExit);
    }

    private void endGame()
    {
        SceneManager.LoadScene("DefeatMenu");
    }
}

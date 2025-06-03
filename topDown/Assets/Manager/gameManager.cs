using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    private float timeToWaitBeforeExit;

    //Boss
    public static gameManager Instance { get; private set; }
    public bool bossAparecio { get; private set; } = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: si querés que persista entre escenas
        }
    }

    public void NotificarAparicionBoss()
    {
        bossAparecio = true;
        Debug.Log("¡El jefe ha aparecido! Deteniendo spawners...");
    }

    public void onPlayerDied()
    {
        Invoke(nameof(endGame), timeToWaitBeforeExit);
    }

    private void endGame()
    {
        SceneManager.LoadScene("DefeatMenu");
    }
}

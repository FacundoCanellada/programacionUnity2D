using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    //Boss
    [SerializeField] private float timeToWaitBeforeExit = 2f;
    public static gameManager Instance { get; private set; }
    public bool bossAparecio { get; private set; } = false;

    //Couunter enemies died
    public int enemiesDefeated = 0;

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
    public void ResetGame()
    {
        enemiesDefeated = 0;
        bossAparecio = false;
        Debug.Log("GameManager reiniciado.");
    }

    public void EnemyDefeated()
    {
        enemiesDefeated++;
        Debug.Log(enemiesDefeated);
    }

    public void NotificarAparicionBoss()
    {
        bossAparecio = true;
        Debug.Log("¡El jefe ha aparecido! Deteniendo spawners...");
    }

    public void onPlayerDied()
    {
        Debug.Log("onPlayerDied llamado");
        Invoke(nameof(endGame), timeToWaitBeforeExit);
    }

    private void endGame()
    {
        SceneManager.LoadScene("DefeatMenu");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            ResetGame();
        }
    }
}

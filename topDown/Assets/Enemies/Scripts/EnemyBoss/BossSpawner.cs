using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [Header("Aparición del jefe")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private float tiempoParaAparecer = 60f;
    [SerializeField] private float radioDeAparicion = 7f;

    [Header("Pared delimitadora del jefe")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private float anchoZona = 12f;
    [SerializeField] private float altoZona = 8f;
    [SerializeField] private float grosorPared = 1f;

    [Header("Mensaje de Tutorial del Jefe")]
    public string bossTutorialID = "BossHasAppeared"; // ID único para el tutorial del jefe
    [TextArea(3, 5)]
    public string bossTutorialDescription = "¡Un poderoso enemigo ha aparecido! Prepárate para la batalla.";
    public string bossTutorialTitle = "¡JEFE FINAL!";
    public bool showBossTutorial = true;

    private float tiempoRestante;
    private bool bossSpawned = false;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        tiempoRestante = tiempoParaAparecer;
    }

    private void Update()
    {
        if (bossSpawned || player == null) return;

        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0f)
        {
            SpawnBoss();
        }
    }

    private void SpawnBoss()
    {
        Vector2 offset = Random.insideUnitCircle.normalized * radioDeAparicion;
        Vector3 posicionSpawn = player.position + (Vector3)offset;

        GameObject boss = Instantiate(bossPrefab, posicionSpawn, Quaternion.identity);
        bossSpawned = true;

        // CALCULAMOS EL CENTRO DEL ÁREA ENTRE PLAYER Y JEFE
        Vector3 centro = (player.position + posicionSpawn) / 2f;

        // DIMENSIONES DE LA ZONA
        float margen = 10f;
        float anchoZona = Mathf.Abs(player.position.x - posicionSpawn.x) + margen;
        float altoZona = Mathf.Abs(player.position.y - posicionSpawn.y) + margen;

        // CREAR LAS PAREDES
        CrearParedes(centro, anchoZona, altoZona);

        if (gameManager.Instance != null)
        {
            gameManager.Instance.NotificarAparicionBoss();
        }

        if (showBossTutorial && TutorialManager.Instance != null && !string.IsNullOrEmpty(bossTutorialID))
        {
            TutorialManager.Instance.ShowTutorialMessage(
                bossTutorialID,
                bossTutorialDescription,
                bossTutorialTitle
            );
        }
        else if (showBossTutorial && (TutorialManager.Instance == null || string.IsNullOrEmpty(bossTutorialID)))
        {
            Debug.LogWarning($"BossSpawner en {gameObject.name}: showBossTutorial es true, pero TutorialManager.Instance es nulo o bossTutorialID está vacío. No se mostrará el tutorial del jefe.", this);
        }
    }

    private void CrearParedes(Vector3 centro, float anchoZona, float altoZona)
    {
        // Obtenemos el tamaño real del bloque en el mundo (en Unity units)
        Vector2 tileSize = wallPrefab.GetComponent<SpriteRenderer>().bounds.size;

        int tilesHorizontales = Mathf.FloorToInt(anchoZona / tileSize.x);
        int tilesVerticales = Mathf.FloorToInt(altoZona / tileSize.y);

        Vector3 esquinaInferiorIzquierda = centro - new Vector3(anchoZona / 2f, altoZona / 2f, 0);

        // Pared inferior
        for (int i = 0; i < tilesHorizontales; i++)
        {
            Vector3 posicion = esquinaInferiorIzquierda + new Vector3(i * tileSize.x + tileSize.x / 2f, tileSize.y / 2f, 0);
            Instantiate(wallPrefab, posicion, Quaternion.identity);
        }

        // Pared superior
        for (int i = 0; i < tilesHorizontales; i++)
        {
            Vector3 posicion = esquinaInferiorIzquierda + new Vector3(i * tileSize.x + tileSize.x / 2f, altoZona - tileSize.y / 2f, 0);
            Instantiate(wallPrefab, posicion, Quaternion.identity);
        }

        // Pared izquierda
        for (int i = 0; i < tilesVerticales; i++)
        {
            Vector3 posicion = esquinaInferiorIzquierda + new Vector3(tileSize.x / 2f, i * tileSize.y + tileSize.y / 2f, 0);
            Instantiate(wallPrefab, posicion, Quaternion.identity);
        }

        // Pared derecha
        for (int i = 0; i < tilesVerticales; i++)
        {
            Vector3 posicion = esquinaInferiorIzquierda + new Vector3(anchoZona - tileSize.x / 2f, i * tileSize.y + tileSize.y / 2f, 0);
            Instantiate(wallPrefab, posicion, Quaternion.identity);
        }
    }

}

using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [Header("Aparici�n del jefe")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private float tiempoParaAparecer = 60f;
    [SerializeField] private float radioDeAparicion = 7f;

    [Header("Pared delimitadora del jefe")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private float anchoZona = 12f;
    [SerializeField] private float altoZona = 8f;
    [SerializeField] private float grosorPared = 1f;

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

        // CALCULAMOS EL CENTRO DEL �REA ENTRE PLAYER Y JEFE
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
    }

    private void CrearParedes(Vector3 centro, float anchoZona, float altoZona)
    {
        float grosor = 1f;

        Vector3 posIzquierda = centro + new Vector3(-anchoZona / 2f - grosor / 2f, 0, 0);
        Vector3 posDerecha = centro + new Vector3(anchoZona / 2f + grosor / 2f, 0, 0);
        Vector3 posArriba = centro + new Vector3(0, altoZona / 2f + grosor / 2f, 0);
        Vector3 posAbajo = centro + new Vector3(0, -altoZona / 2f - grosor / 2f, 0);

        Vector2 sizeVertical = new Vector2(grosor, altoZona + grosor * 2);
        Vector2 sizeHorizontal = new Vector2(anchoZona + grosor * 2, grosor);

        // Izquierda
        GameObject paredIzquierda = Instantiate(wallPrefab, posIzquierda, Quaternion.identity);
        paredIzquierda.GetComponent<SpriteRenderer>().size = sizeVertical;
        BoxCollider2D colIzq = paredIzquierda.GetComponent<BoxCollider2D>();
        colIzq.size = sizeVertical;
        colIzq.offset = Vector2.zero;

        // Derecha
        GameObject paredDerecha = Instantiate(wallPrefab, posDerecha, Quaternion.identity);
        paredDerecha.GetComponent<SpriteRenderer>().size = sizeVertical;
        BoxCollider2D colDer = paredDerecha.GetComponent<BoxCollider2D>();
        colDer.size = sizeVertical;
        colDer.offset = Vector2.zero;

        // Arriba
        GameObject paredArriba = Instantiate(wallPrefab, posArriba, Quaternion.identity);
        paredArriba.GetComponent<SpriteRenderer>().size = sizeHorizontal;
        BoxCollider2D colArr = paredArriba.GetComponent<BoxCollider2D>();
        colArr.size = sizeHorizontal;
        colArr.offset = Vector2.zero;

        // Abajo
        GameObject paredAbajo = Instantiate(wallPrefab, posAbajo, Quaternion.identity);
        paredAbajo.GetComponent<SpriteRenderer>().size = sizeHorizontal;
        BoxCollider2D colAba = paredAbajo.GetComponent<BoxCollider2D>();
        colAba.size = sizeHorizontal;
        colAba.offset = Vector2.zero;
    }
}

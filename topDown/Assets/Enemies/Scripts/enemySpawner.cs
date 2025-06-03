using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [Header("Modo de spawn")]
    [SerializeField] private bool usarSpawnPorOleadas = false;

    [Header("Spawn aleatorio")]
    [SerializeField] private float tiempoMinSpawn = 2f;
    [SerializeField] private float tiempoMaxSpawn = 5f;

    [Header("Spawn por oleadas")]
    [SerializeField] private float tiempoEntreOleadas = 10f;
    [SerializeField] private int cantidadEnemigosPorOleada = 3;

    [Header("General")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float radioSpawn = 5f;

    private float tiempoRestante;
    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (usarSpawnPorOleadas)
            tiempoRestante = tiempoEntreOleadas;
        else
            tiempoRestante = Random.Range(tiempoMinSpawn, tiempoMaxSpawn);
    }

    void Update()
    {
        if (gameManager.Instance != null && gameManager.Instance.bossAparecio) return;
        if (player == null) return;

        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0)
        {
            if (usarSpawnPorOleadas)
            {
                for (int i = 0; i < cantidadEnemigosPorOleada; i++)
                {
                    SpawnEnemyNearPlayer();
                }
                tiempoRestante = tiempoEntreOleadas;
            }
            else
            {
                SpawnEnemyNearPlayer();
                tiempoRestante = Random.Range(tiempoMinSpawn, tiempoMaxSpawn);
            }
        }
    }

    private void SpawnEnemyNearPlayer()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnPosition = player.position + (Vector3)(randomDirection * radioSpawn);

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}

using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private float minimunSpawnTime;
    [SerializeField]
    private float maximunSpawnTime;
    [SerializeField]
    private float untilSpawnTime;
    [SerializeField]
    private float spawnRadius = 5f;

    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeUntilSpawnTime();
    }
    
    void Update()
    {
        untilSpawnTime -= Time.deltaTime;

        if (untilSpawnTime <= 0)
        {
            SpawnEnemyNearPlayer();
            timeUntilSpawnTime();
        }
    }

    private void timeUntilSpawnTime ()
    {
        untilSpawnTime = Random.Range(minimunSpawnTime, maximunSpawnTime);
    }

    private void SpawnEnemyNearPlayer()
    {
        if (player == null) return;

        // Genera una dirección aleatoria en 2D (círculo)
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnPosition = player.position + (Vector3)(randomDirection * spawnRadius);

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}

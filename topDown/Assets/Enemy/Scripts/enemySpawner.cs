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
    private void Awake()
    {
        timeUntilSpawnTime();
    }
    
    void Update()
    {
        untilSpawnTime -= Time.deltaTime;

        if (untilSpawnTime <= 0)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            timeUntilSpawnTime();
        }
    }

    private void timeUntilSpawnTime ()
    {
        untilSpawnTime = Random.Range(minimunSpawnTime, maximunSpawnTime);
    }
}

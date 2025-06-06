using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    [SerializeField] public float fireCooldown = 2f;
    private float fireTimer = 0f;

    public int poolSize = 5;
    private GameObject[] bulletPool;

    [SerializeField] private float bulletLifetime;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        bulletLifetime = fireCooldown * poolSize;
        bulletPool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            bulletPool[i] = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bulletPool[i].SetActive(false);
            bulletPool[i].GetComponent<EnemyBullet>().SetLifetime(bulletLifetime);
        }
    }

    void Update()
    {
        if (PauseMenu.GameIsPaused || player == null) return;

        fireTimer += Time.deltaTime;

        if (fireTimer >= fireCooldown)
        {
            Fire();
            fireTimer = 0f;
        }

        // Gira para mirar al jugador
        Vector3 direction = player.position - transform.position;
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
    }

    void Fire()
    {
        Vector3 direction = (player.position - firePoint.position).normalized;

        // Ángulos en grados para dispersión de las balas
        float[] angles = { -15f, 0f, 15f };

        foreach (float angle in angles)
        {
            GameObject bullet = GetAvailableBullet();
            if (bullet == null) return;

            bullet.transform.position = firePoint.position;

            // Rotamos el vector de dirección
            Vector3 rotatedDir = Quaternion.Euler(0, 0, angle) * direction;
            bullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, rotatedDir); // para que mire la dirección

            bullet.SetActive(true);
            bullet.GetComponent<EnemyBullet>().Launch(rotatedDir.normalized);
        }
    }

    GameObject GetAvailableBullet()
    {
        foreach (GameObject b in bulletPool)
        {
            if (!b.activeInHierarchy)
                return b;
        }
        return null;
    }
}
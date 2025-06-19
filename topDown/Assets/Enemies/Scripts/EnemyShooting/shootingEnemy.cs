using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    private Animator animator;

    [SerializeField] public float fireCooldown = 2f;
    private float fireTimer = 0f;

    public int poolSize = 5;
    private GameObject[] bulletPool;

    [SerializeField] private float bulletLifetime;
    private Transform player;
    public SpriteRenderer enemySpriteRenderer;

    [Header("Fire Point Offset")]
    [SerializeField] private float firePointOffsetX = 0.5f;
    void Start()
    {
        animator = transform.Find("Body").GetComponent<Animator>();
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

        // Detectar si el jugador está a la izquierda o derecha
        bool playerIsLeft = player.position.x < transform.position.x;

        // Flip del sprite
        enemySpriteRenderer.flipX = !playerIsLeft;

        // Ajustar firePoint en X (a la izquierda o derecha del enemigo)
        float offsetX = playerIsLeft ? -Mathf.Abs(firePointOffsetX) : Mathf.Abs(firePointOffsetX);
        firePoint.localPosition = new Vector3(offsetX, firePoint.localPosition.y, firePoint.localPosition.z);
    }


    void Fire()
    {
        
        Vector3 direction = (player.position - firePoint.position).normalized;

        float[] angles = { -15f, 0f, 15f };

    foreach (float angle in angles)
    {
        GameObject bullet = GetAvailableBullet();
        if (bullet == null) return;

        bullet.transform.position = firePoint.position;

        // Rota la dirección en Z
        Vector3 rotatedDir = Quaternion.Euler(0, 0, angle) * direction;
        bullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, rotatedDir);

        bullet.SetActive(true);
        animator.SetTrigger("Shoot");
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
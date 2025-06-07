using UnityEngine;
using System.Collections.Generic;

public class shooting : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;

    public GameObject bulletprefab;
    public Transform firepoint; // ¡Arrastra el BulletTransform a esta ranura en el Inspector del PlayerSprite!

    [SerializeField] public float firecooldown;
    private float firetimer = 0f;

    float bulletLifetime;
    public int poolSize;
    private GameObject[] bulletPool;

    // AÑADE ESTA REFERENCIA AL SPRITERENDERER
    private SpriteRenderer playerSpriteRenderer;

    private void Start()
    {
        // Obtener la referencia al SpriteRenderer que está en PlayerSprite (donde está este script)
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer no encontrado en " + gameObject.name + ". Asegúrate de que PlayerSprite tenga un SpriteRenderer.");
            this.enabled = false;
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                firecooldown = playerStats.startfirecooldown;
            }
            else
            {
                Debug.LogWarning("PlayerStats no encontrado en el objeto 'Player'. Usando valores predeterminados.");
            }
        }
        else
        {
            Debug.LogWarning("Objeto con tag 'Player' no encontrado. Usando valores predeterminados.");
        }

        poolSize = 6;

        mainCam = Camera.main;
        if (mainCam == null)
        {
            Debug.LogError("No se encontró la cámara principal. Asegúrate de que tu cámara principal tenga el tag 'MainCamera'.");
            this.enabled = false;
            return;
        }

        if (bulletprefab == null)
        {
            Debug.LogError("El prefab de la bala no está asignado en el Inspector.");
            this.enabled = false;
            return;
        }

        bulletPool = new GameObject[poolSize];
        SetBulletLife();
    }

    private void Update()
    {
        if (PauseMenu.GameIsPaused) return;

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // Calcula la dirección del mouse en relación con el PlayerSprite (donde está este script)
        Vector3 directionToMouse = mousePos - transform.position;

        // **LÓGICA PARA VOLTEAR EL SPRITE (Izquierda/Derecha):**
        // SOLO usamos flipX para la rotación visual del sprite.
        if (playerSpriteRenderer != null)
        {
            if (directionToMouse.x > 0)
            {
                playerSpriteRenderer.flipX = true; // El sprite mira hacia la derecha
            }
            else if (directionToMouse.x < 0)
            {
                playerSpriteRenderer.flipX = false; // El sprite mira hacia la izquierda (volteado)
            }
        }

        // MUY IMPORTANTE: Asegúrate de que el transform.rotation del PlayerSprite
        // permanezca en su orientación por defecto (ej. 0,0,0) para que flipX funcione correctamente.
        // Si tienes animaciones que esperan una rotación específica, ajústala aquí.
        // Lo más común es mantenerlo en 0,0,0 para que flipX haga todo el trabajo.
        transform.rotation = Quaternion.Euler(0, 0, 0);


        firetimer += Time.deltaTime;

        if (firetimer >= firecooldown)
        {
            Fire();
            firetimer = 0f;
        }
    }

    // MODIFICA EL MÉTODO FIRE PARA QUE LAS BALAS APUNTEN CON PRECISIÓN (360 grados)
    void Fire()
    {
        GameObject bullet = GetAvaibleBullet();
        if (bullet != null)
        {
            bullet.transform.position = firepoint.position;

            // Calculamos la dirección real del mouse desde el firepoint para la bala
            Vector3 directionForBullet = mousePos - firepoint.position;
            directionForBullet.z = 0; // Aseguramos que la bala vuele en el plano 2D

            // Calculamos la rotación de la bala para que apunte 360 grados
            float bulletRotZ = Mathf.Atan2(directionForBullet.y, directionForBullet.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, bulletRotZ);

            bullet.SetActive(true);

            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            if (bulletComponent != null)
            {
                bulletComponent.Launch();
            }
            else
            {
                Debug.LogWarning("El prefab de la bala no tiene un componente 'Bullet'.");
            }
        }
    }

    GameObject GetAvaibleBullet()
    {
        foreach (GameObject b in bulletPool)
        {
            if (b != null && !b.activeInHierarchy)
            {
                return b;
            }
        }
        return null;
    }

    public void SetBulletLife()
    {
        bulletLifetime = firecooldown * poolSize;
        for (int i = 0; i < poolSize; i++)
        {
            if (bulletPool[i] == null)
            {
                bulletPool[i] = Instantiate(bulletprefab, firepoint.position, firepoint.rotation);
                bulletPool[i].SetActive(false);
                Bullet bulletComponent = bulletPool[i].GetComponent<Bullet>();
                if (bulletComponent != null)
                {
                    bulletComponent.SetLifetime(bulletLifetime);
                }
                else
                {
                    Debug.LogWarning("El prefab de la bala no tiene un componente 'Bullet'.");
                }
            }
        }
    }
}
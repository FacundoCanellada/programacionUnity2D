using System.Collections;
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
    
    public Transform brazo;
    private Animator animator;

    // AÑADE ESTA REFERENCIA AL SPRITERENDERER
   

    private void Start()
    {
        animator = transform.Find("Brazo").GetComponent<Animator>();
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
        if (VictoryScreenManager.isGamePaused) return;

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // 🔁 ROTAR EL BRAZO hacia el mouse
        Vector3 direction = mousePos - brazo.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
         

        // ↔️ VOLTEAR el PlayerRoot según la dirección del mouse
        Vector3 directionToMouse = mousePos - transform.position;
        Vector3 playerScale = transform.parent.localScale;

        if (directionToMouse.x > 0)
        {
            // El mouse está a la derecha → VOLTEAR
            playerScale.x = -Mathf.Abs(playerScale.x);
            brazo.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        else if (directionToMouse.x < 0)
        {
            // El mouse está a la izquierda → sin voltear
            playerScale.x = Mathf.Abs(playerScale.x);
            brazo.rotation = Quaternion.Euler(0f, 0f, angle+180f);
        }

        transform.parent.localScale = playerScale;

        // ❗ Asegurarse de que este GameObject no rote
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
        AudioMenu.Instance.PlayShootSound();
        GameObject bullet = GetAvaibleBullet();
        if (bullet != null)
        {
            bullet.transform.position = firepoint.position;

            Vector3 directionForBullet = mousePos - firepoint.position;
            directionForBullet.z = 0;
            float bulletRotZ = Mathf.Atan2(directionForBullet.y, directionForBullet.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, bulletRotZ);

            bullet.SetActive(true); // Activar la bala primero

            animator.SetTrigger("shootTrigger");

            // Iniciar corutina con pequeño delay para evitar error
            StartCoroutine(DelayedLaunch(bullet.GetComponent<Bullet>(), 0.01f));
        }
    }
    private IEnumerator DelayedLaunch(Bullet bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (bullet != null && bullet.gameObject.activeInHierarchy)
        {
            bullet.Launch();
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
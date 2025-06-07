using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("XP Drop")]
    [SerializeField] private GameObject xpOrbPrefab;

    [Header("Health Drop")]
    [SerializeField] private GameObject healthPrefab;
    [Range(0f, 1f)]
    [SerializeField] private float healthDropChance = 0.3f;

    [Header("Estado")]
    [SerializeField] private bool isInvulnerable = false;
    private bool isDead = false;

    [Header("Pantalla de Victoria")]
    [SerializeField] private VictoryScreenManager victoryManagerInstance;

    void Awake()
    {
        // Si no se asignó manualmente por Inspector, buscá en la escena
        if (victoryManagerInstance == null)
        {
            victoryManagerInstance = FindObjectOfType<VictoryScreenManager>();
        }

        if (victoryManagerInstance == null)
        {
            Debug.LogError("No se encontró VictoryScreenManager en la escena.");
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isInvulnerable || isDead) return;

        currentHealth -= damage;
        Debug.Log($"[{gameObject.name}] recibió {damage} de daño. Vida restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        if (gameObject.CompareTag("Boss"))
        {
            Debug.Log("¡El Boss ha muerto! Activando pantalla de victoria.");

            if (victoryManagerInstance != null)
            {
                victoryManagerInstance.OnBossDeath();
            }
            else
            {
                Debug.LogError("VictoryScreenManager no está disponible. No se puede activar la pantalla de victoria.");
            }
        }

        if (xpOrbPrefab != null)
        {
            Instantiate(xpOrbPrefab, transform.position, Quaternion.identity);
        }

        if (healthPrefab != null && Random.value <= healthDropChance)
        {
            Vector3 dropPosition = transform.position + new Vector3(0.5f, 0, 0);
            Instantiate(healthPrefab, dropPosition, Quaternion.identity);
        }

        gameManager.Instance?.EnemyDefeated(); // Evita errores si no hay GameManager en escena

        Destroy(gameObject);
    }

    public void SetInvulnerable(bool value)
    {
        isInvulnerable = value;
    }

    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }
}



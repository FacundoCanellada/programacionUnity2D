using UnityEngine;

public class enemyHealth : MonoBehaviour
{
   [SerializeField] private float maxHealth = 30;
   [SerializeField] private float currentHealth;

   [Header("XP Drop")]
   [SerializeField] private GameObject xpOrbPrefab;

   [Header("Health Drop")]
   [SerializeField] private GameObject healthPrefab;
   [Range(0f, 1f)]
   [SerializeField] private float healthDropChance = 0.3f;
    
   [SerializeField] private bool isInvulnerable = false;
    private bool isDead = false;
    void Start()
   {
      currentHealth = maxHealth;
   }

   public void TakeDamage(float damage)
    {
        if (isInvulnerable) return;
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

   private void Die()
   {
        if (isDead) return; // Asegura que no se ejecute más de una vez
        isDead = true;

        if (xpOrbPrefab != null)
      {
         Instantiate(xpOrbPrefab, transform.position, Quaternion.identity);
      }

        if (healthPrefab != null && Random.value <= healthDropChance)
        {
            // Pod�s cambiar la posici�n si quer�s que no se superpongan
            Vector3 dropPosition = transform.position + new Vector3(0.5f, 0, 0);
            Instantiate(healthPrefab, dropPosition, Quaternion.identity);
        }
        gameManager.Instance.EnemyDefeated();
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

 

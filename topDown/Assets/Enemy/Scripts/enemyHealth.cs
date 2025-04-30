using UnityEngine;

public class enemyHealth : MonoBehaviour
{
   [SerializeField] private int maxHealth = 100;
   private int currentHealth;

   [Header("XP Drop")]
   [SerializeField] private GameObject xpOrbPrefab;

    [Header("Health Drop")]
    [SerializeField] private GameObject healthPrefab;
    [Range(0f, 1f)]
    [SerializeField] private float healthDropChance = 0.3f;
    void Start()
   {
      currentHealth = maxHealth;
   }

   public void TakeDamage(int damage)
   {
      
      currentHealth -= damage;
    if (currentHealth <= 0)
      {
         Die();
      }
    }

   private void Die()
   {
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

        Destroy(gameObject);
   }
}

 

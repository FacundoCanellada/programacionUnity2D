using UnityEngine;

public class enemyHealth : MonoBehaviour
{
   [SerializeField] private int maxHealth = 100;
   private int currentHealth;

   [Header("XP Drop")]
   [SerializeField] private GameObject xpOrbPrefab;
   void Start()
   {
      currentHealth = maxHealth;
   }

   public void TakeDamage(int damage)
   {
      
      currentHealth -= damage;
      Debug.Log(currentHealth);
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
      Destroy(gameObject);
   }
}

 

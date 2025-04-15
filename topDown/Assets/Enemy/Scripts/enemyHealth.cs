using UnityEngine;

public class enemyHealth : MonoBehaviour
{
   [SerializeField] private int maxHealth = 100;
   private int currentHealth;

   void Start()
   {
      currentHealth = maxHealth;
      Debug.Log(currentHealth);
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
      Destroy(gameObject);
   }
}

 

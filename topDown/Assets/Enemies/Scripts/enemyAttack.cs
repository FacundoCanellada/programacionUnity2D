using UnityEngine;

public class enemyAttack : MonoBehaviour
{
    [SerializeField]
    private float damageAmount = 10f;
    [SerializeField]
    private float damageCooldown = 0.2f; // tiempo entre cada daño

    private float lastDamageTime = -Mathf.Infinity;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<playerMovement>())
        {
            var health = other.gameObject.GetComponent<healt>();

            if (Time.time >= lastDamageTime + damageCooldown)
            {
                health.takeDamge(damageAmount);
                lastDamageTime = Time.time;
            }
        }
    }
}
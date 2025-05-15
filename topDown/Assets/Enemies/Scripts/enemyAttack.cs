using UnityEngine;

public class enemyAttack : MonoBehaviour
{
    [SerializeField]
    private float damageAmount = 10f;
    [SerializeField]
    private float damageCooldown = 0.2f; // tiempo entre cada daño

    private float lastDamageTime = -Mathf.Infinity;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<playerMovement>())
        {
            var health = collision.gameObject.GetComponent<healt>();

            if (Time.time >= lastDamageTime + damageCooldown)
            {
                health.takeDamge(damageAmount);
                lastDamageTime = Time.time;
            }
        }
    }
}
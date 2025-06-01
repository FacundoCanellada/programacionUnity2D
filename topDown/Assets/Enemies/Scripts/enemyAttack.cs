using UnityEngine;

public class enemyAttack : MonoBehaviour
{
    [SerializeField]
    private float damageAmount = 10f;
    [SerializeField]
    private float damageCooldown = 0.2f; // tiempo entre cada daño

    private float lastDamageTime = -Mathf.Infinity;

    private BossDash bossDash;

    private void Awake()
    {
        bossDash = GetComponentInParent<BossDash>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var root = other.transform.root;

        var player = root.GetComponent<playerMovement>();
        var health = root.GetComponent<healt>();

        if (player == null || health == null) return;

        // Evitar daño si el jefe está haciendo dash
        if (bossDash != null && bossDash.IsDashing) return;

        if (Time.time >= lastDamageTime + damageCooldown)
        {
            health.takeDamge(damageAmount);
            lastDamageTime = Time.time;
        }
    }
}
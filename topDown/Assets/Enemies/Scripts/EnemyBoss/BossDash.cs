using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossDash : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashSpeed = 25f;
    public float dashDuration = 0.3f;
    public float dashCooldown = 6f;
    public float chargeTime = 0.6f;
    public float dashDamage = 25f;
    public float knockbackForce = 10f;

    [Header("Dependencies")]
    public enemyController enemyDetector;

    private Rigidbody2D rb;
    private EnemyMovement enemyMovement;
    private enemyAttack attackComponent;

    private bool canDash = true;
    private bool isDashing = false;
    private Vector2 dashDirection;

    public bool IsDashing => isDashing;

    [Header("Visual FX")]
    public ParticleSystem chargeEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyMovement = GetComponent<EnemyMovement>();
        attackComponent = GetComponent<enemyAttack>();

        if (enemyDetector == null)
        {
            enemyDetector = GetComponent<enemyController>();
        }
    }

    private void Update()
    {
        if (!canDash || isDashing) return;

        if (enemyDetector != null && enemyDetector.awareOfPlayer)
        {
            StartCoroutine(PerformDash(enemyDetector.directionToPlayer));
        }
    }

    private IEnumerator PerformDash(Vector2 direction)
    {
        canDash = false;

        if (chargeEffect != null)
        {
            chargeEffect.gameObject.SetActive(true);
            chargeEffect.Play();
        }

        yield return new WaitForSeconds(chargeTime);

        if (chargeEffect != null)
        {
            chargeEffect.Stop();
            chargeEffect.gameObject.SetActive(false);
        }

        dashDirection = direction.normalized;
        isDashing = true;

        // Desactivar daño normal mientras dasha
        if (attackComponent != null)
        {
            attackComponent.enabled = false;
        }

        if (enemyMovement != null)
        {
            enemyMovement.isOverridingMovement = true;
        }

        float timer = 0f;
        while (timer < dashDuration)
        {
            if (enemyMovement != null)
            {
                enemyMovement.externalVelocity = dashDirection * dashSpeed;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        // Restaurar movimiento y ataque
        if (enemyMovement != null)
        {
            enemyMovement.externalVelocity = Vector2.zero;
            enemyMovement.isOverridingMovement = false;
        }

        if (attackComponent != null)
        {
            attackComponent.enabled = true;
        }

        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDashing) return;

        if (other.CompareTag("Player"))
        {
            healt playerHealth = other.GetComponent<healt>();
            if (playerHealth != null)
            {
                playerHealth.takeDamge(dashDamage);
                Debug.Log("Daño por dash aplicado");
            }

            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 knockback = dashDirection * knockbackForce;
                playerRb.AddForce(knockback, ForceMode2D.Impulse);
            }

            playerMovement pm = other.GetComponent<playerMovement>();
            if (pm != null)
            {
                pm.ApplyKnockback(0.3f);
            }

            // Cortar el dash al colisionar
            StopAllCoroutines();

            if (enemyMovement != null)
            {
                enemyMovement.externalVelocity = Vector2.zero;
                enemyMovement.isOverridingMovement = false;
            }

            if (attackComponent != null)
            {
                attackComponent.enabled = true;
            }

            isDashing = false;
            canDash = false;
            StartCoroutine(DashCooldown());
        }
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}

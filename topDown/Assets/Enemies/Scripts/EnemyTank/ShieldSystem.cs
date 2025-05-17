using UnityEngine;

public class ShieldSystem : MonoBehaviour
{
    [SerializeField] private float shieldDuration = 3f;
    [SerializeField] private float cooldownDuration = 5f;
    [SerializeField] private GameObject shieldVisual;

    private bool isShieldActive = false;
    private float shieldTimer = 0f;
    private float cooldownTimer = 0f;
    private enemyHealth enemyHealth;

    void Start()
    {
        enemyHealth = GetComponent<enemyHealth>();
        if (shieldVisual != null)
            shieldVisual.SetActive(true);
    }

    void Update()
    {
        if (isShieldActive)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0f)
                DeactivateShield();
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
                ActivateShield();
        }
    }

    private void ActivateShield()
    {
        isShieldActive = true;
        shieldTimer = shieldDuration;
        cooldownTimer = cooldownDuration;

        enemyHealth.SetInvulnerable(true);
        if (shieldVisual != null)
            shieldVisual.SetActive(true);
    }

    private void DeactivateShield()
    {
        isShieldActive = false;
        enemyHealth.SetInvulnerable(false);
        if (shieldVisual != null)
            shieldVisual.SetActive(false);
    }

    public bool IsShieldActive() => isShieldActive;
}

using UnityEngine;

public class playerDamagedInvincibility : MonoBehaviour
{
    [SerializeField]
    private float invincibilityDuration;

    private isInvincibleController isInvincibleController;

    private void Awake()
    {
        isInvincibleController = GetComponent<isInvincibleController>();
    }
    public void startInvincibility()
    {
        isInvincibleController.startInvincibility(invincibilityDuration);
    }

}

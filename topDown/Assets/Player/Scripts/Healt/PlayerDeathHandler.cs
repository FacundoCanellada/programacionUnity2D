using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    private healt playerHealth;

    private void Start()
    {
        playerHealth = GetComponent<healt>();

        if (playerHealth != null)
        {
            playerHealth.onDied.AddListener(HandleDeath);
        }
        else
        {
            Debug.LogError("No se encontr� el componente healt en el jugador.");
        }
    }

    private void HandleDeath()
    {
        Debug.Log("PlayerDeathHandler: Jugador muri�, avisando al GameManager.");
        if (gameManager.Instance != null)
        {
            gameManager.Instance.onPlayerDied();
        }
        else
        {
            Debug.LogError("GameManager.Instance es null al morir el jugador.");
        }
    }
}


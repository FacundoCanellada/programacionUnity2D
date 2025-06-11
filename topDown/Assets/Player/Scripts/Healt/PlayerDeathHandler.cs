using System.Collections;
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    private healt playerHealth;
    private Animator animator;
    private void Start()
    {
        animator = transform.Find("PlayerSprite").GetComponent<Animator>();
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

        AudioListener.pause = true;

        if (gameManager.Instance != null)
        {
            animator.SetBool("canMove", false);
            animator.SetTrigger("DeadTrigger");
            StartCoroutine(WaitAndEndGame(2f));
        }
        else
        {
            Debug.LogError("GameManager.Instance es null al morir el jugador.");
        }
    }
    private IEnumerator WaitAndEndGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameManager.Instance.onPlayerDied();
    }
}


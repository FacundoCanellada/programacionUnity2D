using UnityEngine;

public class XPOrb : MonoBehaviour
{
    public int xpAmount = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            PlayerXp playerXp = other.GetComponent<PlayerXp>();
            if (playerXp != null)
            {
                playerXp.AddXP(xpAmount);
            }
            Destroy(gameObject);
        }
    }
}

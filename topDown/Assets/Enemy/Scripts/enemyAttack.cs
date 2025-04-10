using UnityEngine;

public class enemyAttack : MonoBehaviour
{
    [SerializeField]
    private float damageAmount;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<playerMovement>())
        {
            var healt = collision.gameObject.GetComponent<healt>();

            healt.takeDamge(damageAmount);
        }
    }
}

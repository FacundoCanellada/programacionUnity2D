using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float rotationSpeed;

    private Rigidbody2D rigidbody;
    private enemyController enemyController;
    private Vector2 targetDirection;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        enemyController = GetComponent<enemyController>();
    }
    private void FixedUpdate()
    {
        updateTargetDirection();
        rotateTowardsTarget();
        setVelocity();
    }

    private void updateTargetDirection()
    {
        if(enemyController.awareOfPlayer)
        {
            targetDirection = enemyController.directionToPlayer;
        }
        else
        {
            targetDirection = Vector2.zero;
        }
    }

    private void rotateTowardsTarget()
    {
        if (targetDirection == Vector2.zero)
        {
            return;
        }
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward,  targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        rigidbody.SetRotation(rotation);
    }

    private void setVelocity()
    {
        if(targetDirection == Vector2.zero)
        {
            rigidbody.linearVelocity = Vector2.zero;
        }
        else 
        {
            rigidbody.linearVelocity = transform.up * speed;
        }
    }
}

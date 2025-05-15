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
    private float changeDirectionCooldown;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        enemyController = GetComponent<enemyController>();
        targetDirection = transform.up;
    }
    private void FixedUpdate()
    {
        updateTargetDirection();
        rotateTowardsTarget();
        setVelocity();
    }

    private void updateTargetDirection()
    {
        handleRandomDirectionChange();
        handlePlayerTargeting();
    }

    private void handleRandomDirectionChange()
    {
        changeDirectionCooldown -= Time.deltaTime;

        if (changeDirectionCooldown <= 0)
        {
            float angleChange = Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            targetDirection = rotation * targetDirection;

            changeDirectionCooldown = Random.Range(1f, 5f);
        }
    }

    private void handlePlayerTargeting()
    {
        if (enemyController.awareOfPlayer)
        {
            targetDirection = enemyController.directionToPlayer;
        }
    }

    private void rotateTowardsTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward,  targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        rigidbody.SetRotation(rotation);
    }

    private void setVelocity()
    {
        rigidbody.linearVelocity = transform.up * speed;
     
    }
}

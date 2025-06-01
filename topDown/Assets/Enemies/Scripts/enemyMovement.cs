using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField] 
    private bool useZigZag = false;
    [SerializeField] 
    private float zigZagFrequency = 5f;
    [SerializeField] 
    private float zigZagMagnitude = 0.5f;

    private Rigidbody2D rb;
    private enemyController enemyController;
    private Vector2 targetDirection;
    private float changeDirectionCooldown;
    private float zigzagTimer;
    private Vector2 zigzagOffset;

    public bool isOverridingMovement { get; set; } = false;
    public Vector2 externalVelocity = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

        rb.SetRotation(rotation);
    }

    private void setVelocity()
    {
        if (isOverridingMovement)
        {
            rb.linearVelocity = externalVelocity;
            return;
        }

        Vector2 moveDirection = transform.up;

        if (useZigZag)
        {
            zigzagTimer += Time.fixedDeltaTime * zigZagFrequency;
            Vector2 perpDirection = new Vector2(-moveDirection.y, moveDirection.x); // Perpendicular 2D
            zigzagOffset = perpDirection * Mathf.Sin(zigzagTimer) * zigZagMagnitude;

            moveDirection += zigzagOffset;
            moveDirection.Normalize();
        }

        rb.linearVelocity = moveDirection * speed;

    }
}

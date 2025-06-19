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

    private Animator animator;
    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        enemyController = GetComponent<enemyController>();
        animator = GetComponent<Animator>();
        targetDirection = Vector2.up;
    }
    private void FixedUpdate()
    {
        updateTargetDirection();
        //rotateTowardsTarget();
        setVelocity();
        applySeparationForce();
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
        targetDirection = enemyController.directionToPlayer;
    }

    private void rotateTowardsTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        rb.SetRotation(rotation);

        
    }

    private void setVelocity()
    {
        if (isOverridingMovement)
        {
            rb.linearVelocity = externalVelocity;
            if (animator != null)
                animator.SetBool("Walk?", externalVelocity != Vector2.zero);
            return;
        }

        Vector2 moveDirection = targetDirection.normalized;

        if (useZigZag)
        {
            zigzagTimer += Time.fixedDeltaTime * zigZagFrequency;
            Vector2 perpDirection = new Vector2(-moveDirection.y, moveDirection.x); // perpendicular
            zigzagOffset = perpDirection * Mathf.Sin(zigzagTimer) * zigZagMagnitude;
            moveDirection += zigzagOffset;
            moveDirection.Normalize();
        }

        Vector2 velocity = moveDirection * speed;
        rb.linearVelocity = velocity;

        if (animator != null)
            animator.SetBool("Walk?", velocity != Vector2.zero);
    }

    private void applySeparationForce()
    {
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, 1f, LayerMask.GetMask("Enemy"));
        Vector2 repulsion = Vector2.zero;

        foreach (var other in nearbyEnemies)
        {
            if (other.gameObject != this.gameObject)
            {
                Vector2 away = (Vector2)(transform.position - other.transform.position);
                if (away.magnitude > 0.01f)
                    repulsion += away.normalized / away.magnitude;
            }
        }

        if (repulsion != Vector2.zero)
        {
            rb.AddForce(repulsion * 5f); // Puedes ajustar la fuerza seg�n necesidad
        }

    }
}

using UnityEngine;

public class enemyController : MonoBehaviour
{
    public bool awareOfPlayer { get; private set; }
    public Vector2 directionToPlayer { get; private set; }

    [SerializeField]
    private float playerDistance;

    private Transform player;

    private void Awake()
    {
        player = FindObjectOfType<playerMovement>().transform;
    }
    void Update()
    {
        Vector2 enemyToPlayerVector = player.position - transform.position;
        directionToPlayer = enemyToPlayerVector.normalized;

        if (enemyToPlayerVector.magnitude <= playerDistance)
        {
            awareOfPlayer = true;
        }
        else
        {
            awareOfPlayer = false;
        }
    }
}

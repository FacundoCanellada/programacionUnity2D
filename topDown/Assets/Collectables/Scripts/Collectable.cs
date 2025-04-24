using UnityEngine;

public class Collectable : MonoBehaviour
{
    private ICollectableBehavior collectableBehavior;

    private void Awake()
    {
        collectableBehavior = GetComponent<ICollectableBehavior>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<playerMovement>();

        if (player != null )
        {
            collectableBehavior.onCollected(player.gameObject);
            Destroy(gameObject);
        }
    }
}

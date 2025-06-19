using UnityEngine;

public class EnemyVisualController : MonoBehaviour
{
    private enemyController enemyController;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer spriteRendererBody;

    private void Awake()
    {
        enemyController = GetComponent<enemyController>();
        if (transform.Find("Body") != null)
        {
            spriteRendererBody = transform.Find("Body").GetComponent<SpriteRenderer>();
        }
        spriteRenderer = GetComponent<SpriteRenderer>(); // Busca el sprite en los hijos
    }

    private void Update()
    {
        if (enemyController.awareOfPlayer)
        {
            Vector2 dir = enemyController.directionToPlayer;

            if (dir.x != 0)
            {
                spriteRenderer.flipX = dir.x > 0;
                if (transform.Find("Body") != null)
                {
                    spriteRendererBody.flipX = dir.x > 0;
                }
            }
        }
    }
}

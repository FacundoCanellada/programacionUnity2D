using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 1f;
    public float lifetime = 1f;
    [SerializeField] public float damage = 1f;

    public void SetLifetime(float time)
    {
        lifetime = time;
    }

    public void Launch(Vector3 direction)
    {
        StopAllCoroutines();
        StartCoroutine(MoveAndReturn(direction));
    }

    private IEnumerator MoveAndReturn(Vector3 direction)
    {
        float timer = 0f;
        while (timer < lifetime)
        {
            transform.position += direction * speed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            healt playerHealth = other.GetComponent<healt>();
            if (playerHealth != null)
            {
                playerHealth.takeDamge(damage);
            }

            gameObject.SetActive(false);
        }
        else if (!other.isTrigger)
        {
            gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 1f;
    public float lifetime = 1f;
    [SerializeField] public float damage = 1f;

    private Coroutine activeCoroutine;

    public void SetLifetime(float time)
    {
        lifetime = time;
    }

    public void Launch(Vector3 direction)
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }

        activeCoroutine = StartCoroutine(MoveAndDisable(direction));
    }

    private IEnumerator MoveAndDisable(Vector3 direction)
    {
        float timer = 0f;
        while (timer < lifetime)
        {
            transform.position += direction.normalized * speed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        DisableBullet();
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

            DisableBullet();
        }
        else if (!other.isTrigger)
        {
            DisableBullet();
        }
    }

    private void DisableBullet()
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }

        gameObject.SetActive(false);
    }
}

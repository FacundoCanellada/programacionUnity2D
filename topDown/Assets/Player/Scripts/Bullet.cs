using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 1f;

    public int damage = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetLifetime(float time)
    {
        lifetime = time;
    }

    public void Launch()
    {
        StopAllCoroutines();
        StartCoroutine(MoveAndReturn());
    }

    private IEnumerator MoveAndReturn()
    {
        float timer = 0f;
        while (timer < lifetime)
        {
            transform.position += transform.right * speed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("La bala impactó con un enemigo");
            gameObject.SetActive(false);

            enemyHealth enemyHealth = other.GetComponent<enemyHealth>();
            if (enemyHealth != null)
            {
                Debug.Log($"Enemigo encontrado, aplicando {damage} de daño");
                enemyHealth.TakeDamage(damage);
                Debug.Log("Daño real aplicado: " + damage);
            }
          
            

        }
    }
}

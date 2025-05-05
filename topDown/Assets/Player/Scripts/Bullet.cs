using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 1f;
    [SerializeField] public float damage;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetLifetime(float time)
    {
        lifetime = time;
    }

    public void Launch()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        damage = playerStats.damage;
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
            Debug.Log("¡Colisión con enemigo detectada!");

            gameObject.SetActive(false);

            enemyHealth enemyHealth = other.GetComponent<enemyHealth>();
            if (enemyHealth != null)
            {
                Debug.Log($"Aplicando daño: {damage} a {other.name}");

                enemyHealth.TakeDamage(damage);
                
            }
          
            

        }
    }
}

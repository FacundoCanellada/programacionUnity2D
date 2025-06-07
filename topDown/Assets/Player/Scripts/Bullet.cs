using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1f;
    public float lifetime = 1f;
    [SerializeField] public float damage;

    private bool isReflected = false;

    public void SetLifetime(float time)
    {
        lifetime = time;
    }

    public void Launch()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        damage = playerStats.damage;
        isReflected = false; // Al lanzar, aseguramos que no esté reflejada
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

    void OnTriggerEnter2D(Collider2D other)
    {
        // 🟥 Si es una bala reflejada y choca con el jugador
        if (isReflected && other.CompareTag("Player"))
        {
            Debug.Log("El jugador ha sido golpeado por una bala reflejada.");
            other.GetComponent<healt>()?.takeDamge(damage); // Usás 'healt' como sistema de vida
            gameObject.SetActive(false);
            return;
        }

        // 🟩 Si es una bala normal y choca con un enemigo
        if (!isReflected && (other.CompareTag("Enemy") || other.CompareTag("Boss")))
        {
            enemyHealth enemyHealth = other.GetComponent<enemyHealth>();

            if (enemyHealth != null)
            {
                if (enemyHealth.IsInvulnerable())
                {
                    // Rebote
                    transform.right = -transform.right;
                    isReflected = true;

                    // Opcional: cambiar color de la bala reflejada
                    //GetComponent<SpriteRenderer>().color = Color.green;

                    return;
                }

                Debug.Log($"Aplicando daño: {damage} a {other.name}");
                enemyHealth.TakeDamage(damage);
                gameObject.SetActive(false);
            }
        }
    }
}

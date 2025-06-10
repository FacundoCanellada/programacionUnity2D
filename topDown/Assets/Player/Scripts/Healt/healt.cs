using System;
using UnityEngine;
using UnityEngine.Events;

public class healt : MonoBehaviour
{
    [SerializeField] public float currentHealth;
    [SerializeField] public float maximunHealth;

    public bool isInvincible { get; set; }

    public UnityEvent onDied;
    public UnityEvent onDamaged;
    public UnityEvent onHealthChanged;
    private Animator animator;

    private void Start()
    {
        animator = transform.Find("PlayerSprite").GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        
        maximunHealth = playerStats.startHealth;
        currentHealth = maximunHealth;
    }

    public void ResetHealth()
    {
        currentHealth = maximunHealth;
        onHealthChanged?.Invoke();
    }


    public void takeDamge(float damgeAmount)
    {
        if (currentHealth <= 0) return;
        if (isInvincible) return;

        currentHealth -= damgeAmount;
        onHealthChanged?.Invoke();

        if (currentHealth < 0)
            currentHealth = 0;

        if (currentHealth == 0)
        {
            Debug.Log("El jugador muri�");
            onDied?.Invoke();
        }
        else
        {
            onDamaged?.Invoke();
        }
    }

    public void addHealth(float amountToAdd)
    {
        if (currentHealth == maximunHealth) return;

        currentHealth += amountToAdd;
        if (currentHealth > maximunHealth)
            currentHealth = maximunHealth;

        onHealthChanged?.Invoke();
    }
}

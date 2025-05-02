using System;
using UnityEngine;
using UnityEngine.Events;

public class healt : MonoBehaviour
{
    [SerializeField] public float healthStart;
    [SerializeField] public float currentHealth;
    [SerializeField] public float maximunHealth;

    private void Start()
    {
        healthStart = 100f;    
    maximunHealth = healthStart;
    currentHealth = maximunHealth;
    }
    

    public bool isInvincible { get; set; }

    public UnityEvent onDied;
    public UnityEvent onDamaged;
    public UnityEvent onHealthChanged;

    public void takeDamge(float damgeAmount)
    {
        if (currentHealth == 0)
        {
            return;
        }

        // if (isInvincible)
        // {
        //     return;
        // }

        currentHealth -= damgeAmount;

        onHealthChanged.Invoke();

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        if (currentHealth == 0)
        {
            onDied.Invoke();
        }
        else
        {
            onDamaged.Invoke();
        }
    }

    public void addHealth(float amountToAdd)
    {
        if (currentHealth == maximunHealth)
        {
            return;
        }

        currentHealth += amountToAdd;

        onHealthChanged.Invoke();

        if (currentHealth > maximunHealth)
        {
            currentHealth = maximunHealth;
        }
    }

    public void IncreaseHealt()
    {
        maximunHealth *= 1.20f ;
        Debug.Log(maximunHealth);
        onHealthChanged.Invoke();
    }
}
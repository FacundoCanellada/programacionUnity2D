using UnityEngine;
using UnityEngine.Events;
public class healt : MonoBehaviour
{
    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private float maximunHealth;

    public float remainingHealthPercentage
    {
        get
        {
            return currentHealth / maximunHealth;
        }
    }
    public bool isInvincible { get; set; }

    public UnityEvent onDied;
    public UnityEvent onDamaged;
    public void takeDamge(float damgeAmount)
    {
        if (currentHealth == 0)
        {
            return;
        }

        if (isInvincible)
        {
            return;
        }

        currentHealth -= damgeAmount;

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
        //agregar en este metodo salud 
        if (currentHealth == maximunHealth)
        {
            return;
        }
        currentHealth += amountToAdd;
        if (currentHealth > maximunHealth)
        {
            currentHealth = maximunHealth;
        }
    }
}

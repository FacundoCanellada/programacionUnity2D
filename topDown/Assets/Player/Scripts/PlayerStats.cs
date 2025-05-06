using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float startHealth;
    public int startDamage;
    public float damage;
    public float  startfirecooldown;
    public float startArmor;
    public float startSpeed;
    void Start()
    {
        startHealth = 100f;
        startDamage = 1;
        damage = startDamage;
        startfirecooldown = 1f;
        startArmor = 100f;
        startSpeed = 2f;
    }

 
}

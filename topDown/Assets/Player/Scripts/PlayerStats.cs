using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float startHealth;
    public int startDamage;
    public float damage;
    public float startBullets;
    public float startArmor;
    public float startSpeed;
    void Start()
    {
        startHealth = 100f;
        startDamage = 10;
        damage = startDamage;
        startBullets = 100f;
        startArmor = 100f;
        startSpeed = 100f;
    }

 
}

using UnityEngine;

public class DamageSkillCard : SkillCard
{
    public override void ApplySkill()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject bullet = GameObject.FindGameObjectWithTag("Bullet");
        if (player == null)
        {
            Debug.LogError("No se encontró un GameObject con el tag 'Player'");
            return;
        }

        
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        float amount = playerStats.startDamage * 1.2f;
        playerStats.damage += amount;
        Debug.Log($"🆙 Daño aumentado: {playerStats.damage}");
     
    }
}
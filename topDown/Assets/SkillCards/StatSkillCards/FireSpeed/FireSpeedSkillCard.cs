using UnityEngine;

public class FireSpeedSkillCard : SkillCard
{
    public override void ApplySkill()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("No se encontró un GameObject con el tag 'Player'");
            return;
        }

        shooting shoot = player.GetComponent<shooting>();
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (shoot != null)
        {
            shoot.firecooldown -= playerStats.startfirecooldown * 0.10f;
            shoot.SetBulletLife();

        }
        else
        {
            Debug.LogWarning("El objeto con tag 'Player' no tiene el componente 'playerMovement'");
        }
    }
}
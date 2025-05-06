using UnityEngine;

public class SpeedSkillCard : SkillCard
{
    public override void ApplySkill()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("No se encontró un GameObject con el tag 'Player'");
            return;
        }

        playerMovement movement = player.GetComponent<playerMovement>();
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (movement != null)
        {
            movement.speed += playerStats.startSpeed * 0.2f;

        }
        else
        {
            Debug.LogWarning("El objeto con tag 'Player' no tiene el componente 'playerMovement'");
        }
    }
}
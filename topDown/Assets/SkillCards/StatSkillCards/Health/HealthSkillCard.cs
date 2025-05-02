using UnityEngine;

public class HealthSkillCard : SkillCard
{
    public override void ApplySkill(GameObject _)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("No se encontró un GameObject con el tag 'Player'");
            return;
        }

        healt health = player.GetComponent<healt>();
        if (health != null)
        {
            health.IncreaseHealt();
            Debug.Log("Vida máxima ahora: " + health.maximunHealth);
        }
        else
        {
            Debug.LogWarning("El objeto con tag 'Player' no tiene el componente 'healt'");
        }
    }
}
   
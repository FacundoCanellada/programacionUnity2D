using UnityEngine;

public class HealthSkillCard : SkillCard
{
    public override void ApplySkill()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("No se encontró un GameObject con el tag 'Player'");
            return;
        }

        healt health = player.GetComponent<healt>();
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (health != null)
        {
            //toma el valor de la vida inicial obtiene el 20% y se lo suma a la vida maxima
            float ammount = playerStats.startHealth;
            ammount *= 0.5f;
            health.maximunHealth += ammount ;
            Debug.Log(health.maximunHealth);
            health.onHealthChanged.Invoke();
        }
        else
        {
            Debug.LogWarning("El objeto con tag 'Player' no tiene el componente 'healt'");
        }
    }
}
   
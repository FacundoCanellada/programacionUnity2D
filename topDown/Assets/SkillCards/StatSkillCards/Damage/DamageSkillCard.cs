using UnityEngine;

public class DamageSkillCard : SkillCard
{
    public override void ApplySkill()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject bullet = GameObject.FindGameObjectWithTag("Bullet");
        Bullet bullets = bullet.GetComponent<Bullet>();

        if (player == null)
        {
            Debug.LogError("No se encontró un GameObject con el tag 'Player'");
            return;
        }

        
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        

            //toma el valor de la vida inicial obtiene el 20% y se lo suma a la vida maxima
            float ammount = playerStats.startDamage;
            ammount *= 1.20f;
            playerStats.damage += ammount;
            bullets.damage = ammount;
            Debug.Log(playerStats.damage);
        
     
    }
}
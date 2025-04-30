using UnityEngine;

public class HealthSkillCard : SkillCard
{
    [SerializeField]
    private float healthAmount;
    public override void ApplySkill(GameObject player)
    {
        player.GetComponent<healt>().IncreaseHealt(healthAmount);
        Debug.Log("xd");
    }
}
    

        
   
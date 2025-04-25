using UnityEngine;
using TMPro;
public class PlayerXp : MonoBehaviour
{
    public int currentXP = 0;
    public int level = 1;
    public int xpToNextLevel= 100;
    [SerializeField] private XpBarUi xpBarUi;
    [SerializeField] private TextMeshProUGUI levelText;
    

    public void AddXP(int ammount)
    {
        currentXP += ammount;
        xpBarUi.UpdateXPBar(currentXP, xpToNextLevel);
        if (currentXP >= xpToNextLevel)
        {
            LevelUP();
        }
    }

    private void LevelUP()
    {
        level++;
        currentXP -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);
        xpBarUi.UpdateXPBar(currentXP, xpToNextLevel);
        levelText.text = level.ToString();
        
        SkillSelectionManager.Instance.ShowSkillChoices();
    }
}

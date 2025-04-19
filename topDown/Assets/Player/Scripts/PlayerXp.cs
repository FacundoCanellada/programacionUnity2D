using UnityEngine;

public class PlayerXp : MonoBehaviour
{
    public int currentXP = 0;
    public int level = 1;
    public int xpToNextLevel= 100;

    public void AddXP(int ammount)
    {
        currentXP += ammount;
        Debug.Log(currentXP);
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
        Debug.Log($"subiste de nivel{level}");
    }
}

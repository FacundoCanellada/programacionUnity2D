using UnityEngine;
using UnityEngine.UI;
public class XpBarUi : MonoBehaviour
{
      [SerializeField] private Image xpFillImage;
    


     public void UpdateXPBar(int currentXP, int xpToNextLevel)
     {
          xpFillImage.fillAmount = (float)currentXP / xpToNextLevel;
     }
}

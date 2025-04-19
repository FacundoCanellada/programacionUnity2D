using UnityEngine;

public class healthBarUi : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image healthBarForegroundImage;
    public void updateHealthBar(healt healthController)
    {
        healthBarForegroundImage.fillAmount = healthController.remainingHealthPercentage;
    }
}

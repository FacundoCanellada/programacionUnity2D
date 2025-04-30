using UnityEngine;

public class healthBarUi : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image healthBarForegroundImage;
    public void updateHealthBar(healt hhealt)
    {
        healthBarForegroundImage.fillAmount = hhealt.currentHealth / hhealt.maximunHealth;
        
    }
}

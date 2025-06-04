using TMPro;
using UnityEngine;

public class enemyCounterUI : MonoBehaviour
{
    public TMP_Text counterText;

    void Update()
    {
        if (gameManager.Instance != null)
        {
            counterText.text = "" + gameManager.Instance.enemiesDefeated;
        }
    }
}

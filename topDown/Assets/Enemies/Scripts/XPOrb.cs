using UnityEngine;

public class XPOrb : MonoBehaviour
{
    public int xpAmount = 10;

    [Header("Tutorial Message")]
    public string tutorialMessageID = "FirstXP"; // ID �nico para este tutorial (ej: "FirstXPOrb")
    [TextArea(3, 5)] // Hace que el campo de texto sea m�s grande en el Inspector
    public string tutorialDescription = "Has ganado experiencia. Es crucial que mejores tu personaje durante el nivel." +
        "Cada vez que subas de nivel podr�s elegir que mejora se acomoda mejor a tu estilo de juego";
    public string tutorialTitle = "EXPERIENCIA";
    public bool showTutorialOnCollect = false; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerXp playerXp = other.GetComponent<PlayerXp>();
            if (playerXp != null)
            {
                playerXp.AddXP(xpAmount);

                // --- NUEVO: Mostrar mensaje de tutorial si est� configurado ---
                if (showTutorialOnCollect && TutorialManager.Instance != null && !string.IsNullOrEmpty(tutorialMessageID))
                {
                    TutorialManager.Instance.ShowTutorialMessage(
                        tutorialMessageID,
                        tutorialDescription,
                        tutorialTitle
                    );
                }
                else if (showTutorialOnCollect && (TutorialManager.Instance == null || string.IsNullOrEmpty(tutorialMessageID)))
                {
                    Debug.LogWarning($"XPOrb en {gameObject.name}: showTutorialOnCollect es true, pero TutorialManager.Instance es nulo o tutorialMessageID est� vac�o. No se mostrar� el tutorial.", this);
                }
                // --- FIN NUEVO ---
            }
            Destroy(gameObject);
        }
    }
}

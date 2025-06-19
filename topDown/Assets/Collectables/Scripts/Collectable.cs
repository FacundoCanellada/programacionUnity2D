using UnityEngine;

public class Collectable : MonoBehaviour
{
    private ICollectableBehavior collectableBehavior;

    [Header("Tutorial Message")]
    public string tutorialMessageID = "HealthCollectable"; // ID único para este tutorial (ej: "FirstCoin")
    [TextArea(3, 5)] // Hace que el campo de texto sea más grande en el Inspector
    public string tutorialDescription = "¡Has recogido un objeto! Es escencial que cuides tu vida en los combates." +
        "Recoje salud para curarte";
    public string tutorialTitle = "SALUD";
    public bool showTutorialOnCollect = false;

    private void Awake()
    {
        collectableBehavior = GetComponent<ICollectableBehavior>();

        if (collectableBehavior == null)
        {
            Debug.LogWarning($"Collectable en {gameObject.name}: No se encontró ICollectableBehavior. El coleccionable no tendrá un comportamiento específico.", this);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<playerMovement>();

        if (player != null )
        {
            AudioMenu.Instance.PlayPickupHealth();
            if (collectableBehavior != null)
            {
                collectableBehavior.onCollected(player.gameObject);
            }

            // --- NUEVO: Mostrar mensaje de tutorial si está configurado ---
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
                Debug.LogWarning($"Collectable en {gameObject.name}: showTutorialOnCollect es true, pero TutorialManager.Instance es nulo o tutorialMessageID está vacío. No se mostrará el tutorial.", this);
            }
            Destroy(gameObject);
        }
    }
}

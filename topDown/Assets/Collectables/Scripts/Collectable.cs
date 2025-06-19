using UnityEngine;

public class Collectable : MonoBehaviour
{
    private ICollectableBehavior collectableBehavior;

    [Header("Tutorial Message")]
    public string tutorialMessageID = "HealthCollectable"; // ID �nico para este tutorial (ej: "FirstCoin")
    [TextArea(3, 5)] // Hace que el campo de texto sea m�s grande en el Inspector
    public string tutorialDescription = "�Has recogido un objeto! Es escencial que cuides tu vida en los combates." +
        "Recoje salud para curarte";
    public string tutorialTitle = "SALUD";
    public bool showTutorialOnCollect = false;

    private void Awake()
    {
        collectableBehavior = GetComponent<ICollectableBehavior>();

        if (collectableBehavior == null)
        {
            Debug.LogWarning($"Collectable en {gameObject.name}: No se encontr� ICollectableBehavior. El coleccionable no tendr� un comportamiento espec�fico.", this);
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
                Debug.LogWarning($"Collectable en {gameObject.name}: showTutorialOnCollect es true, pero TutorialManager.Instance es nulo o tutorialMessageID est� vac�o. No se mostrar� el tutorial.", this);
            }
            Destroy(gameObject);
        }
    }
}

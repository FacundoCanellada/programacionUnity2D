using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    [Header("Tutorial - Movement")]
    public string movementTutorialID = "InitialMovement"; // ID único para este tutorial
    [TextArea(3, 5)] // Esto te da un campo de texto más grande en el Inspector para el mensaje
    public string movementTutorialDescription = "Usa las teclas 'W, A, S, D' o el stick izquierdo para moverte por el mundo.";
    public string movementTutorialTitle = "¡Mueve a tu Héroe!";
    private bool hasShownMovementTutorial = false;

    [Header("Setting Movement")]
    [SerializeField] public float speed;
    private Rigidbody2D rb;
    private Vector2 movementInput;

    // Knockback
    public bool isKnockedBack = false;
    private float knockbackTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        speed = playerStats.startSpeed;

        if (TutorialManager.Instance != null)
        {
            // TutorialManager.Instance.LoadTutorialProgress(); // Carga los tutoriales completados
            hasShownMovementTutorial = TutorialManager.Instance.IsTutorialComplete(movementTutorialID);
        }
    }

    private void FixedUpdate()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
            }
            return; // Evita que se sobrescriba la velocidad
        }

        rb.linearVelocity = movementInput * speed;
    }

    private void OnMove(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();

        if (!hasShownMovementTutorial && TutorialManager.Instance != null && movementInput.magnitude > 0)
        {
            // Muestra el mensaje de tutorial usando el TutorialManager
            TutorialManager.Instance.ShowTutorialMessage(movementTutorialID, movementTutorialDescription, movementTutorialTitle);

            hasShownMovementTutorial = true; // Marca que ya se mostró para esta sesión de juego

            // Opcional: Si quieres que el estado del tutorial persista entre escenas o sesiones de juego,
            // descomenta las siguientes líneas y asegúrate de que SaveTutorialProgress esté implementado en TutorialManager.
            // TutorialManager.Instance.MarkTutorialComplete(movementTutorialID);
            // TutorialManager.Instance.SaveTutorialProgress(); // Guarda el progreso inmediatamente si es necesario
        }
    }

    // Método público para activar knockback
    public void ApplyKnockback(float duration)
    {
        isKnockedBack = true;
        knockbackTimer = duration;
    }
}
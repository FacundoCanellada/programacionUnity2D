using UnityEngine;
using TMPro; // Necesario para TextMeshPro
using UnityEngine.UI; // Necesario para Image
using System.Collections;
using System.Collections.Generic; // Para usar HashSet

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialDescriptionText; 
    public TextMeshProUGUI tutorialTitleTextPro;    
    public Image tutorialImage;                     
    public float displayDuration = 3f;
    public float fadeDuration = 1.0f;

    [Header("Tutorial Settings")]
    public bool tutorialEnabled = true;
    //public bool pauseGameOnTutorial = true;
    private Coroutine currentTutorialCoroutine;
    private HashSet<string> completedTutorials = new HashSet<string>();

    private GameObject pauseMenuUIRef; // Referencia al GameObject del UI del menú de pausa
    private bool wasPauseMenuPreviouslyActive = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false); 
        }
    }

    private void Start() // NUEVO: Obtener la referencia al panel de pausa en Start
    {
        if (PauseMenu.Instance != null && PauseMenu.Instance.pauseMenuUI != null)
        {
            pauseMenuUIRef = PauseMenu.Instance.pauseMenuUI;
        }
        else
        {
            Debug.LogWarning("TutorialManager: No se encontró el GameObject del UI del menú de pausa desde PauseMenu.Instance.");
        }
    }

    public void ShowTutorialMessage(string messageID, string messageDescription, string messageTitle)
    {
        if (!tutorialEnabled || completedTutorials.Contains(messageID))
        {
            return;
        }

        if (currentTutorialCoroutine != null)
        {
            StopCoroutine(currentTutorialCoroutine);
            StartCoroutine(FadeOutPanel());
        }

        // --- MODIFICADO: Deshabilitar input del menú de pausa ---
        if (PauseMenu.Instance != null)
        {
            PauseMenu.Instance.SetPauseMenuInputEnabled(false); // Deshabilita el input de Escape para el menú
        }

        // Ocultar el panel de pausa si estaba activo
        if (pauseMenuUIRef != null && pauseMenuUIRef.activeSelf)
        {
            wasPauseMenuPreviouslyActive = true;
            pauseMenuUIRef.SetActive(false);
        }
        else
        {
            wasPauseMenuPreviouslyActive = false;
        }

        // Solicita la pausa al PauseMenu (esto solo gestiona Time.timeScale)
        if (PauseMenu.Instance != null)
        {
            PauseMenu.Instance.RequestPause(true);
        }

        tutorialPanel.SetActive(true);
        tutorialDescriptionText.text = messageDescription;
        tutorialTitleTextPro.text = messageTitle;

        currentTutorialCoroutine = StartCoroutine(ShowAndHideTutorial(messageID));
    }

    private IEnumerator ShowAndHideTutorial(string messageID)
    {
        CanvasGroup canvasGroup = tutorialPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = tutorialPanel.AddComponent<CanvasGroup>();
        }

        // ... (fade in, asignación de completedTutorials, espera, fade out - todo lo mismo)

        canvasGroup.alpha = 0f;
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.unscaledDeltaTime / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 1f;

        completedTutorials.Add(messageID);

        float timer = displayDuration;
        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            yield return null;
        }

        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.unscaledDeltaTime / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 0f;

        tutorialPanel.SetActive(false);
        currentTutorialCoroutine = null;

        // --- MODIFICADO: Re-habilitar input del menú de pausa ---
        if (PauseMenu.Instance != null)
        {
            PauseMenu.Instance.RequestPause(false); // Levanta la solicitud de pausa del tutorial
            PauseMenu.Instance.SetPauseMenuInputEnabled(true); // Re-habilita el input de Escape
        }

        if (pauseMenuUIRef != null && wasPauseMenuPreviouslyActive)
        {
            pauseMenuUIRef.SetActive(true); // Re-activa el menú de pausa si lo deshabilitamos
        }
        // --- FIN MODIFICADO ---
    }

    private IEnumerator FadeOutPanel()
    {
        CanvasGroup canvasGroup = tutorialPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            yield break;
        }

        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.unscaledDeltaTime / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 0f;
        tutorialPanel.SetActive(false);

        // --- MODIFICADO: Re-habilitar input del menú de pausa si se interrumpió ---
        if (PauseMenu.Instance != null)
        {
            PauseMenu.Instance.RequestPause(false);
            PauseMenu.Instance.SetPauseMenuInputEnabled(true); // Re-habilita el input de Escape
        }

        if (pauseMenuUIRef != null && wasPauseMenuPreviouslyActive)
        {
            pauseMenuUIRef.SetActive(true);
        }
        // --- FIN MODIFICADO ---
    }
    public void MarkTutorialComplete(string messageID)
    {
        completedTutorials.Add(messageID);
    }
    public bool IsTutorialComplete(string messageID)
    {
        return completedTutorials.Contains(messageID);
    }
}

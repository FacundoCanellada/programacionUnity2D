using UnityEngine;
using TMPro; 
using UnityEngine.UI; 
using System.Collections;
using System.Collections.Generic;

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
    private Coroutine currentTutorialCoroutine;
    private HashSet<string> completedTutorials = new HashSet<string>();

    private GameObject pauseMenuUIRef; 
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

    private void Start() 
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

        if (PauseMenu.Instance != null)
        {
            PauseMenu.Instance.SetPauseMenuInputEnabled(false); 
        }

        if (pauseMenuUIRef != null && pauseMenuUIRef.activeSelf)
        {
            wasPauseMenuPreviouslyActive = true;
            pauseMenuUIRef.SetActive(false);
        }
        else
        {
            wasPauseMenuPreviouslyActive = false;
        }
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

        if (PauseMenu.Instance != null)
        {
            PauseMenu.Instance.RequestPause(false); 
            PauseMenu.Instance.SetPauseMenuInputEnabled(true);
        }

        if (pauseMenuUIRef != null && wasPauseMenuPreviouslyActive)
        {
            pauseMenuUIRef.SetActive(true); // Re-activa el menú de pausa si lo deshabilitamos
        }
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

        if (PauseMenu.Instance != null)
        {
            PauseMenu.Instance.RequestPause(false);
            PauseMenu.Instance.SetPauseMenuInputEnabled(true); // Re-habilita el input de Escape
        }

        if (pauseMenuUIRef != null && wasPauseMenuPreviouslyActive)
        {
            pauseMenuUIRef.SetActive(true);
        }
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

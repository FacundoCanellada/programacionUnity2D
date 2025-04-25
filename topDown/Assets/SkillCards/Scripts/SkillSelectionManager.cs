using UnityEngine;
using System.Collections.Generic;

public class SkillSelectionManager : MonoBehaviour
{
    private void Start()
    {
        ShowSkillChoices();
    }
    public static SkillSelectionManager Instance { get; private set; }

    [Header("Configuración del menú")]
    public GameObject cardPrefab; 
    public Transform cardSpawnParent; 
    public List<SkillCard> allAvailableSkills;

    private List<SkillCard> currentChoices = new List<SkillCard>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Evita duplicados si hay más de uno en escena
        }
    }

    public void ShowSkillChoices()
    {
        ClearPreviousCards();
        currentChoices.Clear();

        // Elegir 3 cartas aleatorias
        for (int i = 0; i < 3; i++)
        {
            SkillCard randomSkill = allAvailableSkills[Random.Range(0, allAvailableSkills.Count)];
            currentChoices.Add(randomSkill);

            // Instanciar visualmente la carta
            GameObject cardGO = Instantiate(cardPrefab, cardSpawnParent);
            SkillCard cardComponent = cardGO.GetComponent<SkillCard>();
            cardComponent.SetupCard(randomSkill); 
            SkillCardUI uiComponent = cardGO.GetComponent<SkillCardUI>();
            uiComponent.skillCard = randomSkill;
        }

        // Mostrar el menú
        cardSpawnParent.gameObject.SetActive(true);
    }

    public void ClearPreviousCards()
    {
        foreach (Transform child in cardSpawnParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnCardSelected(SkillCard selectedCard)
    {
        // Aplicar efecto de la carta seleccionada
        Debug.Log("Seleccionaste: " + selectedCard.cardName);

        // Ocultar el menú después de elegir
        cardSpawnParent.gameObject.SetActive(false);
    }
}
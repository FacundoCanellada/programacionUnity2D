using UnityEngine;
using System.Collections.Generic;

public class SkillSelectionManager : MonoBehaviour
{
 
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
        cardSpawnParent.gameObject.SetActive(true);
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
        cardSpawnParent.gameObject.SetActive(true);
        Time.timeScale = 0f;
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
        // despues de elegir
        cardSpawnParent.gameObject.SetActive(false);
        Time.timeScale = 1f;
        
    }
}
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SkillSelectionManager : MonoBehaviour
{
    public static SkillSelectionManager Instance { get; private set; }

    [Header("Configuración del menú")]
    public GameObject cardPrefab;
    public Transform cardSpawnParent;
    public List<SkillCard> allAvailableSkills;
    public GameObject player;

    private List<SkillCard> currentChoices = new();

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowSkillChoices() {
        cardSpawnParent.gameObject.SetActive(true);
        ClearPreviousCards();
        currentChoices.Clear();

        var inventory = player.GetComponent<PlayerSkillInventory>();

        // Filtrar solo cartas que puedan aparecer según las reglas:
        List<SkillCard> filteredSkills = allAvailableSkills.Where(skill => {
            // Si ya está y está al máximo nivel, no mostrar
            if (inventory.HasSkill(skill.cardName) && inventory.GetSkill(skill.cardName).IsMaxLevel()) return false;

            // Si no la tiene y ya está lleno el tipo (stat/habilidad), no mostrar
            if (!inventory.HasSkill(skill.cardName) && !inventory.CanAddSkillOfType(skill.cardType)) return false;

            return true;
        }).ToList();

        // Elegir hasta 3 cartas aleatorias del conjunto filtrado
        for (int i = 0; i < 3 && filteredSkills.Count > 0; i++) {
            SkillCard randomSkill = filteredSkills[Random.Range(0, filteredSkills.Count)];
            filteredSkills.Remove(randomSkill); // evitar repetir cartas
            currentChoices.Add(randomSkill);

            // Instanciar la carta visual
            GameObject cardGO = Instantiate(cardPrefab, cardSpawnParent);
            cardGO.GetComponent<SkillCard>().SetupCard(randomSkill);
            cardGO.GetComponent<SkillCardUI>().skillCard = randomSkill;
        }

        Time.timeScale = 0f; // Pausar juego mientras el menú está activo
    }
//limpiar cartas anteriores
    public void ClearPreviousCards() {
        foreach (Transform child in cardSpawnParent) Destroy(child.gameObject);
    }
//al seleccionar la carta
    public void OnCardSelected(SkillCard selectedCard) {
        var inventory = player.GetComponent<PlayerSkillInventory>();

        inventory.AddOrLevelUpSkill(selectedCard);
        var playerSkill = inventory.GetSkill(selectedCard.cardName);
        playerSkill.ApplySkill();

        cardSpawnParent.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}


using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Tipos posibles de carta
public enum CardType { Stat, Ability }
public enum StatType { None, Vida, Daño, Armadura, Velocidad, VelocidadDeAtaque, ProbabilidadCritico, DañoCritico }
public enum AbilityType { None, CampoDeFuego, BolaGiratoria, Meteorito, Summoner, DisparoGiratorio, CampoDeStun, GrietaDeFuego }

public class SkillCard : MonoBehaviour
{
    [Header("Datos de la carta")]
    public string cardName;
    public Sprite cardImage;
    [TextArea] public string description;
    public int currentLevel = 1;
    public int maxLevel = 5;
    public CardType cardType;
    public StatType statType;
    public AbilityType abilityType;

    [Header("Referencias UI")]
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public Image image;

    // Aplica la habilidad y llama a levelup
    public virtual void ApplySkill() {
        LevelUp();
    }

    // Sube de nivel la carta si es posible
    public void LevelUp() {
        if (currentLevel < maxLevel) currentLevel++;
    }

    // Devuelve true si se puede subir de nivel
    public bool CanLevelUp() => currentLevel < maxLevel;

    // Devuelve true si está al nivel máximo
    public bool IsMaxLevel() => currentLevel >= maxLevel;

    // Copia los datos de una carta 
    public void SetupCard(SkillCard data) {
        cardName = data.cardName;
        description = data.description;
        cardImage = data.cardImage;
        cardType = data.cardType;
        statType = data.statType;
        abilityType = data.abilityType;

        nameText.text = cardName;
        descriptionText.text = description;
        image.sprite = cardImage;
    }
}



using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum CardType
{
    Stat,
    Ability
}

public enum StatType
{
    None,
    Vida,
    Daño,
    Armadura,
    Velocidad,
    VelocidadDeAtaque,
    ProbabilidadCritico,
    DañoCritico
}

public enum AbilityType
{
    None,
    CampoDeFuego,
    BolaGiratoria,
    Meteorito,
    Summoner,
    DisparoGiratorio,
    CampoDeStun,
    GrietaDeFuego
}
public class SkillCard : MonoBehaviour
{
    [Header("Datos de la carta")]
    public string cardName;
    public Sprite cardImage;
    [TextArea]
    public string description;
    public int currentLevel = 1;
    public int maxLevel = 5;
    public CardType cardType;
    public StatType statType;
    public AbilityType abilityType;
    [Header("Referencias UI")]
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public Image image;

    public virtual void ApplySkill(GameObject player)
    {
        // Lógica para aplicar la habilidad al jugador
    }

    public bool CanLevelUp()
    {
        return currentLevel < maxLevel;
    }
    public bool IsMaxLevel()
    {
        return currentLevel >= maxLevel;
    }
    public void SetupCard(SkillCard data)
    {
        this.cardName = data.cardName;
        this.description = data.description;
        this.cardImage = data.cardImage;
        this.cardType = data.cardType;
        this.statType = data.statType;
        this.abilityType = data.abilityType;


        nameText.text = cardName;
        descriptionText.text = description;
        image.sprite = cardImage;
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCard : MonoBehaviour
{
    [Header("Datos de la carta")]
    public string cardName;
    public Sprite cardImage;
    [TextArea]
    public string description;
    public int currentLevel = 1;
    public int maxLevel = 5;

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

    public void SetupCard(SkillCard data)
    {
        this.cardName = data.cardName;
        this.description = data.description;
        this.cardImage = data.cardImage;

        nameText.text = cardName;
        descriptionText.text = description;
        image.sprite = cardImage;
    }
}

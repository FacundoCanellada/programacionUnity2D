using UnityEngine;
using UnityEngine.EventSystems;

public class SkillCardUI : MonoBehaviour, IPointerClickHandler
{
    public SkillCard skillCard;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Carta seleccionada: " + skillCard.cardName);
        SkillSelectionManager.Instance.OnCardSelected(skillCard);
    }
}

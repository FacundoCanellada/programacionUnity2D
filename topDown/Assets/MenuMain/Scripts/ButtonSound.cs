using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (AudioMenu.Instance != null)
        {
            AudioMenu.Instance.PlayButtonClick();
        }
    }
}

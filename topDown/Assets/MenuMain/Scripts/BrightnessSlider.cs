using UnityEngine;
using UnityEngine.UI;

public class BrightnessSlider : MonoBehaviour
{
    public Slider slider;
    public Image brightnessPanel;

    void Start()
    {
        float savedValue = PlayerPrefs.GetFloat("brillo", 0.5f);
        slider.value = savedValue;
        ApplyBrightness(savedValue);
    }

    public void ChangeSlider(float value)
    {
        PlayerPrefs.SetFloat("brillo", value);
        ApplyBrightness(value);
    }

    private void ApplyBrightness(float value)
    {
        // Si el valor es menor a 0.5, oscurece (negro). Si es mayor, aclara (blanco).
        float alpha = Mathf.Abs(value - 0.5f) * 2f;
        Color overlayColor = value < 0.5f ? Color.black : Color.white;
        overlayColor.a = alpha;

        brightnessPanel.color = overlayColor;
    }
}

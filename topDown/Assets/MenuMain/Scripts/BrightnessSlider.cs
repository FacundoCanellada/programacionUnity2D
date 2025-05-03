using UnityEngine;
using UnityEngine.UI;

public class BrightnessSlider : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;
    public Image brigthnessPanel;
    public float valorBlack;
    public float valorWhite;

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("brillo", 0.5f);
        brigthnessPanel.color = new Color(brigthnessPanel.color.r, brigthnessPanel.color.g, brigthnessPanel.color.b, slider.value);
    }
    void Update()
    {
        valorBlack = 1 - sliderValue - 0.5f;
        valorWhite = sliderValue - 0.5f;
        if (sliderValue < 0.5f)
        {
            brigthnessPanel.color = new Color(0, 0, 0, valorBlack);
        }
        if (sliderValue > 0.5f)
        {
            brigthnessPanel.color = new Color(255, 255, 255, valorWhite);
        }
    }
    public void changeSlider(float value)
    {
        sliderValue = value;
        PlayerPrefs.SetFloat("brillo", sliderValue);
        brigthnessPanel.color = new Color(brigthnessPanel.color.r, brigthnessPanel.color.g, brigthnessPanel.color.b, sliderValue / 3);
    }
}


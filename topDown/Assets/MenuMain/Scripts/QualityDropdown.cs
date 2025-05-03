using TMPro;
using UnityEngine;

public class QualityDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public int quality;

    private void Start()
    {
        quality = PlayerPrefs.GetInt("numero", 2);
        dropdown.value = quality;
        AdjustQuality();
    }

    public void AdjustQuality()
    {
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("numero", dropdown.value);
        quality = dropdown.value;
    }
}

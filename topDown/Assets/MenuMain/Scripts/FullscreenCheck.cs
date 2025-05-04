using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionSettings : MonoBehaviour
{
    public TMP_Dropdown resolutionsDropDown;
    public Toggle fullscreenToggle;

    private Resolution[] allResolutions;
    private List<Resolution> filteredResolutions = new List<Resolution>();

    public void Start()
    {
        LoadSettings();
    }

    public void LoadSettings()
    {
        allResolutions = Screen.resolutions;
        resolutionsDropDown.ClearOptions();

        List<string> options = new List<string>();
        HashSet<string> uniqueResolutions = new HashSet<string>();
        filteredResolutions.Clear();

        int currentResolutionIndex = 0;

        for (int i = 0; i < allResolutions.Length; i++)
        {
            string option = allResolutions[i].width + " x " + allResolutions[i].height;

            if (uniqueResolutions.Add(option))
            {
                options.Add(option);
                filteredResolutions.Add(allResolutions[i]);

                if (allResolutions[i].width == Screen.currentResolution.width &&
                    allResolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = options.Count - 1;
                }
            }
        }

        resolutionsDropDown.AddOptions(options);

        int savedResolutionIndex = PlayerPrefs.GetInt("resolucion", currentResolutionIndex);
        resolutionsDropDown.value = savedResolutionIndex;
        resolutionsDropDown.RefreshShownValue();

        bool isFullscreen = PlayerPrefs.GetInt("fullscreen", 1) == 1;
        fullscreenToggle.isOn = isFullscreen;

        ApplyResolution(savedResolutionIndex, isFullscreen);
    }

    public void OnResolutionChange(int index)
    {
        PlayerPrefs.SetInt("resolucion", index);
        ApplyResolution(index, fullscreenToggle.isOn);
    }

    public void OnFullscreenToggle(bool isFullscreen)
    {
        PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0);
        ApplyResolution(resolutionsDropDown.value, isFullscreen);
    }

    private void ApplyResolution(int index, bool isFullscreen)
    {
        Resolution resolution = filteredResolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using System;
using System.Collections.Generic;

public class FullscreenCheck : MonoBehaviour
{
    public Toggle toggle;

    public TMP_Dropdown resolutionsDropDown;
    Resolution[] resolutions;
    private void Start()
    {
        if(Screen.fullScreen)
        {
            toggle.isOn = true;
        }
        else 
        {   
            toggle.isOn = false;
        }

        checkResolution();
    }
    public void activateFullscreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

    public void checkResolution()
    {
        resolutions = Screen.resolutions;
        resolutionsDropDown.ClearOptions();

        List<string> options = new List<string>();
        HashSet<string> uniqueResolutions = new HashSet<string>();
        List<Resolution> filteredResolutions = new List<Resolution>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resString = resolutions[i].width + " x " + resolutions[i].height;

            if (!uniqueResolutions.Contains(resString))
            {
                uniqueResolutions.Add(resString);
                options.Add(resString);
                filteredResolutions.Add(resolutions[i]);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = filteredResolutions.Count - 1;
                }
            }
        }

        resolutions = filteredResolutions.ToArray(); // Guardamos la lista limpia

        resolutionsDropDown.AddOptions(options);

        int savedIndex = PlayerPrefs.GetInt("resolucion", currentResolutionIndex);
        resolutionsDropDown.value = savedIndex;
        resolutionsDropDown.RefreshShownValue();
    }

    public void changeResolution(int indexResolution)
    {
        PlayerPrefs.SetInt("resolucion", indexResolution);

        Resolution resolution = resolutions[indexResolution];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

}

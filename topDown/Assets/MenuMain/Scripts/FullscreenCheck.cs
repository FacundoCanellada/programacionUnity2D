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
        int actuallyResolutions = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (Screen.fullScreen && resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                actuallyResolutions = i;
            }
        }

        resolutionsDropDown.AddOptions(options);
        resolutionsDropDown.value = actuallyResolutions;
        resolutionsDropDown.RefreshShownValue();

        resolutionsDropDown.value = PlayerPrefs.GetInt("resolucion", 0);
    }

    public void changeResolution(int indexResolution)
    {
        PlayerPrefs.SetInt("resolucion", resolutionsDropDown.value);

        Resolution resolution = resolutions[indexResolution];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}

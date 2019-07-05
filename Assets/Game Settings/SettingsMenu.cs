using Drivers.LocalizationSettings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;
    public Dropdown graphicDropdown;

    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " : " + resolutions[i].refreshRate + " Hz";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height && resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        graphicDropdown.ClearOptions();
        if (LocalizationManager.Instance.currentLanguageID == 1)
        {
            List<string> graphicOptions = new List<string> { LocalizationManager.Instance.GetText("VERY_LOW"), LocalizationManager.Instance.GetText("LOW"), LocalizationManager.Instance.GetText("MEDIUM"), LocalizationManager.Instance.GetText("HIGH"), LocalizationManager.Instance.GetText("VERY_HIGH"), LocalizationManager.Instance.GetText("ULTRA") };
            graphicDropdown.AddOptions(graphicOptions);
        }
        else
        {
            List<string> graphicOptions = new List<string> { LocalizationManager.Instance.GetText("VERY_LOW"), LocalizationManager.Instance.GetText("LOW"), LocalizationManager.Instance.GetText("MEDIUM"), LocalizationManager.Instance.GetText("HIGH"), LocalizationManager.Instance.GetText("VERY_HIGH"), LocalizationManager.Instance.GetText("ULTRA") };
            graphicDropdown.AddOptions(graphicOptions);
        }
        graphicDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
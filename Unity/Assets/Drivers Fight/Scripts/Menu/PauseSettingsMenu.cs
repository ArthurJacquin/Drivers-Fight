using Drivers.LocalizationSettings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseSettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown graphicDropdown;

    public AudioSource audioSource;

    public Dropdown dropdown;
    public Slider slider;

    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;

        dropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
               resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        dropdown.AddOptions(options);
        dropdown.value = currentResolutionIndex;
        dropdown.RefreshShownValue();

        DisplayGraphicDropdown();

        slider.value = SettingsMenu.volume;
        audioSource.volume = SettingsMenu.volume;
    }

    public void DisplayGraphicDropdown()
    {
        graphicDropdown.ClearOptions();
        List<string> graphicOptions = new List<string> { LocalizationManager.Instance.GetText("VERY_LOW"), LocalizationManager.Instance.GetText("LOW"), LocalizationManager.Instance.GetText("MEDIUM"), LocalizationManager.Instance.GetText("HIGH"), LocalizationManager.Instance.GetText("VERY_HIGH"), LocalizationManager.Instance.GetText("ULTRA") };
        graphicDropdown.AddOptions(graphicOptions);
        graphicDropdown.RefreshShownValue();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        SettingsMenu.volume = volume;
    }

    /*public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }*/

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}

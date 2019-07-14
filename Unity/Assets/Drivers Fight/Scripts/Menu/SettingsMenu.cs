using Drivers.LocalizationSettings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;

    [SerializeField]
    private Dropdown resolutionDropdown;

    public Dropdown graphicDropdown;

    [SerializeField]
    private Slider audioVolumeSlider;

    Resolution[] resolutions;
    
    public static float volume = 1f;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;

            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && 
               resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        DisplayGraphicDropdown();

        audioVolumeSlider.value = volume;
    }

    public void DisplayGraphicDropdown()
    {
        graphicDropdown.ClearOptions();
        List<string> graphicOptions = new List<string> { LocalizationManager.Instance.GetText("VERY_LOW"), LocalizationManager.Instance.GetText("LOW"), LocalizationManager.Instance.GetText("MEDIUM"), LocalizationManager.Instance.GetText("HIGH"), LocalizationManager.Instance.GetText("VERY_HIGH"), LocalizationManager.Instance.GetText("ULTRA") };
        graphicDropdown.AddOptions(graphicOptions);
        graphicDropdown.RefreshShownValue();
    }

    public void SetVolume(float v)
    {
        volume = v;
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
        print("Want to set the resolution to " + resolution);
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}

using Drivers.LocalizationSettings;
using TMPro;
using UnityEngine;

public class LocalizationUITextMeshPro : MonoBehaviour
{
    public string key;

    private TextMeshProUGUI text;

    private void OnEnable()
    {
        LocalizationManager.onChangedLanguage += OnChangedLanguage;
        UpdateText();
    }

    private void OnDisable()
    {
        LocalizationManager.onChangedLanguage -= OnChangedLanguage;
    }

    private void UpdateText()
    {
        if (!text)
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        if (text)
        {
            // Get the string value from localization manager from key 
            // and set the text component text value to the  returned string value 
            text.text = LocalizationManager.Instance.GetText(key);
        }
    }

    private void OnChangedLanguage()
    {
        UpdateText();
    }
}

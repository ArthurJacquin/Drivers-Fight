using UnityEngine;
using System.Collections.Generic;
using System.Xml.Linq;
using System;

namespace Drivers.LocalizationSettings
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance { get { return instance; } }
        public int defaultLanguageID = 0;

        public static Action onChangedLanguage;
        public static int currentLanguageID = 0;

        [Space]
        [SerializeField]
        public List<TextAsset> languageFiles = new List<TextAsset>();
        public List<Language> languages = new List<Language>();

        [Space]
        public SettingsMenu settingsMenu;

        private static LocalizationManager instance;   // GameSystem local instance

        public int ID
        {
            get
            {
                currentLanguageID = PlayerPrefs.GetInt("language_id");
                return currentLanguageID;
            }
            set
            {
                if (currentLanguageID != value)
                {
                    currentLanguageID = value;
                    PlayerPrefs.SetInt("language_id", value);
                    onChangedLanguage?.Invoke();
                }
            }
        }

        void Awake()
        {
            currentLanguageID = PlayerPrefs.GetInt("language_id");
            instance = this;
            DontDestroyOnLoad(this);
            // This will read  each XML file from the languageFiles list<> and populate the languages list with the data
            foreach (TextAsset languageFile in languageFiles)
            {
                XDocument languageXMLData = XDocument.Parse(languageFile.text);
                Language language = new Language();
                language.languageID = System.Int32.Parse(languageXMLData.Element("Language").Attribute("ID").Value);
                language.languageString = languageXMLData.Element("Language").Attribute("LANG").Value;
                foreach (XElement textx in languageXMLData.Element("Language").Elements())
                {
                    TextKeyValue textKeyValue = new TextKeyValue();
                    textKeyValue.key = textx.Attribute("key").Value;
                    textKeyValue.value = textx.Value;
                    language.textKeyValueList.Add(textKeyValue);
                }
                languages.Add(language);
            }
        }

        // GetText will go through each language in the languages list and return a string matching the key provided 
        public string GetText(string key)
        {
            foreach (Language language in languages)
            {
                if (language.languageID == currentLanguageID)
                {
                    foreach (TextKeyValue textKeyValue in language.textKeyValueList)
                    {
                        if (textKeyValue.key == key)
                        {
                            return textKeyValue.value;
                        }
                    }
                }
            }
            return "Undefined";
        }

        public void SetEnglish()
        {
            ID = 0;
            settingsMenu.DisplayGraphicDropdown();
        }

        public void SetFrench()
        {
            ID = 1;
            settingsMenu.DisplayGraphicDropdown();
        }
    }
}

// Simple Class to hold the language metadata
[System.Serializable]
public class Language
{
    public string languageString;
    public int languageID;
    public List<TextKeyValue> textKeyValueList = new List<TextKeyValue>();
}

// Simple class to hold the key/value pair data
[System.Serializable]
public class TextKeyValue
{
    public string key;
    public string value;
}
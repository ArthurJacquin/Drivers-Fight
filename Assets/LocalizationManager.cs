using UnityEngine;
using System.Collections.Generic;
using System.Xml.Linq;
using System;

namespace Drivers.LocalizationSettings
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance { get { return instance; } }
        public int currentLanguageID = 0;

        public static Action onChangedLanguage;

        [SerializeField]
        public List<TextAsset> languageFiles = new List<TextAsset>();
        public List<Language> languages = new List<Language>();

        private static LocalizationManager instance;   // GameSystem local instance

        void Awake()
        {
            //currentLanguageID = PlayerPrefs.GetInt("language_id");
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
            currentLanguageID = 0;
        }

        public void SetFrench()
        {
            currentLanguageID = 1;
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
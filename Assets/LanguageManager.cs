using System;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public const int DefaultLanguage = 0;

    public static Action onChangedLanguage;
    private static int? id = null;

    public static int ID
    {
        get
        {
            if (id == null)
            {
                id = PlayerPrefs.GetInt("language_id", DefaultLanguage);
            }

            return id.Value;
        }
        set
        {
            if (id == null)
            {
                id = PlayerPrefs.GetInt("language_id", DefaultLanguage);
            }

            if (id != value)
            {
                onChangedLanguage?.Invoke();
                id = value;
                PlayerPrefs.SetInt("language_id", value);
            }
        }
    }
}
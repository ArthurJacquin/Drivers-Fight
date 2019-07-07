using UnityEngine;
using Drivers.Localization;

public class MenuManager : MonoBehaviour
{
    public void SetEnglish()
    {
        Localize.SetCurrentLanguage(SystemLanguage.English);
    }

    public void SetFrench()
    {
        Localize.SetCurrentLanguage(SystemLanguage.French);
    }
}
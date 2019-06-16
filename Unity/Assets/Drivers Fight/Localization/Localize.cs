﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class managing UI text localization. Language specific strings shall be saved following this
/// folder structure:
///
///     Resources/localization/English.txt
///     Resources/localization/French.txt
///
/// ... and so on, where the file name (and consequently the resource name) is the string version of
/// the SystemLanguage enumeration.
///
/// The file format is as follows:
///
///     key=value
///
/// A TAB character is also accepted as key/value separator. 
/// In the value you can use the standard /// notation for newline: \n
/// </summary>

namespace Drivers.Localization
{
    [RequireComponent(typeof(Text))]
    public class Localize : LocalizeBase
    {
        private Text text;

        /// <summary>
        /// Update the value of the Text we are attached to.
        /// </summary>
        public override void UpdateLocale()
        {
            if (!text) return; // catching race condition
            if (!System.String.IsNullOrEmpty(localizationKey) && Locale.CurrentLanguageStrings.ContainsKey(localizationKey))
                text.text = Locale.CurrentLanguageStrings[localizationKey].Replace(@"\n", "" + '\n'); ;
        }

        // Use this for initialization
        protected override void Start()
        {
            text = GetComponent<Text>();
            base.Start();
        }
    }
}
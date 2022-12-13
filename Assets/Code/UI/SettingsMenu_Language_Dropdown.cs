using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class SettingsMenu_Language_Dropdown : MonoBehaviour
{
    public TMP_Dropdown languageDropdown;
    public void ChangeSelected()
    {
        int selectedItemVal = languageDropdown.value;
        if (selectedItemVal == 0)//english
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            PlayerPrefs.SetString("SelectedLang", "0");
        }
        else if (selectedItemVal == 1)//türkçe
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
            PlayerPrefs.SetString("SelectedLang", "1");
        }
        PlayerPrefs.Save();
    }
}

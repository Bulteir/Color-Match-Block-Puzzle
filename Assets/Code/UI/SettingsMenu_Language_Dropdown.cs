using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;


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
            ReloadMenu();
        }
        else if (selectedItemVal == 1)//türkçe
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
            PlayerPrefs.SetString("SelectedLang", "1");
            ReloadMenu();
        }
        else if (selectedItemVal == 2)//çince
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[2];
            PlayerPrefs.SetString("SelectedLang", "2");
            ReloadMenu();
        }
        else if (selectedItemVal == 3)//arapça
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[3];
            PlayerPrefs.SetString("SelectedLang", "3");
            ReloadMenu();
        }
        else if (selectedItemVal == 4)//fransýzca
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[4];
            PlayerPrefs.SetString("SelectedLang", "4");
            ReloadMenu();
        }
        else if (selectedItemVal == 5)//almanca
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[5];
            PlayerPrefs.SetString("SelectedLang", "5");
            ReloadMenu();
        }
        else if (selectedItemVal == 6)//japonca
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[6];
            PlayerPrefs.SetString("SelectedLang", "6");
            ReloadMenu();
        }
        else if (selectedItemVal == 7)//portekizce
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[7];
            PlayerPrefs.SetString("SelectedLang", "7");
            ReloadMenu();
        }
        else if (selectedItemVal == 8)//rusça
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[8];
            PlayerPrefs.SetString("SelectedLang", "8");
            ReloadMenu();
        }
        else if (selectedItemVal == 9)//ispanyolca
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[9];
            PlayerPrefs.SetString("SelectedLang", "9");
            ReloadMenu();
        }
        PlayerPrefs.Save();
    }

    void ReloadMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}

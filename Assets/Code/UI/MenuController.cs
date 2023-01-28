using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class MenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject highScoresMenu;
    public Transform musics;
    public Transform sfx;

    void Awake()
    {
        string selectedLangVal = PlayerPrefs.GetString("SelectedLang");
        if (selectedLangVal != "")
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[int.Parse(selectedLangVal)];
        }

        #region oyun baþlangýcý muzik tercihi kontrolü
        string musicPref = PlayerPrefs.GetString("Music");
        if (musicPref != "")
        {
            if (musicPref == "on")
            {
                foreach (Transform item in musics)
                {
                    item.GetComponent<AudioSource>().mute = false;
                }
            }
            else
            {
                foreach (Transform item in musics)
                {
                    item.GetComponent<AudioSource>().mute = true;
                }
            }
        }
        else
        {
            PlayerPrefs.SetString("Music", "on");
            foreach (Transform item in musics)
            {
                item.GetComponent<AudioSource>().mute = false;
            }
        }
        PlayerPrefs.Save();
        #endregion
        #region oyun baþlangýcý ses efektleri tercihi kontrolü
        string soundPref = PlayerPrefs.GetString("Sound");
        if (soundPref != "")
        {
            if (soundPref == "on")
            {
                foreach (Transform item in sfx)
                {
                    item.GetComponent<AudioSource>().mute = false;
                }
            }
            else
            {
                foreach (Transform item in sfx)
                {
                    item.GetComponent<AudioSource>().mute = true;
                }
            }
        }
        else
        {
            PlayerPrefs.SetString("Sound", "on");
            foreach (Transform item in sfx)
            {
                item.GetComponent<AudioSource>().mute = false;
            }
        }
        PlayerPrefs.Save();
        #endregion

    }
    // Start is called before the first frame update
    void Start()
    {
        menuActiveControl();
        //menuMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        menuActiveControl();
    }

    void menuActiveControl()
    {
        if (GlobalVariables.gameState == GlobalVariables.gameState_MainMenu && mainMenu.activeSelf == false)
        {
            mainMenu.SetActive(true);
            settingsMenu.SetActive(false);
            highScoresMenu.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_SettingsMenu && settingsMenu.activeSelf == false)
        {
            mainMenu.SetActive(false);
            settingsMenu.SetActive(true);
            highScoresMenu.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_HighScoresMenu && highScoresMenu.activeSelf == false)
        {
            mainMenu.SetActive(false);
            settingsMenu.SetActive(false);
            highScoresMenu.SetActive(true);
        }
    }
}

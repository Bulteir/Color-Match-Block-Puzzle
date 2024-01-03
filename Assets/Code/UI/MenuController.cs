using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

public class MenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject highScoresMenu;
    public GameObject leaderboardMenu;
    public GameObject storeMenu;
    public Transform musics;
    public Transform sfx;

    void Awake()
    {
        string selectedLangVal = PlayerPrefs.GetString("SelectedLang");
        if (selectedLangVal != "")
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[int.Parse(selectedLangVal)];
        }

        #region oyun ba�lang�c� muzik tercihi kontrol�
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
        #region oyun ba�lang�c� ses efektleri tercihi kontrol�
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

        //oyun a��l�rken store initial yapaca��z. ��nk� restore buton i�in loadcatalog'un �al��m�� olmas� ve OnInitialized fonksiyonuna girmi� olmas� gerekiyor
        storeMenu.GetComponent<StoreController>().UnityServicesInitial();

        #region iosta reklam g�sterebilmek i�in gerekli olan izin kontrol�
#if UNITY_IOS
        // check with iOS to see if the user has accepted or declined tracking
        var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

        if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
        }
#endif
        #endregion

        menuActiveControl();
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
            leaderboardMenu.SetActive(false);
            storeMenu.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_SettingsMenu && settingsMenu.activeSelf == false)
        {
            mainMenu.SetActive(false);
            settingsMenu.SetActive(true);
            highScoresMenu.SetActive(false);
            leaderboardMenu.SetActive(false);
            storeMenu.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_HighScoresMenu && highScoresMenu.activeSelf == false)
        {
            mainMenu.SetActive(false);
            settingsMenu.SetActive(false);
            highScoresMenu.SetActive(true);
            leaderboardMenu.SetActive(false);
            storeMenu.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_LeaderboardMenu && leaderboardMenu.activeSelf == false)
        {
            mainMenu.SetActive(false);
            settingsMenu.SetActive(false);
            highScoresMenu.SetActive(false);
            leaderboardMenu.SetActive(true);
            storeMenu.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_StoreMenu && storeMenu.activeSelf == false)
        {
            mainMenu.SetActive(false);
            settingsMenu.SetActive(false);
            highScoresMenu.SetActive(false);
            leaderboardMenu.SetActive(false);
            storeMenu.SetActive(true);
        }
    }
}

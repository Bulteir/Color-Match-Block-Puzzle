using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InGameMenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject cloud1;
    public GameObject cloud2;
    public GameObject cloud3;
    public Transform musics;
    public Transform sfx;

    // Start is called before the first frame update
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        string noAdsActive = PlayerPrefs.GetString("NoAdsActive");

        this.GetComponent<AdMobRewardedAdController>().LoadAd();

        if (noAdsActive == "")
        {
            this.GetComponent<AdMobBannerViewController>().LoadAd();
            this.GetComponent<AdMobInterstitialAdController>().LoadAd();
            GlobalVariables.requestInterstitialAd = true;
        }

        GlobalVariables.gameState = GlobalVariables.gameState_inGame;
        GlobalVariables.whichButtonRequestInterstitialAd = GlobalVariables.nonButton;
        //this.GetComponent<AdMobController>().RequestAndLoadInterstitialAd();

        GlobalVariables.requestRewardedAd = true;
        GlobalVariables.whichJokerRequestRewardAd = GlobalVariables.joker_non;
        //this.GetComponent<AdMobController>().RequestAndLoadRewardedAd();

        menuActiveControl();
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
        #endregion


    }

    // Update is called once per frame
    void Update()
    {
        menuActiveControl();
    }
    void menuActiveControl()
    {
        if (GlobalVariables.gameState == GlobalVariables.gameState_gamePaused && pauseMenu.activeSelf == false)
        {
            pauseMenu.SetActive(true);
            SpawnedBlocksMaskHelper(true);
            gameOverMenu.SetActive(false);
            this.GetComponent<AdMobBannerViewController>().HideAd();
            //this.GetComponent<AdMobController>().DestroyBannerAd();
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_inGame && pauseMenu.activeSelf == true)
        {
            pauseMenu.SetActive(false);
            SpawnedBlocksMaskHelper(false);
            gameOverMenu.SetActive(false);
            this.GetComponent<AdMobBannerViewController>().ShowAd();
            //this.GetComponent<AdMobController>().RequestBannerAd();
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_gameOver && gameOverMenu.activeSelf == false)
        {
            pauseMenu.SetActive(false);
            SpawnedBlocksMaskHelper(true);
            gameOverMenu.SetActive(true);
            this.GetComponent<AdMobBannerViewController>().DestroyAd();
            //this.GetComponent<AdMobController>().DestroyBannerAd();
        }
    }

    void SpawnedBlocksMaskHelper(bool active)
    {
        foreach (Transform item in this.GetComponent<CreateBlocks>().SpawnPoints)
        {
            foreach (Transform child in item.GetComponent<SpawnPointHelper>().block)
            {
                if (active)
                    child.GetComponent<SpriteRenderer>().sortingOrder = 0;
                else
                    child.GetComponent<SpriteRenderer>().sortingOrder = GlobalVariables.orderInLayer_selectedBlock;
            }
        }

        if (active)
        {
            cloud1.GetComponent<Animator>().speed = 0;
            cloud2.GetComponent<Animator>().speed = 0;
            cloud3.GetComponent<Animator>().speed = 0;
        }
        else
        {
            cloud1.GetComponent<Animator>().speed = 1;
            cloud2.GetComponent<Animator>().speed = 1;
            cloud3.GetComponent<Animator>().speed = 1;
        }



    }
}

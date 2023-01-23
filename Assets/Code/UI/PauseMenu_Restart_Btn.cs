using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu_Restart_Btn : MonoBehaviour
{
    bool adIsReady = false;
    public Transform GeneralControls;
    IEnumerator requestAdCorroutine;

    public void OnClick()
    {
        GlobalVariables.whichButtonRequestInterstitialAd = GlobalVariables.pauseMenuRestart_btn;
        if (adIsReady)
        {
            adIsReady = false;
            GeneralControls.GetComponent<AdMobController>().ShowInterstitialAd();
        }
        else
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        if (GlobalVariables.requestInterstitialAd == true && GlobalVariables.whichButtonRequestInterstitialAd == GlobalVariables.pauseMenuRestart_btn)
        {
            GlobalVariables.requestInterstitialAd = false;
            GlobalVariables.whichButtonRequestInterstitialAd = GlobalVariables.nonButton;
            GeneralControls.GetComponent<AdMobController>().DestroyBannerAd();
            GeneralControls.GetComponent<AdMobController>().DestroyInterstitialAd();
            GlobalVariables.gameState = GlobalVariables.gameState_inGame;
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
        else if (GlobalVariables.whichButtonRequestInterstitialAd == GlobalVariables.pauseMenuRestart_btn)
        {
            GlobalVariables.gameState = GlobalVariables.gameState_inGame;
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
    }

    public void adLoaded()
    {
        if (GlobalVariables.requestInterstitialAd == true)
        {
            adIsReady = true;
        }
    }

    public void RequestInsterstitialAdForFailed()
    {
        if (GlobalVariables.requestInterstitialAd == true)
        {
            if (requestAdCorroutine != null)
            {
                StopCoroutine(requestAdCorroutine);
            }
            requestAdCorroutine = RequestInsterstitialAd();
            StartCoroutine(requestAdCorroutine);
        }
    }

    IEnumerator RequestInsterstitialAd()
    {
        yield return new WaitForSeconds(5);
        GlobalVariables.requestInterstitialAd = true;
        GeneralControls.GetComponent<AdMobController>().RequestAndLoadInterstitialAd();
    }
}

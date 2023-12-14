using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Joker_MaxCombo : MonoBehaviour
{
    public Transform GeneralControls;
    public TMP_Text Counter;
    public int MaxJokerCount;
    int jokerCount = 0;

    bool adIsReady = false;
    IEnumerator requestAdCorroutine;
    bool requestAdFirstTime = true;//jokerinin oyun baþýna sadece bir kez kullanýlmasý için

    // Start is called before the first frame update
    void Start()
    {
        Counter.text = jokerCount.ToString();
    }

    public void OnClick()
    {
        if (jokerCount > 0)
        {
            jokerCount--;
            Counter.text = jokerCount.ToString();
            GeneralControls.GetComponent<ComboBarControl>().MaximizeComboBar();
        }
        else
        {
            if (requestAdFirstTime)
            {
                ShowAd();
            }
        }
    }

    #region reklam methodlarý
    void ShowAd()
    {
        GlobalVariables.whichJokerRequestRewardAd = GlobalVariables.joker_maxCombo;
        if (adIsReady)
        {
            adIsReady = false;
            //GeneralControls.GetComponent<AdMobController>().ShowRewardedAd();
            GeneralControls.GetComponent<AdMobRewardedAdController>().ShowAd();
        }
    }

    public void adLoaded()
    {
        if (GlobalVariables.requestRewardedAd == true)
        {
            adIsReady = true;
        }
    }

    public void RequestRewardAdForFailed()
    {
        if (GlobalVariables.requestRewardedAd == true)
        {
            if (requestAdCorroutine != null)
            {
                StopCoroutine(requestAdCorroutine);
            }
            requestAdCorroutine = RequestRewardAd();
            StartCoroutine(requestAdCorroutine);
        }
    }

    IEnumerator RequestRewardAd()
    {
        yield return new WaitForSeconds(5);
        GlobalVariables.requestRewardedAd = true;
        //GeneralControls.GetComponent<AdMobController>().RequestAndLoadRewardedAd();
        GeneralControls.GetComponent<AdMobRewardedAdController>().LoadAd();
    }

    public void SetRewardForAd()
    {
        if (GlobalVariables.whichJokerRequestRewardAd == GlobalVariables.joker_maxCombo)
        {
            jokerCount = MaxJokerCount;
            Counter.text = jokerCount.ToString();
            requestAdFirstTime = false;

            GlobalVariables.whichJokerRequestRewardAd = GlobalVariables.joker_non;
            GlobalVariables.requestRewardedAd = true;//hiç bir yerde false yapmýyoruz!!!
            //GeneralControls.GetComponent<AdMobController>().RequestAndLoadRewardedAd();
            GeneralControls.GetComponent<AdMobRewardedAdController>().LoadAd();
        }
    }
    #endregion
}

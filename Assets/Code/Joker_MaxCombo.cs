using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Joker_MaxCombo : MonoBehaviour
{
    public Transform GeneralControls;
    public TMP_Text Counter;
    public int MaxJokerCount;
    public GameObject SetRewardAnim;
    int jokerCount = 0;

    bool adIsReady = false;
    IEnumerator requestAdCorroutine;
    bool requestAdFirstTime = true;//jokerinin oyun baþýna sadece bir kez kullanýlmasý için

    // Start is called before the first frame update
    void Start()
    {
        GetJokerCountFromPlayerPrefs();
        Counter.text = jokerCount.ToString();
    }

    public void OnClick()
    {
        if (jokerCount > 0)
        {
            jokerCount--;
            Counter.text = jokerCount.ToString();
            SetJokerCountFromPlayerPrefs();
            GeneralControls.GetComponent<ComboBarControl>().MaximizeComboBar();
            if(jokerCount == 0 && requestAdFirstTime == false)
            {
                gameObject.GetComponent<Button>().interactable = false;
            }
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

            SetRewardAnim.GetComponentInChildren<TMP_Text>().text = "+"+jokerCount.ToString();
            SetRewardAnim.SetActive(true);
            GlobalVariables.whichJokerRequestRewardAd = GlobalVariables.joker_non;
            GlobalVariables.requestRewardedAd = true;//hiç bir yerde false yapmýyoruz!!!
            //GeneralControls.GetComponent<AdMobController>().RequestAndLoadRewardedAd();
            GeneralControls.GetComponent<AdMobRewardedAdController>().LoadAd();
        }
    }
    #endregion

    public void GetJokerCountFromPlayerPrefs()
    {
        try
        {
            string jokerCount_ = PlayerPrefs.GetString("MaxComboJoker");
            if (jokerCount_ != "")
            {
                jokerCount = int.Parse(jokerCount_);
            }
        }
        catch
        {

        }

    }
    public void SetJokerCountFromPlayerPrefs()
    {
        try
        {
            PlayerPrefs.SetString("MaxComboJoker", jokerCount.ToString());
            PlayerPrefs.Save();
        }
        catch
        {

        }
    }
}

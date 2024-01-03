using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class Joker_ChangeSpawnedBlocks : MonoBehaviour
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
            DestroySpawnedBlocks();
            GeneralControls.GetComponent<CreateBlocks>().CreateRandomBlocks(true);
        }
        else
        {
            if (requestAdFirstTime)
            {
                ShowAd();
            }
        }

    }

    void DestroySpawnedBlocks()
    {
        for (int i = 0; i < GeneralControls.GetComponent<CreateBlocks>().SpawnPoints.Count; i++)
        {
            Destroy(GeneralControls.GetComponent<CreateBlocks>().SpawnPoints[i].GetComponent<SpawnPointHelper>().block.gameObject);
            GeneralControls.GetComponent<CreateBlocks>().SpawnPoints[i].GetComponent<SpawnPointHelper>().block = null;
        }
    }

    #region reklam methodlarý
    void ShowAd()
    {
        GlobalVariables.whichJokerRequestRewardAd = GlobalVariables.joker_blockChanger;
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

        if (GlobalVariables.whichJokerRequestRewardAd == GlobalVariables.joker_blockChanger)
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

    public void GetJokerCountFromPlayerPrefs()
    {
        try
        {
            string jokerCount_ = PlayerPrefs.GetString("ChangerJoker");
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
            PlayerPrefs.SetString("ChangerJoker", jokerCount.ToString());
            PlayerPrefs.Save();
        }
        catch
        {

        }
    }
}

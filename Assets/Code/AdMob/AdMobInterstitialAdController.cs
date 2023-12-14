using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdMobInterstitialAdController : MonoBehaviour
{

    public GameObject PauseMenuRestartButton;
    public GameObject GameOverMenuRestartButton;
    public GameObject GameOverMenuMainMenuButton;
    public GameObject JokerBomb;
    public GameObject JokerChangeSpawnedBlock;
    public GameObject JokerMaxCombo;

    [Tooltip("Reklamýn para kazandýrdýðý tahmin edildiðinde tektiklenir.")]
    public UnityEvent OnAdPaidEvent;
    [Tooltip("Reklam gösterim/impression elde ettiðinde tetiklenir.")]
    public UnityEvent OnAdImpressionRecordedEvent;
    [Tooltip("Reklam týklandýðýnda tetiklenir.")]
    public UnityEvent OnAdClickedEvent;
    [Tooltip("Reklam gösterildiðinde tetiklenir.")]
    public UnityEvent OnAdFullScreenContentOpenedEvent;
    [Tooltip("Reklam kapandýðýnda tetiklenir.")]
    public UnityEvent OnAdFullScreenContentClosedEvent;
    [Tooltip("Reklam açýlýrken hata aldýðýnda tetiklenir.")]
    public UnityEvent OnAdFullScreenContentFailedEvent;

    /// <summary>
    /// UI element activated when an ad is ready to show.
    /// </summary>
    //public GameObject AdLoadedStatus;

#if UNITY_ANDROID
    private const string _adUnitId = "ca-app-pub-3475441178822227/6094817285";
#elif UNITY_IPHONE
        private const string _adUnitId = "ca-app-pub-3475441178822227/6315731953";
#else
        private const string _adUnitId = "unused";
#endif

    private InterstitialAd _interstitialAd;

    /// <summary>
    /// Loads the ad.
    /// </summary>
    public void LoadAd()
    {
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            DestroyAd();
        }

        Debug.Log("Loading interstitial ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("game");

        // Send the request to load the ad.
        InterstitialAd.Load(_adUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            // If the operation failed with a reason.
            if (error != null)
            {
                Debug.LogError("Interstitial ad failed to load an ad with error : " + error);
                return;
            }
            // If the operation failed for unknown reasons.
            // This is an unexpected error, please report this bug if it happens.
            if (ad == null)
            {
                Debug.LogError("Unexpected error: Interstitial load event fired with null ad and null error.");
                return;
            }

            // The operation completed successfully.
            //Debug.Log("Interstitial ad loaded with response : " + ad.GetResponseInfo());
            _interstitialAd = ad;

            // Register to ad events to extend functionality.
            RegisterEventHandlers(ad);

            // Inform the UI that the ad is ready.
            //AdLoadedStatus?.SetActive(true);
            PauseMenuRestartButton.GetComponent<PauseMenu_Restart_Btn>().adLoaded();
            GameOverMenuRestartButton.GetComponent<GameOverMenu_Restart_Btn>().adLoaded();
            GameOverMenuMainMenuButton.GetComponent<GameOverMenu_MainMenu_Btn>().adLoaded();
            JokerBomb.GetComponent<Joker_Bomb_TouchControl>().adLoaded();
            JokerChangeSpawnedBlock.GetComponent<Joker_ChangeSpawnedBlocks>().adLoaded();
            JokerMaxCombo.GetComponent<Joker_MaxCombo>().adLoaded();
        });
    }

    /// <summary>
    /// Shows the ad.
    /// </summary>
    public bool ShowAd()
    {
        bool CanShowAd = false;
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            //Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
            CanShowAd = true;
        }

        // Inform the UI that the ad is not ready.
        //AdLoadedStatus?.SetActive(false);
        return CanShowAd;
    }

    /// <summary>
    /// Destroys the ad.
    /// </summary>
    public void DestroyAd()
    {
        if (_interstitialAd != null)
        {
            //Debug.Log("Destroying interstitial ad.");
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        // Inform the UI that the ad is not ready.
        //AdLoadedStatus?.SetActive(false);
    }

    /// <summary>
    /// Logs the ResponseInfo.
    /// </summary>
    public void LogResponseInfo()
    {
        if (_interstitialAd != null)
        {
            var responseInfo = _interstitialAd.GetResponseInfo();
            UnityEngine.Debug.Log(responseInfo);
        }
    }

    private void RegisterEventHandlers(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            //Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
            //    adValue.Value,
            //    adValue.CurrencyCode));
            OnAdPaidEvent.Invoke();
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            //Debug.Log("Interstitial ad recorded an impression.");
            OnAdImpressionRecordedEvent.Invoke();
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            //Debug.Log("Interstitial ad was clicked.");
            OnAdClickedEvent.Invoke();
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            //Debug.Log("Interstitial ad full screen content opened.");
            OnAdFullScreenContentOpenedEvent.Invoke();
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            //Debug.Log("Interstitial ad full screen content closed.");
            OnAdFullScreenContentClosedEvent.Invoke();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            //Debug.LogError("Interstitial ad failed to open full screen content with error : "
            //    + error);
            OnAdFullScreenContentFailedEvent.Invoke();
        };
    }
}

using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdMobBannerViewController : MonoBehaviour
{
    [Tooltip("Banner reklamý yüklendiðinde tetiklenir.")]
    public UnityEvent OnBannerAdLoadedEvent;
    [Tooltip("Banner reklamý yüklenirken hata alýrsa tetiklenir.")]
    public UnityEvent OnBannerAdLoadFailedEvent;
    [Tooltip("Banner reklamýnýn para kazandýrdýðý tahmin edildiðinde tektiklenir.")]
    public UnityEvent OnAdPaidEvent;
    [Tooltip("Banner reklamý gösterim/impression elde ettiðinde tetiklenir.")]
    public UnityEvent OnAdImpressionRecordedEvent;
    [Tooltip("Banner reklamý týklandýðýnda tetiklenir.")]
    public UnityEvent OnAdClickedEvent;
    [Tooltip("Banner reklamý gösterildiðinde tetiklenir.")]
    public UnityEvent OnAdFullScreenContentOpenedEvent;
    [Tooltip("Banner reklamý kapandýðýnda tetiklenir.")]
    public UnityEvent OnAdFullScreenContentClosedEvent;

#if UNITY_ANDROID
    private const string _adUnitId = "ca-app-pub-3475441178822227/8469984595";
#elif UNITY_IPHONE
        private const string _adUnitId = "ca-app-pub-3475441178822227/1594991968";
#else
        private const string _adUnitId = "unused";
#endif

    private BannerView _bannerView;

    /// <summary>
    /// Creates a 320x50 banner at top of the screen.
    /// </summary>
    public void CreateBannerView()
    {
        Debug.Log("Creating banner view.");

        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
        {
            DestroyAd();
        }

        AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        // Create a 320x50 banner at top of the screen.
        _bannerView = new BannerView(_adUnitId, adaptiveSize, AdPosition.Bottom);

        // Listen to events the banner may raise.
        ListenToAdEvents();

        Debug.Log("Banner view created.");
    }

    /// <summary>
    /// Creates the banner view and loads a banner ad.
    /// </summary>
    public void LoadAd()
    {
        // Create an instance of a banner view first.
        if (_bannerView == null)
        {
            CreateBannerView();
        }

        // Create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("game");


        // Send the request to load the ad.
        //Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }

    /// <summary>
    /// Shows the ad.
    /// </summary>
    public void ShowAd()
    {
        if (_bannerView != null)
        {
            //Debug.Log("Showing banner view.");
            _bannerView.Show();
        }
    }

    /// <summary>
    /// Hides the ad.
    /// </summary>
    public void HideAd()
    {
        if (_bannerView != null)
        {
            //Debug.Log("Hiding banner view.");
            _bannerView.Hide();
        }
    }

    /// <summary>
    /// Destroys the ad.
    /// When you are finished with a BannerView, make sure to call
    /// the Destroy() method before dropping your reference to it.
    /// </summary>
    public void DestroyAd()
    {
        if (_bannerView != null)
        {
            //Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    /// <summary>
    /// Logs the ResponseInfo.
    /// </summary>
    public void LogResponseInfo()
    {
        if (_bannerView != null)
        {
            var responseInfo = _bannerView.GetResponseInfo();
            if (responseInfo != null)
            {
                Debug.Log(responseInfo);
            }
        }
    }

    /// <summary>
    /// Listen to events the banner may raise.
    /// </summary>
    private void ListenToAdEvents()
    {
        // Raised when an ad is loaded into the banner view.
        _bannerView.OnBannerAdLoaded += () =>
        {
            //Debug.Log("Banner view loaded an ad with response : "
            //    + _bannerView.GetResponseInfo());
            OnBannerAdLoadedEvent.Invoke();
        };
        // Raised when an ad fails to load into the banner view.
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            //Debug.LogError("Banner view failed to load an ad with error : " + error);
            OnBannerAdLoadFailedEvent.Invoke();
        };
        // Raised when the ad is estimated to have earned money.
        _bannerView.OnAdPaid += (AdValue adValue) =>
        {
            //Debug.Log(String.Format("Banner view paid {0} {1}.",
            //    adValue.Value,
            //    adValue.CurrencyCode));
            OnAdPaidEvent.Invoke();
        };
        // Raised when an impression is recorded for an ad.
        _bannerView.OnAdImpressionRecorded += () =>
        {
            //Debug.Log("Banner view recorded an impression.");
            OnAdImpressionRecordedEvent.Invoke();
        };
        // Raised when a click is recorded for an ad.
        _bannerView.OnAdClicked += () =>
        {
            //Debug.Log("Banner view was clicked.");
            OnAdClickedEvent.Invoke();
        };
        // Raised when an ad opened full screen content.
        _bannerView.OnAdFullScreenContentOpened += () =>
        {
            //Debug.Log("Banner view full screen content opened.");
            OnAdFullScreenContentOpenedEvent.Invoke();
        };
        // Raised when the ad closed full screen content.
        _bannerView.OnAdFullScreenContentClosed += () =>
        {
            //Debug.Log("Banner view full screen content closed.");
            OnAdFullScreenContentClosedEvent.Invoke();
        };
    }
}

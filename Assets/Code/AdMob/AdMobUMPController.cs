using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdMobUMPController : MonoBehaviour
{
    private ConsentForm _consentForm;

    private readonly List<string> TEST_DEVICE_IDS = new List<string>
        {
            AdRequest.TestDeviceSimulator,
            // Add your test device IDs (replace with your own device IDs).
            #if UNITY_IPHONE
                "DF229BBF2B1642998DFF3FFA52D9CD30",
            #elif UNITY_ANDROID
                "6D5FF19D7D159049F551197DCDBCA3FA"
            #endif


        };

    private void Awake()
    {
        // For dispatching events back onto the Unity main thread.
        MobileAds.RaiseAdEventsOnUnityMainThread = true;

        // On Android, Unity is paused when displaying interstitial or rewarded video.
        // This behavior should be made consistent with iOS.
        MobileAds.SetiOSAppPauseOnBackground(true);
    }

    void Start()
    {
        //GDDPR ile alaklý izin isteme formu baþlatýlýr.
        UpdateConsentInformation();
    }

    /// <summary>
    /// Updates the consent information.
    /// </summary>
    //izin formunu istemek için ilk çaðrýlacak metot. Yaptýðýmýz ayarlarla request gönderir.
    public void UpdateConsentInformation()
    {
        Debug.Log("Updating consent information.");

        // Confugre the ConsentDebugSettings.
        // The ConsentDebugSettings is serializable so you may expose this to your monobehavior.
        var consentDebugSettings = new ConsentDebugSettings();

        //debugGeography debug yaparken Avrupadan mý test ediyoruz yoksa avrupa dýþýndan mý setliyoruz.
        consentDebugSettings.DebugGeography = DebugGeography.EEA;
        consentDebugSettings.TestDeviceHashedIds = TEST_DEVICE_IDS;

        // Set tag for under age of consent. Here false means users are not under age.
        var consentRequestParameters = new ConsentRequestParameters();
        consentRequestParameters.ConsentDebugSettings = consentDebugSettings;
        //reþit olmayan kullanýcý etiketi
        consentRequestParameters.TagForUnderAgeOfConsent = false;

        ConsentInformation.Update(consentRequestParameters, OnConsentInformationUpdate);
    }

    //gönderilen request sonrasý bir hata olup olmadýðý dönülür
    private void OnConsentInformationUpdate(FormError error)
    {
        if (error != null)
        {
            // The consent information failed to update.
            Debug.Log("Failed to update consent information with error: " +
                    error.Message);
            return;
        }

        // If the error is null, the consent information state was updated.
        // You are now ready to check if a form is available.
        if (ConsentInformation.IsConsentFormAvailable())
        {
            LoadConsentForm();
        }
    }


    /// <summary>
    /// Loads a consent form.
    /// </summary>
    /// <remarks>
    /// This should be done before it is needed
    /// so that you can show the consent form without delay when needed.
    /// </remarks>
    void LoadConsentForm()
    {
        Debug.Log("Loading consent form.");

        ConsentForm.Load(
            // OnConsentFormLoad
            (ConsentForm form, FormError error) =>
            {
                if (form != null)
                {
                    // The consent form was loaded.
                    // We cache the consent form for showing later.
                    _consentForm = form;

                    if (ConsentInformation.ConsentStatus == ConsentStatus.Required)
                    {
                        _consentForm.Show(// OnConsentFormShow
                                             (FormError error) =>
                                             {
                                                 if (error == null)
                                                 {
                                                     // If the error parameter is null,
                                                     // we showed the consent form without error.
                                                     // Load another consent form for use later.
                                                     LoadConsentForm();
                                                 }
                                                 else
                                                 {
                                                     // The consent form failed to show.
                                                     Debug.Log("Failed to show consent form with error: " +
                                                                       error.Message);
                                                 }
                                             });
                    }
                    else
                    {
                        string status = "";
                        if (ConsentInformation.ConsentStatus == ConsentStatus.Obtained)
                        {
                            status = "obtained";
                        }
                        else if (ConsentInformation.ConsentStatus == ConsentStatus.NotRequired)
                        {
                            status = "not required";
                        }
                        else if (ConsentInformation.ConsentStatus == ConsentStatus.Unknown)
                        {
                            status = "unkown";
                        }

                        Debug.Log("Ad Mob UMP consent status " + status);
                    }
                }
                else
                {
                    // The consent form failed to load.
                    Debug.Log("Failed to load consent form with error: " +
                        error == null ? "unknown error" : error.Message);
                }
            });
    }

    /// <summary>
    /// Clears all consent information from persistent storage.
    /// </summary>
    // test amaçlý kullanýlýyor. Kullanýcýnýn izin verme durumunu sýfýrlýyor.
    public void ResetConsentInformation()
    {
        ConsentInformation.Reset();
        Debug.Log("Consent information has been reset.");
    }
}

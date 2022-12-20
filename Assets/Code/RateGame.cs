using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class RateGame : MonoBehaviour
{
    //kullanýcnýn oy verip vermediðini kontrol edebilme yöntemi olmadýðý için (harhangi bir callback yokmuþ) oy verme ekraný açýlýnca oy vermiþ sayacaðýz.
    bool ratedAlredy = false;
    
    //kullanýcý no thanks butonuna basýp basmadýðýný tutar
    bool doesPlayerWantToRate = true;

    //Kaç kez giriþ yapýldýktan sonra oylama kutusu açýlacak 
    public int countToRate = 0;
    public GameObject rateBox;

    void Start()
    {
        //PlayerPrefs.DeleteKey("PlayCountForRate");
        //PlayerPrefs.DeleteKey("DoesPlayerWantToRate");
        //PlayerPrefs.DeleteKey("DoesPlayerRatedGame");

        #region kullanýcý no thanks butonuna basmýþ mý kontrolü.
        string doesPlayerWantToRateString = PlayerPrefs.GetString("DoesPlayerWantToRate");
        if (doesPlayerWantToRateString == "")
        {
            doesPlayerWantToRateString = "true";
        }
        bool.TryParse(doesPlayerWantToRateString, out doesPlayerWantToRate);
        #endregion

        #region kullanýcý daha önce oy verme ekranýný açtý mý? yani oy verdi sayacak mýyýz?
        string ratedAlredyString = PlayerPrefs.GetString("DoesPlayerRatedGame");
        if (ratedAlredyString == "")
        {
            ratedAlredyString = "false";
        }
        bool.TryParse(ratedAlredyString, out ratedAlredy);
        #endregion

        #region oyunu belirli miktar tekrar açtýktan sonra kullanýcýnýn karþýsýna oy verme ekraný çýkart
        string playCountString = PlayerPrefs.GetString("PlayCountForRate");
        if (playCountString == "")
        {
            playCountString = "0";
        }

        int playCount = -1;
        int.TryParse(playCountString, out playCount);

        if (playCount != -1)
        {
            playCount++;

            if (playCount % countToRate == 0 && ratedAlredy == false && doesPlayerWantToRate == true)
            {
#if UNITY_ANDROID
                rateBox.SetActive(true);
#elif UNITY_IOS
                Device.RequestStoreReview();
                ratedAlredy = true;
                PlayerPrefs.SetString("DoesPlayerRatedGame", ratedAlredy.ToString());
                PlayerPrefs.Save();     
#endif
            }
        }
        #endregion

        PlayerPrefs.SetString("PlayCountForRate", playCount.ToString());
        PlayerPrefs.SetString("DoesPlayerWantToRate", doesPlayerWantToRateString.ToString());
        PlayerPrefs.SetString("DoesPlayerRatedGame", ratedAlredy.ToString());
        PlayerPrefs.Save();
    }

    public void ClickNoThanksBtn()
    {
        PlayerPrefs.SetString("DoesPlayerWantToRate", "false");
        rateBox.SetActive(false);
    }
    public void ClickLaterBtn()
    {
        rateBox.SetActive(false);
    }
    public void ClickRateNowBtn()
    {
        //burasý oyun markete çýkýnca deðiþecek
        Application.OpenURL("market://details?id=com.Alpay.Kubiks");

        ratedAlredy = true;
        PlayerPrefs.SetString("DoesPlayerRatedGame", ratedAlredy.ToString());
        PlayerPrefs.Save();
        rateBox.SetActive(false);
    }
}

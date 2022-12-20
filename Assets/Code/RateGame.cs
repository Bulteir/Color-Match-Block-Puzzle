using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class RateGame : MonoBehaviour
{
    //kullan�cn�n oy verip vermedi�ini kontrol edebilme y�ntemi olmad��� i�in (harhangi bir callback yokmu�) oy verme ekran� a��l�nca oy vermi� sayaca��z.
    bool ratedAlredy = false;
    
    //kullan�c� no thanks butonuna bas�p basmad���n� tutar
    bool doesPlayerWantToRate = true;

    //Ka� kez giri� yap�ld�ktan sonra oylama kutusu a��lacak 
    public int countToRate = 0;
    public GameObject rateBox;

    void Start()
    {
        //PlayerPrefs.DeleteKey("PlayCountForRate");
        //PlayerPrefs.DeleteKey("DoesPlayerWantToRate");
        //PlayerPrefs.DeleteKey("DoesPlayerRatedGame");

        #region kullan�c� no thanks butonuna basm�� m� kontrol�.
        string doesPlayerWantToRateString = PlayerPrefs.GetString("DoesPlayerWantToRate");
        if (doesPlayerWantToRateString == "")
        {
            doesPlayerWantToRateString = "true";
        }
        bool.TryParse(doesPlayerWantToRateString, out doesPlayerWantToRate);
        #endregion

        #region kullan�c� daha �nce oy verme ekran�n� a�t� m�? yani oy verdi sayacak m�y�z?
        string ratedAlredyString = PlayerPrefs.GetString("DoesPlayerRatedGame");
        if (ratedAlredyString == "")
        {
            ratedAlredyString = "false";
        }
        bool.TryParse(ratedAlredyString, out ratedAlredy);
        #endregion

        #region oyunu belirli miktar tekrar a�t�ktan sonra kullan�c�n�n kar��s�na oy verme ekran� ��kart
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
        //buras� oyun markete ��k�nca de�i�ecek
        Application.OpenURL("market://details?id=com.Alpay.Kubiks");

        ratedAlredy = true;
        PlayerPrefs.SetString("DoesPlayerRatedGame", ratedAlredy.ToString());
        PlayerPrefs.Save();
        rateBox.SetActive(false);
    }
}

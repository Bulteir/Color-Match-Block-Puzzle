using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LeaderboardController : MonoBehaviour
{
    public GameObject TotalRecordText;
    public GameObject TemplateRow;
    public GameObject TemplateBrace;
    public GameObject ScrollviewContent;
    public GameObject RowParent;

    public int LineSpacing;
    int LastRowIndex;
    float nextPosY = 0;
    float rowGroupSpacing = 0;

    // Start is called before the first frame update
    async void Start()
    {

        if (UnityServices.State != ServicesInitializationState.Initializing || UnityServices.State != ServicesInitializationState.Initialized)
        {
            await UnityServices.InitializeAsync();
        }

        //AuthenticationService.Instance.ClearSessionToken();
        if (AuthenticationService.Instance.IsAuthorized == false)
        {
            await SignInAnonymously();
        }
    }

    async Task SignInAnonymously()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
        };
        AuthenticationService.Instance.SignInFailed += s =>
        {
            // Take some action here...
            Debug.Log(s);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void SetPlayerName(string name)
    {
        string playerName = name;

        playerName = playerName.Replace(" ", "_");
        Debug.Log("Oyuncu adý güncelleniyor");

        //hiçbirþey ile giriþ yapmadýysa veya baþka bir sebeple boþ kullanýcý adý setlememek adýna
        if (playerName.Length > 1 && playerName.Length < 50)
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
        }
    }

    public async void AddScore(double score)
    {
        if (AuthenticationService.Instance.IsAuthorized == false)
        {
            await SignInAnonymously();
        }

        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(GlobalVariables.LeaderboardId_BestTime, score);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    public async void FillLeaderboardList()
    {
        try
        {
            if (GlobalVariables.internetAvaible)
            {
                //daha önceden oluþturulmuþ satýrlar varsa sil
                foreach (Transform oldRows in RowParent.transform)
                {
                    Destroy(oldRows.gameObject);
                }

                LastRowIndex = 0;
                nextPosY = 0;
                rowGroupSpacing = 0;

                LeaderboardEntry scoreResponse = null;
                //kullanýcýnýn kayýtlý puaný varmý kontrol ediyoruz. Puaný varsa sýralamasýna göre farklý þekilde gösterim yapýyoruz.
                //Kullanýcýnýn kayýtlý puaný yoksa hata dönüyor. Hata aldýðýmýzda kullanýcýn skoru yok diye kabul edebiliriz.
                try
                {
                    scoreResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(GlobalVariables.LeaderboardId_BestTime);
                }
                catch (Exception)
                {

                }

                //Oyuncunun kayýtlý puaný yoktur ya da daha küçük ihtimalle ele almadýðýmýz baþka bir hata oluþmuþtur.
                //bu durumda ilk 30 kaydý göster
                if (scoreResponse == null)
                {

                    int Limit = 30;
                    int page = 0;
                    int Offset = Limit * page;

                    LeaderboardScoresPage topScores = await LeaderboardsService.Instance.GetScoresAsync(GlobalVariables.LeaderboardId_BestTime, new GetScoresOptions { Offset = Offset, Limit = Limit });

                    for (int i = 0; i < topScores.Results.Count; i++)
                    {
                        FillLeaderboardRow(i, topScores.Results[i], new Color(9f / 255f, 183f / 255f, 255f / 255f, 0f / 255f));
                    }

                    float newBraceSpacing = 0;
                    //Eðer toplam kayýt sayýsý listelenenden fazla ise bir ayraç atýyoruz.
                    if (topScores.Total > Limit)
                    {
                        GameObject newBrace = Instantiate(TemplateBrace, RowParent.transform);
                        newBrace.SetActive(true);
                        newBrace.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (topScores.Results.Count * -1 * (TemplateRow.GetComponent<RectTransform>().sizeDelta.y + LineSpacing)) + LineSpacing);
                        newBraceSpacing = newBrace.GetComponent<RectTransform>().sizeDelta.y - LineSpacing;
                    }

                    //toplam kayýt sayýsý Text'i
                    TotalRecordText.SetActive(true);

                    Dictionary<string, string> arguments = new Dictionary<string, string> { { "value", topScores.Total.ToString() } };
                    TotalRecordText.GetComponent<TMP_Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Localizations", "Total", new object[] { arguments });

                    TotalRecordText.GetComponent<RectTransform>().anchoredPosition = new Vector2(TotalRecordText.GetComponent<RectTransform>().anchoredPosition.x, (topScores.Results.Count * -1 * (TemplateRow.GetComponent<RectTransform>().sizeDelta.y + LineSpacing)) - newBraceSpacing);
                }
                else//kullanýcýnýn leaderboarda kayýtlý puaný varsa
                {
                    int Limit = 10;
                    int page = 0;
                    int Offset = Limit * page;
                    int rangeLimit = 5;

                    //oyuncu ilk 10 listesinde deðilse. +rangelimit dememizin sebebi kullanýcýnýn üstünde ve altýndaki 5 oyuncuyu daha listeleyecek olmamýz.
                    if (scoreResponse.Rank >= Limit + rangeLimit)
                    {
                        LeaderboardScoresPage topScores = await LeaderboardsService.Instance.GetScoresAsync(GlobalVariables.LeaderboardId_BestTime, new GetScoresOptions { Offset = Offset, Limit = Limit });

                        // top 10 bilgileri
                        for (int i = 0; i < topScores.Results.Count; i++)
                        {
                            FillLeaderboardRow(i, topScores.Results[i], new Color(9f / 255f, 183f / 255f, 255f / 255f, 0f / 255f));
                        }
                        //bir ayraç atýyoruz.
                        GameObject newBrace = Instantiate(TemplateBrace, RowParent.transform);
                        newBrace.SetActive(true);
                        LastRowIndex++;
                        newBrace.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, LastRowIndex * -1 * (TemplateRow.GetComponent<RectTransform>().sizeDelta.y + LineSpacing) + LineSpacing + rowGroupSpacing);
                        rowGroupSpacing = -(newBrace.GetComponent<RectTransform>().sizeDelta.y - (TemplateRow.GetComponent<RectTransform>().sizeDelta.y + LineSpacing));

                        // kullanýcý ve +- 5 mesafesi skorlarý
                        LeaderboardScores rangedScores = await LeaderboardsService.Instance.GetPlayerRangeAsync(GlobalVariables.LeaderboardId_BestTime, new GetPlayerRangeOptions { RangeLimit = rangeLimit });

                        int tempIndex = LastRowIndex + 1;
                        for (int i = 0; i < rangedScores.Results.Count; i++)
                        {
                            if (scoreResponse.Rank == rangedScores.Results[i].Rank)
                            {
                                //kendi adýný farklý renkte göstermek için kullanýlýyordu. Ancak async çalýþtýðý için gösterirken sýralama deðiþebiliyor ve yanlýþ kiþi highlight edilebiliyor.
                                FillLeaderboardRow(tempIndex + i, rangedScores.Results[i], new Color(83f / 255f, 159f / 255f, 91f / 255f));
                                //FillLeaderboardRow(tempIndex + i, rangedScores.Results[i], Color.white);
                            }
                            else
                            {
                                FillLeaderboardRow(tempIndex + i, rangedScores.Results[i], new Color(0, 0, 0, 0));
                            }
                        }

                        //Eðer kullanýcýdan sonra listelenen 5 kiþiden baþka kiþilerde varsa bir ayraç daha atýyoruz
                        if (topScores.Total > rangedScores.Results[rangedScores.Results.Count - 1].Rank + 1)
                        {
                            //bir ayraç daha atýyoruz
                            newBrace = Instantiate(TemplateBrace, RowParent.transform);
                            newBrace.SetActive(true);
                            LastRowIndex++;
                            newBrace.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, LastRowIndex * -1 * (TemplateRow.GetComponent<RectTransform>().sizeDelta.y + LineSpacing) + LineSpacing + rowGroupSpacing);
                            rowGroupSpacing += -(newBrace.GetComponent<RectTransform>().sizeDelta.y - (TemplateRow.GetComponent<RectTransform>().sizeDelta.y + LineSpacing));
                        }

                        //toplam kayýt sayýsý Text'i
                        LastRowIndex++;
                        TotalRecordText.SetActive(true);
                        Dictionary<string, string> arguments = new Dictionary<string, string> { { "value", topScores.Total.ToString() } };
                        TotalRecordText.GetComponent<TMP_Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Localizations", "Total", new object[] { arguments });
                        TotalRecordText.GetComponent<RectTransform>().anchoredPosition = new Vector2(TotalRecordText.GetComponent<RectTransform>().anchoredPosition.x, nextPosY + -1 * (TemplateRow.GetComponent<RectTransform>().sizeDelta.y + LineSpacing) + rowGroupSpacing);
                    }
                    else//oyuncu puan olarak ilk 10'da ise
                    {
                        Limit = 30;
                        page = 0;
                        Offset = Limit * page;

                        LeaderboardScoresPage topScores = await LeaderboardsService.Instance.GetScoresAsync(GlobalVariables.LeaderboardId_BestTime, new GetScoresOptions { Offset = Offset, Limit = Limit });

                        for (int i = 0; i < topScores.Results.Count; i++)
                        {
                            if (topScores.Results[i].Rank == scoreResponse.Rank)
                            {
                                //kendi adýný farklý renkte göstermek için kullanýlýyordu. Ancak async çalýþtýðý için gösterirken sýralama deðiþebiliyor ve yanlýþ kiþi highlight edilebiliyor.
                                FillLeaderboardRow(i, topScores.Results[i], new Color(83f / 255f, 159f / 255f, 91f / 255f));
                                //FillLeaderboardRow(i, topScores.Results[i], Color.white);
                            }
                            else
                            {
                                FillLeaderboardRow(i, topScores.Results[i], new Color(0, 0, 0, 0));
                            }
                        }

                        float newBraceSpacing = 0;
                        //Eðer toplam kayýt sayýsý listelenenden fazla ise bir ayraç atýyoruz.
                        if (topScores.Total > Limit)
                        {
                            GameObject newBrace = Instantiate(TemplateBrace, RowParent.transform);
                            newBrace.SetActive(true);
                            newBrace.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (topScores.Results.Count * -1 * (TemplateRow.GetComponent<RectTransform>().sizeDelta.y + LineSpacing)) + LineSpacing);
                            newBraceSpacing = newBrace.GetComponent<RectTransform>().sizeDelta.y - LineSpacing;
                        }

                        //toplam kayýt sayýsý Text'i
                        TotalRecordText.SetActive(true);
                        Dictionary<string, string> arguments = new Dictionary<string, string> { { "value", topScores.Total.ToString() } };
                        TotalRecordText.GetComponent<TMP_Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Localizations", "Total", new object[] { arguments });
                        TotalRecordText.GetComponent<RectTransform>().anchoredPosition = new Vector2(TotalRecordText.GetComponent<RectTransform>().anchoredPosition.x, (topScores.Results.Count * -1 * (TemplateRow.GetComponent<RectTransform>().sizeDelta.y + LineSpacing)) - newBraceSpacing);
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("Leaderboard listesi doldurulurken bir hata meydana geldi. " + e);
        }
        finally
        {
            SetContentHeight();
            Canvas.ForceUpdateCanvases();
        }
    }

    void FillLeaderboardRow(int rowIndex, LeaderboardEntry content, Color backgroundColor)
    {
        if (rowIndex > LastRowIndex)
            LastRowIndex = rowIndex;

        GameObject newRow = Instantiate(TemplateRow, RowParent.transform);
        newRow.SetActive(true);
        nextPosY = LastRowIndex * -1 * (TemplateRow.GetComponent<RectTransform>().sizeDelta.y + LineSpacing) + rowGroupSpacing;
        newRow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, nextPosY);
        newRow.transform.GetChild(0).GetComponent<Image>().color = backgroundColor;
        newRow.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().text = (content.Rank + 1).ToString();
        newRow.transform.GetChild(1).transform.GetChild(1).GetComponent<TMP_Text>().text = content.PlayerName.Substring(0, content.PlayerName.IndexOf('#'));
        newRow.transform.GetChild(1).transform.GetChild(2).GetComponent<TMP_Text>().text = content.Score.ToString();
    }

    void SetContentHeight()
    {
        ScrollviewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(ScrollviewContent.GetComponent<RectTransform>().sizeDelta.x, Mathf.Abs(TotalRecordText.GetComponent<RectTransform>().anchoredPosition.y) + TotalRecordText.GetComponent<RectTransform>().sizeDelta.y + 50);
    }
}

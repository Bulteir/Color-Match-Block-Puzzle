using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;

public class HighScoreControl : MonoBehaviour
{
    [System.Serializable]
    public struct HighScoreListUIStruct
    {
        public TMP_Text score;
        public TMP_Text date;
    }

    [System.Serializable]
    public struct HighScoreStruct
    {
        public string date;
        public int score;
    }

    [System.Serializable]
    public class JsonableListWrapper<T>
    {
        public List<T> list;
        public JsonableListWrapper(List<T> list) => this.list = list;
    }

    public List<HighScoreListUIStruct> highScoresUIList;

    void Start()
    {
        getHighScores();
    }

    public void SaveHighScore(int score)
    {
        string json = PlayerPrefs.GetString("HighScores");
        List<HighScoreStruct> highScoreList = new List<HighScoreStruct>();
        if (json != "" && score > 0)
        {
            highScoreList = JsonUtility.FromJson<JsonableListWrapper<HighScoreStruct>>(json).list;

            HighScoreStruct newScore = new HighScoreStruct();
            newScore.score = score;
            newScore.date = System.DateTime.Today.ToShortDateString();
            highScoreList.Add(newScore);

            highScoreList.Sort((x, y) => x.score.CompareTo(y.score));
            highScoreList.Reverse();

            if (highScoreList.Count > 5)
            {
                //son gönderilen skor en kötü skor deðilse. Yeni yüksek skorlardan biri ise.
                if (highScoreList[5].score != score)
                {
                    //yeni yüksek skor mesajý

                    //victoryMessage.text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Congratulations!") + "\n" +
                    //    LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "New Best Time") + "\n" +
                    //    LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Time:") + time + " " +
                    //    LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Moves:") + resolveMoves;
                }
                highScoreList.RemoveAt(5);
            }
            else
            {
                //yeni yüksek skor mesajý

                //victoryMessage.text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Congratulations!") + "\n" +
                //       LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "New Best Time") + "\n" +
                //       LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Time:") + time + " " +
                //       LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Moves:") + resolveMoves;
            }
        }
        else
        {
            HighScoreStruct newScore = new HighScoreStruct();
            newScore.score = score;
            newScore.date = System.DateTime.Today.ToShortDateString();
            highScoreList.Add(newScore);

            //yeni yüksek skor mesajý

            //victoryMessage.text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Congratulations!") + "\n" +
            //    LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "New Best Time") + "\n" +
            //    LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Time:") + time + " " +
            //    LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Moves:") + resolveMoves;
        }

        string tempJson = JsonUtility.ToJson(new JsonableListWrapper<HighScoreStruct>(highScoreList));

        PlayerPrefs.SetString("HighScores", tempJson);
        PlayerPrefs.Save();
    }

    public void getHighScores()
    {
        string json = PlayerPrefs.GetString("HighScores");

        if (json != "")
        {
            List<HighScoreStruct> highscores = JsonUtility.FromJson<JsonableListWrapper<HighScoreStruct>>(json).list;

            for (int i = 0; i < highScoresUIList.Count; i++)
            {
                if (highscores.Count > i)
                {
                    highScoresUIList[i].score.text = highscores[i].score.ToString();
                    highScoresUIList[i].date.text = highscores[i].date;
                }
            }
        }
        else
        {
            foreach (HighScoreListUIStruct item in highScoresUIList)
            {
                item.score.text = "-";
                item.date.text = "-";
            }
        }
    }

}

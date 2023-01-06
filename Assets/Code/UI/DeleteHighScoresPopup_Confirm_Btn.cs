using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteHighScoresPopup_Confirm_Btn : MonoBehaviour
{
    public GameObject popUp;
    public void onClick()
    {
        PlayerPrefs.DeleteKey("HighScores");
        popUp.SetActive(false);
    }
}

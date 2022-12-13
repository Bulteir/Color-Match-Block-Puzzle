using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_HighScore_Btn : MonoBehaviour
{
    public GameObject generalControls;
    public void OnClick()
    {
        generalControls.GetComponent<HighScoreControl>().getHighScores();
        GlobalVariables.gameState = GlobalVariables.gameState_HighScoresMenu;
    }
}

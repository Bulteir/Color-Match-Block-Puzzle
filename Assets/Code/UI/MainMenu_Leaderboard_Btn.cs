using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Leaderboard_Btn : MonoBehaviour
{
    public GameObject generalControls;
    public void OnClick()
    {
        generalControls.GetComponent<LeaderboardController>().FillLeaderboardList();
        GlobalVariables.gameState = GlobalVariables.gameState_LeaderboardMenu;
    }
}

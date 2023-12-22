using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardMenu_Back_Btn : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnClick()
    {
        GlobalVariables.gameState = GlobalVariables.gameState_MainMenu;
    }
}

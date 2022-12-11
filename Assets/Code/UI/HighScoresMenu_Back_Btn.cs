using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoresMenu_Back_Btn : MonoBehaviour
{
    public void OnClick()
    {
        GlobalVariables.gameState = GlobalVariables.gameState_MainMenu;
    }
}

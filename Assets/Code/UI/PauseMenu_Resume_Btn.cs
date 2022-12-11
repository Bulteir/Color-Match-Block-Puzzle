using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu_Resume_Btn : MonoBehaviour
{
    public void OnClick()
    {
        GlobalVariables.gameState = GlobalVariables.gameState_inGame;
    }
}

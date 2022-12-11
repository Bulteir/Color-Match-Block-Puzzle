using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Btn : MonoBehaviour
{
    public void OnClick()
    {
        GlobalVariables.gameState = GlobalVariables.gameState_gamePaused;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables
{
    public const string block = "Block";

    public static int gameState = gameState_inGame;
    public const int gameState_MainMenu = 0;
    public const int gameState_inGame = 1;

    public const int orderInLayer_background = 0;
    public const int orderInLayer_blocks = 1;
    public const int orderInLayer_selectedBlock = 2;

}

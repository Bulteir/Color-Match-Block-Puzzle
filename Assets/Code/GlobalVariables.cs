using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables
{
    public const string block = "Block";
    public const string gridBlock = "GridBlock";

    public static int gameState = gameState_inGame;
    public const int gameState_MainMenu = 0;
    public const int gameState_inGame = 1;

    public const int orderInLayer_background = 0;
    public const int orderInLayer_blocks = 1;
    public const int orderInLayer_selectedBlock = 2;

    public const int gridState_empty = 0;
    public const int gridState_blokA = 1;
    public const int gridState_blokB = 2;

    public static int blockColorType = blockColorType_BlockA;
    public const int blockColorType_BlockA = 0;
    public const int blockColorType_BlockB = 1;
}

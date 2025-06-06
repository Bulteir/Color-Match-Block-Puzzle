using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables
{
    public const string block = "Block";
    public const string gridBlock = "GridBlock";
    public const string scoreText = "ScoreText";
    public const string bomb = "Bomb";

    public static int gameState = gameState_MainMenu;
    public const int gameState_MainMenu = 0;
    public const int gameState_inGame = 1;
    public const int gameState_SettingsMenu = 2;
    public const int gameState_HighScoresMenu = 3;
    public const int gameState_LoadingGame = 4;
    public const int gameState_gamePaused = 5;
    public const int gameState_gameOver = 6;
    public const int gameState_LeaderboardMenu = 7;
    public const int gameState_StoreMenu = 8;

    public const int orderInLayer_background = 0;
    public const int orderInLayer_blocks = 1;
    public const int orderInLayer_selectedBlock = 2;

    public const int gridState_empty = 0;
    public const int gridState_blokA = 1;
    public const int gridState_blokB = 2;

    public static int blockColorType = blockColorType_BlockA;
    public const int blockColorType_BlockA = 0;
    public const int blockColorType_BlockB = 1;

    public static Vector3 scaleSpawnBlocks = new Vector3(0.65f, 0.65f, 1);

    public const int baseScore = 10;
    public const int maxScoreMultiplier = 5;

    public static int selectedLanguage = notSelected;
    public const int notSelected = -1;
    public const int english = 0;
    public const int turkish = 1;
    public const int spanish = 2;
    public const int chinese= 3;
    public const int arabic = 4;
    public const int german = 5;
    public const int japanese = 6;

    public static bool requestInterstitialAd = false;
    public static bool requestRewardedAd = false;

    public static int whichButtonRequestInterstitialAd = nonButton;
    public const int nonButton = 0;
    public const int pauseMenuRestart_btn = 1;
    public const int gameOverMenuRestart_btn = 2;
    public const int gameOverMenuMainMenu_btn = 3;

    public static int whichJokerRequestRewardAd = joker_non;
    public const int joker_non= 0;
    public const int joker_bomb = 1;
    public const int joker_blockChanger = 2;
    public const int joker_maxCombo = 3;

    public const string LeaderboardId_BestTime = "Leaderboard";
    public static bool internetAvaible = true;
}

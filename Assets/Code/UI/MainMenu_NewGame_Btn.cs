using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu_NewGame_Btn : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        GlobalVariables.gameState = GlobalVariables.gameState_inGame;
    }
}

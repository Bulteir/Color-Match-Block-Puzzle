using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject highScoresMenu;

    // Start is called before the first frame update
    void Start()
    {
        menuActiveControl();
    }

    // Update is called once per frame
    void Update()
    {
        menuActiveControl();
    }

    void menuActiveControl()
    {
        if (GlobalVariables.gameState == GlobalVariables.gameState_MainMenu && mainMenu.activeSelf == false)
        {
            mainMenu.SetActive(true);
            settingsMenu.SetActive(false);
            highScoresMenu.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_SettingsMenu && settingsMenu.activeSelf == false)
        {
            mainMenu.SetActive(false);
            settingsMenu.SetActive(true);
            highScoresMenu.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_HighScoresMenu && highScoresMenu.activeSelf == false)
        {
            mainMenu.SetActive(false);
            settingsMenu.SetActive(false);
            highScoresMenu.SetActive(true);
        }
    }
}

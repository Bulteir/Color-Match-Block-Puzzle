using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    // Start is called before the first frame update
    void Start()
    {
        menuActiveControl();
        GlobalVariables.requestInterstitialAd = true;
        GlobalVariables.whichButtonRequestInterstitialAd = GlobalVariables.nonButton;
        this.GetComponent<AdMobController>().RequestAndLoadInterstitialAd();
    }

    // Update is called once per frame
    void Update()
    {
        menuActiveControl();
    }
    void menuActiveControl()
    {
        if (GlobalVariables.gameState == GlobalVariables.gameState_gamePaused && pauseMenu.activeSelf == false)
        {
            pauseMenu.SetActive(true);
            SpawnedBlocksMaskHelper(true);
            gameOverMenu.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_inGame && pauseMenu.activeSelf == true)
        {
            pauseMenu.SetActive(false);
            SpawnedBlocksMaskHelper(false);
            gameOverMenu.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_gameOver && gameOverMenu.activeSelf == false)
        {
            pauseMenu.SetActive(false);
            SpawnedBlocksMaskHelper(true);
            gameOverMenu.SetActive(true);
        }
    }

    void SpawnedBlocksMaskHelper(bool active)
    {
        foreach (Transform item in this.GetComponent<CreateBlocks>().SpawnPoints)
        {
            foreach (Transform child in item.GetComponent<SpawnPointHelper>().block)
            {
                if (active)
                    child.GetComponent<SpriteRenderer>().sortingOrder = 0;
                else
                    child.GetComponent<SpriteRenderer>().sortingOrder = GlobalVariables.orderInLayer_selectedBlock;
            }
        }
        
    }
}

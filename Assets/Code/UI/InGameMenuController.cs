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
            PauseMenuMaskHelper(true);
            gameOverMenu.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_inGame && pauseMenu.activeSelf == true)
        {
            pauseMenu.SetActive(false);
            PauseMenuMaskHelper(false);
            gameOverMenu.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_gameOver && gameOverMenu.activeSelf == false)
        {
            pauseMenu.SetActive(false);
            PauseMenuMaskHelper(false);
            gameOverMenu.SetActive(true);
        }
    }

    void PauseMenuMaskHelper(bool active)
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

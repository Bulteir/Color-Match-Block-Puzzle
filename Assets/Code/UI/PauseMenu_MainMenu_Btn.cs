using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu_MainMenu_Btn : MonoBehaviour
{
    public Transform generalControls;
    public void OnClick()
    {
        generalControls.GetComponent<AdMobBannerViewController>().DestroyAd();
        //generalControls.GetComponent<AdMobController>().DestroyBannerAd();
        GlobalVariables.gameState = GlobalVariables.gameState_MainMenu;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu_RateGame_Btn : MonoBehaviour
{
    public GameObject GeneralControl;
    public GameObject RateGameBox;
    public void OnClick()
    {
        RateGameBox.SetActive(true);
    }
}

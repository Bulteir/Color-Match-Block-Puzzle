using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu_DeleteHighScores_Btn : MonoBehaviour
{
    public GameObject popUp;
    public void onClick()
    {
        popUp.SetActive(true);
    }
}

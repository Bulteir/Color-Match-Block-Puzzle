using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu_DeleteHighScores_Btn : MonoBehaviour
{
    public GameObject popUp;
    public void onClick()
    {
        PlayerPrefs.DeleteKey("HighScores");
        Debug.Log("Yüksek skorlar silindi.");
        //popUp.GetComponent<Lean.Gui.LeanWindow>().On = true;
    }
}

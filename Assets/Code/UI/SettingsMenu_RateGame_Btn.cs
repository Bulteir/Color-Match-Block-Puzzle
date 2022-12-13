using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu_RateGame_Btn : MonoBehaviour
{
    public void OnClick()
    {
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=YOUR_ID");
#elif UNITY_IPHONE
 Application.OpenURL("itms-apps://itunes.apple.com/app/idYOUR_ID");
#endif
  
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu_Sounds_Btn : MonoBehaviour
{
    public Transform sounds;
    public Sprite soundOn;
    public Sprite soundOff;

    // Start is called before the first frame update
    void Start()
    {
        string soundPref = PlayerPrefs.GetString("Sound");
        if (soundPref != "")
        {
            if (soundPref == "on")
            {
                this.GetComponent<Image>().sprite = soundOn;
            }
            else
            {
                this.GetComponent<Image>().sprite = soundOff;
            }
        }
    }

    public void Onclick()
    {
        string soundPref = PlayerPrefs.GetString("Sound");
        if (soundPref != "")
        {
            if (soundPref == "on")
            {
                foreach (Transform item in sounds)
                {
                    item.GetComponent<AudioSource>().mute = true;
                }
                this.GetComponent<Image>().sprite = soundOff;
                PlayerPrefs.SetString("Sound", "off");
            }
            else
            {
                foreach (Transform item in sounds)
                {
                    item.GetComponent<AudioSource>().mute = false;
                }
                this.GetComponent<Image>().sprite = soundOn;
                PlayerPrefs.SetString("Sound", "on");
            }
            PlayerPrefs.Save();

        }
    }
}

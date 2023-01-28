using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu_Music_Btn : MonoBehaviour
{
    public Transform musics;
    public Sprite musicOn;
    public Sprite musicOff;

    // Start is called before the first frame update
    void Start()
    {
        string musicPref = PlayerPrefs.GetString("Music");
        if (musicPref != "")
        {
            if (musicPref == "on")
            {
                this.GetComponent<Image>().sprite = musicOn;
            }
            else
            {
                this.GetComponent<Image>().sprite = musicOff;
            }
        }
    }
    public void Onclick()
    {
        string musicPref = PlayerPrefs.GetString("Music");
        if (musicPref != "")
        {
            if (musicPref == "on")
            {
                foreach (Transform item in musics)
                {
                    item.GetComponent<AudioSource>().mute = true;
                }
                this.GetComponent<Image>().sprite = musicOff;
                PlayerPrefs.SetString("Music", "off");
            }
            else
            {
                foreach (Transform item in musics)
                {
                    item.GetComponent<AudioSource>().mute = false;
                }
                this.GetComponent<Image>().sprite = musicOn;
                PlayerPrefs.SetString("Music", "on");
            }
            PlayerPrefs.Save();
        }
    }
}

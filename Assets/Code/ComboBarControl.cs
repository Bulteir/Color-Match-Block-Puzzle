using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComboBarControl : MonoBehaviour
{

    public Slider comboBar;
    public TMP_Text comboBarText;
    public int comboMultiplier = 1;
    public float comboDurationSecond = 2f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (GlobalVariables.gameState == GlobalVariables.gameState_inGame)
        {
            if (comboBar.value > 0 && comboBar.value <= 100)
            {
                comboBar.value = comboBar.value - (comboBar.maxValue / comboDurationSecond) * Time.deltaTime;
            }
            if (comboBar.value == 0)
            {
                DecreaseComboMultiplier();
            }
        }
    }

    public void FillTheBar()
    {
        //e�er bar bo�almadan tekrar doldurulursa kombo �arpan�n� 1 artt�r.
        if (comboBar.value >= 0 && comboMultiplier < GlobalVariables.maxScoreMultiplier)
        {
            comboMultiplier++;
            comboBarText.text = "x" + comboMultiplier.ToString();
            comboBar.value = 100;
        }
    }

    void DecreaseComboMultiplier()
    {
        if (comboMultiplier > 1 && comboMultiplier <= GlobalVariables.maxScoreMultiplier)
        {
            comboMultiplier--;
            comboBarText.text = "x" + comboMultiplier.ToString();
            if (comboMultiplier > 1)
            {
                comboBar.value = 100;
            }
        }
    }

}

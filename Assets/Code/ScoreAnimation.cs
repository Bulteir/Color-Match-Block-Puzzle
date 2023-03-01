using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreAnimation : MonoBehaviour
{
    public TMP_Text mainScoreText;
    bool isScoreAnimationStart = false;

    public void startScoreAnimation()
    {
        isScoreAnimationStart = true;
    }

    float velocity = 0.2f;
    void FixedUpdate()
    {
        if (isScoreAnimationStart && GlobalVariables.gameState == GlobalVariables.gameState_inGame)
        {
            if (transform.localScale.x < 3f* 0.461f)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.Scale(new Vector3(3, 3, 1), new Vector3(0.461f, 0.461f, 0.461f)), 0.05f * 2.5f);
                if (transform.localScale.x > 2.99f * 0.461f)
                {
                    transform.localScale = Vector3.Scale(new Vector3(3, 3, 1), new Vector3(0.461f, 0.461f, 0.461f));
                }
            }
            else if (Vector3.Distance(transform.position, mainScoreText.transform.position) > 5f)
            {
                transform.position = Vector3.Lerp(transform.position, mainScoreText.transform.position, velocity * 0.0005f * 2.5f);
                velocity = velocity * 1.30f;
            }
            else
            {
                int totalScore = int.Parse(mainScoreText.text);
                totalScore += int.Parse(transform.GetComponent<TMP_Text>().text);
                mainScoreText.text = totalScore.ToString();

                isScoreAnimationStart = false;
                velocity = 0.2f;

                GameObject.Destroy(gameObject);
            }
        }
    }
}

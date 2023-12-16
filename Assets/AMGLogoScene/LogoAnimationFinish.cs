using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogoAnimationFinish : MonoBehaviour
{
    public GameObject Slider;
    public string AfterSceneName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadScene()
    {
        StartCoroutine(LoadAsyncScene());
    }

    IEnumerator LoadAsyncScene()
    {
        yield return null;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(AfterSceneName);
        asyncLoad.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while (asyncLoad.progress < 0.9f)
        {
            Slider.GetComponent<Slider>().value = asyncLoad.progress;
            yield return new WaitForEndOfFrame();
        }

        Slider.GetComponent<Slider>().value = 1;
        yield return new WaitForEndOfFrame();

        while (GetComponent<Image>().color.a < 0.9f)
        {
            yield return new WaitForEndOfFrame();
        }
        asyncLoad.allowSceneActivation = true;
    }
}

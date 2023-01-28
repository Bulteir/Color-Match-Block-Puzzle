using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//[RequireComponent (typeof(Button))]
public class ButtonPressEffect : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public AudioSource click;
    public void OnPointerUp(PointerEventData eventData)
    {
        Button button;
        TMP_Dropdown dropdown;
        if (this.TryGetComponent<Button>(out button))
        {
            this.GetComponentInChildren<TMP_Text>().color = Color.white;
        }
        else if (this.TryGetComponent<TMP_Dropdown>(out dropdown))
        {
            click.Play();
            dropdown.placeholder.GetComponent<TMP_Text>().color = Color.white;
            dropdown.captionText.GetComponent<TMP_Text>().color = Color.white;
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        Button button;
        TMP_Dropdown dropdown;
        if (this.TryGetComponent<Button>(out button))
        {
            this.GetComponentInChildren<TMP_Text>().color = new Color(164.0f / 255, 164.0f / 255, 164.0f / 255);
        }
        else if (this.TryGetComponent<TMP_Dropdown>(out dropdown))
        {
            dropdown.placeholder.GetComponent<TMP_Text>().color = new Color(164.0f / 255, 164.0f / 255, 164.0f / 255);
            dropdown.captionText.GetComponent<TMP_Text>().color = new Color(164.0f / 255, 164.0f / 255, 164.0f / 255);
        }
    }
}

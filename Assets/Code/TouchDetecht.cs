using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDetecht : MonoBehaviour
{
    public Transform touchedGrid;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        touchedGrid = collision.gameObject.transform;
        Debug.Log("stay " + collision.gameObject.name);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log("blok dokundu " + collision.gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}

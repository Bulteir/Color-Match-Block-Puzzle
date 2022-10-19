using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTouchControl : MonoBehaviour
{

    GameObject touchedBlock;
    Vector3 deltaPos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVariables.gameState == GlobalVariables.gameState_inGame)
            ControlWithMouse();
        //ControlWithTouch();

        if (touchedBlock != null)
        {
            touchedBlock.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            touchedBlock.transform.position += deltaPos;
        }
    }

    void ControlWithMouse()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (raycastHit.collider != null)
            {
                if (raycastHit.collider.gameObject.tag == GlobalVariables.block)
                {
                    touchedBlock = raycastHit.transform.gameObject;

                    if (touchedBlock.transform.parent != null)
                    {
                        touchedBlock = touchedBlock.transform.parent.gameObject;
                    }

                    SetBlocksSpriteLayer(touchedBlock.transform, GlobalVariables.orderInLayer_selectedBlock);
                    deltaPos = touchedBlock.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    deltaPos.z = 0;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if(touchedBlock != null)
            {
                SetBlocksSpriteLayer(touchedBlock.transform, GlobalVariables.orderInLayer_blocks);
                touchedBlock = null;
                deltaPos = Vector3.zero;
            }
        }
    }

    void SetBlocksSpriteLayer(Transform parentTransfrom, int layer)
    {
        foreach (var item in parentTransfrom.GetComponentsInChildren<SpriteRenderer>())
        {
            item.sortingOrder = layer;
        }
    }
}
/*
    void ControlWithTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("dokunma baþladý");
                RaycastHit raycastHit;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out raycastHit, 100f))
                {
                    if (raycastHit.transform.gameObject.tag == GlobalVariables.block)
                    {
                        Debug.Log("bloklara dokundun");
                    }
                }
                else
                {

                }
            }

            if (touch.phase == TouchPhase.Moved)
            {

            }

            if (touch.phase == TouchPhase.Ended)
            {

            }

        }
    }
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTouchControl : MonoBehaviour
{

    Transform touchedBlock;
    Transform touchedBlockParent;
    List<Transform> toBePlacedGrids;
    List<Vector3> snapPosList;
    Vector3 deltaPos;

    // Start is called before the first frame update
    void Start()
    {
        toBePlacedGrids = new List<Transform>();
        snapPosList = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVariables.gameState == GlobalVariables.gameState_inGame)
        {
            ControlWithMouse();
            MoveSelectedBlocks();
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
                    touchedBlock = raycastHit.transform;
                    if (touchedBlock.parent != null)
                    {
                        touchedBlockParent = touchedBlock.parent;
                    }

                    SetBlocksSpriteLayer(GlobalVariables.orderInLayer_selectedBlock);

                    deltaPos = touchedBlockParent.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    deltaPos.z = 0;
                    deltaPos.y += 1f;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (touchedBlock != null)
            {
                SetBlocksSpriteLayer(GlobalVariables.orderInLayer_blocks);

                for (int i = 0; i < touchedBlockParent.childCount; i++)
                {
                    if (toBePlacedGrids.Count > 0)
                    {
                        Vector3 snapPos = toBePlacedGrids[i].position;
                        snapPos.z = 0;
                        touchedBlockParent.GetChild(i).transform.position = snapPos;
                    }
                }

                foreach (var item in toBePlacedGrids)
                {
                    item.GetComponent<SpriteRenderer>().color = Color.white;
                }

                touchedBlock = null;
                touchedBlockParent = null;
                deltaPos = Vector3.zero;
                snapPosList.Clear();
                toBePlacedGrids.Clear();
            }
        }
    }

    void MoveSelectedBlocks()
    {
        //dokunulan bir bloðun hareket ettirilmesi
        if (touchedBlock != null)
        {

            Vector3 touchPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (touchedBlock.parent != null)
            {
                touchedBlock.parent.position = new Vector3(touchPoint.x, touchPoint.y, 0);
                touchedBlock.parent.position += deltaPos;
            }
            else
            {
                touchedBlock.position = new Vector3(touchPoint.x, touchPoint.y, 0);
                touchedBlock.position += deltaPos;
            }

            #region bir bloðu hareket ettirirken merkezinden ray fýrlatýp hangi gridin üstünde tespit ediyoruz.

            if (touchedBlockParent != null)
            {
                List<Transform> tempToBePlacedGrids = new List<Transform>();
                for (int i = 0; i < touchedBlockParent.childCount; i++)
                {
                    RaycastHit2D raycastHit = Physics2D.Raycast(touchedBlockParent.GetChild(i).position, Vector2.zero);
                    if (raycastHit.collider != null)
                    {
                        if (raycastHit.collider.gameObject.tag == GlobalVariables.gridBlock)
                        {
                            raycastHit.collider.transform.GetComponent<SpriteRenderer>().color = Color.blue;
                            tempToBePlacedGrids.Add(raycastHit.collider.transform);
                            //snapPos = raycastHit.collider.transform.position;
                            //snapPos.z = touchedBlock.position.z;
                        }
                    }
                }

                foreach (var item in toBePlacedGrids)
                {
                    if (!tempToBePlacedGrids.Contains(item))
                    {
                        item.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }

                toBePlacedGrids.Clear();
                toBePlacedGrids.AddRange(tempToBePlacedGrids);
            }
            #endregion
        }
    }

    //birden çok bloklu parçalarýn hareket ettirilirken üst üste binme gibi görüntü bozulmasýný önler
    void SetBlocksSpriteLayer(int layer)
    {
        foreach (var item in touchedBlockParent.GetComponentsInChildren<SpriteRenderer>())
        {
            item.sortingOrder = layer;

            //Aþaðýdaki layer deðiþtirme iþlemi, bir blok seçildiðinde ray'in bloðun içinden geçip altýnda ne olduðunu bulabilmemiz için gerekli
            if (layer == GlobalVariables.orderInLayer_selectedBlock)
            {
                item.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            }
            else
            {
                item.gameObject.layer = LayerMask.NameToLayer("Default");
            }
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTouchControl : MonoBehaviour
{

    Transform touchedBlock;
    Transform touchedBlockParent;
    List<Transform> toBePlacedGrids;
    Vector3 deltaPos;
    Vector3 preCorrectPos;
    bool blockScaleAnimationStarted = false;
    Coroutine BlockScaleSmoothLerpCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        toBePlacedGrids = new List<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVariables.gameState == GlobalVariables.gameState_inGame)
        {
            if (Application.isEditor)
            {
                ControlWithMouse();
                MoveSelectedBlocks();
            }
            else
            {
                ControlWithTouch();
            }
        }
    }

    void ControlWithTouch()
    {
        if (Input.touchCount == 2)
        {
            Touch touch2 = Input.GetTouch(1);

            if (touch2.phase == TouchPhase.Began)
            {
                touchedBlockParent.rotation = Quaternion.Euler(0, 0, 90 + touchedBlockParent.eulerAngles.z);
                if (touchedBlockParent.rotation.z >= 360)
                {
                    touchedBlockParent.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
        else if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit2D raycastHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (raycastHit.collider != null)
                {
                    if (raycastHit.collider.gameObject.tag == GlobalVariables.block && raycastHit.collider.gameObject.GetComponent<BlockProperties>().isSnapped == false)
                    {
                        touchedBlock = raycastHit.transform;
                        if (touchedBlock.parent != null)
                        {
                            touchedBlockParent = touchedBlock.parent;
                        }

                        SetBlocksSpriteLayer(GlobalVariables.orderInLayer_selectedBlock);

                        deltaPos = touchedBlockParent.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        deltaPos.z = 0;
                        deltaPos.y += 1;

                        preCorrectPos = touchedBlockParent.position;

                        //dokunulan block animasyonlu þekilde büyür
                        if (blockScaleAnimationStarted)
                        {
                            StopCoroutine(BlockScaleSmoothLerpCoroutine);
                        }
                        BlockScaleSmoothLerpCoroutine = StartCoroutine(BlockScaleSmoothLerp(touchedBlockParent, true));
                    }
                }
            }

            if (touch.phase == TouchPhase.Moved)
            {
                MoveSelectedBlocks();
            }

            if (touch.phase == TouchPhase.Ended)
            {
                if (touchedBlock != null)
                {
                    SetBlocksSpriteLayer(GlobalVariables.orderInLayer_blocks);

                    #region snap ettirme yeri
                    //hareket ettirilen blok grubunun tamamý grid üzerinde ise snap yapýlýr
                    if (touchedBlockParent.childCount == toBePlacedGrids.Count)
                    {
                        for (int i = 0; i < touchedBlockParent.childCount; i++)
                        {
                            Vector3 snapPos = toBePlacedGrids[i].position;
                            snapPos.z = 0;
                            touchedBlockParent.GetChild(i).transform.position = snapPos;

                            if (touchedBlockParent.GetChild(i).transform.GetComponent<BlockProperties>().BlockColor == GlobalVariables.blockColorType_BlockA)
                            {
                                toBePlacedGrids[i].GetComponent<GridRowColumnControlHelper>().gridState = GlobalVariables.gridState_blokA;
                            }
                            else if (touchedBlockParent.GetChild(i).transform.GetComponent<BlockProperties>().BlockColor == GlobalVariables.blockColorType_BlockB)
                            {
                                toBePlacedGrids[i].GetComponent<GridRowColumnControlHelper>().gridState = GlobalVariables.gridState_blokB;
                            }

                            toBePlacedGrids[i].GetComponent<GridRowColumnControlHelper>().snapedBlockTile = touchedBlockParent.GetChild(i).transform;
                        }

                        //bir blok spawn noktasýndan alýnýp gride yerleþtirildiðinde spawn noktasýný boþ olarak iþaretliyoruz
                        foreach (Transform item in touchedBlockParent)
                        {
                            item.GetComponent<BlockProperties>().SpawnPoint.GetComponent<SpawnPointHelper>().hasBlocks = false;
                            item.GetComponent<BlockProperties>().isSnapped = true;
                        }

                        transform.GetComponent<CreateBlocks>().CreateRandomBlocks();
                    }
                    else // deðilse eski doðru konumuna gönderilir
                    {
                        touchedBlockParent.position = preCorrectPos;
                        //block spawn noktasýna gönderiliyorsa tekrar küçültülür
                        if (touchedBlockParent.GetComponentInChildren<BlockProperties>().isSnapped == false)
                        {
                            if (blockScaleAnimationStarted)
                            {
                                StopCoroutine(BlockScaleSmoothLerpCoroutine);
                            }
                            BlockScaleSmoothLerpCoroutine = StartCoroutine(BlockScaleSmoothLerp(touchedBlockParent, false));
                        }
                    }
                    #endregion

                    foreach (var item in toBePlacedGrids)
                    {
                        item.GetComponent<SpriteRenderer>().color = Color.white;
                    }

                    touchedBlock = null;
                    touchedBlockParent = null;
                    deltaPos = Vector3.zero;
                    toBePlacedGrids.Clear();
                    preCorrectPos = Vector3.zero;

                    transform.GetComponent<GridRowCloumnControl>().RowColumnControl();
                }
            }
        }
    }

    #region mouse ile editörde kontol etme. Eski kalabilir.
    void ControlWithMouse()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (raycastHit.collider != null)
            {
                if (raycastHit.collider.gameObject.tag == GlobalVariables.block && raycastHit.collider.gameObject.GetComponent<BlockProperties>().isSnapped == false)
                {
                    touchedBlock = raycastHit.transform;
                    if (touchedBlock.parent != null)
                    {
                        touchedBlockParent = touchedBlock.parent;
                    }

                    SetBlocksSpriteLayer(GlobalVariables.orderInLayer_selectedBlock);

                    deltaPos = touchedBlockParent.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    deltaPos.z = 0;
                    deltaPos.y += 2;

                    preCorrectPos = touchedBlockParent.position;

                    //dokunulan block animasyonlu þekilde büyür
                    if (blockScaleAnimationStarted)
                    {
                        StopCoroutine(BlockScaleSmoothLerpCoroutine);
                    }
                    BlockScaleSmoothLerpCoroutine = StartCoroutine(BlockScaleSmoothLerp(touchedBlockParent, true));
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (touchedBlock != null)
            {
                SetBlocksSpriteLayer(GlobalVariables.orderInLayer_blocks);

                #region snap ettirme yeri
                //hareket ettirilen blok grubunun tamamý grid üzerinde ise snap yapýlýr
                if (touchedBlockParent.childCount == toBePlacedGrids.Count)
                {
                    for (int i = 0; i < touchedBlockParent.childCount; i++)
                    {
                        Vector3 snapPos = toBePlacedGrids[i].position;
                        snapPos.z = 0;
                        touchedBlockParent.GetChild(i).transform.position = snapPos;

                        if (touchedBlockParent.GetChild(i).transform.GetComponent<BlockProperties>().BlockColor == GlobalVariables.blockColorType_BlockA)
                        {
                            toBePlacedGrids[i].GetComponent<GridRowColumnControlHelper>().gridState = GlobalVariables.gridState_blokA;
                        }
                        else if (touchedBlockParent.GetChild(i).transform.GetComponent<BlockProperties>().BlockColor == GlobalVariables.blockColorType_BlockB)
                        {
                            toBePlacedGrids[i].GetComponent<GridRowColumnControlHelper>().gridState = GlobalVariables.gridState_blokB;
                        }

                        toBePlacedGrids[i].GetComponent<GridRowColumnControlHelper>().snapedBlockTile = touchedBlockParent.GetChild(i).transform;
                    }

                    //bir blok spawn noktasýndan alýnýp gride yerleþtirildiðinde spawn noktasýný boþ olarak iþaretliyoruz
                    foreach (Transform item in touchedBlockParent)
                    {
                        item.GetComponent<BlockProperties>().SpawnPoint.GetComponent<SpawnPointHelper>().hasBlocks = false;
                        item.GetComponent<BlockProperties>().isSnapped = true;
                    }

                    transform.GetComponent<CreateBlocks>().CreateRandomBlocks();
                }
                else // deðilse eski doðru konumuna gönderilir
                {
                    touchedBlockParent.position = preCorrectPos;

                    //block spawn noktasýna gönderiliyorsa tekrar küçültülür
                    if (touchedBlockParent.GetComponentInChildren<BlockProperties>().isSnapped == false)
                    {
                        if (blockScaleAnimationStarted)
                        {
                            StopCoroutine(BlockScaleSmoothLerpCoroutine);
                        }
                        BlockScaleSmoothLerpCoroutine = StartCoroutine(BlockScaleSmoothLerp(touchedBlockParent, false));
                    }
                }
                #endregion

                foreach (var item in toBePlacedGrids)
                {
                    item.GetComponent<SpriteRenderer>().color = Color.white;
                }

                touchedBlock = null;
                touchedBlockParent = null;
                deltaPos = Vector3.zero;
                toBePlacedGrids.Clear();
                preCorrectPos = Vector3.zero;

                transform.GetComponent<GridRowCloumnControl>().RowColumnControl();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            touchedBlockParent.rotation = Quaternion.Euler(0, 0, 90 + touchedBlockParent.eulerAngles.z);
            if (touchedBlockParent.rotation.z >= 360)
            {
                touchedBlockParent.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
    #endregion
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
                            raycastHit.collider.transform.GetComponent<SpriteRenderer>().color = Color.gray;
                            tempToBePlacedGrids.Add(raycastHit.collider.transform);
                        }
                    }
                }

                foreach (var item in toBePlacedGrids)
                {
                    if (!tempToBePlacedGrids.Contains(item))
                    {
                        item.GetComponent<SpriteRenderer>().color = Color.white;

                        item.GetComponent<GridRowColumnControlHelper>().gridState = GlobalVariables.gridState_empty;
                        item.GetComponent<GridRowColumnControlHelper>().snapedBlockTile = null;
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

    IEnumerator BlockScaleSmoothLerp(Transform blockParent, bool scaleUp)
    {
        blockScaleAnimationStarted = true;
        if (scaleUp)
        {
            while (blockParent.localScale != new Vector3(1,1,1))
            {
                blockParent.localScale = Vector3.Lerp(blockParent.localScale, new Vector3(1, 1, 1), 0.5f);
                yield return null;
            }
            blockParent.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            while (blockParent.localScale != new Vector3(1, 1, 1))
            {
                blockParent.localScale = Vector3.Lerp(blockParent.localScale, GlobalVariables.scaleSpawnBlocks, 0.5f);
                yield return null;
            }
            blockParent.localScale = GlobalVariables.scaleSpawnBlocks;

        }

        blockScaleAnimationStarted = false;
    }
}
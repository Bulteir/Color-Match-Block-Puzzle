using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTouchControl : MonoBehaviour
{
    public Transform gridForScale;
    public Transform canvasForScale;

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
                MoveSelectedBlocks(Input.mousePosition);
            }
            else
            {
                ControlWithTouch();
            }
        }
    }

    #region dokunma ile kontrol. En son mouse ile kontrolden güncelleþtirilecek
    void ControlWithTouch()
    {
        if (Input.touchCount == 2)
        {
            Touch touch2 = Input.GetTouch(1);

            if (touch2.phase == TouchPhase.Began)
            {

                float angle = (90 + touchedBlockParent.eulerAngles.z) / 90;
                angle = (90 + touchedBlockParent.eulerAngles.z) >= 360 ? 0 : angle;

                this.GetComponent<CreateBlocks>().RotateBlock(touchedBlockParent, (int)angle);

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
                RaycastHit2D raycastHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
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

                        deltaPos = touchedBlockParent.position - Camera.main.ScreenToWorldPoint(touch.position);
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

            if (touch.phase == TouchPhase.Moved)
            {
                MoveSelectedBlocks(touch.position);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                if (touchedBlock != null)
                {

                    float blockTouchPositionDelta = Vector3.Distance(new Vector3(preCorrectPos.x, preCorrectPos.y, touchedBlockParent.position.z), touchedBlockParent.position);
                    //eðer blok spawn notkasýnda olduðu yerde üzerine týklanýrsa döndürme yaptýrmaya yarar
                    if (blockTouchPositionDelta < 3f)
                    {
                        float angle = (90 + touchedBlockParent.eulerAngles.z) / 90;
                        angle = (90 + touchedBlockParent.eulerAngles.z) >= 360 ? 0 : angle;

                        this.GetComponent<CreateBlocks>().RotateBlock(touchedBlockParent, (int)angle);

                        if (touchedBlockParent.rotation.z >= 360)
                        {
                            touchedBlockParent.rotation = Quaternion.Euler(0, 0, 0);
                        }

                        SetBlocksSpriteLayer(GlobalVariables.orderInLayer_blocks);

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

                        foreach (var item in toBePlacedGrids)
                        {
                            item.GetComponent<SpriteRenderer>().color = Color.white;
                            if (item.GetComponent<GridRowColumnControlHelper>().gridState == GlobalVariables.gridState_empty)
                            {
                                item.GetComponent<GridRowColumnControlHelper>().snapedBlockTile = null;
                            }
                        }

                        touchedBlock = null;
                        touchedBlockParent = null;
                        deltaPos = Vector3.zero;
                        toBePlacedGrids.Clear();
                        preCorrectPos = Vector3.zero;
                    }
                    else
                    {
                        SetBlocksSpriteLayer(GlobalVariables.orderInLayer_blocks);

                        #region snap ettirme yeri
                        //hareket ettirilen blok grubunun tamamý grid üzerinde ise snap yapýlýr
                        if (touchedBlockParent.childCount == toBePlacedGrids.Count)
                        {
                            for (int i = 0; i < touchedBlockParent.childCount; i++)
                            {
                                Vector3 snapPos = toBePlacedGrids[i].position;
                                snapPos.z = gridForScale.GetChild(1).position.z - 1;
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

                                //dokunmayý kolaylaþtýrmak için kullanýdýðýmýz collider olmasý gerektiði hale getiriliyor.
                                touchedBlockParent.GetChild(i).GetComponent<BoxCollider2D>().size = new Vector2(10.8f, 10.8f);
                            }

                            //bir blok spawn noktasýndan alýnýp gride yerleþtirildiðinde spawn noktasýný boþ olarak iþaretliyoruz
                            foreach (Transform item in touchedBlockParent)
                            {
                                item.GetComponent<BlockProperties>().SpawnPoint.GetComponent<SpawnPointHelper>().block = null;
                                item.GetComponent<BlockProperties>().isSnapped = true;
                                item.GetComponent<SpriteRenderer>().sortingOrder = 0;
                            }
                            touchedBlockParent.SetParent(canvasForScale);

                            SnappedBlocksPositionAndScaleCorrection();

                            transform.GetComponent<CreateBlocks>().CreateRandomBlocks(true);

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

                        //snap yapýlan griddeki gölgeler kaldýrýlýr
                        foreach (var item in toBePlacedGrids)
                        {
                            item.GetComponent<SpriteRenderer>().color = Color.white;
                        }

                        //herhangi bir blok býrakýldýðýnda satýr/sütünlar kontrol edilir. 
                        transform.GetComponent<GridRowCloumnControl>().RowColumnControl(touchedBlockParent);

                        touchedBlock = null;
                        touchedBlockParent = null;
                        deltaPos = Vector3.zero;
                        toBePlacedGrids.Clear();
                        preCorrectPos = Vector3.zero;
                    }

                }
            }
        }
    }
    #endregion

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
                float blockTouchPositionDelta = Vector3.Distance(new Vector3(preCorrectPos.x,preCorrectPos.y,touchedBlockParent.position.z), touchedBlockParent.position);
                //eðer blok spawn notkasýnda olduðu yerde üzerine týklanýrsa döndürme yaptýrmaya yarar
                if (blockTouchPositionDelta < 3f)
                {
                    float angle = (90 + touchedBlockParent.eulerAngles.z) / 90;
                    angle = (90 + touchedBlockParent.eulerAngles.z) >= 360 ? 0 : angle;

                    this.GetComponent<CreateBlocks>().RotateBlock(touchedBlockParent, (int)angle);

                    if (touchedBlockParent.rotation.z >= 360)
                    {
                        touchedBlockParent.rotation = Quaternion.Euler(0, 0, 0);
                    }

                    SetBlocksSpriteLayer(GlobalVariables.orderInLayer_blocks);

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

                    foreach (var item in toBePlacedGrids)
                    {
                        item.GetComponent<SpriteRenderer>().color = Color.white;
                        if (item.GetComponent<GridRowColumnControlHelper>().gridState == GlobalVariables.gridState_empty)
                        {
                            item.GetComponent<GridRowColumnControlHelper>().snapedBlockTile = null;
                        }
                    }

                    touchedBlock = null;
                    touchedBlockParent = null;
                    deltaPos = Vector3.zero;
                    toBePlacedGrids.Clear();
                    preCorrectPos = Vector3.zero;
                }
                else
                {
                    SetBlocksSpriteLayer(GlobalVariables.orderInLayer_blocks);

                    #region snap ettirme yeri
                    //hareket ettirilen blok grubunun tamamý grid üzerinde ise snap yapýlýr
                    if (touchedBlockParent.childCount == toBePlacedGrids.Count)
                    {
                        for (int i = 0; i < touchedBlockParent.childCount; i++)
                        {
                            Vector3 snapPos = toBePlacedGrids[i].position;
                            snapPos.z = gridForScale.GetChild(1).position.z - 1;
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

                            //dokunmayý kolaylaþtýrmak için kullanýdýðýmýz collider olmasý gerektiði hale getiriliyor.
                            touchedBlockParent.GetChild(i).GetComponent<BoxCollider2D>().size = new Vector2(10.8f, 10.8f);
                        }

                        //bir blok spawn noktasýndan alýnýp gride yerleþtirildiðinde spawn noktasýný boþ olarak iþaretliyoruz
                        foreach (Transform item in touchedBlockParent)
                        {
                            item.GetComponent<BlockProperties>().SpawnPoint.GetComponent<SpawnPointHelper>().block = null;
                            item.GetComponent<BlockProperties>().isSnapped = true;
                            item.GetComponent<SpriteRenderer>().sortingOrder = 0;
                        }
                        touchedBlockParent.SetParent(canvasForScale);
                        
                        SnappedBlocksPositionAndScaleCorrection();

                        transform.GetComponent<CreateBlocks>().CreateRandomBlocks(true);

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

                    //snap yapýlan griddeki gölgeler kaldýrýlýr
                    foreach (var item in toBePlacedGrids)
                    {
                        item.GetComponent<SpriteRenderer>().color = Color.white;
                    }

                    //herhangi bir blok býrakýldýðýnda satýr/sütünlar kontrol edilir. 
                    transform.GetComponent<GridRowCloumnControl>().RowColumnControl(touchedBlockParent);

                    touchedBlock = null;
                    touchedBlockParent = null;
                    deltaPos = Vector3.zero;
                    toBePlacedGrids.Clear();
                    preCorrectPos = Vector3.zero;

                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            float angle = (90 + touchedBlockParent.eulerAngles.z) / 90;
            angle = (90 + touchedBlockParent.eulerAngles.z) >= 360 ? 0 : angle;

            this.GetComponent<CreateBlocks>().RotateBlock(touchedBlockParent, (int)angle);

            if (touchedBlockParent.rotation.z >= 360)
            {
                touchedBlockParent.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
    #endregion
    void MoveSelectedBlocks(Vector3 touchPosition)
    {
        //dokunulan bir bloðun hareket ettirilmesi
        if (touchedBlock != null)
        {
            Vector3 touchPoint = Camera.main.ScreenToWorldPoint(touchPosition);
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
                        if (raycastHit.collider.gameObject.tag == GlobalVariables.gridBlock && raycastHit.collider.gameObject.GetComponent<GridRowColumnControlHelper>().gridState == GlobalVariables.gridState_empty)
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
        blockParent.GetComponent<Animator>().enabled = false;
        blockScaleAnimationStarted = true;
        Vector3 blockScale = Vector3.Scale(gridForScale.localScale, canvasForScale.localScale);
        if (scaleUp)
        {
            while (blockParent.localScale != blockScale)
            {
                blockParent.localScale = Vector3.Lerp(blockParent.localScale, blockScale, 0.5f);
                yield return null;
            }
            blockParent.localScale = blockScale;
        }
        else
        {
            while (blockParent.localScale != Vector3.Scale(blockScale, GlobalVariables.scaleSpawnBlocks))
            {
                blockParent.localScale = Vector3.Lerp(blockParent.localScale, Vector3.Scale(blockScale, GlobalVariables.scaleSpawnBlocks), 0.5f);
                yield return null;
            }
            blockParent.localScale = Vector3.Scale(blockScale, GlobalVariables.scaleSpawnBlocks);

        }
        blockScaleAnimationStarted = false;
    }

    void SnappedBlocksPositionAndScaleCorrection()
    {
        if (blockScaleAnimationStarted)
        {
            StopCoroutine(BlockScaleSmoothLerpCoroutine);
        }
        touchedBlockParent.localScale = gridForScale.localScale;

        for (int i = 0; i < touchedBlockParent.childCount; i++)
        {
            Vector3 snapPos = toBePlacedGrids[i].position;
            snapPos.z = gridForScale.GetChild(1).position.z - 1;
            touchedBlockParent.GetChild(i).transform.position = snapPos;
        }
    }
}
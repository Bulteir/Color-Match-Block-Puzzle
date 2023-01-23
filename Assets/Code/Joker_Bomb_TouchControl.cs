using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Joker_Bomb_TouchControl : MonoBehaviour
{
    public Transform GeneralControl;
    public Transform BombPlaceHolderPos;
    public TMP_Text BombCounter;
    public int MaxBombCount;
    int bombCount = 0;

    Transform touchedBlock;
    Vector3 deltaPos;
    List<Transform> toBePlacedGrids;

    bool adIsReady = false;
    IEnumerator requestAdCorroutine;
    bool requestAdFirstTime = true;//bomba jokerinin oyun ba��na sadece bir kez kullan�lmas� i�in

    // Start is called before the first frame update
    void Start()
    {
        toBePlacedGrids = new List<Transform>();
        BombCounter.text = bombCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVariables.gameState == GlobalVariables.gameState_inGame && bombCount > 0)
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
        else if (GlobalVariables.gameState == GlobalVariables.gameState_inGame && bombCount == 0)
        {
            BombClickForRewardedAd();
        }
    }

    void ControlWithMouse()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (raycastHit.collider != null)
            {
                if (raycastHit.collider.gameObject.tag == GlobalVariables.bomb)
                {
                    touchedBlock = raycastHit.transform;

                    deltaPos = touchedBlock.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    //deltaPos.z = 0;
                    //deltaPos.y += 2;

                    touchedBlock.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (touchedBlock != null)
            {
                #region snap ettirme ve bomba etki alan�ndaki bloklar� patlatma 
                //bomban�n etki alan� tamamen grid �zerinde ise etki alan�ndaki bloklar� patlat
                if (touchedBlock.GetChild(0).childCount == toBePlacedGrids.Count)
                {
                    StartCoroutine(BlockDisapperAnimationDelayer());
                    touchedBlock.position = BombPlaceHolderPos.position;
                    touchedBlock.gameObject.layer = LayerMask.NameToLayer("Default");

                    //bomba patlad��� i�in bomba say�s�n� d���r
                    bombCount--;
                    BombCounter.text = bombCount.ToString();
                }
                else // de�ilse eski do�ru konumuna g�nderilir
                {
                    touchedBlock.position = BombPlaceHolderPos.position;
                    touchedBlock.gameObject.layer = LayerMask.NameToLayer("Default");

                }
                #endregion

                //bombay� b�rak�nca patlamadan prjinal yerine d�n�yorsa griddeki g�lgeler kald�r�l�r
                foreach (var item in toBePlacedGrids)
                {
                    if (item.GetComponent<GridRowColumnControlHelper>().gridState == GlobalVariables.gridState_empty)
                    {
                        item.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    else
                    {
                        item.GetComponent<GridRowColumnControlHelper>().snapedBlockTile.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }

                touchedBlock = null;
                deltaPos = Vector3.zero;
            }
        }
    }
    void MoveSelectedBlocks(Vector3 touchPosition)
    {
        //dokunulan bomban�n hareket ettirilmesi
        if (touchedBlock != null)
        {
            Vector3 touchPoint = Camera.main.ScreenToWorldPoint(touchPosition);

            touchedBlock.position = new Vector3(touchPoint.x, touchPoint.y, 0);
            touchedBlock.position += deltaPos;

            #region bombay� hareket ettirirken etki alan�nda yer alan childlar�n�n merkezinden ray f�rlat�p hangi gridin �st�nde tespit ediyoruz.

            List<Transform> tempToBePlacedGrids = new List<Transform>();
            for (int i = 0; i < touchedBlock.GetChild(0).childCount; i++)
            {
                RaycastHit2D raycastHit = Physics2D.Raycast(touchedBlock.GetChild(0).GetChild(i).position, Vector2.zero);
                if (raycastHit.collider != null)
                {
                    if (raycastHit.collider.gameObject.tag == GlobalVariables.gridBlock)
                    {
                        tempToBePlacedGrids.Add(raycastHit.collider.transform);

                        //gridi gri yap
                        if (raycastHit.collider.gameObject.GetComponent<GridRowColumnControlHelper>().gridState == GlobalVariables.gridState_empty)
                        {
                            raycastHit.collider.transform.GetComponent<SpriteRenderer>().color = Color.gray;
                        }
                        else//gride yerle�tirilmi� blo�u gri yap
                        {
                            raycastHit.collider.gameObject.GetComponent<GridRowColumnControlHelper>().snapedBlockTile.GetComponent<SpriteRenderer>().color = Color.gray;
                        }
                    }
                }
            }

            foreach (var item in toBePlacedGrids)
            {
                if (!tempToBePlacedGrids.Contains(item))
                {
                    if (item.GetComponent<GridRowColumnControlHelper>().gridState == GlobalVariables.gridState_empty)
                    {
                        item.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    else
                    {
                        item.GetComponent<GridRowColumnControlHelper>().snapedBlockTile.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
            }

            toBePlacedGrids.Clear();
            toBePlacedGrids.AddRange(tempToBePlacedGrids);

            #endregion
        }
    }

    void ControlWithTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit2D raycastHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
                if (raycastHit.collider != null)
                {
                    if (raycastHit.collider.gameObject.tag == GlobalVariables.bomb)
                    {
                        touchedBlock = raycastHit.transform;

                        deltaPos = touchedBlock.position - Camera.main.ScreenToWorldPoint(touch.position);
                        //deltaPos.z = 0;
                        //deltaPos.y += 2;

                        touchedBlock.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
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
                    #region snap ettirme ve bomba etki alan�ndaki bloklar� patlatma 
                    //bomban�n etki alan� tamamen grid �zerinde ise etki alan�ndaki bloklar� patlat
                    if (touchedBlock.GetChild(0).childCount == toBePlacedGrids.Count)
                    {
                        StartCoroutine(BlockDisapperAnimationDelayer());
                        touchedBlock.position = BombPlaceHolderPos.position;
                        touchedBlock.gameObject.layer = LayerMask.NameToLayer("Default");

                        //bomba patlad��� i�in bomba say�s�n� d���r
                        bombCount--;
                        BombCounter.text = bombCount.ToString();
                    }
                    else // de�ilse eski do�ru konumuna g�nderilir
                    {
                        touchedBlock.position = BombPlaceHolderPos.position;
                        touchedBlock.gameObject.layer = LayerMask.NameToLayer("Default");
                    }
                    #endregion

                    //bombay� b�rak�nca patlamadan prjinal yerine d�n�yorsa griddeki g�lgeler kald�r�l�r
                    foreach (var item in toBePlacedGrids)
                    {
                        if (item.GetComponent<GridRowColumnControlHelper>().gridState == GlobalVariables.gridState_empty)
                        {
                            item.GetComponent<SpriteRenderer>().color = Color.white;
                        }
                        else
                        {
                            item.GetComponent<GridRowColumnControlHelper>().snapedBlockTile.GetComponent<SpriteRenderer>().color = Color.white;
                        }
                    }

                    touchedBlock = null;
                    deltaPos = Vector3.zero;
                }
            }
        }
    }

    IEnumerator BlockDisapperAnimationDelayer()
    {
        for (int i = 0; i < toBePlacedGrids.Count;)
        {
            yield return new WaitUntil(() => GlobalVariables.gameState == GlobalVariables.gameState_inGame);
            if (toBePlacedGrids[i].GetComponent<GridRowColumnControlHelper>().snapedBlockTile != null)
            {
                toBePlacedGrids[i].GetComponent<GridRowColumnControlHelper>().snapedBlockTile.gameObject.GetComponent<Animator>().enabled = true;
                toBePlacedGrids[i].GetComponent<GridRowColumnControlHelper>().snapedBlockTile = null;
                toBePlacedGrids[i].GetComponent<GridRowColumnControlHelper>().gridState = GlobalVariables.gridState_empty;
            }
            i++;
        }
        touchedBlock = null;
        deltaPos = Vector3.zero;
        toBePlacedGrids.Clear();
    }

    #region Reklam methodlar�
    void BombClickForRewardedAd()
    {
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                RaycastHit2D raycastHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (raycastHit.collider != null)
                {
                    if (raycastHit.collider.gameObject.tag == GlobalVariables.bomb && requestAdFirstTime)
                    {
                        ShowAd();
                    }
                }
            }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    RaycastHit2D raycastHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
                    if (raycastHit.collider != null)
                    {
                        if (raycastHit.collider.gameObject.tag == GlobalVariables.bomb && requestAdFirstTime)
                        {
                            ShowAd();
                        }
                    }
                }
            }
        }
    }

    void ShowAd()
    {
        GlobalVariables.whichJokerRequestRewardAd = GlobalVariables.joker_bomb;
        if (adIsReady)
        {
            adIsReady = false;
            GeneralControl.GetComponent<AdMobController>().ShowRewardedAd();
        }
    }

    public void adLoaded()
    {
        if (GlobalVariables.requestRewardedAd == true)
        {
            adIsReady = true;
        }
    }

    public void RequestRewardAdForFailed()
    {
        if (GlobalVariables.requestRewardedAd == true)
        {
            if (requestAdCorroutine != null)
            {
                StopCoroutine(requestAdCorroutine);
            }
            requestAdCorroutine = RequestRewardAd();
            StartCoroutine(requestAdCorroutine);
        }
    }

    IEnumerator RequestRewardAd()
    {
        yield return new WaitForSeconds(5);
        GlobalVariables.requestRewardedAd = true;
        GeneralControl.GetComponent<AdMobController>().RequestAndLoadInterstitialAd();
    }

    public void SetRewardForAd()
    {
        if (GlobalVariables.whichJokerRequestRewardAd == GlobalVariables.joker_bomb)
        {
            bombCount = MaxBombCount;
            BombCounter.text = bombCount.ToString();
            requestAdFirstTime = false;

            GlobalVariables.whichJokerRequestRewardAd = GlobalVariables.joker_non;
            GlobalVariables.requestRewardedAd = true;//hi� bir yerde false yapm�yoruz!!!
            GeneralControl.GetComponent<AdMobController>().RequestAndLoadRewardedAd();
        }
    }
    #endregion
}

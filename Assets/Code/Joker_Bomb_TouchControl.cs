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
    bool requestAdFirstTime = true;//bomba jokerinin oyun baþýna sadece bir kez kullanýlmasý için

    public AudioSource click;
    public AudioSource bomb;
    // Start is called before the first frame update
    void Start()
    {
        GetJokerCountFromPlayerPrefs();
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
                    click.Play();
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (touchedBlock != null)
            {
                #region snap ettirme ve bomba etki alanýndaki bloklarý patlatma 
                //bombanýn etki alaný tamamen grid üzerinde ise etki alanýndaki bloklarý patlat
                if (touchedBlock.GetChild(0).childCount == toBePlacedGrids.Count)
                {
                    StartCoroutine(BlockDisapperAnimationDelayer());
                    Vector3 _bombPlaceHolderPos = BombPlaceHolderPos.localPosition;
                    _bombPlaceHolderPos.z = 1;
                    touchedBlock.localPosition = _bombPlaceHolderPos;
                   

                    touchedBlock.gameObject.layer = LayerMask.NameToLayer("Default");

                    //bomba patladýðý için bomba sayýsýný düþür
                    bombCount--;
                    BombCounter.text = bombCount.ToString();
                    SetJokerCountFromPlayerPrefs();
                }
                else // deðilse eski doðru konumuna gönderilir
                {
                    Vector3 _bombPlaceHolderPos = BombPlaceHolderPos.localPosition;
                    _bombPlaceHolderPos.z = 1;
                    touchedBlock.localPosition = _bombPlaceHolderPos;
                    touchedBlock.gameObject.layer = LayerMask.NameToLayer("Default");

                }
                #endregion

                //bombayý býrakýnca patlamadan prjinal yerine dönüyorsa griddeki gölgeler kaldýrýlýr
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
        //dokunulan bombanýn hareket ettirilmesi
        if (touchedBlock != null)
        {
            Vector3 touchPoint = Camera.main.ScreenToWorldPoint(touchPosition);

            touchedBlock.position = new Vector3(touchPoint.x, touchPoint.y, 0);
            touchedBlock.position += deltaPos;

            #region bombayý hareket ettirirken etki alanýnda yer alan childlarýnýn merkezinden ray fýrlatýp hangi gridin üstünde tespit ediyoruz.

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
                        else//gride yerleþtirilmiþ bloðu gri yap
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
                        click.Play();

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
                    #region snap ettirme ve bomba etki alanýndaki bloklarý patlatma 
                    //bombanýn etki alaný tamamen grid üzerinde ise etki alanýndaki bloklarý patlat
                    if (touchedBlock.GetChild(0).childCount == toBePlacedGrids.Count)
                    {
                        StartCoroutine(BlockDisapperAnimationDelayer());
                        Vector3 _bombPlaceHolderPos = BombPlaceHolderPos.localPosition;
                        _bombPlaceHolderPos.z = 1;
                        touchedBlock.localPosition = _bombPlaceHolderPos;
                        touchedBlock.gameObject.layer = LayerMask.NameToLayer("Default");

                        //bomba patladýðý için bomba sayýsýný düþür
                        bombCount--;
                        BombCounter.text = bombCount.ToString();
                        SetJokerCountFromPlayerPrefs();
                    }
                    else // deðilse eski doðru konumuna gönderilir
                    {
                        Vector3 _bombPlaceHolderPos = BombPlaceHolderPos.localPosition;
                        _bombPlaceHolderPos.z = 1;
                        touchedBlock.localPosition = _bombPlaceHolderPos;
                        touchedBlock.gameObject.layer = LayerMask.NameToLayer("Default");
                    }
                    #endregion

                    //bombayý býrakýnca patlamadan prjinal yerine dönüyorsa griddeki gölgeler kaldýrýlýr
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
        bomb.Play();
    }

    #region Reklam methodlarý
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
                        click.Play();
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
                            click.Play();
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
            //GeneralControl.GetComponent<AdMobController>().ShowRewardedAd();
            GeneralControl.GetComponent<AdMobRewardedAdController>().ShowAd();
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
        //GeneralControl.GetComponent<AdMobController>().RequestAndLoadRewardedAd();
        GeneralControl.GetComponent<AdMobRewardedAdController>().LoadAd();

    }

    public void SetRewardForAd()
    {
        if (GlobalVariables.whichJokerRequestRewardAd == GlobalVariables.joker_bomb)
        {
            bombCount = MaxBombCount;
            BombCounter.text = bombCount.ToString();
            requestAdFirstTime = false;

            GlobalVariables.whichJokerRequestRewardAd = GlobalVariables.joker_non;
            GlobalVariables.requestRewardedAd = true;//hiç bir yerde false yapmýyoruz!!!
            //GeneralControl.GetComponent<AdMobController>().RequestAndLoadRewardedAd();
            GeneralControl.GetComponent<AdMobRewardedAdController>().LoadAd();
        }
    }
    #endregion
    public void GetJokerCountFromPlayerPrefs()
    {
        try
        {
            string jokerCount_ = PlayerPrefs.GetString("BombJoker");
            if (jokerCount_ != "")
            {
                bombCount = int.Parse(jokerCount_);
            }
        }
        catch
        {

        }

    }
    public void SetJokerCountFromPlayerPrefs()
    {
        try
        {
            PlayerPrefs.SetString("BombJoker", bombCount.ToString());
            PlayerPrefs.Save();
        }
        catch
        {

        }
    }
}

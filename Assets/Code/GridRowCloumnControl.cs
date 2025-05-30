using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridRowCloumnControl : MonoBehaviour
{
    public List<Transform> row1;
    public List<Transform> row2;
    public List<Transform> row3;
    public List<Transform> row4;
    public List<Transform> row5;
    public List<Transform> row6;
    public List<Transform> row7;
    public List<Transform> row8;
    public List<Transform> row9;
    public List<Transform> row10;
    public List<Transform> column1;
    public List<Transform> column2;
    public List<Transform> column3;
    public List<Transform> column4;
    public List<Transform> column5;
    public List<Transform> column6;
    public List<Transform> column7;
    public List<Transform> column8;
    public List<Transform> column9;
    public List<Transform> column10;

    List<List<Transform>> rows;
    List<List<Transform>> columns;
    Vector2[] rowsCounter;
    Vector2[] columnsCounter;

    public TMP_Text score_Prefab;
    public TMP_Text scoreText;
    public Transform canvas;
    public AudioSource pop;

    // Start is called before the first frame update
    void Start()
    {
        rows = new List<List<Transform>>();
        columns = new List<List<Transform>>();
        rowsCounter = new Vector2[10];
        columnsCounter = new Vector2[10];

        rows.Add(row1);
        rows.Add(row2);
        rows.Add(row3);
        rows.Add(row4);
        rows.Add(row5);
        rows.Add(row6);
        rows.Add(row7);
        rows.Add(row8);
        rows.Add(row9);
        rows.Add(row10);
        columns.Add(column1);
        columns.Add(column2);
        columns.Add(column3);
        columns.Add(column4);
        columns.Add(column5);
        columns.Add(column6);
        columns.Add(column7);
        columns.Add(column8);
        columns.Add(column9);
        columns.Add(column10);
    }

    //BlockTouchControl'den �a�r�l�yor. Herhangi bir blok b�rak�ld���nda �a�r�l�yor
    public void RowColumnControl(Transform grid)
    {
        int sameTimeCompletedRowCount = 0;
        int sameTimeCompletedColumnCount = 0;

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (rows[i][j].GetComponent<GridRowColumnControlHelper>().gridState == GlobalVariables.gridState_blokA)
                {
                    rowsCounter[i].x++;
                }
                else if ((rows[i])[j].GetComponent<GridRowColumnControlHelper>().gridState == GlobalVariables.gridState_blokB)
                {
                    rowsCounter[i].y++;
                }

                if (columns[i][j].GetComponent<GridRowColumnControlHelper>().gridState == GlobalVariables.gridState_blokA)
                {
                    columnsCounter[i].x++;
                }
                else if ((columns[i])[j].GetComponent<GridRowColumnControlHelper>().gridState == GlobalVariables.gridState_blokB)
                {
                    columnsCounter[i].y++;
                }
            }
        }

        for (int i = 0; i < 10; i++)
        {
            if (rowsCounter[i].x == 10 || rowsCounter[i].y == 10)
            {
                sameTimeCompletedRowCount++;
                StartCoroutine(BlockDisapperAnimationDelayer(rows[i]));
            }

            if (columnsCounter[i].x == 10 || columnsCounter[i].y == 10)
            {
                sameTimeCompletedColumnCount++;
                StartCoroutine(BlockDisapperAnimationDelayer(columns[i]));
            }
        }

        if (sameTimeCompletedColumnCount > 0 || sameTimeCompletedRowCount > 0)
        {
            CreateScore(grid, GlobalVariables.baseScore * (sameTimeCompletedColumnCount + sameTimeCompletedRowCount) * this.GetComponent<ComboBarControl>().comboMultiplier);
            this.GetComponent<ComboBarControl>().FillTheBar();
        }

        //counter'lar� s�f�rl�yoruz
        for (int i = 0; i < 10; i++)
        {
            rowsCounter[i] = Vector2.zero;
            columnsCounter[i] = Vector2.zero;
        }
    }

    void CreateScore(Transform grid, int score)
    {
        GameObject scoreLabel = GameObject.Instantiate(score_Prefab.gameObject);
        scoreLabel.transform.SetParent(canvas);
        scoreLabel.transform.SetAsFirstSibling();

        scoreLabel.transform.localScale = new Vector3(1, 1, 1);
        Vector3 score_PrefabPos = grid.position;
        score_PrefabPos.z = 0;
        scoreLabel.transform.position = score_PrefabPos;

        scoreLabel.GetComponent<TMP_Text>().text = score.ToString();
        scoreLabel.SetActive(true);
        scoreLabel.GetComponent<ScoreAnimation>().startScoreAnimation();
    }

    //yeni bloklar spawn edildikten sonra �a�r�l�r ve spawn edilen bloklar�n grid �zerinde yerle�ririlecek yeri olup olmad���n� kontrol eder.
    public bool IsGameOver(Transform spawnedBlock)
    {
        bool isThereAvaiblePlace = false;
        for (int i = 0; i < 10 && isThereAvaiblePlace == false; i++)
        {
            for (int j = 0; j < 10 && isThereAvaiblePlace == false; j++)
            {
                if (IsGameOverHelper(spawnedBlock, i, j, 0))
                {
                    isThereAvaiblePlace = true;
                }

                if (IsGameOverHelper(spawnedBlock, i, j, 90))
                {
                    isThereAvaiblePlace = true;
                }

                if (IsGameOverHelper(spawnedBlock, i, j, 180))
                {
                    isThereAvaiblePlace = true;
                }

                if (IsGameOverHelper(spawnedBlock, i, j, 270))
                {
                    isThereAvaiblePlace = true;
                }
            }
        }
        return !isThereAvaiblePlace;
    }

    bool IsGameOverHelper(Transform spawnedBlock, int i, int j, int coordinate)
    {
        bool isThereAvaiblePlace = false;
        int availableGrids = 0;
        foreach (Transform cell in spawnedBlock)
        {
            int rowX = 0;
            int rowY = 0;

            if (coordinate == 0)
            {
                rowX = j + (int)cell.GetComponent<BlockProperties>().coordinate.x;
                rowY = i + (int)cell.GetComponent<BlockProperties>().coordinate.y;
            }
            else if (coordinate == 90)
            {
                rowX = j + (int)cell.GetComponent<BlockProperties>().coordinate90.x;
                rowY = i + (int)cell.GetComponent<BlockProperties>().coordinate90.y;
            }
            else if (coordinate == 180)
            {
                rowX = j + (int)cell.GetComponent<BlockProperties>().coordinate180.x;
                rowY = i + (int)cell.GetComponent<BlockProperties>().coordinate180.y;
            }
            else if (coordinate == 270)
            {
                rowX = j + (int)cell.GetComponent<BlockProperties>().coordinate270.x;
                rowY = i + (int)cell.GetComponent<BlockProperties>().coordinate270.y;
            }

            //grid s�n�rlar� i�inde
            if (rowX >= 0 && rowX < 10 && rowY >= 0 && rowY < 10)
            {
                if (rows[rowY][rowX].GetComponent<GridRowColumnControlHelper>().gridState == GlobalVariables.gridState_empty)
                {
                    availableGrids++;
                }
            }
        }

        //blo�un t�m h�creleri yerle�tirilebilir.
        if (availableGrids == spawnedBlock.childCount)
        {
            isThereAvaiblePlace = true;
        }
        return isThereAvaiblePlace;
    }

    IEnumerator BlockDisapperAnimationDelayer(List<Transform> blocks)
    {
        for (int i = 0; i < blocks.Count;)
        {
            yield return new WaitUntil(() => GlobalVariables.gameState == GlobalVariables.gameState_inGame);
            if (blocks[i].GetComponent<GridRowColumnControlHelper>().snapedBlockTile != null)
            {
                blocks[i].GetComponent<GridRowColumnControlHelper>().snapedBlockTile.gameObject.GetComponent<Animator>().enabled = true;
                blocks[i].GetComponent<GridRowColumnControlHelper>().snapedBlockTile = null;
                blocks[i].GetComponent<GridRowColumnControlHelper>().gridState = GlobalVariables.gridState_empty;
                pop.Play();
                yield return new WaitForSeconds(0.05f);
            }
            i++;
        }
    }
}

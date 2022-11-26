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

    // Update is called once per frame
    void Update()
    {
 
    }

    //BlockTouchControl'den çaðrýlýyor. Herhangi bir blok býrakýldýðýnda çaðrýlýyor
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
                foreach (var item in rows[i])
                {
                    if (item.GetComponent<GridRowColumnControlHelper>().snapedBlockTile != null)
                    {
                        if (item.GetComponent<GridRowColumnControlHelper>().snapedBlockTile.gameObject)
                        {
                            Destroy(item.GetComponent<GridRowColumnControlHelper>().snapedBlockTile.gameObject);
                        }
                        item.GetComponent<GridRowColumnControlHelper>().snapedBlockTile = null;
                        item.GetComponent<GridRowColumnControlHelper>().gridState = GlobalVariables.gridState_empty;
                    }
                }
            }

            if (columnsCounter[i].x == 10 || columnsCounter[i].y == 10)
            {
                sameTimeCompletedColumnCount++;
                foreach (var item in columns[i])
                {

                    if (item.GetComponent<GridRowColumnControlHelper>().snapedBlockTile != null)
                    {
                        Destroy(item.GetComponent<GridRowColumnControlHelper>().snapedBlockTile.gameObject);
                    }
                    item.GetComponent<GridRowColumnControlHelper>().snapedBlockTile = null;
                    item.GetComponent<GridRowColumnControlHelper>().gridState = GlobalVariables.gridState_empty;
                }
            }
        }

        if (sameTimeCompletedColumnCount > 0 || sameTimeCompletedRowCount > 0)
        {
            CreateScore(grid, GlobalVariables.baseScore * (sameTimeCompletedColumnCount + sameTimeCompletedRowCount));
        }

        //counter'larý sýfýrlýyoruz
        for (int i = 0; i < 10; i++)
        {
            rowsCounter[i] = Vector2.zero;
            columnsCounter[i] = Vector2.zero;
        }
    }

    void CreateScore(Transform grid,int score)
    {
        GameObject scoreLabel = GameObject.Instantiate(score_Prefab.gameObject);
        scoreLabel.transform.SetParent(canvas);
        scoreLabel.transform.localScale = new Vector3(1, 1, 1);
        Vector3 score_PrefabPos = grid.position;
        score_PrefabPos.z = 0;
        scoreLabel.transform.position = score_PrefabPos;

        scoreLabel.GetComponent<TMP_Text>().text = score.ToString();
        scoreLabel.SetActive(true);
        StartCoroutine(scoreAnimation(scoreLabel));
    }

    IEnumerator scoreAnimation(GameObject score)
    {
        while (score.transform.localScale.x < 3f)
        {
            score.transform.localScale = Vector3.Lerp(score.transform.localScale, new Vector3(3, 3, 1), 10f * Time.deltaTime );
            if (score.transform.localScale.x > 2.99f)
            {
                score.transform.localScale = new Vector3(3, 3, 1);
            }
            yield return null;
        }

        float velocity = 0.2f;
        while (Vector3.Distance(score.transform.position, scoreText.transform.position) > 5f)
        {
            score.transform.position = Vector3.Lerp(score.transform.position, scoreText.transform.position, velocity * 0.1f * Time.deltaTime);
            velocity = velocity * 1.05f;
            yield return null;
        }

        int totalScore = int.Parse(scoreText.text);
        totalScore += int.Parse(score.GetComponent<TMP_Text>().text);
        scoreText.text = totalScore.ToString();

        GameObject.Destroy(score);
    }
}

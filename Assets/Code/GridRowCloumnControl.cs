using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //BlockTouchControl'den çaðrýlýyor. Bloklar snapped olduðunda çaðrýlýyor
    public void RowColumnControl()
    {
        //vector2.x=blockA counter vector2.y=blockB counter 
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
            if (rowsCounter[i].x==10 || rowsCounter[i].y == 10)
            {
                foreach (var item in rows[i])
                {
                    Destroy(item.GetComponent<GridRowColumnControlHelper>().snapedBlockTile.gameObject); 
                }
            }

            if (columnsCounter[i].x == 10 || columnsCounter[i].y == 10)
            {
                foreach (var item in columns[i])
                {
                    Destroy(item.GetComponent<GridRowColumnControlHelper>().snapedBlockTile.gameObject);
                }
            }
        }

        //counter'larý sýfýrlýyoruz
        for (int i = 0; i < 10; i++)
        {
            rowsCounter[i] = Vector2.zero;
            columnsCounter[i] = Vector2.zero;
        }

    }
}

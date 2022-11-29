using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBlocks : MonoBehaviour
{
    public List<Sprite> blockSprites;
    public List<Transform> blocks;
    public List<Transform> SpawnPoints;
    public Transform CreatedBlocksGroup;
    public Transform gridForScale;
    public Transform canvasForScale;
    // Start is called before the first frame update
    void Start()
    {
        CreateRandomBlocks(false);
    }

    public void CreateRandomBlocks(bool playSpawnAnimation)
    {
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            if (SpawnPoints[i].GetComponent<SpawnPointHelper>().block == null)
            {
                createRandomBlock(SpawnPoints[i], playSpawnAnimation);
            }
        }
    }

    //rastgele renkte rastgele bir k�p olu�turur. Blok par�alar�n�n orta noktas�n� bulur ve spawn noktas�na yerle�tirir.
    void createRandomBlock(Transform spawnPoint,bool playSpawnAnimation)
    {
        //int randomBlockType = Random.Range(0, blocks.Count);
        //int randomBlockColor = Random.Range(0, 2);
        //int randomBlockAngle = Random.Range(0, 4);

        //g�rsel test i�in
        int randomBlockType = 5;
        int randomBlockColor = 0;
        int randomBlockAngle = 0;


        GameObject randomBlock = GameObject.Instantiate(blocks[randomBlockType].gameObject);

        Vector3 sumVector = new Vector3(0f, 0f, 0f);
        List<Transform> children = new List<Transform>();
        Transform blockGroupParent = randomBlock.transform;

        foreach (Transform item in blockGroupParent)
        {
            children.Add(item);
            sumVector += item.position;
            item.GetComponent<SpriteRenderer>().sprite = blockSprites[randomBlockColor];
            if (randomBlockColor == GlobalVariables.blockColorType_BlockA)
            {
                item.GetComponent<BlockProperties>().BlockColor = GlobalVariables.blockColorType_BlockA;
            }
            else if (randomBlockColor == GlobalVariables.blockColorType_BlockB)
            {
                item.GetComponent<BlockProperties>().BlockColor = GlobalVariables.blockColorType_BlockB;
            }
        }
        Vector3 blockGroupCenter = sumVector / randomBlock.transform.childCount;

        foreach (Transform item in children)
        {
            item.parent = null;
        }

        blockGroupParent.position = blockGroupCenter;

        foreach (Transform item in children)
        {
            item.parent = blockGroupParent;
            item.GetComponent<BlockProperties>().SpawnPoint = spawnPoint;
        }

        blockGroupParent.position = spawnPoint.position;
        blockGroupParent.rotation = Quaternion.Euler(0, 0, 90 * randomBlockAngle);
        Vector3 blockScale = Vector3.Scale(gridForScale.localScale, canvasForScale.localScale);
        blockScale = Vector3.Scale(blockScale, GlobalVariables.scaleSpawnBlocks);
        blockGroupParent.localScale = blockScale;
        spawnPoint.GetComponent<SpawnPointHelper>().block = blockGroupParent;
        blockGroupParent.GetComponent<Animator>().enabled = playSpawnAnimation;

        #region oyun biti�ini anlamak i�in spawn olmu� olan bloklar�n t�m�m�n grid �zerinde uygun yeri var m� kontrol ediliyor
        int totalAvaiblePlace = 0;
        for (int i = 0; i < 3; i++)
        {
            if (SpawnPoints[i].GetComponent<SpawnPointHelper>().block && this.GetComponent<GridRowCloumnControl>().IsGameOver(SpawnPoints[i].GetComponent<SpawnPointHelper>().block))
            {
                totalAvaiblePlace++;
            }
        }

        if (totalAvaiblePlace == 3)
        {
            Debug.Log("oyun bitti.");
        }

        #endregion
    }
}

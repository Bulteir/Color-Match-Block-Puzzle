using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBlocks : MonoBehaviour
{
    public List<Sprite> blockSprites;
    public List<Transform> blocks;
    public List<Transform> SpawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        CreateRandomBlocks();
    }

    public void CreateRandomBlocks()
    {
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            if (!SpawnPoints[i].GetComponent<SpawnPointHelper>().hasBlocks)
            {
                createRandomBlock(SpawnPoints[i]);
            }
        }
    }

    //rastgele renkte rastgele bir k�p olu�turur. Blok par�alar�n�n orta noktas�n� bulur ve spawn noktas�na yerle�tirir.
    void createRandomBlock(Transform spawnPoint)
    {
        int randomBlockType = Random.Range(0, blocks.Count);
        int randomBlockColor = Random.Range(0, 2);
        int randomBlockAngle = Random.Range(0, 4);

        GameObject randomBlock =  GameObject.Instantiate(blocks[randomBlockType].gameObject);
        
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
        spawnPoint.GetComponent<SpawnPointHelper>().hasBlocks = true;
    }
}

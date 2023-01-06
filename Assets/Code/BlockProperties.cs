using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockProperties : MonoBehaviour
{
    public int BlockColor;
    public Transform SpawnPoint;
    public bool isSnapped = false;

    /*game over kontrolü için.
    Her blok hayali bir 5'e 5 grid üzerinde yerleþmiþcesine düþünülerek her bir blok hücresinin bu 5'e 5 lik gridde hangi hücrede olduðu iþlenecek.
    x yatay, y dikey'i temsil ediyor*/
    public Vector2 coordinate;
    public Vector2 coordinate90;
    public Vector2 coordinate180;
    public Vector2 coordinate270;

    public void DestroyBlock()
    {
        Destroy(gameObject);
    }
}

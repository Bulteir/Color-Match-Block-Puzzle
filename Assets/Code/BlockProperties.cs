using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockProperties : MonoBehaviour
{
    public int BlockColor;
    public Transform SpawnPoint;
    public bool isSnapped = false;

    /*game over kontrol� i�in.
    Her blok hayali bir 5'e 5 grid �zerinde yerle�mi�cesine d���n�lerek her bir blok h�cresinin bu 5'e 5 lik gridde hangi h�crede oldu�u i�lenecek.
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

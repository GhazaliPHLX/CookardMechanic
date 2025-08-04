using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingManager
{
    private static int currentSortingOrder = 0;
    private static float zOffset = -0.01f;

    public static void BringToFront(GameObject card)
    {
        var sr = card.GetComponent<SpriteRenderer>();
        if(sr != null)
        {
            currentSortingOrder++;
            sr.sortingOrder = currentSortingOrder;

            Vector3 position = card.transform.position;
            position.z = currentSortingOrder * zOffset;
            card.transform.position = position;
        }

    }
}

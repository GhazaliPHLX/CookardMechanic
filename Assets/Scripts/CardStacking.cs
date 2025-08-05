using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStacking 
{
    public static GameObject SortTopCard(Vector3 cardCenter, Vector3 halfExtents, GameObject self)
    {
        Collider[] hits =  Physics.OverlapBox(cardCenter, halfExtents);

        GameObject topCard = null;
        int highestOrder = int.MinValue;

        foreach (var hit in hits)
        {
            if(hit.gameObject == self) continue;
            var sr = hit.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sortingOrder > highestOrder)
            {
                Debug.Log("another Card Found");
                highestOrder = sr.sortingOrder;
                topCard = hit.gameObject;
            }
        }
        return topCard;
    }
}

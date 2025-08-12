using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStacking 
{
    public static GameObject SortTopCard(Vector3 cardCenter, Vector3 halfExtents, GameObject self)
    {
        Vector3 downOffset = new Vector3(0, 0, 0.1f);
        Collider[] hits =  Physics.OverlapBox(cardCenter + downOffset, halfExtents);

        GameObject topCard = null;
        int highestOrder = int.MinValue;

        foreach (var hit in hits)
        {
            if (hit.transform == self.transform || hit.transform.IsChildOf(self.transform) || self.transform.IsChildOf(hit.transform))
                continue;

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

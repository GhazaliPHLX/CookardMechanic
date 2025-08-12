using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingManager
{
    private static int currentSortingOrder = 0;
    private static float zOffset = -0.01f;

    private static Dictionary<GameObject, (int sortingOrder, float zPos)> originalData
        = new Dictionary<GameObject, (int, float)>();

    public static void BringToFront(GameObject card)
    {
        // Ambil semua SpriteRenderer dari card & child-nya
        var renderers = card.GetComponentsInChildren<SpriteRenderer>();

        foreach (var sr in renderers)
        {
            if (!originalData.ContainsKey(sr.gameObject))
            {
                originalData[sr.gameObject] = (sr.sortingOrder, sr.transform.position.z);
            }

            currentSortingOrder++;
            sr.sortingOrder = currentSortingOrder;

            Vector3 pos = sr.transform.position;
            pos.z = currentSortingOrder * zOffset;
            sr.transform.position = pos;
        }
    }


    public static void ReleasePosition(GameObject card)
    {
        var renderers = card.GetComponentsInChildren<SpriteRenderer>();

        foreach (var sr in renderers)
        {
            if (originalData.TryGetValue(sr.gameObject, out var data))
            {
                sr.sortingOrder = data.sortingOrder;

                Vector3 pos = sr.transform.position;
                pos.z = data.zPos;
                sr.transform.position = pos;

                originalData.Remove(sr.gameObject);
            }
        }
    }

}

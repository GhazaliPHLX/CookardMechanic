using System.Collections.Generic;
using UnityEngine;

public static class SortingManager
{
    private static int currentSortingOrder = 0;
    private static Dictionary<GameObject, int> originalOrders = new Dictionary<GameObject, int>();

    public static void BringToFront(IEnumerable<GameObject> cards)
    {
        foreach (var c in cards)
        {
            if (c == null) continue;
            var renderers = c.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (var sr in renderers)
            {
                if (!originalOrders.ContainsKey(sr.gameObject))
                    originalOrders[sr.gameObject] = sr.sortingOrder;

                currentSortingOrder++;
                sr.sortingOrder = currentSortingOrder;
            }
        }
    }

    public static void BringToFront(GameObject card)
    {
        BringToFront(new[] { card });
    }

    public static void ReleasePosition(GameObject card)
    {
        if (card == null) return;
        var renderers = card.GetComponentsInChildren<SpriteRenderer>(true);
        foreach (var sr in renderers)
        {
            if (originalOrders.TryGetValue(sr.gameObject, out var ord))
            {
                sr.sortingOrder = ord;
                originalOrders.Remove(sr.gameObject);
            }
        }

        if (originalOrders.Count == 0)
            currentSortingOrder = 0;
    }
}

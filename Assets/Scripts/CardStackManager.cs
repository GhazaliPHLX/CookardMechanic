using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class CardStackingManager : MonoBehaviour
{
    public static CardStackingManager Instance;

    [Tooltip("Name of layer used for card objects")]
    public string cardLayerName = "Card";

    [Tooltip("Vertical spacing between stacked cards")]
    public float stackSpacing = 0.2f;

    [Tooltip("Half extents for overlap query (x,y,z)")]
    public Vector3 queryHalfExtents = new Vector3(0.4f, 0.25f, 0.1f);

    private int cardMask;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); return; }
        Instance = this;

        cardMask = LayerMask.GetMask(cardLayerName);
        if (cardMask == 0)
        {
            Debug.LogWarning("[CardStackingManager] Layer '" + cardLayerName + "' not found. Overlap will search all layers.");
            cardMask = ~0;
        }
    }

    // find topmost CardComponent under position 'pos' excluding any in exclude set
    public CardComponent FindTopCardUnderPosition(Vector3 pos, HashSet<CardComponent> exclude = null)
    {
        Collider[] hits = Physics.OverlapBox(pos, queryHalfExtents, Quaternion.identity, cardMask);

        CardComponent best = null;
        int highestOrder = int.MinValue;

        foreach (var h in hits)
        {
            if (h == null) continue;
            var cc = h.GetComponentInParent<CardComponent>();
            if (cc == null) continue;
            if (exclude != null && exclude.Contains(cc)) continue;

            var sr = cc.GetComponent<SpriteRenderer>();
            if (sr == null) continue;

            if (sr.sortingOrder > highestOrder)
            {
                highestOrder = sr.sortingOrder;
                best = cc;
            }
        }
        return best;
    }

    // bottomComp = bottom-most CardComponent of the moving stack (the one user grabbed)
    public bool TryStackOnto(CardComponent bottomComp)
    {
        if (bottomComp == null) return false;
        var moving = bottomComp.GetStackAbove();
        var exclude = new HashSet<CardComponent>(moving);

        CardComponent found = FindTopCardUnderPosition(bottomComp.transform.position, exclude);
        if (found == null) return false;

        CardComponent topTarget = found.GetTop();

        // attach bottom to topTarget
        bottomComp.AttachBelow(topTarget);

        // snap visuals
        bottomComp.SnapOnTopOf(topTarget, stackSpacing);

        // bring visuals to front
        var movedObjects = new List<GameObject>();
        foreach (var m in moving) movedObjects.Add(m.gameObject);
        SortingManager.BringToFront(movedObjects);

        return true;
    }
}

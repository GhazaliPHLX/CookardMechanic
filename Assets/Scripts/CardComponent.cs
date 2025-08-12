using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CardComponent : MonoBehaviour
{
    public CardComponent cardAbove;
    public CardComponent cardBelow;

    private static Transform cardsContainer;

    private void Awake()
    {
        if (cardsContainer == null)
        {
            var obj = GameObject.Find("CardsContainer");
            if (obj != null) cardsContainer = obj.transform;
        }
    }

    // Return list from this (bottom) up to top inclusive: [this, above, above.above, ...]
    public List<CardComponent> GetStackAbove()
    {
        var list = new List<CardComponent>();
        CardComponent cur = this;
        while (cur != null)
        {
            list.Add(cur);
            cur = cur.cardAbove;
        }
        return list;
    }

    // Return topmost node of this stack
    public CardComponent GetTop()
    {
        CardComponent cur = this;
        while (cur != null && cur.cardAbove != null) cur = cur.cardAbove;
        return cur;
    }

    public void DetachFromBelow()
    {
        if (cardBelow != null)
        {
            cardBelow.cardAbove = null;
            cardBelow = null;
        }
    }

    public void DetachAbove()
    {
        if (cardAbove != null)
        {
            cardAbove.cardBelow = null;
            cardAbove = null;
        }
    }

    // attach this (bottom) above 'below' (so this.cardBelow = below; below.cardAbove = this)
    public void AttachBelow(CardComponent below)
    {
        DetachFromBelow();
        cardBelow = below;
        if (below != null) below.cardAbove = this;
    }

    // Snap moved stack visually so bottom (this) sits just below targetTop
    public void SnapOnTopOf(CardComponent targetTop, float spacing)
    {
        var moved = GetStackAbove(); // this, this.cardAbove, ...
        for (int i = 0; i < moved.Count; i++)
        {
            var node = moved[i];
            Vector3 pos = targetTop.transform.position + Vector3.down * spacing * (i + 1);
            pos.z = node.transform.position.z; // preserve z
            node.transform.position = pos;
        }
    }

    public void EnsureInContainer()
    {
        if (cardsContainer != null)
            transform.SetParent(cardsContainer, true);
    }
}

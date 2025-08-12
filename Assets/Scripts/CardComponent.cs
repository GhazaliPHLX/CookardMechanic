using System.Collections.Generic;
using UnityEngine;

public class CardComponent : MonoBehaviour
{
    public CardComponent parentCard;
    public List<CardComponent> stackedCards = new List<CardComponent>();

    private static Transform cardsContainer;

    private void Awake()
    {
        if (cardsContainer == null)
        {
            var obj = GameObject.Find("CardsContainer");
            if (obj != null) cardsContainer = obj.transform;
        }
    }

    public void StackOnto(CardComponent target)
    {
        Debug.Log($"Stacked {name} onto {target.name}");

        parentCard = target;
        target.stackedCards.Add(this);

        transform.SetParent(cardsContainer); // tetap di container global
        transform.position = target.transform.position + new Vector3(0, -0.2f, 0);

        SortingManager.BringToFront(gameObject);
    }

    public void ReleaseCard()
    {
        if (parentCard != null)
        {
            parentCard.stackedCards.Remove(this);
            parentCard = null;
        }
    }
}

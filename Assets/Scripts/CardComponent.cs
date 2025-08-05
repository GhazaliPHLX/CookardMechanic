using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardComponent : MonoBehaviour
{
    public CardComponent parentCard;

    public void StackOnto(CardComponent target)
    {
        Debug.Log("Stacked Card");
        parentCard = target;

        transform.SetParent(target.transform);

        transform.localPosition = new Vector3(0, -0.2f, 0);

        SortingManager.BringToFront(gameObject);
    }
}

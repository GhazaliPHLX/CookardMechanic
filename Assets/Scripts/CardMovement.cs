using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardMovement : MonoBehaviour, ILeftClick
{
    public InputActionReference PointerPos;

    private Transform cardPos;

    private bool isDragging = false;
    private Vector3 mouseOffset;
    private Vector2 MousePosition;
    private SpriteRenderer card;
    public void Clicked()
    {
        isDragging = true;

        SortingManager.BringToFront(gameObject);

        Vector2 MousePosition = PointerPos.action.ReadValue<Vector2>();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(MousePosition);
        worldPosition.z = 0;
        if(cardPos == null)
        {
            cardPos = transform;
        }
        mouseOffset = worldPosition - cardPos.position;
    }

    public void Released()
    {
        if (isDragging)
        {
            // Ngurus Stacking, bisa di refactor ntar klo sempet
            Vector3 cardPos = transform.position;
            Vector3 cardSize = card.bounds.extents;

            GameObject topCard =  CardStacking.SortTopCard(cardPos, cardSize, gameObject);
            

            if (topCard != null && topCard != this.gameObject)
            {
                Debug.Log("TopCard found, otw Stacking");
                var targetCard = topCard.GetComponent<CardComponent>();
                var thisCard = GetComponent<CardComponent>();

                thisCard.StackOnto(targetCard);
            }

            if(topCard == null)
            {
                var thisCard = GetComponent<CardComponent>();
                thisCard.ReleaseCard();
                SortingManager.ReleasePosition(gameObject);

            }


            isDragging = false;
        }
    }

        void Start()
    {
        cardPos = GetComponent<Transform>();
        card = GetComponent<SpriteRenderer>();
        
    }

    void Update()
    {
        if (isDragging)
        {
            Vector2 MousePosition = PointerPos.action.ReadValue<Vector2>();
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(MousePosition);
            worldPosition.z = 0;

            cardPos.position = worldPosition - mouseOffset;
        }
    }
}

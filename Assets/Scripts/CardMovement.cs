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
    public void Clicked()
    {
        Debug.Log("LeftClicked Card");
        isDragging = true;

        Vector2 MousePosition = PointerPos.action.ReadValue<Vector2>();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(MousePosition);
        worldPosition.z = 0;
        mouseOffset = worldPosition - cardPos.position;
    }

    public void Released()
    {
        if (isDragging)
        {
            isDragging = false;
        }
    }

        void Start()
    {
        cardPos = GetComponent<Transform>();
        
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

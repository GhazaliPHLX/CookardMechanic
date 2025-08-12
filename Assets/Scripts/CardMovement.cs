using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CardComponent))]
public class CardMovement : MonoBehaviour, ILeftClick, IRightClick
{
    private Camera mainCam;
    private bool isDragging = false;
    private bool isMultiDrag = false;
    private Vector3 offset;
    private List<CardComponent> movingStack = new List<CardComponent>();
    private CardComponent selfComp;

    private void Start()
    {
        mainCam = Camera.main;
        selfComp = GetComponent<CardComponent>();
        if (selfComp == null)
            Debug.LogError("[CardMovement] CardComponent missing on " + gameObject.name);
    }

    // LEFT: multi-card drag
    public void Clicked()
    {
        if (selfComp == null) return;

        movingStack = selfComp.GetStackAbove();
        if (movingStack.Count == 0) return;

        // detach bottom from below so it becomes independent while dragging
        selfComp.DetachFromBelow();

        // compute offset from mouse to bottom card position
        Vector3 mouseWorld = GetMouseWorldPosition();
        offset = transform.position - mouseWorld;

        isDragging = true;
        isMultiDrag = true;

        // visuals: bring stack front
        var gos = new List<GameObject>();
        foreach (var c in movingStack) gos.Add(c.gameObject);
        SortingManager.BringToFront(gos);
    }

    public void Released()
    {
        if (!isDragging) return;
        isDragging = false;

        if (movingStack.Count > 0)
        {
            bool didStack = CardStackingManager.Instance != null && CardStackingManager.Instance.TryStackOnto(movingStack[0]);
            if (!didStack)
            {
                // keep where released; ensure container parent
                foreach (var c in movingStack) c.EnsureInContainer();
            }
        }
        movingStack.Clear();
    }

    // RIGHT: single-card drag, re-link top to below if middle
    public void RightClicked()
    {
        if (selfComp == null) return;

        // if in middle, re-link above to below
        if (selfComp.cardAbove != null && selfComp.cardBelow != null)
        {
            var above = selfComp.cardAbove;
            var below = selfComp.cardBelow;

            // detach this from both sides
            selfComp.DetachFromBelow();
            selfComp.DetachAbove();

            // attach above's bottom to below
            above.AttachBelow(below);
        }
        else
        {
            // simply detach so it can be moved
            selfComp.DetachFromBelow();
            selfComp.DetachAbove();
        }

        movingStack.Clear();
        movingStack.Add(selfComp);

        Vector3 mouseWorld = GetMouseWorldPosition();
        offset = transform.position - mouseWorld;

        isDragging = true;
        isMultiDrag = false;

        SortingManager.BringToFront(new[] { gameObject });
    }

    public void RightReleased()
    {
        if (!isDragging) return;
        isDragging = false;

        if (movingStack.Count > 0)
        {
            bool didStack = CardStackingManager.Instance != null && CardStackingManager.Instance.TryStackOnto(movingStack[0]);
            if (!didStack) foreach (var c in movingStack) c.EnsureInContainer();
        }
        movingStack.Clear();
    }

    private void Update()
    {
        if (!isDragging) return;

        Vector3 mouseWorld = GetMouseWorldPosition();
        Vector3 basePos = mouseWorld + offset;

        for (int i = 0; i < movingStack.Count; i++)
        {
            var node = movingStack[i];
            Vector3 pos = basePos + Vector3.down * (CardStackingManager.Instance != null ? CardStackingManager.Instance.stackSpacing * i : 0.2f * i);
            pos.z = node.transform.position.z; // preserve z-plane
            node.transform.position = pos;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Use InputSystem mouse safely
        Vector2 mouseScreen = Vector2.zero;
        try
        {
            if (Mouse.current != null) mouseScreen = Mouse.current.position.ReadValue();
            else mouseScreen = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        catch
        {
            mouseScreen = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        // For orthographic camera or top-down, compute world point at card z
        float zDist = Mathf.Abs(mainCam.transform.position.z - transform.position.z);
        Vector3 screen = new Vector3(mouseScreen.x, mouseScreen.y, zDist);
        Vector3 world = mainCam.ScreenToWorldPoint(screen);
        world.z = transform.position.z;
        return world;
    }
}

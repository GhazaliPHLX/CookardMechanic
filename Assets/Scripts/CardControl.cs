using UnityEngine.InputSystem;
using UnityEngine;
using System.Linq;

public class CardControl : MonoBehaviour
{
    public InputActionReference Leftclick;
    public InputActionReference Rightclick;
    public InputActionReference MousePos;

    private float pressTime;
    private bool isLeftPressing = false;
    private bool isRightPressing = false;
    private bool isDragging = false;

    private CardMovement activeCard = null;

    private void Update()
    {
        Vector2 MousePosition = MousePos.action.ReadValue<Vector2>();

        // Left click pressed
        if (Leftclick.action.WasPressedThisFrame())
        {
            pressTime = 0f;
            isLeftPressing = true;
            isDragging = false;
        }

        // Right click pressed
        if (Rightclick.action.WasPressedThisFrame())
        {
            pressTime = 0f;
            isRightPressing = true;
            isDragging = false;
        }

        // Handle left dragging
        if (isLeftPressing)
        {
            pressTime += Time.deltaTime;

            if (!isDragging && pressTime >= 0.1f)
            {
                isDragging = true;
                HandleClick(MousePosition, true);
            }
        }

        // Handle right dragging
        if (isRightPressing)
        {
            pressTime += Time.deltaTime;

            if (!isDragging && pressTime >= 0.1f)
            {
                isDragging = true;
                HandleClick(MousePosition, false);
            }
        }

        // Release left
        if (Leftclick.action.WasReleasedThisFrame())
        {
            if (!isDragging)
            {
                Debug.Log("Clicked Something");
            }
            else
            {
                activeCard?.Released();
                Debug.Log("Released something");
            }

            isLeftPressing = false;
            isDragging = false;
            pressTime = 0f;
        }

        // Release right
        if (Rightclick.action.WasReleasedThisFrame())
        {
            if (!isDragging)
            {
                Debug.Log("Clicked Something");
            }
            else
            {
                activeCard?.RightReleased();
                Debug.Log("Released something");
            }

            isRightPressing = false;
            isDragging = false;
            pressTime = 0f;
        }
    }

    private void HandleClick(Vector2 MousePosition, bool isLeft)
    {
        Ray ray = Camera.main.ScreenPointToRay(MousePosition);

        // Ambil semua collider yang kena
        RaycastHit[] hits = Physics.RaycastAll(ray);

        if (hits.Length > 0)
        {
            // Pilih yang paling depan (terdekat ke kamera)
            var hit = hits.OrderBy(h => h.distance).First();

            if (isLeft)
            {
                var clickable = hit.collider.GetComponent<ILeftClick>();
                clickable?.Clicked();
            }
            else
            {
                var clickable = hit.collider.GetComponent<IRightClick>();
                clickable?.RightClicked();
            }

            activeCard = hit.collider.GetComponent<CardMovement>();
        }
        else
        {
            Debug.Log("No GameObject found");
        }
    }

    private void OnEnable()
    {
        Rightclick.action.Enable();
        Leftclick.action.Enable();
        MousePos.action.Enable();
    }

    private void OnDisable()
    {
        Rightclick.action.Disable();
        Leftclick.action.Disable();
        MousePos.action.Disable();
    }
}

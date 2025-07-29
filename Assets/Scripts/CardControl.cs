using UnityEngine.InputSystem;
using UnityEngine;

public class CardControl : MonoBehaviour
{
    public InputActionReference click;
    public InputActionReference MousePos;

    private void Start()
    {
        OnEnable();

    }

    private void Update()
    {
        if(click.action.WasPerformedThisFrame())
        {
            Vector2 MousePosition = MousePos.action.ReadValue<Vector2>();

            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(MousePosition);

            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
            if(hit.collider != null)
            {
                Debug.Log("Object di klik" + hit.collider.gameObject.name);
            }

        }
    }

    private void OnEnable()
    {
        click.action.Enable();
        MousePos.action.Enable();
    }

    private void OnDisable()
    {
        click.action.Disable(); 
        MousePos.action.Disable();
    }
}

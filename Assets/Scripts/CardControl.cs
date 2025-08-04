using UnityEngine.InputSystem;
using UnityEngine;
using System.Linq;

public class CardControl : MonoBehaviour
{
    public InputActionReference click;
    public InputActionReference MousePos;

    private float pressTime;
    private bool isPressing = false;
    private bool isDragging = false;

    private CardMovement activeCard = null;

    private void Start()
    {

    }

    private void Update()
    {

        Vector2 MousePosition = MousePos.action.ReadValue<Vector2>();


        if (click.action.WasPressedThisFrame())
        {

            pressTime = 0f;
            isPressing = true;
            isDragging = false;
        }

        if (isPressing)
        {
            pressTime += Time.deltaTime;

            if(!isDragging && pressTime >= 0.1f)
            {
                isDragging = true;

                Ray ray = Camera.main.ScreenPointToRay(MousePosition);
                bool hashit = Physics.Raycast(ray,  out RaycastHit hit);

                if (hashit)
                {
                    Debug.Log("Dragged a gameObject");
                    var clickable = hit.collider.GetComponent<ILeftClick>();
                    clickable?.Clicked();

                    activeCard = hit.collider.GetComponent<CardMovement>();
                }
                else
                {
                    Debug.Log("No Gameobject found");
                }

                }
            }

            if (click.action.WasReleasedThisFrame()) 
            {
                if (!isDragging)
                {
                    Debug.Log("Clicked Something");
                }

                else if(isDragging) 
                {
                    
                    activeCard?.Released();

                }

                isPressing = false;
                isDragging= false;
                pressTime = 0f;

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

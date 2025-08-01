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
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(MousePosition);


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
                RaycastHit2D[] hits = Physics2D.RaycastAll(worldPosition, Vector2.zero);
                if (hits.Length > 0)
                {
                    var hit = hits
                        .OrderByDescending(h => h.collider.GetComponent<SpriteRenderer>()?.sortingOrder ?? 0 )
                        .First(); 

                    Debug.Log("Dragged a gameObject");
                    var clickable = hit.collider.GetComponent<ILeftClick>();
                    clickable?.Clicked();

                    activeCard = hit.collider.GetComponent<CardMovement>();
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

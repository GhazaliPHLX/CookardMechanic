using UnityEngine;
using UnityEngine.InputSystem;


public class CameraControl : MonoBehaviour
{
    
    public InputActionReference CameraClick;
    public InputActionReference CameraMove;

    private Transform MainCamera;

    private Vector2 lastMousePos;
    private float sensitivity = 0.01f;
    



    private void Start()
    {
        MainCamera = GetComponent<Transform>();
        OnEnable();
    }

    private void Update()
    {
        if (MainCamera)
        {
            if (CameraClick.action.WasPressedThisFrame())
            {
                lastMousePos = CameraMove.action.ReadValue<Vector2>();
            }

            if(CameraClick.action.ReadValue<float>() > 0)
            {
                // Ambil value perubahan posisi maouse
                Vector2 currentMousePos = CameraMove.action.ReadValue<Vector2>();
                Vector2 delta = currentMousePos - lastMousePos;

                // Gerakin Transformnya
                MainCamera.position -= new Vector3(delta.x * sensitivity, delta.y * sensitivity, 0);

                lastMousePos = currentMousePos;
            }
        }

    }

    private void OnEnable()
    {
        CameraClick.action.Enable();
        CameraMove.action.Enable();
    }

    private void OnDisable()
    {
        CameraClick.action.Disable();
        CameraMove.action.Disable();
    }
}

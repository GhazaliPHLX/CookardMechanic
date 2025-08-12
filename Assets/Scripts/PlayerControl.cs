using UnityEngine;
using UnityEngine.InputSystem;


public class CameraControl : MonoBehaviour
{
    
    public InputActionReference CameraClick;
    public InputActionReference CameraMove;
    public InputActionReference Zoom;

    private Transform MainCamera;
    private Camera mainCam;

    private Vector2 lastMousePos;
    private float sensitivity = 0.012f;
    private float ZoomCamera = 5f;
    private float zoomSpeed = 0.25f;



    private void Start()
    {
        MainCamera = GetComponent<Transform>();
        mainCam = GetComponent<Camera>();
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

                //Dynamic Sensitivity biar kerasa natural
                float dynamicSensitivity = sensitivity * (ZoomCamera / 5f) ;
                // Gerakin Transformnya
                MainCamera.position -= new Vector3(delta.x * dynamicSensitivity, delta.y * dynamicSensitivity, 0);

                lastMousePos = currentMousePos;
            }

            if (Zoom.action.WasPerformedThisFrame())
            {
                Vector2 scrollDelta = Zoom.action.ReadValue<Vector2>();

                //Normalize value scroll
                float normalizedScroll = Mathf.Sign(scrollDelta.y);

                ZoomCamera += -normalizedScroll * zoomSpeed;
                ZoomCamera = Mathf.Clamp(ZoomCamera, 5f, 15f);

                mainCam.orthographicSize = ZoomCamera;
                
            }


        }

    }

    private void OnEnable()
    {
        CameraClick.action.Enable();
        CameraMove.action.Enable();
        Zoom.action.Enable();
    }

    private void OnDisable()
    {
        CameraClick.action.Disable();
        CameraMove.action.Disable();
        Zoom.action.Disable();
    }
}

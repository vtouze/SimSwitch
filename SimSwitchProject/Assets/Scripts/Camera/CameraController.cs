using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    #region Fields
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _zoomSpeed = 10f;
    [SerializeField] private float _rotationSpeed = 100f;

    private CameraControls controls;

    private Vector2 movementInput;
    private Vector2 rotationInput;
    private float zoomInput;
    #endregion Fields

    #region Methods
    private void Awake()
    {
        controls = new CameraControls();

        controls.Keyboard.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        controls.Keyboard.Movement.canceled += ctx => movementInput = Vector2.zero;

        controls.Mouse.Look.performed += ctx => rotationInput = ctx.ReadValue<Vector2>();
        controls.Mouse.Look.canceled += ctx => rotationInput = Vector2.zero;

        controls.Mouse.Zoom.performed += ctx => zoomInput = ctx.ReadValue<float>();
        controls.Mouse.Zoom.canceled += ctx => zoomInput = 0f;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
        HandleRotation();
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y);
        Vector3 relativeMovement = (_cameraTransform.forward * moveDirection.z) + (_cameraTransform.right * moveDirection.x);
        relativeMovement.y = 0;

        _cameraTransform.Translate(relativeMovement * _moveSpeed * Time.deltaTime, Space.World);
    }

    private void HandleZoom()
    {
        if (zoomInput != 0)
        {
            _cameraTransform.position += _cameraTransform.forward * zoomInput * _zoomSpeed;
        }
    }

    private void HandleRotation()
    {
        if (rotationInput != Vector2.zero)
        {
            float rotationX = rotationInput.x * _rotationSpeed * Time.deltaTime;
            float rotationY = rotationInput.y * _rotationSpeed * Time.deltaTime;

            _cameraTransform.Rotate(Vector3.up, rotationX, Space.World);
            _cameraTransform.Rotate(Vector3.left, rotationY);
        }
    }
    #endregion Methods
}

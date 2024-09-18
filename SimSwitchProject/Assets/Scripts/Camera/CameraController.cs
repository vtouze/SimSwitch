using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    #region Fields
    [Header("Movement & Rotation")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _zoomSpeed = 10f;
    //[HideInInspector]
    public float _rotationSpeed = 100f;
    public float _mouseSensitivity = 1f;
    private Vector3 _currentRotation;
    public bool _isMenuing = false;
    
    [Header("Input")]
    public InputActionReference _movementActionReference;
    public InputActionReference _zoomActionReference;
    private Vector2 _movementInput;
    private float _zoomInput;
    #endregion Fields

    #region Methods
    private void Awake()
    {
        LoadSettings();

        _movementActionReference.action.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
        _movementActionReference.action.canceled += ctx => _movementInput = Vector2.zero;

        _zoomActionReference.action.performed += ctx => _zoomInput = ctx.ReadValue<float>();
        _zoomActionReference.action.canceled += ctx => _zoomInput = 0f;
    }

    private void OnEnable()
    {
        _movementActionReference.action.Enable();
        _zoomActionReference.action.Enable();
    }

    private void OnDisable()
    {
        _movementActionReference.action.Disable();
        _zoomActionReference.action.Disable();
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
        HandleRotation();
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(_movementInput.x, 0, _movementInput.y);
        Vector3 relativeMovement = (_cameraTransform.forward * moveDirection.z) + (_cameraTransform.right * moveDirection.x);
        relativeMovement.y = 0;

        _cameraTransform.Translate(relativeMovement * _moveSpeed * Time.deltaTime, Space.World);
    }

    private void HandleZoom()
    {
        if (_zoomInput != 0 && !_isMenuing)
        {
            _cameraTransform.position += _cameraTransform.forward * _zoomInput * _zoomSpeed;
        }
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float rotationX = Input.GetAxis("Mouse X") * _rotationSpeed * _mouseSensitivity * Time.deltaTime;
            float rotationY = Input.GetAxis("Mouse Y") * _rotationSpeed * _mouseSensitivity * Time.deltaTime;

            _currentRotation.x -= rotationY;
            _currentRotation.y += rotationX;

            _cameraTransform.rotation = Quaternion.Euler(_currentRotation.x, _currentRotation.y, 0);
        }
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("RotationSpeed", _rotationSpeed);
        PlayerPrefs.SetFloat("MouseSensitivity", _mouseSensitivity);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("RotationSpeed"))
        {
            _rotationSpeed = PlayerPrefs.GetFloat("RotationSpeed");
        }

        if (PlayerPrefs.HasKey("MouseSensitivity"))
        {
            _mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
        }
    }
    #endregion Methods
}
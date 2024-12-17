using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    #region Fields
    [Header("Movement & Rotation")]
    [Tooltip("Transform of the camera to control its position and rotation.")]
    [SerializeField] private Transform _cameraTransform;

    [Tooltip("Speed at which the camera moves.")]
    [SerializeField] private float _moveSpeed = 10f;

    [Tooltip("Speed at which the camera zooms.")]
    [SerializeField] private float _zoomSpeed = 10f;

    [Tooltip("Rotation speed of the camera.")]
    public float _rotationSpeed = 100f;

    [Tooltip("Sensitivity of the mouse for rotating the camera.")]
    public float _mouseSensitivity = 1f;

    private Vector3 _currentRotation; // Tracks the current camera rotation

    [HideInInspector] public bool _isMenuing = false; // Used to disable controls during menu interactions

    [Header("Input")]
    [Tooltip("Reference to the input action for movement.")]
    public InputActionReference _movementActionReference;

    [Tooltip("Reference to the input action for zoom.")]
    public InputActionReference _zoomActionReference;

    private Vector2 _movementInput; // Stores movement input data
    private float _zoomInput; // Stores zoom input data
    #endregion Fields

    #region Methods
    /// <summary>
    /// Initializes camera settings and input actions.
    /// </summary>
    private void Awake()
    {
        LoadSettings();

        // Assign input actions to update movement and zoom input values
        _movementActionReference.action.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
        _movementActionReference.action.canceled += ctx => _movementInput = Vector2.zero;

        _zoomActionReference.action.performed += ctx => _zoomInput = ctx.ReadValue<float>();
        _zoomActionReference.action.canceled += ctx => _zoomInput = 0f;
    }

    /// <summary>
    /// Enables the input actions when the game object is enabled.
    /// </summary>
    private void OnEnable()
    {
        _movementActionReference.action.Enable();
        _zoomActionReference.action.Enable();
    }

    /// <summary>
    /// Disables the input actions when the game object is disabled.
    /// </summary>
    private void OnDisable()
    {
        _movementActionReference.action.Disable();
        _zoomActionReference.action.Disable();
    }

    /// <summary>
    /// Updates the camera controls every frame (movement, zoom, and rotation).
    /// </summary>
    private void Update()
    {
        HandleMovement();
        HandleZoom();
        HandleRotation();
    }

    /// <summary>
    /// Handles the movement of the camera based on user input.
    /// </summary>
    private void HandleMovement()
    {
        // Only handle movement if the menu is not active
        if(!_isMenuing)
        {
            // Calculate the movement direction based on user input
            Vector3 moveDirection = new Vector3(_movementInput.x, 0, _movementInput.y);

            // Convert to world-space movement relative to the camera
            Vector3 relativeMovement = (_cameraTransform.forward * moveDirection.z) + (_cameraTransform.right * moveDirection.x);
            relativeMovement.y = 0; // Keep movement flat

            // Move the camera based on calculated direction and speed
            _cameraTransform.Translate(relativeMovement * _moveSpeed * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// Handles zooming the camera based on user input.
    /// </summary>
    private void HandleZoom()
    {
        // Only handle zoom if there is input and the menu is not active
        if (_zoomInput != 0 && !_isMenuing)
        {
            _cameraTransform.position += _cameraTransform.forward * _zoomInput * _zoomSpeed;
        }
    }

    /// <summary>
    /// Handles camera rotation based on mouse input.
    /// </summary>
    private void HandleRotation()
    {
        // Only allow rotation while the right mouse button is pressed and the menu is not active
        if (Input.GetMouseButton(1) & !_isMenuing)
        {
            // Get mouse movement along the X and Y axes
            float rotationX = Input.GetAxis("Mouse X") * _rotationSpeed * _mouseSensitivity * Time.deltaTime;
            float rotationY = Input.GetAxis("Mouse Y") * _rotationSpeed * _mouseSensitivity * Time.deltaTime;

            // Update the current rotation values
            _currentRotation.x -= rotationY;
            _currentRotation.y += rotationX;

            // Apply the rotation to the camera transform
            _cameraTransform.rotation = Quaternion.Euler(_currentRotation.x, _currentRotation.y, 0);
        }
    }

    /// <summary>
    /// Saves the camera settings (rotation speed and mouse sensitivity) to PlayerPrefs.
    /// </summary>
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("RotationSpeed", _rotationSpeed);
        PlayerPrefs.SetFloat("MouseSensitivity", _mouseSensitivity);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads the saved camera settings (rotation speed and mouse sensitivity) from PlayerPrefs.
    /// </summary>
    public void LoadSettings()
    {
        // Check if PlayerPrefs contains saved values for rotation speed and mouse sensitivity
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
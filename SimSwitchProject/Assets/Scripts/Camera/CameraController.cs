using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Fields
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _zoomSpeed = 10f;
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private Vector2 _zoomRange = new Vector2(10f, 100f);

    private Vector3 _dragOrigin;
    private Vector3 _currentRotation;
    #endregion Fields

    #region Methods
    void Update()
    {
        HandleMovement();
        HandleZoom();
        HandleRotation();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = (_cameraTransform.forward * moveZ) + (_cameraTransform.right * moveX);
        moveDirection.y = 0;

        _cameraTransform.Translate(moveDirection * _moveSpeed * Time.deltaTime, Space.World);

        if (Input.GetMouseButtonDown(2))
        {
            _dragOrigin = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 difference = _dragOrigin - Input.mousePosition;
            _dragOrigin = Input.mousePosition;

            Vector3 move = new Vector3(difference.x * _moveSpeed * Time.deltaTime, 0, difference.y * _moveSpeed * Time.deltaTime);
            _cameraTransform.Translate(move, Space.Self);
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            _cameraTransform.position += _cameraTransform.forward * scroll * _zoomSpeed;

            float distance = Vector3.Distance(_cameraTransform.position, transform.position);

            if (distance < _zoomRange.x || distance > _zoomRange.y)
            {
                _cameraTransform.position -= _cameraTransform.forward * scroll * _zoomSpeed;
            }
        }
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float rotationX = Input.GetAxis("Mouse X") * _rotationSpeed * Time.deltaTime;
            float rotationY = Input.GetAxis("Mouse Y") * _rotationSpeed * Time.deltaTime;

            _currentRotation.x -= rotationY;
            _currentRotation.y += rotationX;

            _cameraTransform.rotation = Quaternion.Euler(_currentRotation.x, _currentRotation.y, 0);
        }
    }
    #endregion Methods
}

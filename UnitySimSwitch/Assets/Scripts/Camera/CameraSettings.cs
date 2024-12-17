using UnityEngine;
using UnityEngine.UI;

public class CameraSettings : MonoBehaviour
{
    [Header("Camera Rotation & Sensitivity")]
    [Tooltip("Slider to adjust the camera's rotation speed.")]
    [SerializeField] private Slider _rotationSlider = null;

    [Tooltip("Slider to adjust the mouse sensitivity for the camera.")]
    [SerializeField] private Slider _sensitivitySlider = null;

    [Tooltip("Reference to the CameraController to modify camera settings.")]
    [SerializeField] private CameraController _cameraController = null;

    /// <summary>
    /// Initializes the sliders based on the current camera settings and adds listeners for slider changes.
    /// </summary>
    private void Start()
    {
        // Set slider values to the current camera controller settings
        _rotationSlider.value = _cameraController._rotationSpeed;
        _sensitivitySlider.value = _cameraController._mouseSensitivity;

        // Add listeners to update settings when sliders are changed
        _rotationSlider.onValueChanged.AddListener(SetCameraRotationSpeed);
        _sensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
    }

    /// <summary>
    /// Sets the camera rotation speed based on the slider value.
    /// </summary>
    /// <param name="value">The new rotation speed value.</param>
    public void SetCameraRotationSpeed(float value)
    {
        _cameraController._rotationSpeed = value;
        _cameraController.SaveSettings();
    }

    /// <summary>
    /// Sets the mouse sensitivity based on the slider value.
    /// </summary>
    /// <param name="value">The new mouse sensitivity value.</param>
    public void SetMouseSensitivity(float value)
    {
        _cameraController._mouseSensitivity = value;
        _cameraController.SaveSettings();
    }
}
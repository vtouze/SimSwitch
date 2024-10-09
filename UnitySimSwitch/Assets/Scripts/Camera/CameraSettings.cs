using UnityEngine;
using UnityEngine.UI;

public class CameraSettings : MonoBehaviour
{
    [Header("Camera Rotation & Sensitivity")]
    [SerializeField] private Slider _rotationSlider = null;
    [SerializeField] private Slider _sensitivitySlider = null;
    [SerializeField] private CameraController _cameraController = null;

    private void Start()
    {
        _rotationSlider.value = _cameraController._rotationSpeed;
        _sensitivitySlider.value = _cameraController._mouseSensitivity;

        _rotationSlider.onValueChanged.AddListener(SetCameraRotationSpeed);
        _sensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
    }

    public void SetCameraRotationSpeed(float value)
    {
        _cameraController._rotationSpeed = value;
        _cameraController.SaveSettings();
    }

    public void SetMouseSensitivity(float value)
    {
        _cameraController._mouseSensitivity = value;
        _cameraController.SaveSettings();
    }
}

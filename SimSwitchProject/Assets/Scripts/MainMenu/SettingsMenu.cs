using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using System.Linq;


public class SettingsMenu : MonoBehaviour
{
    #region Fields
    [Header("Game Object")]
    [SerializeField] private GameObject _mainMenu = null;
    [SerializeField] private GameObject _settingsMenu = null;
    [SerializeField] private GameObject[] _tab = null;
    [SerializeField] private Image[] _title = null;
    [Header("Toggle")]
    [SerializeField] private Toggle _fullScreenToggle = null;
    [SerializeField] private Toggle _vSyncToggle = null;
    [Header("Resolutions")]
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private TMP_Dropdown _qualityDropdown;
    private Resolution[] _resolutions;
    [Header("Animations")]
    [SerializeField] private Animator _closeSettings = null;
    [Header("Buttons")]
    public List<Button> _buttons;
    private float _clickedAlpha = 1f;
    private float _unclickedAlpha = 0.5f;
    [Header("Sprite")]
    [SerializeField] private Sprite _pressedSprite = null;
    [SerializeField] private Sprite _normalSprite = null;
    [Header("Slider")]
    [SerializeField] private Slider _brightnessSlider;
    [SerializeField] private Light _directionalLight;
    #endregion Fields

    #region Methods
    void Start()
    {
        InitResolution();
        InitVSync(); 
        InitQuality();
        InitBrightness();       
    }

    public void OpenSettingsTab(GameObject gameObject)
    {
        foreach (GameObject obj in _tab)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        gameObject.SetActive(true);
    }

    public void OnButtonClick(Button clickedButton)
    {
        SetButtonAlpha(clickedButton, _clickedAlpha);
        clickedButton.image.sprite = _pressedSprite;

        foreach (Button button in _buttons)
        {
            if (button != clickedButton)
            {
                button.image.sprite = _normalSprite;
                SetButtonAlpha(button, _unclickedAlpha);
            }
        }
    }

    private void SetButtonAlpha(Button button, float alpha)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            Color newColor = buttonImage.color;
            newColor.a = alpha;
            buttonImage.color = newColor;
        }
    }

    public void BackMainMenu()
    {
        _closeSettings.SetBool("isOpening", false);
    }
    public void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetVsync()
    {
        if (_vSyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    private void InitVSync()
    {
        if (QualitySettings.vSyncCount == 0)
        {
            _vSyncToggle.isOn = false;
        }
        else
        {
            _vSyncToggle.isOn = true;
        }
    }

    private void InitQuality()
    {
        string[] qualityLevels = QualitySettings.names;

        _qualityDropdown.ClearOptions();

        List<string> options = new List<string>(qualityLevels);
        _qualityDropdown.AddOptions(options);

        _qualityDropdown.value = QualitySettings.GetQualityLevel();
        _qualityDropdown.RefreshShownValue();

        _qualityDropdown.onValueChanged.AddListener(ChangeQuality);
    }

    public void ChangeQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log("Quality Level Changed to: " + QualitySettings.names[qualityIndex]);
    }

    private void InitResolution()
    {
        _resolutions = Screen.resolutions;

        _resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();

        int currentResolutionIndex = 0;
        
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string resolutionOption = _resolutions[i].width + " x " + _resolutions[i].height;
            resolutionOptions.Add(resolutionOption);

            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        _resolutionDropdown.AddOptions(resolutionOptions);

        _resolutionDropdown.value = currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();

        _resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
    }

    public void ChangeResolution(int resolutionIndex)
    {
        Resolution selectedResolution = _resolutions[resolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
    }

    private void InitBrightness()
    {
        _brightnessSlider.value = _directionalLight.intensity;

        _brightnessSlider.onValueChanged.AddListener(ChangeBrightness);
    }

    public void ChangeBrightness(float value)
    {
        _directionalLight.intensity = value;
    }
    #endregion Methods
}

/*In a Unity URP (Universal Render Pipeline) project, you can change the brightness of the scene using a Slider by manipulating the global lighting settings or applying post-processing effects like adjusting exposure or modifying the color 
grading.

Here's how to achieve this using a slider to adjust the brightness with post-processing in Unity:

Step-by-Step Implementation:
1. Set up Post-Processing in URP:
First, ensure that post-processing is enabled in your project.

Go to your Forward Renderer Asset (usually located in Assets > Settings) and check Post-Processing.
Make sure you have a Volume GameObject in your scene with a Post Process Volume component. If not, create a new empty GameObject in your scene and add the Post-process Volume component.
Set the Is Global checkbox to true to make sure the volume affects the entire scene.
2. Add the Color Grading Effect:
In the Post Process Volume, add a Color Grading override by clicking on Add Override > Post-processing > Color Grading.
In the Color Grading settings, make sure Exposure is enabled.
3. Script for Adjusting Brightness with a Slider:
Here's the script to link the slider to the brightness control using post-processing.

csharp
Copy code
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BrightnessController : MonoBehaviour
{
    public Slider brightnessSlider; // Reference to the slider
    public Volume postProcessVolume; // Reference to the global post-processing volume
    private ColorAdjustments colorAdjustments;

    void Start()
    {
        // Find the ColorAdjustments component in the post-processing volume
        if (postProcessVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            // Initialize the slider to reflect the current exposure value
            brightnessSlider.value = colorAdjustments.postExposure.value;
            
            // Add a listener to detect slider value changes
            brightnessSlider.onValueChanged.AddListener(ChangeBrightness);
        }
    }

    // This method is called whenever the slider value changes
    public void ChangeBrightness(float value)
    {
        // Adjust the post-exposure (which controls the brightness)
        colorAdjustments.postExposure.value = value;
    }
}
Explanation:
References:

brightnessSlider is the slider UI element that you want to use to adjust brightness.
postProcessVolume is the post-processing volume that holds the Color Grading effect.
colorAdjustments is a reference to the Color Adjustments effect, where we can change the postExposure value to control brightness.
Start():

The script finds the Color Adjustments override in the post-processing volume and uses it to modify the brightness.
It initializes the slider to reflect the current postExposure value and sets up a listener that calls ChangeBrightness() when the slider value changes.
ChangeBrightness(): This method takes the slider value and sets it as the new postExposure value to adjust the brightness in real time.

How to Use:
Create a UI Slider in your scene (you can do this via GameObject > UI > Slider).
Attach the script to an empty GameObject or your UI Manager.
Assign the slider and the Post Process Volume from the scene to the script in the Inspector.
Set the slider min and max values in the slider component, typically between -1 and 2 (this range works well for exposure).
Notes:
The postExposure property directly influences the overall brightness by simulating camera exposure.
If you want to fine-tune the brightness control, adjust the slider’s min and max values accordingly.*/
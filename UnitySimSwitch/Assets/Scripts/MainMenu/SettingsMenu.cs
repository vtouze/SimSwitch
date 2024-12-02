using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles the functionality of the settings menu, including adjusting screen resolution,
/// quality settings, fullscreen mode, VSync, and brightness. It also manages button 
/// interactions and the display of different settings tabs.
/// </summary>
public class SettingsMenu : MonoBehaviour
{
    #region Fields
    [Header("Game Objects")]
    [Tooltip("Array of GameObjects representing different tabs in the settings menu.")]
    [SerializeField] private GameObject[] _tab = null;

    [Header("Toggle")]
    [Tooltip("Toggle for enabling/disabling V-Sync.")]
    [SerializeField] private Toggle _vSyncToggle = null;

    [Header("Resolutions")]
    [Tooltip("Dropdown for selecting the screen resolution.")]
    [SerializeField] private TMP_Dropdown _resolutionDropdown;

    [Tooltip("Dropdown for selecting the quality settings.")]
    [SerializeField] private TMP_Dropdown _qualityDropdown;

    private Resolution[] _resolutions;

    [Header("Animations")]
    [Tooltip("Animator for closing the settings menu.")]
    [SerializeField] private Animator _closeSettings = null;

    [Header("Buttons")]
    [Tooltip("List of buttons in the settings menu.")]
    public List<Button> _buttons;

    private float _clickedAlpha = 1f; // Alpha value when a button is clicked
    private float _unclickedAlpha = 0.5f; // Alpha value when a button is not clicked

    [Header("Sprite")]
    [Tooltip("Sprite shown when a button is clicked.")]
    [SerializeField] private Sprite _pressedSprite = null;

    [Tooltip("Sprite shown when a button is not clicked.")]
    [SerializeField] private Sprite _normalSprite = null;

    [Header("Slider")]
    [Tooltip("Slider for adjusting the screen brightness.")]
    [SerializeField] private Slider _brightnessSlider;

    [Tooltip("Directional light to change brightness.")]
    [SerializeField] private Light _directionalLight;
    #endregion Fields

    #region Methods
    /// <summary>
    /// Called at the start to initialize settings like resolution, V-Sync, quality, and brightness.
    /// </summary>
    void Start()
    {
        InitResolution();
        InitVSync(); 
        InitQuality();
        InitBrightness();       
    }

    /// <summary>
    /// Opens a specified settings tab and closes others.
    /// </summary>
    /// <param name="gameObject">The GameObject representing the tab to open.</param>
    public void OpenSettingsTab(GameObject gameObject)
    {
        foreach (GameObject obj in _tab)
        {
            if (obj != null)
            {
                obj.SetActive(false); // Hide all tabs
            }
        }
        gameObject.SetActive(true); // Show the selected tab
    }

    /// <summary>
    /// Handles button click behavior: changes the button's sprite and adjusts its alpha.
    /// </summary>
    /// <param name="clickedButton">The button that was clicked.</param>
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

    /// <summary>
    /// Sets the alpha (opacity) of a button's image component.
    /// </summary>
    /// <param name="button">The button whose alpha needs to be adjusted.</param>
    /// <param name="alpha">The alpha value to set (0-1).</param>
    private void SetButtonAlpha(Button button, float alpha)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            Color newColor = buttonImage.color;
            newColor.a = alpha; // Change alpha value
            buttonImage.color = newColor;
        }
    }

    /// <summary>
    /// Closes the settings menu by triggering the closing animation.
    /// </summary>
    public void BackMainMenu()
    {
        _closeSettings.SetBool("isOpening", false);
    }

    /// <summary>
    /// Sets the screen to fullscreen or windowed mode based on the toggle state.
    /// </summary>
    /// <param name="isFullScreen">True for fullscreen, false for windowed mode.</param>
    public void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    /// <summary>
    /// Enables or disables V-Sync based on the toggle state.
    /// </summary>
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

    /// <summary>
    /// Sets the quality level of the game based on the dropdown selection.
    /// </summary>
    /// <param name="qualityIndex">Index of the selected quality level.</param>
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    /// <summary>
    /// Initializes the V-Sync toggle state based on the current settings.
    /// </summary>
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

    /// <summary>
    /// Initializes the quality dropdown to match the current quality settings.
    /// </summary>
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

    /// <summary>
    /// Changes the quality level based on the dropdown selection.
    /// </summary>
    /// <param name="qualityIndex">Index of the selected quality level.</param>
    public void ChangeQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log("Quality Level Changed to: " + QualitySettings.names[qualityIndex]);
    }

    /// <summary>
    /// Initializes the resolution dropdown to match available screen resolutions.
    /// </summary>
    private void InitResolution()
    {
        _resolutions = Screen.resolutions;

        _resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();

        int currentResolutionIndex = 0;

        // Add available resolutions to the dropdown and find the current resolution
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

    /// <summary>
    /// Changes the screen resolution based on the selected dropdown value.
    /// </summary>
    /// <param name="resolutionIndex">Index of the selected resolution.</param>
    public void ChangeResolution(int resolutionIndex)
    {
        Resolution selectedResolution = _resolutions[resolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
    }

    /// <summary>
    /// Initializes the brightness slider to match the current directional light intensity.
    /// </summary>
    private void InitBrightness()
    {
        _brightnessSlider.value = _directionalLight.intensity;

        _brightnessSlider.onValueChanged.AddListener(ChangeBrightness);
    }

    /// <summary>
    /// Changes the brightness of the directional light based on the slider value.
    /// </summary>
    /// <param name="value">New brightness value (0 to 1).</param>
    public void ChangeBrightness(float value)
    {
        _directionalLight.intensity = value;
    }
    #endregion Methods
}
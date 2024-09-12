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
    [SerializeField] private GameObject[] _tab = null;
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
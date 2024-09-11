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
    public int _selectedResolutions;
    public TMP_Text _resolutionsText = null;
    public List<ResolutionIndex> _resolutions = new List<ResolutionIndex>();
    [Header("Animations")]
    [SerializeField] private Animator _closeSettings = null;

    #endregion Fields

    #region Methods
    void Start()
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

    public void TabSelection(Image index)
    {
        foreach(Image img in _title)
        {
            if(img != null)
            {
                img.color = new Color(255, 255, 255, 150);
            }
        }
        index.color = new Color(255, 255, 255, 150);
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

    public void ResolutionsInf()
    {
        _selectedResolutions--;
        if (_selectedResolutions < 0)
        {
            _selectedResolutions = 0;
        }
        SetResolutionText();
        SetResolution();
    }

    public void ResolutionSup()
    {
        _selectedResolutions++;
        if (_selectedResolutions > _resolutions.Count - 1)
        {
            _selectedResolutions = _resolutions.Count - 1;
        }
        SetResolutionText();
        SetResolution();
    }

    public void SetResolutionText()
    {
        _resolutionsText.text = _resolutions[_selectedResolutions].horizontal.ToString() + "x" + _resolutions[_selectedResolutions].vertical.ToString();
    }

    public void SetResolution()
    {
        Screen.SetResolution(_resolutions[_selectedResolutions].horizontal, _resolutions[_selectedResolutions].vertical, _fullScreenToggle.isOn);
    }

    #endregion Methods
}

[System.Serializable]
public class ResolutionIndex
{
    public int horizontal;
    public int vertical;
}
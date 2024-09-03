using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LocalizationManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Sprite _englishSprite;
    [SerializeField] private Sprite _frenchSprite;
    private int _currentLanguageIndex = 0;
    private string[] _languageCodes = { "en", "fr" };
    #endregion Fields

    #region Methods
    public void ToggleLanguage()
    {
        _currentLanguageIndex = (_currentLanguageIndex + 1) % _languageCodes.Length;
        string currentLanguageCode = _languageCodes[_currentLanguageIndex];
        SetLocale(currentLanguageCode);
        UpdateButtonImage();
    }

    public void SetLocale(string code)
    {
        var localeQuery = (from locale in LocalizationSettings.AvailableLocales.Locales
                           where locale.Identifier.Code == code
                           select locale).FirstOrDefault();

        if (localeQuery == null)
        {
            Debug.LogError($"No locale for {code} found");
            return;
        }

        LocalizationSettings.SelectedLocale = localeQuery;
    }

    public void UpdateButtonImage()
    {
        if (_buttonImage == null)
        {
            Debug.LogWarning("ButtonImage reference is missing.");
            return;
        }

        switch (_currentLanguageIndex)
        {
            case 0:
                _buttonImage.sprite = _frenchSprite;
                break;
            case 1:
                _buttonImage.sprite = _englishSprite;
                break;
            default:
                Debug.LogWarning("Unsupported language index.");
                break;
        }
    }
    #endregion Methods
}

using UnityEngine;
using UnityEngine.UI;

public class LawsMenu : MonoBehaviour
{
    #region Fields
    [SerializeField] private Sprite _nullBackground = null;
    [SerializeField] private Sprite _lowBackground = null;
    [SerializeField] private Sprite _mediumBackground = null;
    [SerializeField] private Sprite _highBackground = null;

    [SerializeField] private Sprite _lowIconNormal = null;
    [SerializeField] private Sprite _lowIconSelected = null;
    [SerializeField] private Sprite _mediumIconNormal = null;
    [SerializeField] private Sprite _mediumIconSelected = null;
    [SerializeField] private Sprite _highIconNormal = null;
    [SerializeField] private Sprite _highIconSelected = null;

    [SerializeField] private Button[] _horizontalBoxButtons = null;
    [SerializeField] private Image[] _icons = null;

    private int _selectedButtonIndex = -1;
    #endregion Fields

    void Start()
    {
        UpdateButtons(_selectedButtonIndex);
    }

    public void LowButton()
    {
        _selectedButtonIndex = 0;
        UpdateButtons(_selectedButtonIndex);
    }

    public void MediumButton()
    {
        _selectedButtonIndex = 1;
        UpdateButtons(_selectedButtonIndex);
    }

    public void HighButton()
    {
        _selectedButtonIndex = 2;
        UpdateButtons(_selectedButtonIndex);
    }

    private void UpdateButtons(int selectedButtonIndex)
    {
        for (int i = 0; i < _horizontalBoxButtons.Length; i++)
        {
            Image buttonBackground = _horizontalBoxButtons[i].GetComponent<Image>();
            buttonBackground.sprite = (i == selectedButtonIndex) ? GetBackgroundSprite(i) : _nullBackground;

            Image buttonIcon = _icons[i];
            buttonIcon.sprite = (i == selectedButtonIndex) ? GetSelectedIcon(i) : GetNormalIcon(i);
        }
    }

    private Sprite GetBackgroundSprite(int index)
    {
        switch (index)
        {
            case 0: return _lowBackground;
            case 1: return _mediumBackground;
            case 2: return _highBackground;
            default: return _nullBackground;
        }
    }

    private Sprite GetNormalIcon(int index)
    {
        switch (index)
        {
            case 0: return _lowIconNormal;
            case 1: return _mediumIconNormal;
            case 2: return _highIconNormal;
            default: return null;
        }
    }

    private Sprite GetSelectedIcon(int index)
    {
        switch (index)
        {
            case 0: return _lowIconSelected;
            case 1: return _mediumIconSelected;
            case 2: return _highIconSelected;
            default: return null;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class LawsMenu : MonoBehaviour
{
    #region Fields
    [Header("Backgrounds")]
    [SerializeField] private Sprite _nullBackground = null;
    [SerializeField] private Sprite _lowBackground = null;
    [SerializeField] private Sprite _mediumBackground = null;
    [SerializeField] private Sprite _highBackground = null;

    [Header("Icons")]
    [SerializeField] private Sprite _lowestIconNormal = null;
    [SerializeField] private Sprite _lowestIconSelected = null;
    [SerializeField] private Sprite _lowIconNormal = null;
    [SerializeField] private Sprite _lowIconSelected = null;
    [SerializeField] private Sprite _mediumIconNormal = null;
    [SerializeField] private Sprite _mediumIconSelected = null;
    [SerializeField] private Sprite _highIconNormal = null;
    [SerializeField] private Sprite _highIconSelected = null;
    [SerializeField] private Sprite _highestIconNormal = null;
    [SerializeField] private Sprite _highestIconSelected = null;

    private Button[] _horizontalBoxButtons;
    private Image[] _icons;

    private int _selectedButtonIndex = -1;
    #endregion Fields

    #region Methods
    void Start()
    {
        _horizontalBoxButtons = new Button[5];
        _icons = new Image[5];

        _horizontalBoxButtons[0] = transform.Find("PriorityButtons/Lowest_Background").GetComponent<Button>();
        _icons[0] = transform.Find("PriorityButtons/Lowest_Background/Lowest_Icon").GetComponent<Image>();

        _horizontalBoxButtons[1] = transform.Find("PriorityButtons/Low_Background").GetComponent<Button>();
        _icons[1] = transform.Find("PriorityButtons/Low_Background/Low_Icon").GetComponent<Image>();

        _horizontalBoxButtons[2] = transform.Find("PriorityButtons/Medium_Background").GetComponent<Button>();
        _icons[2] = transform.Find("PriorityButtons/Medium_Background/Medium_Icon").GetComponent<Image>();

        _horizontalBoxButtons[3] = transform.Find("PriorityButtons/High_Background").GetComponent<Button>();
        _icons[3] = transform.Find("PriorityButtons/High_Background/High_Icon").GetComponent<Image>();

        _horizontalBoxButtons[4] = transform.Find("PriorityButtons/Highest_Background").GetComponent<Button>();
        _icons[4] = transform.Find("PriorityButtons/Highest_Background/Highest_Icon").GetComponent<Image>();

        _horizontalBoxButtons[0].onClick.AddListener(LowestButton);
        _horizontalBoxButtons[1].onClick.AddListener(LowButton);
        _horizontalBoxButtons[2].onClick.AddListener(MediumButton);
        _horizontalBoxButtons[3].onClick.AddListener(HighButton);
        _horizontalBoxButtons[4].onClick.AddListener(HighestButton);


        _selectedButtonIndex = 2;
        UpdateButtons(_selectedButtonIndex);
    }

    public void LowestButton()
    {
        _selectedButtonIndex = 0;
        UpdateButtons(_selectedButtonIndex);
    }

    public void LowButton()
    {
        _selectedButtonIndex = 1;
        UpdateButtons(_selectedButtonIndex);
    }

    public void MediumButton()
    {
        _selectedButtonIndex = 2;
        UpdateButtons(_selectedButtonIndex);
    }

    public void HighButton()
    {
        _selectedButtonIndex = 3;
        UpdateButtons(_selectedButtonIndex);
    }

    public void HighestButton()
    {
        _selectedButtonIndex = 4;
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
            case 1: return _lowBackground;
            case 2: return _mediumBackground;
            case 3: return _highBackground;
            case 4: return _highBackground;
            default: return _nullBackground;
        }
    }

    private Sprite GetNormalIcon(int index)
    {
        switch (index)
        {
            case 0: return _lowestIconNormal;
            case 1: return _lowIconNormal;
            case 2: return _mediumIconNormal;
            case 3: return _highIconNormal;
            case 4: return _highestIconNormal;
            default: return null;
        }
    }

    private Sprite GetSelectedIcon(int index)
    {
        switch (index)
        {
            case 0: return _lowestIconSelected;
            case 1: return _lowIconSelected;
            case 2: return _mediumIconSelected;
            case 3: return _highIconSelected;
            case 4: return _highestIconSelected;
            default: return null;
        }
    }
    #endregion Methods
}
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the Laws menu, allowing users to select priority levels with visual feedback.
/// Updates button backgrounds and icons based on the selected priority.
/// </summary>
public class LawsMenu : MonoBehaviour
{
    #region Fields
    [Header("Backgrounds")]
    [Tooltip("Background sprite when no priority is selected.")]
    [SerializeField] private Sprite _nullBackground = null;

    [Tooltip("Background sprite for low priority.")]
    [SerializeField] private Sprite _lowBackground = null;

    [Tooltip("Background sprite for medium priority.")]
    [SerializeField] private Sprite _mediumBackground = null;

    [Tooltip("Background sprite for high priority.")]
    [SerializeField] private Sprite _highBackground = null;

    [Header("Icons")]
    [Tooltip("Icon for lowest priority in normal state.")]
    [SerializeField] private Sprite _lowestIconNormal = null;

    [Tooltip("Icon for lowest priority in selected state.")]
    [SerializeField] private Sprite _lowestIconSelected = null;

    [Tooltip("Icon for low priority in normal state.")]
    [SerializeField] private Sprite _lowIconNormal = null;

    [Tooltip("Icon for low priority in selected state.")]
    [SerializeField] private Sprite _lowIconSelected = null;

    [Tooltip("Icon for medium priority in normal state.")]
    [SerializeField] private Sprite _mediumIconNormal = null;

    [Tooltip("Icon for medium priority in selected state.")]
    [SerializeField] private Sprite _mediumIconSelected = null;

    [Tooltip("Icon for high priority in normal state.")]
    [SerializeField] private Sprite _highIconNormal = null;

    [Tooltip("Icon for high priority in selected state.")]
    [SerializeField] private Sprite _highIconSelected = null;

    [Tooltip("Icon for highest priority in normal state.")]
    [SerializeField] private Sprite _highestIconNormal = null;

    [Tooltip("Icon for highest priority in selected state.")]
    [SerializeField] private Sprite _highestIconSelected = null;

    // Array of priority buttons representing different levels.
    private Button[] _horizontalBoxButtons;

    // Array of icons corresponding to each priority button.
    private Image[] _icons;

    // Index of the currently selected priority button.
    private int _selectedButtonIndex = -1;
    #endregion Fields

    #region Methods
    /// <summary>
    /// Initializes button and icon references and sets up button listeners.
    /// </summary>
    private void Start()
    {
        _horizontalBoxButtons = new Button[5];
        _icons = new Image[5];

        // Finds and assigns buttons and icons by hierarchy.
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

        // Adds listeners to buttons to trigger corresponding methods.
        _horizontalBoxButtons[0].onClick.AddListener(LowestButton);
        _horizontalBoxButtons[1].onClick.AddListener(LowButton);
        _horizontalBoxButtons[2].onClick.AddListener(MediumButton);
        _horizontalBoxButtons[3].onClick.AddListener(HighButton);
        _horizontalBoxButtons[4].onClick.AddListener(HighestButton);

        // Sets default selected button to medium and updates UI.
        _selectedButtonIndex = 2;
        UpdateButtons(_selectedButtonIndex);
    }

    /// <summary>
    /// Sets the lowest priority button as selected.
    /// </summary>
    public void LowestButton()
    {
        _selectedButtonIndex = 0;
        UpdateButtons(_selectedButtonIndex);
    }

    /// <summary>
    /// Sets the low priority button as selected.
    /// </summary>
    public void LowButton()
    {
        _selectedButtonIndex = 1;
        UpdateButtons(_selectedButtonIndex);
    }

    /// <summary>
    /// Sets the medium priority button as selected.
    /// </summary>
    public void MediumButton()
    {
        _selectedButtonIndex = 2;
        UpdateButtons(_selectedButtonIndex);
    }

    /// <summary>
    /// Sets the high priority button as selected.
    /// </summary>
    public void HighButton()
    {
        _selectedButtonIndex = 3;
        UpdateButtons(_selectedButtonIndex);
    }

    /// <summary>
    /// Sets the highest priority button as selected.
    /// </summary>
    public void HighestButton()
    {
        _selectedButtonIndex = 4;
        UpdateButtons(_selectedButtonIndex);
    }

    /// <summary>
    /// Updates the background and icon of each button based on the selected button index.
    /// </summary>
    /// <param name="selectedButtonIndex">The index of the currently selected button.</param>
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

    /// <summary>
    /// Returns the appropriate background sprite for the given index.
    /// </summary>
    /// <param name="index">Index of the button.</param>
    /// <returns>The corresponding background sprite.</returns>
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

    /// <summary>
    /// Returns the normal icon sprite for the given index.
    /// </summary>
    /// <param name="index">Index of the button.</param>
    /// <returns>The corresponding normal icon sprite.</returns>
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

    /// <summary>
    /// Returns the selected icon sprite for the given index.
    /// </summary>
    /// <param name="index">Index of the button.</param>
    /// <returns>The corresponding selected icon sprite.</returns>
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
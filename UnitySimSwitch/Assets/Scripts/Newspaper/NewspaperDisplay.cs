using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Responsible for displaying the content of a specific newspaper event. It manages the visual updates 
/// of various UI elements such as cover image, event name, date, and description. Additionally, it 
/// handles dynamic updates to the satisfaction, money, and sustainability stats associated with the event.
/// </summary>
public class NewspaperDisplay : MonoBehaviour
{
    #region Fields
    [Tooltip("The image displayed as the cover of the newspaper event.")]
    [SerializeField] private Image _coverImage;

    [Tooltip("Text component displaying the name of the event.")]
    [SerializeField] private TMP_Text _entryNameText;

    [Tooltip("Text component displaying the date of the event.")]
    [SerializeField] private TMP_Text _dateText;

    [Tooltip("Text component displaying the subtitle for the event.")]
    [SerializeField] private TMP_Text _subtitleText;

    [Tooltip("Text component displaying the description of the cover image.")]
    [SerializeField] private TMP_Text _coverDescriptionText;

    [Tooltip("Text component displaying the full description of the event.")]
    [SerializeField] private TMP_Text _descriptionText;

    [Tooltip("Icon representing the effect on money.")]
    [SerializeField] private Image _moneyIcon;

    [Tooltip("Icon representing the effect on satisfaction.")]
    [SerializeField] private Image _satisfactionIcon;

    [Tooltip("Icon representing the effect on sustainability.")]
    [SerializeField] private Image _sustainabilityIcon;

    [Tooltip("Text component displaying the amount of money change.")]
    [SerializeField] private TMP_Text _moneyText;

    [Tooltip("Text component displaying the satisfaction change value.")]
    [SerializeField] private TMP_Text _satisfactionText;

    [Tooltip("Text component displaying the sustainability change value.")]
    [SerializeField] private TMP_Text _sustainabilityText;

    [Tooltip("Parent container for the event display elements.")]
    [SerializeField] private RectTransform _parentContainer;

    [Tooltip("Base height of the parent container before any dynamic elements.")]
    [SerializeField] private float _baseHeight = 0;

    [Tooltip("Height added for each active item in the event display.")]
    [SerializeField] private float _heightPerItem = 100f;

    [Tooltip("Sprite for the happy satisfaction icon.")]
    [SerializeField] private Sprite _happySatisfactionIcon;

    [Tooltip("Sprite for the angry satisfaction icon.")]
    [SerializeField] private Sprite _angrySatisfactionIcon;
    #endregion Fields

    #region Methods

    /// <summary>
    /// Displays the content of a specific newspaper event. This method updates all the relevant UI elements 
    /// with the event's details.
    /// </summary>
    /// <param name="newspaperEvent">The newspaper event to display.</param>
    public void DisplayEvent(NewspaperEvent newspaperEvent)
    {
        // Update UI elements with event data
        _coverImage.sprite = newspaperEvent._coverImage;
        _entryNameText.text = newspaperEvent._eventName;
        _dateText.text = newspaperEvent._date;
        _subtitleText.text = newspaperEvent._subheading;
        _coverDescriptionText.text = newspaperEvent._captionImage;
        _descriptionText.text = newspaperEvent._description;

        // Set event effects for money, satisfaction, and sustainability
        SetEventEffect(_moneyIcon, _moneyText, newspaperEvent._moneyChange);
        SetSatisfactionEffect(newspaperEvent._satisfactionChange);
        SetEventEffect(_sustainabilityIcon, _sustainabilityText, newspaperEvent._sustainabilityChange);
    }

    /// <summary>
    /// Updates the satisfaction icon and text based on the satisfaction change value. 
    /// Displays a green or red icon based on whether the value is positive or negative, respectively.
    /// If no change, the satisfaction element is hidden.
    /// </summary>
    /// <param name="changeValue">The change value for satisfaction.</param>
    private void SetSatisfactionEffect(int changeValue)
    {
        GameObject parentObject = _satisfactionIcon.transform.parent.gameObject;

        if (changeValue != 0)
        {
            // Show satisfaction effect
            parentObject.SetActive(true);
            _satisfactionText.text = changeValue > 0 ? $"+{changeValue}" : $"{changeValue}";
            
            _satisfactionIcon.sprite = changeValue > 0 ? _happySatisfactionIcon : _angrySatisfactionIcon;
            _satisfactionText.color = changeValue > 0 ? Color.green : Color.red;
        }
        else
        {
            // Hide satisfaction effect if no change
            parentObject.SetActive(false);
        }

        // Recalculate the size of the parent container based on active elements
        RecalculateParentSize();
    }

    /// <summary>
    /// Updates the event effect icons and text based on the provided change value (for money or sustainability).
    /// Displays a green or red text color depending on whether the value is positive or negative.
    /// </summary>
    /// <param name="icon">The icon representing the effect (money or sustainability).</param>
    /// <param name="text">The text component displaying the change value.</param>
    /// <param name="changeValue">The change value for the event effect.</param>
    private void SetEventEffect(Image icon, TMP_Text text, int changeValue)
    {
        GameObject parentObject = icon.transform.parent.gameObject;

        if (changeValue != 0)
        {
            parentObject.SetActive(true);
            text.text = changeValue > 0 ? $"+{changeValue}" : $"{changeValue}";
            text.color = changeValue > 0 ? Color.green : Color.red;
        }
        else
        {
            parentObject.SetActive(false);
        }

        RecalculateParentSize();
    }

    /// <summary>
    /// Recalculates the size of the parent container based on the number of active child elements.
    /// This ensures the container expands or shrinks to accommodate the content.
    /// </summary>
    private void RecalculateParentSize()
    {
        int activeElements = 0;

        // Count active child elements
        foreach (Transform child in _parentContainer)
        {
            if (child.gameObject.activeSelf)
            {
                activeElements++;
            }
        }

        // Adjust the height of the parent container based on active elements
        float newHeight = _baseHeight + (activeElements * _heightPerItem);
        _parentContainer.sizeDelta = new Vector2(_parentContainer.sizeDelta.x, newHeight);
    }

    #endregion Methods
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewspaperDisplay : MonoBehaviour
{
    #region Fields
    [SerializeField] private Image _coverImage;
    [SerializeField] private TMP_Text _entryNameText;
    [SerializeField] private TMP_Text _dateText;
    [SerializeField] private TMP_Text _subtitleText;
    [SerializeField] private TMP_Text _coverDescriptionText;
    [SerializeField] private TMP_Text _descriptionText;

    [SerializeField] private Image _moneyIcon;
    [SerializeField] private Image _satisfactionIcon;
    [SerializeField] private Image _sustainabilityIcon;

    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private TMP_Text _satisfactionText;
    [SerializeField] private TMP_Text _sustainabilityText;

    [SerializeField] private RectTransform _parentContainer;
    [SerializeField] private float _baseHeight = 0;
    [SerializeField] private float _heightPerItem = 100f;

    [SerializeField] private Sprite _happySatisfactionIcon;
    [SerializeField] private Sprite _angrySatisfactionIcon;
    #endregion Fields

    #region Methods
    public void DisplayEvent(NewspaperEvent newspaperEvent)
    {
        _coverImage.sprite = newspaperEvent._coverImage;
        _entryNameText.text = newspaperEvent._eventName;
        _dateText.text = newspaperEvent._date;
        _subtitleText.text = newspaperEvent._subheading;
        _coverDescriptionText.text = newspaperEvent._captionImage;
        _descriptionText.text = newspaperEvent._description;

        SetEventEffect(_moneyIcon, _moneyText, newspaperEvent._moneyChange);
        SetSatisfactionEffect(newspaperEvent._satisfactionChange);
        SetEventEffect(_sustainabilityIcon, _sustainabilityText, newspaperEvent._sustainabilityChange);
    }

    private void SetSatisfactionEffect(int changeValue)
    {
        GameObject parentObject = _satisfactionIcon.transform.parent.gameObject;

        if (changeValue != 0)
        {
            parentObject.SetActive(true);
            _satisfactionText.text = changeValue > 0 ? $"+{changeValue}" : $"{changeValue}";
            
            _satisfactionIcon.sprite = changeValue > 0 ? _happySatisfactionIcon : _angrySatisfactionIcon;
            
            _satisfactionText.color = changeValue > 0 ? Color.green : Color.red;
        }
        else
        {
            parentObject.SetActive(false);
        }

        RecalculateParentSize();
    }

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

    private void RecalculateParentSize()
    {
        int activeElements = 0;

        foreach (Transform child in _parentContainer)
        {
            if (child.gameObject.activeSelf)
            {
                activeElements++;
            }
        }

        float newHeight = _baseHeight + (activeElements * _heightPerItem);
        _parentContainer.sizeDelta = new Vector2(_parentContainer.sizeDelta.x, newHeight);
    }
    #endregion Methods
}
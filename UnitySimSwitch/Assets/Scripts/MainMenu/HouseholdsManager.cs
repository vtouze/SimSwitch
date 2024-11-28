using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HouseholdsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image _householdsIcon;
    public TMP_Text _type;
    public TMP_Text _age;
    public TMP_Text _children;
    public TMP_Text _distribution;
    public TMP_Text _income;
    
    public Button _prevButton;
    public Button _nextButton;

    [Header("Data")]
    public List<HouseholdsEntry> _entries;

    [Header("Selection Settings")]
    public Sprite _selectedSprite;
    public Sprite _unselectedSprite;

    private int _currentIndex = 1;
    private Button _currentlySelectedButton;

    private void Awake()
    {
        DontDestroyOnLoad(_prevButton);
        DontDestroyOnLoad(_nextButton);
    }
    private void Start()
    {
        UpdateCardUI(_currentIndex);
        UpdateNavigationButtons();
    }

    public void DisplayEntry(HouseholdsEntry entry)
    {
        _householdsIcon.sprite = entry._householdsIcon;
        _type.text = entry._type;
        _age.text = entry._age;
        _children.text = entry._children.ToString();
        _distribution.text = entry._distribution;
        _income.text = entry._income.ToString();

        Button clickedButton = EventSystem.current.currentSelectedGameObject?.GetComponent<Button>();
        if (clickedButton != null)
        {
            if (_currentlySelectedButton != null && _currentlySelectedButton != clickedButton)
            {
                _currentlySelectedButton.GetComponent<Image>().sprite = _unselectedSprite;
            }
            clickedButton.GetComponent<Image>().sprite = _selectedSprite;
            _currentlySelectedButton = clickedButton;
        }
    }

    public void NavigateForward()
    {
        if (_currentIndex < _entries.Count - 1)
        {
            _currentIndex++;
            UpdateCardUI(_currentIndex);
        }
        UpdateNavigationButtons();
    }

    public void NavigateBackward()
    {
        if (_currentIndex > 0)
        {
            _currentIndex--;
            UpdateCardUI(_currentIndex);
        }
        UpdateNavigationButtons();
    }

    private void UpdateCardUI(int index)
    {
        DisplayEntry(_entries[index]);
    }

    private void UpdateNavigationButtons()
    {
        _prevButton.interactable = _currentIndex > 0;
        _nextButton.interactable = _currentIndex < _entries.Count - 1;
    }
}
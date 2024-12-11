using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HouseholdsManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image _householdsIcon;
    [SerializeField] private TMP_Text _type;
    [SerializeField] private TMP_Text _age;
    [SerializeField] private TMP_Text _children;
    [SerializeField] private TMP_Text _distribution;
    [SerializeField] private TMP_Text _income;

    [SerializeField] private Button _prevButton;
    [SerializeField] private Button _nextButton;

    [Header("Data")]
    [SerializeField] private List<HouseholdsEntry> _entries;

    [Header("Selection Settings")]
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private Sprite _unselectedSprite;
    [SerializeField] private Sprite _selectedTab;
    [SerializeField] private Sprite _unselectedTab;
    [SerializeField] private List<Button> _selectionButtons;

    [Header("Tab Settings")]
    [SerializeField] private List<Button> _tabButtons;
    [SerializeField] private List<GameObject> _tabPanels;

    private Dictionary<Button, HouseholdsEntry> _buttonEntryMap = new Dictionary<Button, HouseholdsEntry>();
    private Button _currentlySelectedButton;
    private int _currentIndex = 0;

    private void Start()
    {
        for (int i = 0; i < _selectionButtons.Count && i < _entries.Count; i++)
        {
            Button button = _selectionButtons[i];
            HouseholdsEntry entry = _entries[i];
            _buttonEntryMap[button] = entry;

            button.onClick.AddListener(() => OnHouseholdButtonClick(button));
        }

        for (int i = 0; i < _tabButtons.Count; i++)
        {
            Button tabButton = _tabButtons[i];
            int index = i;
            tabButton.onClick.AddListener(() => OpenTab(index));
        }

        UpdateCardUI(_currentIndex);
        UpdateNavigationButtons();
    }

    private void OnHouseholdButtonClick(Button clickedButton)
    {
        if (_buttonEntryMap.TryGetValue(clickedButton, out HouseholdsEntry entry))
        {
            DisplayEntry(entry, clickedButton);

            int selectedIndex = _selectionButtons.IndexOf(clickedButton);
            if (selectedIndex != -1)
            {
                _currentIndex = selectedIndex;
                UpdateNavigationButtons();
            }
        }
    }

    public void DisplayEntry(HouseholdsEntry entry, Button clickedButton)
    {
        _householdsIcon.sprite = entry._householdsIcon;
        _type.text = entry._type;
        _age.text = entry._age;
        _children.text = entry._children.ToString();
        _distribution.text = entry._distribution;
        _income.text = entry._income.ToString();

        if (clickedButton != null && _selectionButtons.Contains(clickedButton))
        {
            UpdateButtonSelection(clickedButton);
        }
    }

    public void NavigateForward()
    {
        if (_currentIndex < _entries.Count - 1)
        {
            _currentIndex++;
            UpdateCardUI(_currentIndex);
            UpdateSelectionButtonSprite();
        }
        UpdateNavigationButtons();
    }

    public void NavigateBackward()
    {
        if (_currentIndex > 0)
        {
            _currentIndex--;
            UpdateCardUI(_currentIndex);
            UpdateSelectionButtonSprite();
        }
        UpdateNavigationButtons();
    }

    private void UpdateCardUI(int index)
    {
        DisplayEntry(_entries[index], null);
    }

    private void UpdateNavigationButtons()
    {
        bool canGoBackward = _currentIndex > 0;
        bool canGoForward = _currentIndex < _entries.Count - 1;

        _prevButton.interactable = canGoBackward;
        _nextButton.interactable = canGoForward;

        Debug.Log($"Prev Interactable: {_prevButton.interactable}, Next Interactable: {_nextButton.interactable}");

        _prevButton.GetComponent<Image>().color = canGoBackward ? Color.white : Color.grey;
        _nextButton.GetComponent<Image>().color = canGoForward ? Color.white : Color.grey;
    }

    private void UpdateButtonSelection(Button clickedButton)
    {
        if (_currentlySelectedButton != null)
        {
            _currentlySelectedButton.GetComponent<Image>().sprite = _unselectedSprite;
        }

        clickedButton.GetComponent<Image>().sprite = _selectedSprite;
        _currentlySelectedButton = clickedButton;
    }

    private void UpdateSelectionButtonSprite()
    {
        if (_currentlySelectedButton != null)
        {
            _currentlySelectedButton.GetComponent<Image>().sprite = _unselectedSprite;
        }

        Button newSelectedButton = _selectionButtons[_currentIndex];
        newSelectedButton.GetComponent<Image>().sprite = _selectedSprite;
        _currentlySelectedButton = newSelectedButton;
    }

    public void OpenTab(int tabIndex)
    {
        foreach (var panel in _tabPanels)
        {
            panel.SetActive(false);
        }

        if (tabIndex >= 0 && tabIndex < _tabPanels.Count)
        {
            _tabPanels[tabIndex].SetActive(true);
        }

        for (int i = 0; i < _tabButtons.Count; i++)
        {
            if (i == tabIndex)
            {
                _tabButtons[i].GetComponent<Image>().sprite = _selectedTab;
            }
            else
            {
                _tabButtons[i].GetComponent<Image>().sprite = _unselectedTab;
            }
        }
    }
}
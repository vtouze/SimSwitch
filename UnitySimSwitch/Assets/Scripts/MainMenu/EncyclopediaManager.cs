using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the encyclopedia UI by dynamically creating buttons for each entry 
/// and displaying the selected entry's details in the UI.
/// </summary>
public class EncyclopediaManager : MonoBehaviour
{
    #region Fields

    [Header("UI Elements")]
    [Tooltip("Parent transform where dynamically generated buttons will be added.")]
    public Transform _buttonListParent;

    [Tooltip("Prefab used to create buttons for each encyclopedia entry.")]
    public GameObject _buttonPrefab;

    [Tooltip("Image used to display the cover of the selected encyclopedia entry.")]
    public Image _coverImage;

    [Tooltip("Text field to display the title of the selected encyclopedia entry.")]
    public TMP_Text _entryTitle;

    [Tooltip("Text field to display the short description of the selected encyclopedia entry.")]
    public TMP_Text _entryDescription;

    [Tooltip("Text field to display the detailed article of the selected encyclopedia entry.")]
    public TMP_Text _entryArticle;

    [Header("Scrollbar")]
    [Tooltip("Scrollbar used to control the scroll position of the encyclopedia UI.")]
    [SerializeField] private Scrollbar _scrollbar = null;

    [Header("Data")]
    [Tooltip("List of encyclopedia entries to be displayed.")]
    public List<EncyclopediaEntry> _entries;

    #endregion Fields

    #region Methods

    /// <summary>
    /// Initializes the UI by displaying the first entry and populating buttons.
    /// </summary>
    private void Awake()
    {
        DisplayEntry(_entries[0]);
        PopulateButtons();
    }

    /// <summary>
    /// Initializes the scrollbar position to the top when the scene starts.
    /// </summary>
    private void Start()
    {
        Canvas.ForceUpdateCanvases(); // Ensures UI layout is updated before setting the scrollbar value.
        _scrollbar.value = 1;
    }

    /// <summary>
    /// Creates buttons for each encyclopedia entry and assigns display logic to each button.
    /// </summary>
    private void PopulateButtons()
    {
        foreach (EncyclopediaEntry entry in _entries)
        {
            // Instantiate a button from the prefab and set its parent to the button list.
            GameObject newButton = Instantiate(_buttonPrefab, _buttonListParent);
            
            // Set the button's text to the entry's element name.
            newButton.GetComponentInChildren<TMP_Text>().text = entry._elementName;
            
            // Assign an onClick listener to display the corresponding entry when the button is clicked.
            newButton.GetComponent<Button>().onClick.AddListener(() => DisplayEntry(entry));
        }
    }

    /// <summary>
    /// Updates the UI to display the selected encyclopedia entry's details.
    /// </summary>
    /// <param name="entry">The selected encyclopedia entry to be displayed.</param>
    private void DisplayEntry(EncyclopediaEntry entry)
    {
        _coverImage.sprite = entry._coverImage;
        _entryTitle.text = entry._elementName;
        _entryDescription.text = entry._description;
        _entryArticle.text = entry._listArticles;
    }

    #endregion Methods
}
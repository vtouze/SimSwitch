using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the behavior of the newspaper UI, managing both the home menu (list of articles) 
/// and the body menu (detailed content of a selected article). Handles the navigation between menus,
/// manages scrollable content, and displays event content based on the user's interaction.
/// </summary>
public class NewspaperController : MonoBehaviour
{
    #region Fields
    [Tooltip("Reference to the NewspaperDisplay component that handles event content display.")]
    [SerializeField] private NewspaperDisplay _newspaperDisplay;

    [Tooltip("Array of NewspaperEvent objects representing the events to be displayed in the newspaper.")]
    [SerializeField] private NewspaperEvent[] _events;

    [Tooltip("Reference to the home menu GameObject (where the list of articles is shown).")]
    [SerializeField] private GameObject _homeMenu;

    [Tooltip("Reference to the body menu GameObject (where detailed article content is shown).")]
    [SerializeField] private GameObject _bodyMenu;

    [Tooltip("Reference to the panel holding the newspaper UI.")]
    public GameObject _newspaperPanel;

    [Tooltip("Reference to the home content GameObject that holds the content for the home menu.")]
    [SerializeField] private GameObject _homeContent;

    [Tooltip("Reference to the body content GameObject that holds the detailed content for an article.")]
    [SerializeField] private GameObject _bodyContent;

    [Tooltip("Scrollbar component for the home menu content.")]
    [SerializeField] private Scrollbar _homeScrollBar;

    [Tooltip("Scrollbar component for the body menu content.")]
    [SerializeField] private Scrollbar _bodyScrollBar;

    [Tooltip("The index of the currently selected event in the newspaper.")]
    private int _currentEventIndex = 0;
    #endregion Fields

    #region Methods
    /// <summary>
    /// Initializes the home menu by opening it and closing the body menu.
    /// </summary>
    private void Start()
    {
        ShowHomeMenu();
    }

    /// <summary>
    /// Opens the home menu and closes the body menu. Initializes the scroll rect for the home menu.
    /// </summary>
    public void ShowHomeMenu()
    {
        OpenMenu(_homeMenu);
        CloseMenu(_bodyMenu);

        InitScrollRect(_homeContent, _homeMenu, _homeScrollBar);
    }

    /// <summary>
    /// Opens the body menu, closes the home menu, and resets the body scrollbar to the top.
    /// </summary>
    public void ShowBodyMenu()
    {
        CloseMenu(_homeMenu);
        OpenMenu(_bodyMenu);
        _bodyScrollBar.value = 1; // Reset the body scrollbar to the top

        InitScrollRect(_bodyContent, _bodyMenu, _bodyScrollBar);
    }

    /// <summary>
    /// Initializes the ScrollRect with the specified content, viewport, and scrollbar.
    /// This setup ensures the scrolling functionality works properly for both menus.
    /// </summary>
    /// <param name="content">The content GameObject that holds the list of items to be scrolled.</param>
    /// <param name="viewport">The viewport GameObject that defines the visible area for the scrollable content.</param>
    /// <param name="scrollbar">The scrollbar component that controls the scrolling behavior.</param>
    public void InitScrollRect(GameObject content, GameObject viewport, Scrollbar scrollbar)
    {
        ScrollRect scrollRect = _newspaperPanel.gameObject.GetComponent<ScrollRect>();
        scrollRect.content = content.GetComponent<RectTransform>();
        scrollRect.viewport = viewport.GetComponent<RectTransform>();
        scrollRect.verticalScrollbar = scrollbar;
    }

    /// <summary>
    /// Displays the content of a specific event in the body menu by updating the NewspaperDisplay.
    /// Switches to the body menu to show detailed content for the selected event.
    /// </summary>
    /// <param name="eventIndex">The index of the event in the _events array to display.</param>
    public void ShowEventContent(int eventIndex)
    {
        _currentEventIndex = eventIndex;
        _newspaperDisplay.DisplayEvent(_events[_currentEventIndex]);
        ShowBodyMenu(); // Switch to the body menu to display detailed content
    }

    /// <summary>
    /// Activates the given menu GameObject.
    /// </summary>
    /// <param name="gameobject">The GameObject representing the menu to open.</param>
    public void OpenMenu(GameObject gameobject)
    {
        gameobject.SetActive(true);
    }

    /// <summary>
    /// Deactivates the given menu GameObject.
    /// </summary>
    /// <param name="gameObject">The GameObject representing the menu to close.</param>
    public void CloseMenu(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
    #endregion Methods
}
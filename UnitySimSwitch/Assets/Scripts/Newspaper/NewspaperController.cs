using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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

    [Tooltip("The animation image component to display the event's cover image.")]
    [SerializeField] private Image _animationImage;

    [Tooltip("The animation text component to display the event's title.")]
    [SerializeField] private TMP_Text _animationTitle;

    [Tooltip("The index of the currently selected event in the newspaper.")]
    private int _currentEventIndex = 0;

    [SerializeField] private GameObject _newspaperUpdates = null;

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
    public void AddRandomEventWithAnimation()
    {      
        if (_events.Length == 0) return;

        // Select a random event
        NewspaperEvent randomEvent = _events[Random.Range(0, _events.Length)];

        // Check if the event has already been added
        if (IsEventDuplicate(randomEvent))
        {
            Debug.Log("Event is a duplicate. Skipping.");
            return; // Exit if the event is already added
        }

        // Trigger the newspaper animation
        StartCoroutine(DisplayNewspaperAnimation(randomEvent));

        // Add the new event to the list of added events
        NewspaperHome.Instance.AddNewEvent(randomEvent);
    }

    private IEnumerator DisplayNewspaperAnimation(NewspaperEvent newEvent)
    {
        if (_newspaperPanel == null)
        {
            Debug.LogError("_newspaperPanel is not assigned.");
            yield break;
        }

        if (_newspaperUpdates == null)
        {
            Debug.LogError("_newspaperUpdates is not assigned.");
            yield break;
        }

        _newspaperPanel.SetActive(true);

        UpdateAnimationContent(newEvent);  // Set the content for the animation

        // Trigger the animation without adding the event again
        PlayAnimation();

        yield return new WaitForSeconds(10);

        Animator animator = _newspaperUpdates.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isOpening", false);
        }
        else
        {
            Debug.LogError("Animator component not found on _newspaperUpdates.");
        }

        if (NewspaperHome.Instance == null)
        {
            Debug.LogError("NewspaperHome instance is null.");
            yield break; // Exit coroutine to prevent further issues
        }
    }   

    /// <summary>
    /// Updates the animation content with the given newspaper event data.
    /// </summary>
    /// <param name="newspaperEvent">The event data to display in the animation.</param>
    public void UpdateAnimationContent(NewspaperEvent newspaperEvent)
    {
        // Update the image and title based on the event data
        _animationImage.sprite = newspaperEvent._coverImage;
        _animationTitle.text = newspaperEvent._eventName;
    }

    /// <summary>
    /// Adds a new newspaper event to the timeline and triggers the animation.
    /// </summary>
    /// <param name="newspaperEvent">The new event to add.</param>
    public void AddNewspaperEvent(NewspaperEvent newspaperEvent)
    {
        // Update animation content with the new event
        UpdateAnimationContent(newspaperEvent);

        // Trigger the animation (using Animator, Tweening, or Custom Logic)
        PlayAnimation();
    }

    /// <summary>
    /// Plays the newspaper animation.
    /// </summary>
    private void PlayAnimation()
    {
        Animator animator = _newspaperUpdates.GetComponent<Animator>();
        if (animator != null)
        {
            Debug.Log("Animator found, playing animation.");
            animator.SetBool("isOpening", true); // Ensure this triggers the correct animation
        }
        else
        {
            Debug.LogError("Animator not found on _newspaperUpdates.");
        }
    }

    /// <summary>
    /// Checks if the event has already been added to the newspaper timeline.
    /// </summary>
    /// <param name="newspaperEvent">The event to check for duplication.</param>
    /// <returns>True if the event already exists; otherwise, false.</returns>
    private bool IsEventDuplicate(NewspaperEvent newspaperEvent)
    {
        var addedEvents = NewspaperHome.Instance.GetAddedEvents();  // Get the list of added events

        foreach (var eventItem in addedEvents)
        {
            if (eventItem._eventName == newspaperEvent._eventName)
            {
                return true; // Event already exists, so it's a duplicate
            }
        }

        return false; // Event is unique
    }
    #endregion Methods
}
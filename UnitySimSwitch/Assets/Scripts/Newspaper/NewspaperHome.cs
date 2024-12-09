using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the creation and display of the home screen content for the newspaper. It instantiates UI elements
/// such as article titles, subtitles, covers, and "Read More" buttons for each newspaper event. It also links
/// each article to its respective content for easy navigation in the newspaper.
/// </summary>
public class NewspaperHome : MonoBehaviour
{
    [Tooltip("Reference to the NewspaperController that handles content display.")]
    [SerializeField] private NewspaperController _newspaperController;

    [Tooltip("Array of NewspaperEvent objects that represent all available events to display.")]
    [SerializeField] private NewspaperEvent[] _events;

    [Header("Prefabs")]
    [Tooltip("Prefab for the 'Read More' button.")]
    [SerializeField] private GameObject _readMore;

    [Tooltip("Prefab for the article title.")]
    [SerializeField] private GameObject _title;

    [Tooltip("Prefab for the article content container.")]
    [SerializeField] private GameObject _content;

    [Tooltip("Prefab for the news overlay element.")]
    [SerializeField] private GameObject _newsOverlay;

    [Tooltip("Prefab for the article container.")]
    [SerializeField] private GameObject _article;

    [Tooltip("Prefab for the subtitle text of an article.")]
    [SerializeField] private GameObject _subtitle;

    [Tooltip("Prefab for the background behind the subtitle.")]
    [SerializeField] private GameObject _subtitleBackground;

    [Tooltip("Prefab for the cover image of an article.")]
    [SerializeField] private GameObject _cover;

    public static NewspaperHome Instance;

    /// <summary>
    /// Instantiates the necessary UI elements for each newspaper event and sets up the UI hierarchy.
    /// It links each event's data to the appropriate UI components and sets up the "Read More" button
    /// to navigate to the event's full content when clicked.
    /// </summary>
    private void Start()
    {
        // Loop through each event and instantiate the necessary UI components
        for (int i = 0; i < _events.Length; i++)
        {
            int index = i;

            // Instantiate the prefabs for each article
            GameObject article = Instantiate(_article);
            GameObject title = Instantiate(_title);
            GameObject newsOverlay = Instantiate(_newsOverlay);
            GameObject cover = Instantiate(_cover);
            GameObject subtitleBackground = Instantiate(_subtitleBackground);
            GameObject subtitle = Instantiate(_subtitle);
            GameObject button = Instantiate(_readMore);

            // Set the parent-child relationships in the UI hierarchy
            article.transform.SetParent(_content.transform, false);
            title.transform.SetParent(article.transform, false);
            newsOverlay.transform.SetParent(article.transform, false);
            cover.transform.SetParent(newsOverlay.transform, false);
            subtitleBackground.transform.SetParent(newsOverlay.transform, false);
            subtitle.transform.SetParent(subtitleBackground.transform, false);
            button.transform.SetParent(article.transform, false);

            // Set the article's title, subtitle, and cover image using the event data
            title.GetComponentInChildren<TMP_Text>().text = _events[index]._eventName;
            subtitle.GetComponent<TMP_Text>().text = _events[index]._subheading;
            cover.GetComponent<Image>().sprite = _events[index]._coverImage;

            // Add a listener to the "Read More" button to display the full event content
            button.GetComponent<Button>().onClick.AddListener(() => _newspaperController.ShowEventContent(index));
        }
    }

    public void AddNewEvent(NewspaperEvent newEvent)
    {
        // Dynamically add a single event (reuse logic from `Start` method)
        GameObject article = Instantiate(_article);
        GameObject title = Instantiate(_title);
        GameObject newsOverlay = Instantiate(_newsOverlay);
        GameObject cover = Instantiate(_cover);
        GameObject subtitleBackground = Instantiate(_subtitleBackground);
        GameObject subtitle = Instantiate(_subtitle);
        GameObject button = Instantiate(_readMore);

        article.transform.SetParent(_content.transform, false);
        title.transform.SetParent(article.transform, false);
        newsOverlay.transform.SetParent(article.transform, false);
        cover.transform.SetParent(newsOverlay.transform, false);
        subtitleBackground.transform.SetParent(newsOverlay.transform, false);
        subtitle.transform.SetParent(subtitleBackground.transform, false);
        button.transform.SetParent(article.transform, false);

        title.GetComponentInChildren<TMP_Text>().text = newEvent._eventName;
        subtitle.GetComponent<TMP_Text>().text = newEvent._subheading;
        cover.GetComponent<Image>().sprite = newEvent._coverImage;

        button.GetComponent<Button>().onClick.AddListener(() =>
            _newspaperController.ShowEventContent(System.Array.IndexOf(_events, newEvent)));
    }
}
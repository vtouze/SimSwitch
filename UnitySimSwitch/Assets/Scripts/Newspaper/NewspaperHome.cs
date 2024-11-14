using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewspaperHome : MonoBehaviour
{
    [SerializeField] private NewspaperController _newspaperController;
    [SerializeField] private NewspaperEvent[] _events;
    
    [Header("Prefabs")]
    [SerializeField] private GameObject _readMore;
    [SerializeField] private GameObject _title;
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _newsOverlay;
    [SerializeField] private GameObject _article;
    [SerializeField] private GameObject _subtitle;
    [SerializeField] private GameObject _subtitleBackground;
    [SerializeField] private GameObject _cover;

    private void Start()
    {
        for (int i = 0; i < _events.Length; i++)
        {
            int index = i;

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

            title.GetComponentInChildren<TMP_Text>().text = _events[index]._eventName;
            subtitle.GetComponent<TMP_Text>().text = _events[index]._subheading;
            cover.GetComponent<Image>().sprite = _events[index]._coverImage;

            button.GetComponent<Button>().onClick.AddListener(() => _newspaperController.ShowEventContent(index));
        }
    }
}

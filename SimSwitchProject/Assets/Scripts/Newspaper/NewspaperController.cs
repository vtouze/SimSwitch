using UnityEngine;
using UnityEngine.UI;

public class NewspaperController : MonoBehaviour
{
    #region Fields
    [SerializeField] private NewspaperDisplay _newspaperDisplay;
    [SerializeField] private NewspaperEvent[] _events;
    [SerializeField] private GameObject _homeMenu;
    [SerializeField] private GameObject _bodyMenu;
    public GameObject _newspaperPanel;
    [SerializeField] private GameObject _homeContent;
    [SerializeField] private GameObject _bodyContent;
    [SerializeField] private Scrollbar _homeScrollBar;
    [SerializeField] private Scrollbar _bodyScrollBar;
    private int _currentEventIndex = 0;
    #endregion Fields

    #region Methods
    private void Start()
    {
        ShowHomeMenu();
    }

    public void ShowHomeMenu()
    {
        OpenMenu(_homeMenu);
        CloseMenu(_bodyMenu);

        InitScrollRect(_homeContent, _homeMenu, _homeScrollBar);
    }

    public void ShowBodyMenu()
    {
        CloseMenu(_homeMenu);
        OpenMenu(_bodyMenu);
        _bodyScrollBar.value = 1;

        InitScrollRect(_bodyContent, _bodyMenu, _bodyScrollBar);
    }

    public void InitScrollRect(GameObject content, GameObject viewport, Scrollbar scrollbar)
    {
        ScrollRect scrollRect = _newspaperPanel.gameObject.GetComponent<ScrollRect>();
        scrollRect.content = content.GetComponent<RectTransform>();
        scrollRect.viewport = viewport.GetComponent<RectTransform>();
        scrollRect.verticalScrollbar = scrollbar;
    }

    public void ShowEventContent(int eventIndex)
    {
        _currentEventIndex = eventIndex;
        _newspaperDisplay.DisplayEvent(_events[_currentEventIndex]);
        ShowBodyMenu();
    }

    public void OpenMenu(GameObject gameobject)
    {
        gameobject.SetActive(true);
    }

    public void CloseMenu(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
    #endregion Methods
}
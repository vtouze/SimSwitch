using UnityEngine;
public class NewspaperController : MonoBehaviour
{
    [SerializeField] private NewspaperDisplay _newspaperDisplay;
    [SerializeField] private NewspaperEvent[] _events;
    private int _currentEventIndex = 0;

    private void Start()
    {
        _newspaperDisplay.DisplayEvent(_events[_currentEventIndex]);
    }

    public void ShowNextEvent()
    {
        _currentEventIndex++;
        if (_currentEventIndex >= _events.Length)
        {
            _currentEventIndex = 0;
        }
        _newspaperDisplay.DisplayEvent(_events[_currentEventIndex]);
    }
}
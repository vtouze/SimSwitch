using UnityEngine;

public class FollowWaypoint : MonoBehaviour
{
    #region Fields
    RectTransform _goal;
    float _speed = 50.0f;
    float _accuracy = 5.0f;
    public GameObject _waypointsManager;
    private GameObject[] _waypoints;
    private GameObject _currentNode;
    private int _currentWaypoint = 0;
    private Graph _graph;
    private RectTransform _rectTransform;
    #endregion Fields

    #region Methods
    private void Start()
    {
        _waypoints = _waypointsManager.GetComponent<WaypointsManager>()._waypoints;
        _graph = _waypointsManager.GetComponent<WaypointsManager>()._graph;
        _currentNode = _waypoints[0];

        _rectTransform = GetComponent<RectTransform>();

        Invoke("GoToEast", 1);
    }

    public void GoToEast()
    {
        _graph.AStar(_currentNode, _waypoints[20]);
        _currentWaypoint = 0;
    }

    private void LateUpdate()
    {
        if (_graph._pathList.Count == 0 || _currentWaypoint == _graph._pathList.Count)
        {
            return;
        }

        if (Vector2.Distance(
            _graph._pathList[_currentWaypoint].GetId().GetComponent<RectTransform>().anchoredPosition,
            _rectTransform.anchoredPosition) < _accuracy)
        {
            _currentNode = _graph._pathList[_currentWaypoint].GetId();
            _currentWaypoint++;
        }

        if (_currentWaypoint < _graph._pathList.Count)
        {
            _goal = _graph._pathList[_currentWaypoint].GetId().GetComponent<RectTransform>();
            Vector2 direction = _goal.anchoredPosition - _rectTransform.anchoredPosition;
            direction.Normalize();

            _rectTransform.anchoredPosition += direction * _speed * Time.deltaTime;
        }
    }
    #endregion Methods
}
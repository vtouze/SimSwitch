using UnityEngine;

public class FollowWaypoint : MonoBehaviour
{
    #region Fields
    public enum VehicleType { Bike, Bus, Car }
    public VehicleType vehicleType;

    private RectTransform _goal;
    private float _baseSpeed;
    private float _speed;
    private float _accuracy = 1.0f;
    public GameObject _waypointsManager;
    private GameObject[] _waypoints;
    private GameObject _currentNode;
    private int _currentWaypoint = 0;
    private Graph _graph;
    private RectTransform _rectTransform;
    [SerializeField] private int _targetWaypoint = 20;
    private System.Random _random = new System.Random();
    private float _slowDownFactor = 0.25f;
    #endregion Fields

    #region Methods
    private void Start()
    {
        switch (vehicleType)
        {
            case VehicleType.Bike:
                _baseSpeed = 30.0f;
                break;
            case VehicleType.Bus:
                _baseSpeed = 40.0f;
                break;
            case VehicleType.Car:
                _baseSpeed = 50.0f;
                break;
        }
        _speed = _baseSpeed;

        _waypoints = _waypointsManager.GetComponent<WaypointsManager>()._waypoints;
        _graph = _waypointsManager.GetComponent<WaypointsManager>()._graph;
        _currentNode = _waypoints[0];

        _rectTransform = GetComponent<RectTransform>();

        Invoke("MoveTo", 1);
    }

    public void MoveTo()
    {
        _targetWaypoint = _random.Next(0, _waypoints.Length);
        Debug.Log("New target waypoint: " + _waypoints[_targetWaypoint].name);

        _graph.AStar(_currentNode, _waypoints[_targetWaypoint]);

        if (_graph._pathList.Count > 0)
        {
            _currentNode = _graph._pathList[0].GetId();
            _currentWaypoint = 1;
        }
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

            AdjustSpeedIfNeeded();

            _rectTransform.anchoredPosition += direction * _speed * Time.deltaTime;
        }

        if (_currentWaypoint == _graph._pathList.Count)
        {
            MoveTo();
        }
    }

    private void AdjustSpeedIfNeeded()
    {
        FollowWaypoint[] allVehicles = FindObjectsOfType<FollowWaypoint>();
        foreach (var vehicle in allVehicles)
        {
            if (vehicle != this)
            {
                float distanceToOtherVehicle = Vector2.Distance(_rectTransform.anchoredPosition, vehicle._rectTransform.anchoredPosition);
                if (distanceToOtherVehicle < 5.0f)
                {
                    _speed = _baseSpeed * _slowDownFactor;
                    return;
                }
            }
        }
        _speed = _baseSpeed;
    }
    #endregion Methods
}

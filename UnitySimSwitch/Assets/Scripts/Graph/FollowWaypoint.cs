using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class FollowWaypoint : MonoBehaviour
{
    #region Fields
    public enum VehicleType { Bike, Bus, Car }

    [Header("Vehicles")]
    public VehicleManager _vehicleManager = null;
    public VehicleType vehicleType;
    private RectTransform _goal;
    private float _baseSpeed;
    private float _speed;
    private float _accuracy = 1.0f;
    private List<Node> _vehiclePath = new List<Node>();
    private float _minDistanceToOtherVehicle = 100.0f;
    private float _speedCheckInterval = 0.5f;
    private float _speedCheckTimer = 0.0f;

    [Header("Waypoints")]
    public GameObject _waypointsManager;
    private GameObject[] _waypoints;
    private GameObject _currentNode;
    private int _currentWaypoint = 0;
    private Graph _graph;
    private RectTransform _rectTransform;
    private int _targetWaypoint = 20;
    private System.Random _random = new System.Random();

    [Header("Animations")]
    [SerializeField] private Animator _happyAnimator;
    [SerializeField] private Animator _angryAnimator;
    private float _minAnimationInterval = 25.0f;
    private float _maxAnimationInterval = 45.0f;

    [Header("Satisfaction")]
    [SerializeField] private SatisfactionBarController satisfactionUI;

    private int _speedChangeCount = 0;
    private const int MaxSpeedChangeCount = 5;

    #endregion Fields

    #region Properties
    public float Speed
    {
        get { return _speed; }
        set
        {
            _speed = Mathf.Clamp(value, 30f, 400f);
        }
    }

    public float BaseSpeed
    {
        get { return _baseSpeed; }
        set { _baseSpeed = value; }
    }
    #endregion Properties

    #region Methods
    private void Start()
    {
        _vehicleManager.RegisterVehicle(this);

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

        Speed = _baseSpeed;

        _waypoints = _waypointsManager.GetComponent<WaypointsManager>()._waypoints;
        _graph = _waypointsManager.GetComponent<WaypointsManager>()._graph;

        _rectTransform = GetComponent<RectTransform>();

        _currentNode = FindClosestWaypoint();
        MoveTo();

        StartCoroutine(DisplayRandomEmotion());

        satisfactionUI.ChangeSatisfaction(0);
    }

    #region Waypoints
    private GameObject FindClosestWaypoint()
    {
        GameObject closestWaypoint = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject waypoint in _waypoints)
        {
            float distance = Vector2.Distance(_rectTransform.anchoredPosition, waypoint.GetComponent<RectTransform>().anchoredPosition);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestWaypoint = waypoint;
            }
        }
        return closestWaypoint;
    }

    public void MoveTo()
    {
        if (GameManager.Instance.IsPaused == false)
        {
            foreach (Node node in _vehiclePath)
            {
                node.ClearPath();
            }

            _targetWaypoint = _random.Next(0, _waypoints.Length);

            bool pathFound = _graph.AStar(_currentNode, _waypoints[_targetWaypoint]);

            if (pathFound)
            {
                _vehiclePath = new List<Node>(_graph._pathList);

                if (_vehiclePath.Count > 0)
                {
                    _currentNode = _vehiclePath[0].GetId();
                    _currentWaypoint = 1;
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.IsPaused == false)
        {
            _speedCheckTimer += Time.deltaTime;
            if (_speedCheckTimer >= _speedCheckInterval)
            {
                AdjustSpeed();
                _speedCheckTimer = 0.0f;
            }

            if (_vehiclePath.Count == 0 || _currentWaypoint >= _vehiclePath.Count)
            {
                MoveTo();
                return;
            }

            if (Vector2.Distance(
                _vehiclePath[_currentWaypoint].GetId().GetComponent<RectTransform>().anchoredPosition,
                _rectTransform.anchoredPosition) < _accuracy)
            {
                _currentNode = _vehiclePath[_currentWaypoint].GetId();
                _currentWaypoint++;
            }

            if (_currentWaypoint < _vehiclePath.Count)
            {
                _goal = _vehiclePath[_currentWaypoint].GetId().GetComponent<RectTransform>();
                Vector2 direction = _goal.anchoredPosition - _rectTransform.anchoredPosition;
                direction.Normalize();

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                float rotationSpeed = _speed * 0.1f;
                _rectTransform.rotation = Quaternion.Slerp(_rectTransform.rotation, Quaternion.Euler(0, 0, angle + 180f), Time.deltaTime * rotationSpeed);

                _rectTransform.anchoredPosition += direction * _speed * Time.deltaTime;
            }
        }
    }


    private void AdjustSpeed()
    {
        FollowWaypoint closestVehicle = null;
        float closestDistance = float.MaxValue;

        foreach (var vehicle in VehicleManager.Instance.GetAllVehicles())
        {
            if (vehicle != this)
            {
                float distanceToOtherVehicleAlongPath = GetDistanceAlongPath(vehicle);
            
                if (distanceToOtherVehicleAlongPath < _minDistanceToOtherVehicle && distanceToOtherVehicleAlongPath < closestDistance)
                {
                    closestDistance = distanceToOtherVehicleAlongPath;
                    closestVehicle = vehicle;
                }
            }
        }

        if (closestVehicle != null)
        {
            Speed = closestVehicle.Speed;
            _speedChangeCount = 0;
        }
        else
        {
            Speed = BaseSpeed;
        }
    }
    private float GetDistanceAlongPath(FollowWaypoint vehicle)
    {
        float totalDistance = 0f;

        int currentWaypointIndex = vehicle._currentWaypoint;
        int myCurrentWaypointIndex = _currentWaypoint;

        while (myCurrentWaypointIndex < _vehiclePath.Count && currentWaypointIndex < vehicle._vehiclePath.Count)
        {
            GameObject myWaypoint = _vehiclePath[myCurrentWaypointIndex].GetId();
            GameObject otherWaypoint = vehicle._vehiclePath[currentWaypointIndex].GetId();

            float distanceBetweenWaypoints = Vector2.Distance(myWaypoint.GetComponent<RectTransform>().anchoredPosition, 
                                                           otherWaypoint.GetComponent<RectTransform>().anchoredPosition);
        
            totalDistance += distanceBetweenWaypoints;

            myCurrentWaypointIndex++;
            currentWaypointIndex++;
        }

        return totalDistance;
    }

    #endregion Waypoints

    #region Emotions
    private IEnumerator DisplayRandomEmotion()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minAnimationInterval, _maxAnimationInterval));
            if (GameManager.Instance.IsPaused)
            continue;

            if (_random.Next(2) == 0)
            {
                _happyAnimator.SetTrigger("DisplayHappy");
                yield return new WaitForSeconds(_happyAnimator.GetCurrentAnimatorStateInfo(0).length);
                _happyAnimator.ResetTrigger("DisplayHappy");

                satisfactionUI.ChangeSatisfaction(20);
            }
            else
            {
                _angryAnimator.SetTrigger("DisplayAnger");
                yield return new WaitForSeconds(_angryAnimator.GetCurrentAnimatorStateInfo(0).length);
                _angryAnimator.ResetTrigger("DisplayAnger");

                satisfactionUI.ChangeSatisfaction(-15);
            }
        }
    }
    #endregion Emotions
    #endregion Methods
}
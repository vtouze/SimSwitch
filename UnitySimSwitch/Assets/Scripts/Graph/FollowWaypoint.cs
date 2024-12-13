using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class FollowWaypoint : MonoBehaviour
{
    #region Fields
    public enum VehicleType { Bike, Bus, Car }

    [Header("Vehicles")]
    
    [Tooltip("The vehicle manager that manages all vehicles in the scene.")]
    public VehicleManager _vehicleManager = null;

    [Tooltip("The type of vehicle (Bike, Bus, Car).")]
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
    
    [Tooltip("The manager that handles all waypoints.")]
    public GameObject _waypointsManager;

    private GameObject[] _waypoints;
    private GameObject _currentNode;
    private int _currentWaypoint = 0;
    private Graph _graph;
    private RectTransform _rectTransform;
    private int _targetWaypoint = 20;
    private System.Random _random = new System.Random();

    [Header("Animations")]
    
    [SerializeField, Tooltip("Animator for the happy emotion.")]
    private Animator _happyAnimator;
    
    [SerializeField, Tooltip("Animator for the angry emotion.")]
    private Animator _angryAnimator;
    
    private float _minAnimationInterval = 25.0f;
    private float _maxAnimationInterval = 45.0f;

    [Header("Satisfaction")]
    
    [SerializeField, Tooltip("Satisfaction bar controller to update user satisfaction.")]
    private SatisfactionBarController satisfactionUI;

    private int _speedChangeCount = 0;
    private const int MaxSpeedChangeCount = 5;

    #endregion Fields

    #region Properties

    /// <summary>
    /// The speed of the vehicle, clamped between 30 and 400.
    /// </summary>
    public float Speed
    {
        get { return _speed; }
        set
        {
            _speed = Mathf.Clamp(value, 30f, 400f);  // Clamp speed between 30 and 400
        }
    }

    /// <summary>
    /// The base speed of the vehicle, set based on its type.
    /// </summary>
    public float BaseSpeed
    {
        get { return _baseSpeed; }
        set { _baseSpeed = value; }
    }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Initializes the vehicle, registers it with the vehicle manager, and sets up waypoints and animations.
    /// </summary>
    private void Start()
    {
        _vehicleManager.RegisterVehicle(this);

        // Set the base speed according to the vehicle type
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

        // Find the closest waypoint to start from
        _currentNode = FindClosestWaypoint();
        MoveTo();

        // Start random emotion display coroutine
        StartCoroutine(DisplayRandomEmotion());

        // Initialize satisfaction UI
        satisfactionUI.ChangeSatisfaction(0);
    }

    #region Waypoints

    /// <summary>
    /// Finds the closest waypoint to the vehicle's current position.
    /// </summary>
    /// <returns>The closest waypoint GameObject.</returns>
    private GameObject FindClosestWaypoint()
    {
        GameObject closestWaypoint = null;
        float closestDistance = Mathf.Infinity;

        // Loop through all waypoints to find the one closest to the vehicle's position
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

    /// <summary>
    /// Moves the vehicle to the next waypoint by finding a path using the A* algorithm.
    /// </summary>
    public void MoveTo()
    {
        if (GameManager.Instance.IsPaused == false)
        {
            // Clear previous path if any
            foreach (Node node in _vehiclePath)
            {
                node.ClearPath();
            }

            // Pick a random target waypoint
            _targetWaypoint = _random.Next(0, _waypoints.Length);

            // Find a path to the target waypoint
            bool pathFound = _graph.AStar(_currentNode, _waypoints[_targetWaypoint]);

            if (pathFound)
            {
                _vehiclePath = new List<Node>(_graph._pathList);

                // Set the first node in the path as the current node
                if (_vehiclePath.Count > 0)
                {
                    _currentNode = _vehiclePath[0].GetId();
                    _currentWaypoint = 1;
                }
            }
        }
    }

    /// <summary>
    /// Checks the distance to the next waypoint and moves the vehicle towards it.
    /// </summary>
    private void LateUpdate()
    {
        if (GameManager.Instance.IsPaused == false)
        {
            // Check the vehicle speed periodically
            _speedCheckTimer += Time.deltaTime;
            if (_speedCheckTimer >= _speedCheckInterval)
            {
                AdjustSpeed();
                _speedCheckTimer = 0.0f;
            }

            // If there are no waypoints or all waypoints are visited, move to the next target
            if (_vehiclePath.Count == 0 || _currentWaypoint >= _vehiclePath.Count)
            {
                MoveTo();
                return;
            }

            // If the vehicle is close enough to the current waypoint, move to the next one
            if (Vector2.Distance(
                _vehiclePath[_currentWaypoint].GetId().GetComponent<RectTransform>().anchoredPosition,
                _rectTransform.anchoredPosition) < _accuracy)
            {
                _currentNode = _vehiclePath[_currentWaypoint].GetId();
                _currentWaypoint++;
            }

            // Move towards the next waypoint
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

    /// <summary>
    /// Adjusts the vehicle's speed based on its proximity to other vehicles.
    /// </summary>
    private void AdjustSpeed()
    {
        FollowWaypoint closestVehicle = null;
        float closestDistance = float.MaxValue;

        // Check the distance to all other vehicles
        foreach (var vehicle in VehicleManager.Instance.GetAllVehicles())
        {
            if (vehicle != this)
            {
                float distanceToOtherVehicleAlongPath = GetDistanceAlongPath(vehicle);
            
                // If another vehicle is too close, adjust speed
                if (distanceToOtherVehicleAlongPath < _minDistanceToOtherVehicle && distanceToOtherVehicleAlongPath < closestDistance)
                {
                    closestDistance = distanceToOtherVehicleAlongPath;
                    closestVehicle = vehicle;
                }
            }
        }

        // If a vehicle is detected nearby, match its speed
        if (closestVehicle != null)
        {
            Speed = closestVehicle.Speed;
            _speedChangeCount = 0;
        }
        else
        {
            Speed = BaseSpeed;  // Reset to base speed
        }
    }

    /// <summary>
    /// Calculates the distance along the path between this vehicle and another vehicle.
    /// </summary>
    /// <param name="vehicle">The other vehicle to calculate distance to.</param>
    /// <returns>The total distance along the path.</returns>
    private float GetDistanceAlongPath(FollowWaypoint vehicle)
    {
        float totalDistance = 0f;

        int currentWaypointIndex = vehicle._currentWaypoint;
        int myCurrentWaypointIndex = _currentWaypoint;

        // Loop through the waypoints of both vehicles and calculate the distance
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

    /// <summary>
    /// Coroutine that displays random emotions (happy or angry) periodically.
    /// </summary>
    private IEnumerator DisplayRandomEmotion()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minAnimationInterval, _maxAnimationInterval));
            if (GameManager.Instance.IsPaused)
                continue;

            // Randomly display happy or angry emotion
            if (_random.Next(2) == 0)
            {
                _happyAnimator.SetTrigger("DisplayHappy");
                yield return new WaitForSeconds(_happyAnimator.GetCurrentAnimatorStateInfo(0).length);
                _happyAnimator.ResetTrigger("DisplayHappy");

                // Increase satisfaction when happy
                satisfactionUI.ChangeSatisfaction(20);
            }
            else
            {
                _angryAnimator.SetTrigger("DisplayAnger");
                yield return new WaitForSeconds(_angryAnimator.GetCurrentAnimatorStateInfo(0).length);
                _angryAnimator.ResetTrigger("DisplayAnger");

                // Decrease satisfaction when angry
                satisfactionUI.ChangeSatisfaction(-15);
            }
        }
    }

    #endregion Emotions

    #endregion Methods
}
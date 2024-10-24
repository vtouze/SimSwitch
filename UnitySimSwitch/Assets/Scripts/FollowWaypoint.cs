using System.Collections.Generic;
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
    private int _targetWaypoint = 20;
    private System.Random _random = new System.Random();
    private List<Node> _vehiclePath = new List<Node>();
    private float _minDistanceToOtherVehicle = 100.0f;
    private FollowWaypoint[] allVehicles;
    private float _speedCheckInterval = 0.5f;
    private float _speedCheckTimer = 0.0f;

    #endregion Fields

    #region Methods
    private void Start()
    {
        allVehicles = FindObjectsOfType<FollowWaypoint>();

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

        _rectTransform = GetComponent<RectTransform>();

        _currentNode = FindClosestWaypoint();
        MoveTo();
    }

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
        Debug.Log(vehicleType + " starting at closest waypoint: " + closestWaypoint.name);
        return closestWaypoint;
    }

    public void MoveTo()
    {
        if(GameManager.Instance.IsPaused == false)
        {
            foreach (Node node in _vehiclePath)
            {
                node.ClearPath();
            }

            if (_currentNode == null)
            {
                Debug.LogError(vehicleType + " current node is null!");
                return;
            }

            _targetWaypoint = _random.Next(0, _waypoints.Length);
            Debug.Log(vehicleType + " new target waypoint: " + _waypoints[_targetWaypoint]?.name ?? "null");

            if (_waypoints[_targetWaypoint] == null)
            {
                Debug.LogError(vehicleType + " target waypoint is null!");
                return;
            }

            bool pathFound = _graph.AStar(_currentNode, _waypoints[_targetWaypoint]);
            Debug.Log(vehicleType + " AStar path found: " + pathFound);

            if (pathFound)
            {
                _vehiclePath = new List<Node>(_graph._pathList);
        
                if (_vehiclePath.Count > 0)
                {
                    Debug.Log(vehicleType + " path length: " + _vehiclePath.Count);
                    _currentNode = _vehiclePath[0].GetId();
                    _currentWaypoint = 1;
                    Debug.Log(vehicleType + " starting at: " + _currentNode.name);
                }
                else
                {
                    Debug.LogError(vehicleType + " AStar returned an empty path!");
                }
            }
            else
            {
                Debug.LogError(vehicleType + " failed to find a path!");
            }
        }
    }

    private void LateUpdate()
    {
        if(GameManager.Instance.IsPaused == false)
        {
            _speedCheckTimer += Time.deltaTime;
            if (_speedCheckTimer >= _speedCheckInterval)
            {
                AdjustSpeedIfNeeded();
                _speedCheckTimer = 0.0f;
            }

            if (_vehiclePath.Count == 0 || _currentWaypoint >= _vehiclePath.Count)
            {
                Debug.Log(vehicleType + " has no path or reached the end of path. Reassigning path...");
                MoveTo();
                return;
            }

            if (Vector2.Distance(
                _vehiclePath[_currentWaypoint].GetId().GetComponent<RectTransform>().anchoredPosition,
                _rectTransform.anchoredPosition) < _accuracy)
            {
                Debug.Log(vehicleType + " reached waypoint: " + _currentWaypoint);

                _currentNode = _vehiclePath[_currentWaypoint].GetId();
                _currentWaypoint++;

                Debug.Log(vehicleType + " moving to next waypoint: " + _currentWaypoint);
            }

            if (_currentWaypoint < _vehiclePath.Count)
            {
                _goal = _vehiclePath[_currentWaypoint].GetId().GetComponent<RectTransform>();
                Vector2 direction = _goal.anchoredPosition - _rectTransform.anchoredPosition;
                direction.Normalize();

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                _rectTransform.rotation = Quaternion.Slerp(_rectTransform.rotation, Quaternion.Euler(0, 0, angle + 180f), Time.deltaTime * 5f);


                _rectTransform.anchoredPosition += direction * _speed * Time.deltaTime;
            }
        }
    }


    private void AdjustSpeedIfNeeded()
    {
        FollowWaypoint closestVehicle = null;
        float closestDistance = float.MaxValue;

        foreach (var vehicle in allVehicles)
        {
            if (vehicle != this)
            {
                float distanceToOtherVehicle = Vector2.Distance(_rectTransform.anchoredPosition, vehicle._rectTransform.anchoredPosition);
                if (distanceToOtherVehicle < _minDistanceToOtherVehicle && distanceToOtherVehicle < closestDistance)
                {
                    closestDistance = distanceToOtherVehicle;
                    closestVehicle = vehicle;
                }
            }
        }

        if (closestVehicle != null)
        {
            Debug.Log(vehicleType + " slowing down to match speed with another vehicle. Distance: " + closestDistance);
            _speed = closestVehicle._speed;
        }
        else
        {
            _speed = _baseSpeed;
        }
    }
    #endregion Methods
}
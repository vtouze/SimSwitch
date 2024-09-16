using UnityEngine;

public class VehicleController : MonoBehaviour
{
    #region Fields
    public Transform[] _waypoints;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _waypointThreshold = 0.5f;
    private int _currentWaypointIndex = 0;

    #endregion Fields

    #region Methods
    void Update()
    {
        if (IsOnRoad())
        {
            MoveTowardsWaypoint();
        }
        else
        {
            Debug.Log(gameObject.name + " : is not on the road.");
        }
    }

    void MoveTowardsWaypoint()
    {
        if (_waypoints.Length == 0) return;

        Transform targetWaypoint = _waypoints[_currentWaypointIndex];

        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, _moveSpeed * Time.deltaTime);

        Vector3 directionToWaypoint = (targetWaypoint.position - transform.position).normalized;

        RotateVehicle(directionToWaypoint);

        float distanceToWaypoint = Vector3.Distance(transform.position, targetWaypoint.position);
        if (distanceToWaypoint < _waypointThreshold)
        {
            _currentWaypointIndex++;

            if (_currentWaypointIndex >= _waypoints.Length)
            {
                _currentWaypointIndex = 0;
            }
        }
    }

    void RotateVehicle(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

            float adjustedAngle = angle + 90f;

            if (Mathf.Abs(direction.z) > Mathf.Abs(direction.x)) 
            {
                adjustedAngle += 180f;
            }

            float snappedAngle = Mathf.Round(adjustedAngle / 90f) * 90f;

            transform.rotation = Quaternion.Euler(0, snappedAngle, 0);
        }
    }


    bool IsOnRoad()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 1f, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f))
        {
            if (hit.collider != null && hit.collider.CompareTag("road"))
            {
                return true;
            }
        }

        return false;
    }
    #endregion Methods
}
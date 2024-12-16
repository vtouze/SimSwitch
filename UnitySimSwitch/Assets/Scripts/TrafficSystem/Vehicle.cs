using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public float Speed = 5f;
    public string CurrentDistrict;
    public string DestinationDistrict;

    private Transform _destinationPoint;

    private void Start()
    {
        SetDestination(DestinationDistrict);
    }

    private void Update()
    {
        if (_destinationPoint != null)
        {
            MoveTowardsDestination();
        }
    }

    public void SetDestination(string districtName)
    {
        if (districtName == CurrentDistrict)
        {
            Debug.Log("Vehicle is staying in the current district.");
            _destinationPoint = null;
            return;
        }

        _destinationPoint = DistrictManager.Instance.GetEntryPoint(districtName);

        if (_destinationPoint != null)
        {
            Debug.Log($"Vehicle is heading to district: {districtName}");
            DestinationDistrict = districtName;
        }
    }

    private void MoveTowardsDestination()
    {
        Vector3 direction = (_destinationPoint.position - transform.position).normalized;
        transform.position += direction * Speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, _destinationPoint.position) < 0.1f)
        {
            ReachDestination();
        }
    }

    private void ReachDestination()
    {
        Debug.Log($"Vehicle has arrived at {DestinationDistrict}");
        CurrentDistrict = DestinationDistrict;
        _destinationPoint = null;

        TrafficManager.Instance.AssignRandomDestination(this);
    }
}
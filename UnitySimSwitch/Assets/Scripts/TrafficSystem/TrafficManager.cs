using System.Collections.Generic;
using UnityEngine;

public class TrafficManager : MonoBehaviour
{
    public static TrafficManager Instance;

    public List<Vehicle> Vehicles;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        foreach (var vehicle in Vehicles)
        {
            AssignRandomDestination(vehicle);
        }
    }

    public void AssignRandomDestination(Vehicle vehicle)
    {
        if (DistrictManager.Instance.Districts.Count == 0) return;

        string currentDistrict = vehicle.CurrentDistrict;

        // Get a random district that isn't the current one
        List<string> availableDistricts = new List<string>();
        foreach (var district in DistrictManager.Instance.Districts)
        {
            if (district.Name != currentDistrict)
            {
                availableDistricts.Add(district.Name);
            }
        }

        if (availableDistricts.Count > 0)
        {
            string randomDistrict = availableDistricts[Random.Range(0, availableDistricts.Count)];
            vehicle.SetDestination(randomDistrict);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public static VehicleManager Instance { get; private set; }

    private List<FollowWaypoint> vehicles = new List<FollowWaypoint>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterVehicle(FollowWaypoint vehicle)
    {
        if (!vehicles.Contains(vehicle))
        {
            vehicles.Add(vehicle);
        }
    }

    public void UnregisterVehicle(FollowWaypoint vehicle)
    {
        if (vehicles.Contains(vehicle))
        {
            vehicles.Remove(vehicle);
        }
    }

    public List<FollowWaypoint> GetAllVehicles()
    {
        return vehicles;
    }

    public void ChangeSpeedForAllVehicles(float multiplier)
    {
        foreach (var vehicle in vehicles)
        {
            vehicle.Speed *= multiplier;
            vehicle.BaseSpeed = vehicle.Speed;
            vehicle.Speed = Mathf.Clamp(vehicle.Speed, 30f, 400f);
        }
    }

    public void ResetAllVehicles()
    {
        foreach (var vehicle in vehicles)
        {
            vehicle.Speed = vehicle.BaseSpeed;
        }
    }
}
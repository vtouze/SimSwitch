using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    [Tooltip("Singleton instance of the VehicleManager.")]
    public static VehicleManager Instance { get; private set; }

    [Tooltip("List of vehicles currently being managed.")]
    private List<FollowWaypoint> vehicles = new List<FollowWaypoint>();

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// Ensures that there is only one instance of VehicleManager (Singleton pattern).
    /// </summary>
    private void Awake()
    {
        // If there is no instance of VehicleManager, set this instance as the singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // If an instance already exists, destroy the current game object to enforce singleton
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Registers a vehicle by adding it to the vehicle list if not already registered.
    /// </summary>
    /// <param name="vehicle">The vehicle to register.</param>
    public void RegisterVehicle(FollowWaypoint vehicle)
    {
        // Add the vehicle to the list if it is not already present
        if (!vehicles.Contains(vehicle))
        {
            vehicles.Add(vehicle);
        }
    }

    /// <summary>
    /// Unregisters a vehicle by removing it from the vehicle list if it exists.
    /// </summary>
    /// <param name="vehicle">The vehicle to unregister.</param>
    public void UnregisterVehicle(FollowWaypoint vehicle)
    {
        // Remove the vehicle from the list if it is present
        if (vehicles.Contains(vehicle))
        {
            vehicles.Remove(vehicle);
        }
    }

    /// <summary>
    /// Gets all the vehicles currently registered with the manager.
    /// </summary>
    /// <returns>A list of all registered vehicles.</returns>
    public List<FollowWaypoint> GetAllVehicles()
    {
        return vehicles;
    }

    /// <summary>
    /// Changes the speed of all vehicles by a given multiplier.
    /// </summary>
    /// <param name="multiplier">The factor by which to change the speed of all vehicles.</param>
    public void ChangeSpeedForAllVehicles(float multiplier)
    {
        // Iterate through each vehicle and apply the speed multiplier
        foreach (var vehicle in vehicles)
        {
            vehicle.Speed *= multiplier;  // Change the vehicle's speed
            vehicle.BaseSpeed = vehicle.Speed;  // Update the base speed
            // Ensure the speed is within a reasonable range (30 to 400)
            vehicle.Speed = Mathf.Clamp(vehicle.Speed, 30f, 400f);
        }
    }

    /// <summary>
    /// Resets the speed of all vehicles to their base speed.
    /// </summary>
    public void ResetAllVehicles()
    {
        // Iterate through each vehicle and reset its speed to the base speed
        foreach (var vehicle in vehicles)
        {
            vehicle.Speed = vehicle.BaseSpeed;
        }
    }
}
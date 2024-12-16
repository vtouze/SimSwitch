using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    public GameObject VehiclePrefab;
    public Transform SpawnPoint;
    public int InitialVehicleCount = 5;

    private void Start()
    {
        for (int i = 0; i < InitialVehicleCount; i++)
        {
            SpawnVehicle();
        }
    }

    public void SpawnVehicle()
    {
        GameObject newVehicle = Instantiate(VehiclePrefab, SpawnPoint.position, Quaternion.identity);
        Vehicle vehicleComponent = newVehicle.GetComponent<Vehicle>();

        // Assign the vehicle to a random district initially
        string randomDistrict = GetRandomDistrict();
        vehicleComponent.CurrentDistrict = randomDistrict;

        // Add the vehicle to the traffic system
        TrafficManager.Instance.Vehicles.Add(vehicleComponent);

        Debug.Log($"Spawned a vehicle in {randomDistrict}");
    }

    private string GetRandomDistrict()
    {
        var districts = DistrictManager.Instance.Districts;
        return districts[Random.Range(0, districts.Count)].Name;
    }
}
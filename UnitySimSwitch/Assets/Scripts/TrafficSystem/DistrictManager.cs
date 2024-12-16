using System.Collections.Generic;
using UnityEngine;

public class DistrictManager : MonoBehaviour
{
    public static DistrictManager Instance;

    [System.Serializable]
    public class District
    {
        public string Name;
        public Transform EntryPoint; // The entry point for vehicles entering the district
    }

    public List<District> Districts;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public Transform GetEntryPoint(string districtName)
    {
        foreach (var district in Districts)
        {
            if (district.Name == districtName)
            {
                return district.EntryPoint;
            }
        }
        Debug.LogError($"District '{districtName}' not found!");
        return null;
    }
}
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager Instance { get; private set; }

    private Dictionary<RoadSelection, PublicWorksType> roadTransportMapping = new Dictionary<RoadSelection, PublicWorksType>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("ConstructionManager Instance assigned.");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Duplicate ConstructionManager instance destroyed.");
        }
    }

    public void AddTransportType(RoadSelection road, PublicWorksType type)
    {
        if (!road.isUnderConstruction)
        {
            roadTransportMapping[road] = type;
            Debug.Log($"Added transport type: {type}, Is under construction: {road.isUnderConstruction}");
        }
        else
        {
            Debug.LogWarning($"Road is under construction and cannot be assigned a new transport type.");
        }
    }

    public void RemoveTransportType(RoadSelection road)
    {
        if (roadTransportMapping.ContainsKey(road))
        {
            roadTransportMapping.Remove(road);
        }
    }

    public void ClearTransportTypes()
    {
        roadTransportMapping.Clear();
    }

    public int GetSelectedTransportCount()
    {
        return roadTransportMapping.Count;
    }

    public string GetSelectedTransportTypesAsString()
    {
        Dictionary<PublicWorksType, int> transportCount = new Dictionary<PublicWorksType, int>();

        foreach (var type in roadTransportMapping.Values)
        {
            if (!transportCount.ContainsKey(type))
            {
                transportCount[type] = 1;
            }
            else
            {
                transportCount[type]++;
            }
        }

        List<string> transportNames = new List<string>();
        foreach (var transport in transportCount)
        {
            string name = transport.Key.ToString();
            int count = transport.Value;
            transportNames.Add(count > 1 ? $"{name} ({count})" : name);
        }

        return string.Join(" and ", transportNames);
    }

    public (float totalCost, float totalDuration) GetTotalCostAndDuration(Dictionary<PublicWorksType, RoadsEntries> transportEntries)
    {
        float totalCost = 0f;
        float totalDuration = 0f;

        foreach (var type in roadTransportMapping.Values)
        {
            if (transportEntries.ContainsKey(type))
            {
                totalCost += transportEntries[type]._cost;
                totalDuration += transportEntries[type]._duration;
            }
        }

        return (totalCost, totalDuration);
    }
}
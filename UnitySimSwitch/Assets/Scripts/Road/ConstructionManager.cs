using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager Instance { get; private set; }

    public List<PublicWorksType> SelectedTransportTypes { get; private set; } = new List<PublicWorksType>();

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

    public void AddTransportType(PublicWorksType type)
    {
        SelectedTransportTypes.Add(type);
    }

    public void RemoveTransportType(PublicWorksType type)
    {
        if (SelectedTransportTypes.Contains(type))
        {
            SelectedTransportTypes.Remove(type);
        }
    }


    public string GetSelectedTransportTypesAsString()
    {
        var transportCount = new Dictionary<PublicWorksType, int>();

        foreach (var transport in SelectedTransportTypes)
        {
            if (!transportCount.ContainsKey(transport))
            {
                transportCount[transport] = 1;
            }
            else
            {
                transportCount[transport]++;
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

        foreach (var type in SelectedTransportTypes)
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
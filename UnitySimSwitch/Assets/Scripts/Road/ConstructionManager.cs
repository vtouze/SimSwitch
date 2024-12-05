using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager Instance { get; private set; }

    private readonly Dictionary<RoadSelection, PublicWorksType> roadTransportMapping = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    #region Public Methods

    public void AddTransportType(RoadSelection road, PublicWorksType type)
    {
        roadTransportMapping[road] = type;
    }

    public void RemoveTransportType(RoadSelection road)
    {
        roadTransportMapping.Remove(road);
    }

    public string GetSelectedTransportTypesAsString()
    {
        var transportTypes = roadTransportMapping.Values.Distinct();
        return string.Join(", ", transportTypes);
    }

    public int GetSelectedTransportCount() => roadTransportMapping.Count;

    public (float totalCost, float totalDuration) GetTotalCostAndDuration(Dictionary<PublicWorksType, RoadsEntries> transportEntries)
    {
        return roadTransportMapping.Values.Aggregate((totalCost: 0f, totalDuration: 0f), (acc, type) =>
        {
            if (transportEntries.TryGetValue(type, out var entry))
            {
                acc.totalCost += entry._cost;
                acc.totalDuration += entry._duration;
            }
            return acc;
        });
    }

    #endregion Public Methods
}
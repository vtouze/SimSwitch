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
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Dictionary<PublicWorksType, int> GetConstructionCountsByType()
    {
        var counts = new Dictionary<PublicWorksType, int>();

        foreach (var type in roadTransportMapping.Values)
        {
            if (counts.ContainsKey(type))
            {
                counts[type]++;
            }
            else
            {
                counts[type] = 1;
            }
        }

        return counts;
    }


    #region Public Methods
    public void AddTransportType(RoadSelection road, PublicWorksType type) => roadTransportMapping[road] = type;

    public void RemoveTransportType(RoadSelection road) => roadTransportMapping.Remove(road);

    public string GetSelectedTransportTypesAsString() => 
        string.Join(", ", roadTransportMapping.Values.Distinct());

    public int GetSelectedTransportCount() => roadTransportMapping.Count;

    public (float totalCost, float totalDuration) GetTotalCostAndDuration(Dictionary<PublicWorksType, RoadsEntries> transportEntries)
    {
        float totalCost = 0f, totalDuration = 0f;
        
        foreach (var type in roadTransportMapping.Values)
        {
            if (transportEntries.TryGetValue(type, out RoadsEntries entry))
            {
                totalCost += entry._cost;
                totalDuration += entry._duration;
            }
        }
        
        return (totalCost, totalDuration);
    }

    #endregion Public Methods
}
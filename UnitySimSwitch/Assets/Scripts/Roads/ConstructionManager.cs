using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages the mapping between road selections and their assigned public works types. 
/// Provides utilities to retrieve data related to ongoing constructions.
/// </summary>
public class ConstructionManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the ConstructionManager.
    /// </summary>
    public static ConstructionManager Instance { get; private set; }

    /// <summary>
    /// Maps each road to its associated public works type.
    /// </summary>
    private readonly Dictionary<RoadSelection, PublicWorksType> roadTransportMapping = new();

    private void Awake()
    {
        // Ensure the ConstructionManager is a singleton.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist this object across scenes.
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate managers.
        }
    }

    /// <summary>
    /// Calculates the number of constructions for each public works type.
    /// </summary>
    /// <returns>A dictionary mapping each public works type to the count of its constructions.</returns>
    public Dictionary<PublicWorksType, int> GetConstructionCountsByType()
    {
        var counts = new Dictionary<PublicWorksType, int>();

        // Iterate through all mapped types and count occurrences.
        foreach (var type in roadTransportMapping.Values)
        {
            if (counts.ContainsKey(type))
            {
                counts[type]++; // Increment count for existing type.
            }
            else
            {
                counts[type] = 1; // Initialize count for new type.
            }
        }

        return counts;
    }

    #region Public Methods

    /// <summary>
    /// Adds a road to the mapping with its associated public works type.
    /// </summary>
    /// <param name="road">The road to be added.</param>
    /// <param name="type">The public works type assigned to the road.</param>
    public void AddTransportType(RoadSelection road, PublicWorksType type) => roadTransportMapping[road] = type;

    /// <summary>
    /// Removes a road from the mapping.
    /// </summary>
    /// <param name="road">The road to be removed.</param>
    public void RemoveTransportType(RoadSelection road) => roadTransportMapping.Remove(road);

    /// <summary>
    /// Retrieves a comma-separated string of unique selected public works types.
    /// </summary>
    /// <returns>A string listing selected public works types.</returns>
    public string GetSelectedTransportTypesAsString() =>
        string.Join(", ", roadTransportMapping.Values.Distinct());

    /// <summary>
    /// Gets the total number of selected roads.
    /// </summary>
    /// <returns>The count of selected roads.</returns>
    public int GetSelectedTransportCount() => roadTransportMapping.Count;

    /// <summary>
    /// Calculates the total cost and duration of all selected constructions.
    /// </summary>
    /// <param name="transportEntries">Dictionary containing cost and duration data for each public works type.</param>
    /// <returns>A tuple containing the total cost and duration.</returns>
    public (float totalCost, float totalDuration) GetTotalCostAndDuration(Dictionary<PublicWorksType, RoadsEntry> transportEntries)
    {
        float totalCost = 0f, totalDuration = 0f;

        // Iterate through all mapped types and sum up costs and durations.
        foreach (var type in roadTransportMapping.Values)
        {
            if (transportEntries.TryGetValue(type, out RoadsEntry entry))
            {
                totalCost += entry._cost; // Accumulate cost for the type.
                totalDuration += entry._duration; // Accumulate duration for the type.
            }
        }

        return (totalCost, totalDuration);
    }

    #endregion Public Methods
}
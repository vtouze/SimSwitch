using UnityEngine;

public class RadarChartController : MonoBehaviour
{
    [SerializeField] private UIRadarChart[] uiRadarCharts; // Array to store multiple UIRadarCharts

    private void Start()
    {
        foreach (UIRadarChart uiRadarChart in uiRadarCharts)
        {
            int numberOfEdges = (int)GetNumberOfEdgesForRadarChart(uiRadarChart); // Explicit cast from enum to int

            // Create and initialize RadarStats
            RadarStats stats = gameObject.AddComponent<RadarStats>(); // Dynamically adding RadarStats component

            // Initialize the radarStats array with the correct number of edges
            stats.radarStats = new RadarStat[numberOfEdges];

            // Assign values according to the selected enum
            switch (numberOfEdges)
            {
                case 3:
                    InitializeStatsWithEnum(stats, typeof(ERadarChart.RadarChart3Edges));
                    break;
                case 5:
                    InitializeStatsWithEnum(stats, typeof(ERadarChart.RadarChart5Edges));
                    break;
                case 9:
                    InitializeStatsWithEnum(stats, typeof(ERadarChart.RadarChart9Edges));
                    break;
                default:
                    Debug.LogWarning("Unsupported number of edges.");
                    break;
            }

            // Set the stats to the UIRadarChart
            uiRadarChart.SetStats(stats);
        }
    }

    // Example function to determine the number of edges for each radar chart (customize as needed)
    private ERadarChart.NumberOfEdges GetNumberOfEdgesForRadarChart(UIRadarChart radarChart)
    {
        return radarChart.numberOfEdges; // Assuming `UIRadarChart` has a numberOfEdges field of type `NumberOfEdges`
    }

    // Function to initialize stats with values from an enum
    private void InitializeStatsWithEnum(RadarStats stats, System.Type enumType)
    {
        int index = 0;
        foreach (var enumValue in System.Enum.GetValues(enumType))
        {
            stats.radarStats[index] = new RadarStat
            {
                Name = enumValue.ToString(),
                Value = (int)enumValue // Assigning the value from the enum
            };
            index++;
        }
    }
}
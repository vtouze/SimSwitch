using UnityEngine;

/// <summary>
/// Controls the initialization and setup of radar charts.
/// </summary>
public class RadarChartController : MonoBehaviour
{
    [Tooltip("Array of radar charts to initialize and control.")]
    [SerializeField] private UIRadarChart[] uiRadarCharts;

    private void Start()
    {
        // Initialize radar charts with corresponding stats.
        foreach (UIRadarChart uiRadarChart in uiRadarCharts)
        {
            int numberOfEdges = (int)GetNumberOfEdgesForRadarChart(uiRadarChart);

            // Dynamically create and attach RadarStats component.
            RadarStats stats = gameObject.AddComponent<RadarStats>();
            stats.radarStats = new RadarStat[numberOfEdges];

            // Initialize stats based on number of edges.
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

            // Assign stats to radar chart.
            uiRadarChart.SetStats(stats);
        }
    }

    /// <summary>
    /// Determines the number of edges for a radar chart.
    /// </summary>
    private ERadarChart.NumberOfEdges GetNumberOfEdgesForRadarChart(UIRadarChart radarChart)
    {
        return radarChart.numberOfEdges;
    }

    /// <summary>
    /// Initializes radar stats based on the provided enum type.
    /// </summary>
    private void InitializeStatsWithEnum(RadarStats stats, System.Type enumType)
    {
        int index = 0;
        foreach (var enumValue in System.Enum.GetValues(enumType))
        {
            stats.radarStats[index] = new RadarStat
            {
                Name = enumValue.ToString(),
                Value = (int)enumValue
            };
            index++;
        }
    }
}
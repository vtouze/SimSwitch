using UnityEngine;

public class RadarChartController : MonoBehaviour {

    [SerializeField] private UIRadarChart uiRadarChart;

    private void Start() {
        int numberOfEdges = 9; // Example number of edges (3, 5, or 9)

        // Create and initialize RadarStats
        RadarStats stats = gameObject.AddComponent<RadarStats>(); // Adding RadarStats component dynamically

        // Initialize the radarStats array with the correct number of edges
        stats.radarStats = new RadarStat[numberOfEdges];
        for (int i = 0; i < numberOfEdges; i++) {
            stats.radarStats[i] = new RadarStat {
                Name = $"Stat {i + 1}", // Name for each stat (optional)
                Value = Random.Range(0, 20)
            };
        }

        // Set the stats to the UIRadarChart
        uiRadarChart.SetStats(stats);
    }
}
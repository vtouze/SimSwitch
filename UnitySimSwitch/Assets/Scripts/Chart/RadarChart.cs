using UnityEngine;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Manages the creation and visualization of a radar chart to compare transportation methods.
/// </summary>
public class RadarChart : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The canvas containing the radar chart UI elements.")]
    public Canvas canvas;

    [Tooltip("The container for radar chart elements.")]
    public GameObject radarChartContainer;

    [Tooltip("The text component displaying the chart title.")]
    public TMP_Text chartTitle;

    [Tooltip("Prefab used to create axis labels dynamically.")]
    public TMP_Text axisLabelPrefab;

    [Tooltip("Prefab used to create legend text entries dynamically.")]
    public TMP_Text legendTextPrefab;

    [Header("Radar Data")]
    [Tooltip("Data points representing car statistics.")]
    public float[] carData = new float[9];

    [Tooltip("Data points representing bike statistics.")]
    public float[] bikeData = new float[9];

    [Tooltip("Data points representing bus statistics.")]
    public float[] busData = new float[9];

    [Header("Visuals")]
    [Tooltip("Prefab used to create LineRenderer elements for the radar chart.")]
    public GameObject lineRendererPrefab;

    [Tooltip("Color representing car data.")]
    public Color carColor = Color.red;

    [Tooltip("Color representing bike data.")]
    public Color bikeColor = Color.green;

    [Tooltip("Color representing bus data.")]
    public Color busColor = Color.blue;

    [Header("Materials")]
    [Tooltip("Material used for the radar chart lines.")]
    [SerializeField] private Material lineMaterial;

    // Lists to store dynamically created elements
    private List<TMP_Text> axisLabels = new List<TMP_Text>();
    private List<LineRenderer> lineRenderers = new List<LineRenderer>();

    /// <summary>
    /// Initializes the radar chart and its legend on start.
    /// </summary>
    private void Start()
    {
        CreateChart();
        CreateLegend();
    }

    /// <summary>
    /// Creates the radar chart by setting up axis labels and plotting data lines.
    /// </summary>
    private void CreateChart()
    {
        // Set the chart title if the reference is assigned.
        if (chartTitle != null)
        {
            chartTitle.text = "Transportation Comparison"; // Set the chart's title
        }

        // Define the axis names.
        string[] axisNames = { "Habits", "Norm", "Potential", "Fast", "Economical", "Ecological", "Comfortable", "Safety", "Easy" };
        float angle = 360f / axisNames.Length; // Calculate the angle between axes.
        float labelDistance = 220f; // Distance from the center for axis labels.

        // Create axis labels around the chart.
        for (int i = 0; i < axisNames.Length; i++)
        {
            TMP_Text axisLabel = Instantiate(axisLabelPrefab, radarChartContainer.transform);
            axisLabel.text = axisNames[i]; // Set the axis label text.
            axisLabels.Add(axisLabel);

            // Calculate label position using trigonometry.
            float x = Mathf.Cos(Mathf.Deg2Rad * (i * angle)) * labelDistance;
            float y = Mathf.Sin(Mathf.Deg2Rad * (i * angle)) * labelDistance;
            axisLabel.transform.localPosition = new Vector3(x, y, 0); // Position the label.
        }

        // Create radar lines for each data set.
        CreateRadarLine(carData, carColor, "Car");
        CreateRadarLine(bikeData, bikeColor, "Bike");
        CreateRadarLine(busData, busColor, "Bus");
    }

    /// <summary>
    /// Creates a radar line representing a data set.
    /// </summary>
    /// <param name="data">Array of data points.</param>
    /// <param name="lineColor">Color of the line.</param>
    /// <param name="lineName">Name for the line.</param>
    private void CreateRadarLine(float[] data, Color lineColor, string lineName)
    {
        LineRenderer lineRenderer = Instantiate(lineRendererPrefab).GetComponent<LineRenderer>();
        lineRenderers.Add(lineRenderer);
        lineRenderer.transform.SetParent(radarChartContainer.transform, false);

        // Configure LineRenderer properties.
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = 2f;
        lineRenderer.endWidth = 2f;

        // Apply material if assigned.
        if (lineMaterial != null)
        {
            lineRenderer.material = lineMaterial;
        }
        else
        {
            Debug.LogWarning("Line material is not assigned! Please assign a material in the inspector.");
        }

        // Calculate positions for data points.
        List<Vector3> positions = new List<Vector3>();
        RectTransform radarChartRect = radarChartContainer.GetComponent<RectTransform>();

        for (int i = 0; i < data.Length; i++)
        {
            float angle = Mathf.Deg2Rad * (i * (360f / data.Length)); // Angle for each point.
            float x = Mathf.Cos(angle) * data[i] * 15f; // Scale the data for visibility.
            float y = Mathf.Sin(angle) * data[i] * 15f;
            positions.Add(new Vector3(x, y, 0)); // Add position to the list.
        }

        // Close the loop to complete the radar shape.
        positions.Add(positions[0]);
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    /// <summary>
    /// Creates the legend for the radar chart.
    /// </summary>
    private void CreateLegend()
    {
        float yPos = 100f; // Initial Y position for the legend.

        // Add entries for each data set.
        CreateLegendEntry("Car", carColor, ref yPos);
        CreateLegendEntry("Bike", bikeColor, ref yPos);
        CreateLegendEntry("Bus", busColor, ref yPos);
    }

    /// <summary>
    /// Creates a single entry in the legend.
    /// </summary>
    /// <param name="name">Name of the data set.</param>
    /// <param name="color">Color of the legend entry.</param>
    /// <param name="yPos">Y position for the entry, modified after creation.</param>
    private void CreateLegendEntry(string name, Color color, ref float yPos)
    {
        TMP_Text legendText = Instantiate(legendTextPrefab, radarChartContainer.transform);
        legendText.text = name; // Set the legend text.
        legendText.color = color; // Apply the color.
        legendText.transform.localPosition = new Vector3(-250f, yPos, 0); // Position the legend entry.
        yPos -= 30f; // Adjust position for the next entry.
    }
}
using UnityEngine;
using TMPro;  // Import the TextMeshPro namespace
using System.Collections.Generic;

public class RadarChart : MonoBehaviour
{
    [Header("UI References")]
    public Canvas canvas;
    public GameObject radarChartContainer;
    public TMP_Text chartTitle;               // Changed to TMP_Text
    public TMP_Text axisLabelPrefab;          // Changed to TMP_Text for axis labels
    public TMP_Text legendTextPrefab;         // Changed to TMP_Text for legend entries

    [Header("Radar Data")]
    public float[] carData = new float[9];    // Car data
    public float[] bikeData = new float[9];   // Bike data
    public float[] busData = new float[9];    // Bus data

    [Header("Visuals")]
    public GameObject lineRendererPrefab;     // Prefab for the LineRenderer
    public Color carColor = Color.red;
    public Color bikeColor = Color.green;
    public Color busColor = Color.blue;

    private List<TMP_Text> axisLabels = new List<TMP_Text>();  // Changed to TMP_Text
    private List<LineRenderer> lineRenderers = new List<LineRenderer>();

    private void Start()
    {
        CreateChart();
        CreateLegend();
    }

    // Create the radar chart
    private void CreateChart()
    {
        // Setup chart title
        if (chartTitle != null)
        {
            chartTitle.text = "Transportation Comparison";  // Set the title text
        }

        // Create the axis labels (Habits, Norm, etc.)
        string[] axisNames = { "Habits", "Norm", "Potential", "Fast", "Economical", "Ecological", "Comfortable", "Safety", "Easy" };
        float angle = 360f / axisNames.Length;

        for (int i = 0; i < axisNames.Length; i++)
        {
            // Create axis labels dynamically
            TMP_Text axisLabel = Instantiate(axisLabelPrefab, radarChartContainer.transform);
            axisLabel.text = axisNames[i];
            axisLabels.Add(axisLabel);

            // Position the labels around the radar chart
            float x = Mathf.Cos(Mathf.Deg2Rad * (i * angle)) * 200f;  // Adjust 200f for size
            float y = Mathf.Sin(Mathf.Deg2Rad * (i * angle)) * 200f;
            axisLabel.transform.localPosition = new Vector3(x, y, 0);
        }

        // Create the radar lines
        CreateRadarLine(carData, carColor, "Car");
        CreateRadarLine(bikeData, bikeColor, "Bike");
        CreateRadarLine(busData, busColor, "Bus");
    }

    // Create a radar line for each data series (Car, Bike, Bus)
    private void CreateRadarLine(float[] data, Color lineColor, string lineName)
    {
        // Instantiate the LineRenderer prefab and get the component
        LineRenderer lineRenderer = Instantiate(lineRendererPrefab).GetComponent<LineRenderer>();
        lineRenderers.Add(lineRenderer);

        // Set the LineRenderer as a child of the radarChartContainer (UI canvas)
        lineRenderer.transform.SetParent(radarChartContainer.transform, false);  // false to keep local positions

        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = 2f;
        lineRenderer.endWidth = 2f;

        List<Vector3> positions = new List<Vector3>();
        RectTransform radarChartRect = radarChartContainer.GetComponent<RectTransform>(); // Get the rect of the container

        for (int i = 0; i < data.Length; i++)
        {
            float angle = Mathf.Deg2Rad * (i * (360f / data.Length));
            float x = Mathf.Cos(angle) * data[i] * 20f; // Scale data for visibility
            float y = Mathf.Sin(angle) * data[i] * 20f;

            // Convert the radar chart's position to local position within the canvas
            Vector2 localPosition = new Vector2(x, y); // Local 2D position relative to the parent container
            positions.Add(localPosition);
        }

        // Complete the circle by closing the loop
        positions.Add(positions[0]);
        lineRenderer.positionCount = positions.Count;

        // Convert to canvas local positions using the radarChartContainer RectTransform
        Vector3[] localPositions = new Vector3[positions.Count];
        for (int i = 0; i < positions.Count; i++)
        {
            // Convert local position to canvas space
            localPositions[i] = radarChartRect.TransformPoint(positions[i]);
        }   

        // Set the LineRenderer's positions in local space, so they follow the canvas
        lineRenderer.SetPositions(localPositions);
    }

    // Create the legend
    private void CreateLegend()
    {
        float yPos = 100f;  // Initial Y position for the legend

        // Add legend entries for Car, Bike, and Bus
        CreateLegendEntry("Car", carColor, ref yPos);
        CreateLegendEntry("Bike", bikeColor, ref yPos);
        CreateLegendEntry("Bus", busColor, ref yPos);
    }

    // Create a single legend entry
    private void CreateLegendEntry(string name, Color color, ref float yPos)
    {
        TMP_Text legendText = Instantiate(legendTextPrefab, radarChartContainer.transform);  // Changed to TMP_Text
        legendText.text = name;
        legendText.color = color;
        legendText.transform.localPosition = new Vector3(-250f, yPos, 0);  // Position to the left of the radar
        yPos -= 30f;  // Adjust spacing between entries
    }
}
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the visualization of a budget line graph, including grid lines, data points, and a line renderer.
/// </summary>
public class BudgetLineGraph : MonoBehaviour
{
    [Header("Graph Settings")]
    [Tooltip("The container for the graph area.")]
    [SerializeField] private RectTransform graphContainer;

    [Tooltip("Prefab used to instantiate data points on the graph.")]
    [SerializeField] private GameObject pointPrefab;

    [Tooltip("Prefab used to instantiate grid lines.")]
    [SerializeField] private GameObject gridLinePrefab;

    [Tooltip("The number of weeks to display on the graph.")]
    [SerializeField] private int weeksToShow = 10;

    [Tooltip("The maximum value for the Y-axis on the graph.")]
    [SerializeField] private float yMaxValue = 1000f;

    [Header("Line Renderer Settings")]
    [Tooltip("The line renderer used to connect the data points.")]
    [SerializeField] private LineRenderer lineRenderer;

    [Tooltip("The color of the line connecting data points.")]
    [SerializeField] private Color lineColor = Color.blue;

    [Tooltip("The width of the line connecting data points.")]
    [SerializeField] private float lineWidth = 2f;

    private List<Vector2> dataPoints = new List<Vector2>(); // Stores the local positions of data points.
    private List<GameObject> gridLines = new List<GameObject>(); // Tracks instantiated grid lines.
    private float xSpacing; // Horizontal spacing between data points.
    private float yUnitHeight; // Vertical unit height based on yMaxValue and graph height.

    /// <summary>
    /// Initializes the graph and starts simulating data points.
    /// </summary>
    private void Start()
    {
        InitializeGraph(); // Prepare the graph dimensions and grid lines.
        StartCoroutine(SimulateDataPoints()); // Simulate data points over time.
    }

    /// <summary>
    /// Sets up the graph's dimensions, line renderer, and grid lines.
    /// </summary>
    private void InitializeGraph()
    {
        // Calculate horizontal spacing for data points and vertical unit height.
        xSpacing = graphContainer.sizeDelta.x / weeksToShow;
        yUnitHeight = graphContainer.sizeDelta.y / yMaxValue;

        // Configure the line renderer's appearance.
        lineRenderer.positionCount = 0;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.useWorldSpace = false;

        GenerateGridLines(); // Create the initial grid lines.
    }

    /// <summary>
    /// Generates grid lines for both the X and Y axes.
    /// </summary>
    private void GenerateGridLines()
    {
        // Destroy existing grid lines to avoid duplicates.
        foreach (var gridLine in gridLines)
        {
            Destroy(gridLine);
        }
        gridLines.Clear();

        // Create vertical grid lines based on the number of weeks to show.
        for (int i = 0; i <= weeksToShow; i++)
        {
            float xPos = i * xSpacing;
            CreateGridLine(new Vector2(xPos, 0), new Vector2(xPos, graphContainer.sizeDelta.y));
        }

        // Create horizontal grid lines for Y-axis steps.
        int ySteps = 5;
        for (int i = 0; i <= ySteps; i++)
        {
            float yPos = i * (graphContainer.sizeDelta.y / ySteps);
            CreateGridLine(new Vector2(0, yPos), new Vector2(graphContainer.sizeDelta.x, yPos));
        }
    }

    /// <summary>
    /// Instantiates a grid line between two points in the graph container.
    /// </summary>
    /// <param name="start">The start position of the grid line.</param>
    /// <param name="end">The end position of the grid line.</param>
    private void CreateGridLine(Vector2 start, Vector2 end)
    {
        // Instantiate the grid line prefab.
        var line = Instantiate(gridLinePrefab, graphContainer);
        gridLines.Add(line);

        // Configure the size, position, and direction of the grid line.
        RectTransform lineRect = line.GetComponent<RectTransform>();
        Vector2 direction = (end - start).normalized;
        float distance = Vector2.Distance(start, end);

        lineRect.sizeDelta = new Vector2(distance, lineRect.sizeDelta.y);
        lineRect.anchoredPosition = start + direction * distance / 2;
        lineRect.right = direction;
    }

    /// <summary>
    /// Simulates the addition of data points over time, updating the graph.
    /// </summary>
    private IEnumerator SimulateDataPoints()
    {
        int week = 1; // Tracks the current week number.
        while (true)
        {
            // Generate a random budget value and add it as a data point.
            float budget = Random.Range(100f, yMaxValue);
            AddDataPoint(budget, week);

            week++;
            // Extend the number of weeks displayed if necessary.
            if (week > weeksToShow)
            {
                weeksToShow++;
                GenerateGridLines();
            }

            yield return new WaitForSeconds(2f); // Wait 2 seconds before generating the next data point.
        }
    }

    /// <summary>
    /// Adds a new data point to the graph and updates the line renderer.
    /// </summary>
    /// <param name="budget">The Y-axis value of the data point.</param>
    /// <param name="week">The X-axis value representing the week.</param>
    public void AddDataPoint(float budget, int week)
    {
        // Remove the oldest data point if exceeding the maximum number of points.
        if (dataPoints.Count >= weeksToShow)
        {
            dataPoints.RemoveAt(0);
            lineRenderer.positionCount--;
        }

        // Calculate the local position of the new data point.
        float xPos = (dataPoints.Count) * xSpacing;
        float yPos = Mathf.Clamp(budget * yUnitHeight, 0, graphContainer.sizeDelta.y);
        Vector2 localPoint = new Vector2(xPos, yPos);

        // Add the new data point to the list.
        dataPoints.Add(localPoint);

        // Instantiate a visual representation of the data point.
        Instantiate(pointPrefab, graphContainer).GetComponent<RectTransform>().anchoredPosition = localPoint;

        // Update the line renderer to include the new data point.
        lineRenderer.positionCount = dataPoints.Count;
        for (int i = 0; i < dataPoints.Count; i++)
        {
            Vector3 currentWorldPoint = graphContainer.TransformPoint(dataPoints[i]);
            lineRenderer.SetPosition(i, currentWorldPoint);
        }

        // Optionally draw a debug line in the scene view between the last two data points.
        if (dataPoints.Count > 1)
        {
            Vector3 previousWorldPoint = graphContainer.TransformPoint(dataPoints[dataPoints.Count - 2]);
            Vector3 worldPoint = graphContainer.TransformPoint(localPoint);
            Debug.DrawLine(previousWorldPoint, worldPoint, Color.red, 2f);
        }
    }
}
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BudgetLineGraph : MonoBehaviour
{
    [Header("Graph Settings")]
    [SerializeField] private RectTransform graphContainer;
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private GameObject gridLinePrefab;
    [SerializeField] private int weeksToShow = 10;
    [SerializeField] private float yMaxValue = 1000f;

    [Header("Line Renderer Settings")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Color lineColor = Color.blue;
    [SerializeField] private float lineWidth = 2f;

    private List<Vector2> dataPoints = new List<Vector2>();
    private List<GameObject> gridLines = new List<GameObject>();
    private float xSpacing;
    private float yUnitHeight;

    private void Start()
    {
        InitializeGraph();
        StartCoroutine(SimulateDataPoints());
    }

    private void InitializeGraph()
    {
        xSpacing = graphContainer.sizeDelta.x / weeksToShow;
        yUnitHeight = graphContainer.sizeDelta.y / yMaxValue;

        lineRenderer.positionCount = 0;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.useWorldSpace = false;

        GenerateGridLines();
    }

    private void GenerateGridLines()
    {
        foreach (var gridLine in gridLines)
        {
            Destroy(gridLine);
        }
        gridLines.Clear();

        for (int i = 0; i <= weeksToShow; i++)
        {
            float xPos = i * xSpacing;
            CreateGridLine(new Vector2(xPos, 0), new Vector2(xPos, graphContainer.sizeDelta.y));
        }

        int ySteps = 5;
        for (int i = 0; i <= ySteps; i++)
        {
            float yPos = i * (graphContainer.sizeDelta.y / ySteps);
            CreateGridLine(new Vector2(0, yPos), new Vector2(graphContainer.sizeDelta.x, yPos));
        }
    }

    private void CreateGridLine(Vector2 start, Vector2 end)
    {
        var line = Instantiate(gridLinePrefab, graphContainer);
        gridLines.Add(line);
        RectTransform lineRect = line.GetComponent<RectTransform>();
        Vector2 direction = (end - start).normalized;
        float distance = Vector2.Distance(start, end);

        lineRect.sizeDelta = new Vector2(distance, lineRect.sizeDelta.y);
        lineRect.anchoredPosition = start + direction * distance / 2;
        lineRect.right = direction;
    }

    private IEnumerator SimulateDataPoints()
    {
        int week = 1;
        while (true)
        {
            float budget = Random.Range(100f, 1000f);
            AddDataPoint(budget, week);

            week++;
            if (week > weeksToShow)
            {
                weeksToShow++;
                GenerateGridLines();
            }

            yield return new WaitForSeconds(2f);
        }
    }

    public void AddDataPoint(float budget, int week)
    {
        if (dataPoints.Count >= weeksToShow)
        {
            dataPoints.RemoveAt(0);
            lineRenderer.positionCount--;
        }

        float xPos = (dataPoints.Count) * xSpacing;
        float yPos = Mathf.Clamp(budget * yUnitHeight, 0, graphContainer.sizeDelta.y);
        Vector2 localPoint = new Vector2(xPos, yPos);

        dataPoints.Add(localPoint);

        Instantiate(pointPrefab, graphContainer).GetComponent<RectTransform>().anchoredPosition = localPoint;

        lineRenderer.positionCount = dataPoints.Count;
        for (int i = 0; i < dataPoints.Count; i++)
        {
            Vector3 currentWorldPoint = graphContainer.TransformPoint(dataPoints[i]);
            lineRenderer.SetPosition(i, currentWorldPoint);
        }

        if (dataPoints.Count > 1)
        {
            Vector3 previousWorldPoint = graphContainer.TransformPoint(dataPoints[dataPoints.Count - 2]);
            Vector3 worldPoint = graphContainer.TransformPoint(localPoint);
            Debug.DrawLine(previousWorldPoint, worldPoint, Color.red, 2f);
        }
    }
}
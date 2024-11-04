using UnityEngine;
using System.Collections.Generic;

public class BudgetLineGraph : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float timeBetweenPoints = 1.0f;
    [SerializeField] private float scaleY = 0.1f;

    private List<float> budgetValues = new List<float>();
    private float nextUpdateTime = 0f;
    private int weekCount = 0;

    private void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.useWorldSpace = false;
        lineRenderer.numCapVertices = 10;
        lineRenderer.numCornerVertices = 10;
    }

    private void Update()
    {
        if (Time.time >= nextUpdateTime)
        {
            AddNewPoint(Random.Range(0, 100));

            nextUpdateTime = Time.time + 7f;
        }
    }

    private void AddNewPoint(float budget)
    {
        budgetValues.Add(budget);
        lineRenderer.positionCount = budgetValues.Count;

        Vector3 newPosition = new Vector3(weekCount * timeBetweenPoints, budget * scaleY, 0);
        lineRenderer.SetPosition(weekCount, newPosition);

        if (weekCount > 0)
        {
            Vector3 previousPosition = lineRenderer.GetPosition(weekCount - 1);
            Debug.DrawLine(previousPosition, newPosition, Color.red, 7f);
        }

        weekCount++;
    }
}
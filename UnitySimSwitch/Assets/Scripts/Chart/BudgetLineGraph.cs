using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class BudgetLineGraph : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float timeBetweenPoints = 68f;
    [SerializeField] private float scaleY = 0.5f;
    [SerializeField] private TMP_Text _budgetText = null;
    
    private List<float> budgetValues = new List<float>();
    private float nextUpdateTime = 0f;
    private int weekCount = 0;
    public float budget;

    private void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 5f;
        lineRenderer.endWidth = 5f;
        lineRenderer.useWorldSpace = false;
        lineRenderer.numCapVertices = 10;
        lineRenderer.numCornerVertices = 10;

        budget = Random.Range(0, 100);
        AddNewPoint(budget);
    }

    private void Update()
    {
        if (Time.time >= nextUpdateTime)
        {
            budget = Random.Range(0, 100);
            AddNewPoint(budget);
            nextUpdateTime = Time.time + 7f;
        }
    }

    private void AddNewPoint(float budget)
    {
        budgetValues.Add(budget);
        lineRenderer.positionCount = budgetValues.Count;

        Vector3 newPosition = new Vector3(weekCount * timeBetweenPoints, budget * scaleY, 0);
        lineRenderer.SetPosition(weekCount, newPosition);

        weekCount++;
        //UpdateBudgetText();
    }

    /*private void UpdateBudgetText()
    {
        _budgetText.text = "Budget: " + budget.ToString("F2");
    }*/
}
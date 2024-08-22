using UnityEngine;
using UnityEngine.UI;

public class PieChart : MonoBehaviour
{
    #region Fields
    [SerializeField] private Image[] _imagesPieChart;
    [SerializeField] private float[] _values;
    #endregion Fields

    #region Methods

    private void Start()
    {
        SetValues(_values);
    }

    private void Update()
    {
        SetValues(_values);
    }

    public void SetValues (float[] valuesToSet)
    {
        float totalValues = 0;
        for(int i = 0; i < _imagesPieChart.Length; i++)
        {
            totalValues += FindPercentage(valuesToSet, i);
            _imagesPieChart[i].fillAmount = totalValues;
        }
    }

    public float FindPercentage(float[] valueToSet, int index)
    {
        float totalAmount = 0;
        for(int i = 0; i < valueToSet.Length; i++)
        {
            totalAmount += valueToSet[i];
        }

        return valueToSet[index] / totalAmount;
    }
    #endregion Methods
}
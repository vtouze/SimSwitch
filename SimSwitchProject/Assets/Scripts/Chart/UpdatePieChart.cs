using UnityEngine;

public class UpdatePieChart : MonoBehaviour
{
    public static float _valueA = 10;
    public static float _valueB = 5;
    public static float _valueC = 20;

    private void Update()
    {
        _valueA = _valueA + 2;
        _valueB = _valueB + 3;
        _valueC = _valueC + 1;
        Debug.Log("Value A: " + _valueA + "Value B: " + _valueB + "Value C: " + _valueC);
    }
}
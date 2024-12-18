using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Households Entry", menuName = "ScriptableObjects/HouseholdsEntry", order = 3)]
public class HouseholdsEntry : ScriptableObject
{
    public enum IncomeLevel
    {
        LOW,
        MEDIUM,
        HIGH
    }

    public Sprite _householdsIcon;
    public string _type;
    public string _age;
    public int _children;
    public string _distribution;
    public IncomeLevel _income;
}
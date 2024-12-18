using UnityEngine;

[CreateAssetMenu(fileName = "NewspaperEvent", menuName = "ScriptableObjects/NewspaperEvent", order = 1)]
public class NewspaperEvent : ScriptableObject
{
    public string _eventName;
    public string _date;
    public string _subheading;
    public Sprite _coverImage;
    public string _captionImage;
    public string _description;

    public int _moneyChange;
    public int _satisfactionChange;
    public int _sustainabilityChange;
}
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encyclopedia Entry", menuName = "Encyclopedia/Entry")]
public class EncyclopediaEntry : ScriptableObject
{
    public string _entryName;
    public Sprite _coverImage;
    public string _description;
    public string _articles;
}
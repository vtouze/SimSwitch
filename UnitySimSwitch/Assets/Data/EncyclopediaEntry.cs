using UnityEngine;

[CreateAssetMenu(fileName = "Encylopedia Entry", menuName = "ScriptableObjects/EncylopediaEntry", order = 2)]
public class EncyclopediaEntry : ScriptableObject
{
    public string _entryName;
    public Sprite _coverImage;
    public string _description;
    public string _articles;
}
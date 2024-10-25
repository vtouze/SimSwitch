using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public List<Edge> _edgeList;
    public Node _path;
    public GameObject _id;
    public float _f, _g, _h;
    public Node _cameFrom;

    public Node(GameObject id)
    {
        _id = id;
        _edgeList = new List<Edge>();
        _path = null;
        _cameFrom = null;
        ResetCosts();
    }

    public GameObject GetId()
    {
        return _id;
    }

    public void ClearPath() 
    {
        _path = null;
        _cameFrom = null;
    }

    public void ResetCosts()
    {
        _f = _g = _h = 0;
    }
}
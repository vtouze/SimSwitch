using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public List<Edge> _edgeList = new List<Edge>();
    public Node _path = null;
    private GameObject _id;
    public float _f, _g, _h;
    public Node _cameFrom;

    public Node(GameObject i)
    {
        _id = i;
        _path = null;

    }

    public GameObject GetId()
    {
        return _id;
    }
}
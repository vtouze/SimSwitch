using UnityEngine;

[System.Serializable]
public struct Link
{
    public enum EDirection { UNI, BI };
    public GameObject _node1;
    public GameObject _node2;
    public EDirection _direction;
}

public class WaypointsManager : MonoBehaviour
{
    public GameObject[] _waypoints;
    public Link[] _links;
    public Graph _graph = new Graph();

    void Start()
    {
        if (_waypoints.Length > 0)
        {
            foreach (GameObject wp in _waypoints)
            {
                _graph.AddNode(wp);
            }
            foreach (Link l in _links)
            {
                _graph.AddEdge(l._node1, l._node2);
                if (l._direction == Link.EDirection.BI)
                {
                    _graph.AddEdge(l._node2, l._node1);
                }
            }
        }
    }
}
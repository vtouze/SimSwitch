using UnityEngine;

[System.Serializable]
public struct Link
{
    public enum EDirection { UNI, BI }
    public GameObject _node1;
    public GameObject _node2;
    public EDirection _direction;
}

public class WaypointsManager : MonoBehaviour
{
    public GameObject[] _waypoints;
    public Link[] _links;
    public Graph _graph;

    void Start()
    {        
        _graph = new Graph();
        InitializeGraph();
    }

    private void InitializeGraph()
    {
        if (_waypoints.Length > 0)
        {
            foreach (GameObject waypoint in _waypoints)
            {
                if (waypoint != null)
                {
                    _graph.AddNode(waypoint);
                }
            }

            foreach (Link link in _links)
            {
                if (link._node1 != null && link._node2 != null)
                {
                    _graph.AddEdge(link._node1, link._node2);
                    if (link._direction == Link.EDirection.BI)
                    {
                        _graph.AddEdge(link._node2, link._node1);
                    }
                }
            }
        }
    }
}
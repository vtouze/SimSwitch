using UnityEngine;

[System.Serializable]
public struct Link
{
    /// <summary>
    /// Enumeration for link direction types.
    /// </summary>
    public enum EDirection { UNI, BI }

    [Tooltip("The first node in the link.")]
    public GameObject _node1;

    [Tooltip("The second node in the link.")]
    public GameObject _node2;

    [Tooltip("The direction of the link (unidirectional or bidirectional).")]
    public EDirection _direction;
}

public class WaypointsManager : MonoBehaviour
{
    [Tooltip("Array of waypoints in the scene.")]
    public GameObject[] _waypoints;

    [Tooltip("Array of links between waypoints.")]
    public Link[] _links;

    [Tooltip("Graph that represents the network of waypoints.")]
    public Graph _graph;

    /// <summary>
    /// Start is called before the first frame update.
    /// Initializes the graph by adding nodes and edges.
    /// </summary>
    void Start()
    {        
        _graph = new Graph();  // Initialize a new graph instance
        InitializeGraph();  // Call method to add nodes and edges to the graph
    }

    /// <summary>
    /// Initializes the graph with nodes and edges based on waypoints and links.
    /// </summary>
    private void InitializeGraph()
    {
        // Check if there are any waypoints in the array
        if (_waypoints.Length > 0)
        {
            // Add each waypoint as a node in the graph
            foreach (GameObject waypoint in _waypoints)
            {
                if (waypoint != null)
                {
                    _graph.AddNode(waypoint);  // Add the waypoint as a node
                }
            }

            // Iterate through the links array and add edges between waypoints
            foreach (Link link in _links)
            {
                // Ensure both nodes in the link are not null
                if (link._node1 != null && link._node2 != null)
                {
                    _graph.AddEdge(link._node1, link._node2);  // Add an edge from node1 to node2
                    // If the link is bidirectional, add the reverse edge
                    if (link._direction == Link.EDirection.BI)
                    {
                        _graph.AddEdge(link._node2, link._node1);  // Add reverse edge for bidirectional link
                    }
                }
            }
        }
    }
}
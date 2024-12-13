using UnityEngine;

public class Edge
{
    [Tooltip("The starting node of the edge.")]
    public Node _startNode;

    [Tooltip("The ending node of the edge.")]
    public Node _endNode;

    /// <summary>
    /// Constructor that initializes an edge between two nodes.
    /// </summary>
    /// <param name="from">The starting node of the edge.</param>
    /// <param name="to">The ending node of the edge.</param>
    public Edge(Node from, Node to)
    {
        // Initialize the start node with the provided 'from' node
        _startNode = from;

        // Initialize the end node with the provided 'to' node
        _endNode = to;
    }
}
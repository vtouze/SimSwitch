using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    #region Fields
    [Tooltip("List of all edges in the graph.")]
    private List<Edge> _edges = new List<Edge>();

    [Tooltip("List of all nodes in the graph.")]
    private List<Node> _nodes = new List<Node>();

    [Tooltip("List representing the path from start to end node.")]
    public List<Node> _pathList = new List<Node>();

    private Dictionary<GameObject, Node> nodeDictionary = new Dictionary<GameObject, Node>();

    public Graph() {}
    #endregion Fields

    #region Methods

    /// <summary>
    /// Adds a node to the graph if it does not already exist.
    /// </summary>
    /// <param name="id">The GameObject representing the node to add.</param>
    public void AddNode(GameObject id)
    {
        // Check if node already exists
        if (!nodeDictionary.ContainsKey(id))
        {
            Node node = new Node(id);
            _nodes.Add(node);
            nodeDictionary.Add(id, node);
        }
    }

    /// <summary>
    /// Adds an edge between two existing nodes.
    /// </summary>
    /// <param name="fromNode">The starting node for the edge.</param>
    /// <param name="toNode">The ending node for the edge.</param>
    public void AddEdge(GameObject fromNode, GameObject toNode)
    {
        Node from = FindNode(fromNode);
        Node to = FindNode(toNode);

        // If both nodes are found, add an edge between them
        if (from != null && to != null)
        {
            Edge edge = new Edge(from, to);
            _edges.Add(edge);
            from._edgeList.Add(edge);
        }
    }

    /// <summary>
    /// Finds and returns the node corresponding to a given GameObject.
    /// </summary>
    /// <param name="id">The GameObject representing the node.</param>
    /// <returns>The corresponding Node object, or null if not found.</returns>
    private Node FindNode(GameObject id)
    {
        nodeDictionary.TryGetValue(id, out Node foundNode);
        return foundNode;
    }

    /// <summary>
    /// Implements the A* algorithm to find the shortest path between two nodes.
    /// </summary>
    /// <param name="startID">The GameObject representing the start node.</param>
    /// <param name="endID">The GameObject representing the end node.</param>
    /// <returns>True if a path was found, otherwise false.</returns>
    public bool AStar(GameObject startID, GameObject endID)
    {
        Node start = FindNode(startID);
        Node end = FindNode(endID);

        // If start or end node is null, return false
        if (start == null || end == null)
        {
            return false;
        }

        // Reset costs and clear paths of all nodes
        foreach (Node node in _nodes)
        {
            node.ResetCosts();
            node.ClearPath();
        }

        List<Node> openSet = new List<Node> { start };
        HashSet<Node> closedSet = new HashSet<Node>();

        // Initialize the costs of the start node
        start._g = 0;
        start._h = GetDistance(start, end);
        start._f = start._h;

        // A* search loop
        while (openSet.Count > 0)
        {
            int currentIndex = GetLowestF(openSet);
            Node currentNode = openSet[currentIndex];

            // If the end node is found, reconstruct the path
            if (currentNode.GetId() == endID)
            {
                ReconstructPath(start, end);
                return true;
            }

            openSet.RemoveAt(currentIndex);
            closedSet.Add(currentNode);

            // Check neighboring nodes
            foreach (Edge edge in currentNode._edgeList)
            {
                Node neighbor = edge._endNode;

                // Skip if the neighbor is already in the closed set
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }

                // Calculate the tentative G score
                float tentativeGScore = currentNode._g + GetDistance(currentNode, neighbor);

                // If the neighbor is not in open set or has a better G score, update it
                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentativeGScore >= neighbor._g)
                {
                    continue;
                }

                neighbor._cameFrom = currentNode;
                neighbor._g = tentativeGScore;
                neighbor._h = GetDistance(neighbor, end);
                neighbor._f = neighbor._g + neighbor._h;
            }
        }

        return false;
    }

    /// <summary>
    /// Reconstructs the path from start node to end node based on the A* algorithm.
    /// </summary>
    /// <param name="startNode">The start node.</param>
    /// <param name="endNode">The end node.</param>
    public void ReconstructPath(Node startNode, Node endNode)
    {
        _pathList.Clear();
        Node currentNode = endNode;

        // Reconstruct the path by following the cameFrom pointers
        while (currentNode != null)
        {
            _pathList.Insert(0, currentNode);
            if (currentNode == startNode) break;
            currentNode = currentNode._cameFrom;
        }
    }

    /// <summary>
    /// Calculates the Euclidean distance between two nodes.
    /// </summary>
    /// <param name="a">The first node.</param>
    /// <param name="b">The second node.</param>
    /// <returns>The distance between the two nodes.</returns>
    private float GetDistance(Node a, Node b)
    {
        return Vector2.Distance(a.GetId().GetComponent<RectTransform>().anchoredPosition, 
                                b.GetId().GetComponent<RectTransform>().anchoredPosition);
    }

    /// <summary>
    /// Finds the index of the node with the lowest F score in the list.
    /// </summary>
    /// <param name="list">The list of nodes to search through.</param>
    /// <returns>The index of the node with the lowest F score.</returns>
    private int GetLowestF(List<Node> list)
    {
        float lowestF = float.MaxValue;
        int lowestIndex = -1;

        // Loop through the list to find the node with the lowest F score
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i]._f < lowestF)
            {
                lowestF = list[i]._f;
                lowestIndex = i;
            }
        }

        return lowestIndex;
    }

    #endregion Methods
}
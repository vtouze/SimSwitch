using System.Collections.Generic;
using UnityEngine;

public class Node
{
    [Tooltip("List of edges connected to this node.")]
    public List<Edge> _edgeList;

    [Tooltip("The node representing the path to this one during pathfinding.")]
    public Node _path;

    [Tooltip("The GameObject representing this node in the scene.")]
    public GameObject _id;

    [Tooltip("The F, G, and H scores used in A* pathfinding.")]
    public float _f, _g, _h;

    [Tooltip("The node that came before this one in the pathfinding process.")]
    public Node _cameFrom;

    /// <summary>
    /// Constructor for the Node class, initializes the node with the given GameObject ID.
    /// Also initializes edge list and resets costs.
    /// </summary>
    /// <param name="id">The GameObject representing this node.</param>
    public Node(GameObject id)
    {
        _id = id;  // Assign the GameObject as the ID for this node
        _edgeList = new List<Edge>();  // Initialize the edge list as empty
        _path = null;  // Clear the path (will be used in pathfinding)
        _cameFrom = null;  // Clear the cameFrom reference
        ResetCosts();  // Reset the pathfinding costs (f, g, h)
    }

    /// <summary>
    /// Gets the GameObject ID associated with this node.
    /// </summary>
    /// <returns>The GameObject representing this node.</returns>
    public GameObject GetId()
    {
        return _id;  // Return the GameObject associated with this node
    }

    /// <summary>
    /// Clears the path and cameFrom references, useful after pathfinding is completed.
    /// </summary>
    public void ClearPath() 
    {
        _path = null;  // Clear the path reference
        _cameFrom = null;  // Clear the cameFrom reference
    }

    /// <summary>
    /// Resets the F, G, and H scores to zero, used to initialize nodes for pathfinding.
    /// </summary>
    public void ResetCosts()
    {
        _f = _g = _h = 0;  // Reset all pathfinding scores (f, g, and h) to zero
    }
}
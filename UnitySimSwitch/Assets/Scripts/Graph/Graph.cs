using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    #region Fields
    private List<Edge> _edges = new List<Edge>();
    private List<Node> _nodes = new List<Node>();
    public List<Node> _pathList = new List<Node>();

    private Dictionary<GameObject, Node> nodeDictionary = new Dictionary<GameObject, Node>();

    public Graph() {}
    #endregion Fields

    #region Methods
    public void AddNode(GameObject id)
    {
        if (!nodeDictionary.ContainsKey(id))
        {
            Node node = new Node(id);
            _nodes.Add(node);
            nodeDictionary.Add(id, node);
        }
    }

    public void AddEdge(GameObject fromNode, GameObject toNode)
    {
        Node from = FindNode(fromNode);
        Node to = FindNode(toNode);

        if (from != null && to != null)
        {
            Edge edge = new Edge(from, to);
            _edges.Add(edge);
            from._edgeList.Add(edge);
        }
    }

    private Node FindNode(GameObject id)
    {
        nodeDictionary.TryGetValue(id, out Node foundNode);
        return foundNode;
    }

    public bool AStar(GameObject startID, GameObject endID)
    {
        Node start = FindNode(startID);
        Node end = FindNode(endID);

        if (start == null || end == null)
        {
            return false;
        }

        foreach (Node node in _nodes)
        {
            node.ResetCosts();
            node.ClearPath();
        }

        List<Node> openSet = new List<Node> { start };
        HashSet<Node> closedSet = new HashSet<Node>();

        start._g = 0;
        start._h = GetDistance(start, end);
        start._f = start._h;

        while (openSet.Count > 0)
        {
            int currentIndex = GetLowestF(openSet);
            Node currentNode = openSet[currentIndex];

            if (currentNode.GetId() == endID)
            {
                ReconstructPath(start, end);
                return true;
            }

            openSet.RemoveAt(currentIndex);
            closedSet.Add(currentNode);

            foreach (Edge edge in currentNode._edgeList)
            {
                Node neighbor = edge._endNode;

                if (closedSet.Contains(neighbor))
                {
                    continue;
                }

                float tentativeGScore = currentNode._g + GetDistance(currentNode, neighbor);

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

    public void ReconstructPath(Node startNode, Node endNode)
    {
        _pathList.Clear();
        Node currentNode = endNode;

        while (currentNode != null)
        {
            _pathList.Insert(0, currentNode);
            if (currentNode == startNode) break;
            currentNode = currentNode._cameFrom;
        }
    }

    private float GetDistance(Node a, Node b)
    {
        return Vector2.Distance(a.GetId().GetComponent<RectTransform>().anchoredPosition, 
                                b.GetId().GetComponent<RectTransform>().anchoredPosition);
    }

    private int GetLowestF(List<Node> list)
    {
        float lowestF = float.MaxValue;
        int lowestIndex = -1;

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
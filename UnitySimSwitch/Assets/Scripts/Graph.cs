using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    #region Fields
    List<Edge> _edges = new List<Edge>();
    List<Node> _nodes = new List<Node>();
    public List<Node> _pathList = new List<Node>();
    public Graph() {}
    #endregion Fields

    #region Methods
    public void AddNode(GameObject id)
    {
        Node node = new Node(id);
        _nodes.Add(node);
    }

    public void AddEdge(GameObject fromNode, GameObject toNode)
    {
        Node from = FindNode(fromNode);
        Node to = FindNode(toNode);

        if (from != null && to != null)
        {
            Edge e = new Edge(from, to);
            _edges.Add(e);
            from._edgeList.Add(e);
        }
    }

    private Node FindNode(GameObject id)
    {
        foreach (Node n in _nodes)
        {
            if (n.GetId() == id)
            {
                return n;
            }
        }

        return null;
    }

    public bool AStar(GameObject startID, GameObject endID)
    {
        Node start = FindNode(startID);
        Node end = FindNode(endID);

        if (start == null || end == null)
        {
            return false;
        }

        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();
        float tentative_g_score = 0;
        bool tentative_is_better;

        start._g = 0;
        start._h = GetDistance(start, end);
        start._f = start._h;

        open.Add(start);
        while (open.Count > 0)
        {
            int i = GetLowestF(open);
            Node thisNode = open[i];
            if (thisNode.GetId() == endID)
            {
                ReconstructPath(start, end);
                return true;
            }

            open.RemoveAt(i);
            closed.Add(thisNode);
            Node neighbour;
            foreach (Edge e in thisNode._edgeList)
            {
                neighbour = e._endNode;

                if (closed.IndexOf(neighbour) > -1)
                {
                    continue;
                }

                tentative_g_score = thisNode._g + GetDistance(thisNode, neighbour);
                if (open.IndexOf(neighbour) == -1)
                {
                    open.Add(neighbour);
                    tentative_is_better = true;
                }
                else if (tentative_g_score < neighbour._g)
                {
                    tentative_is_better = true;
                }
                else
                {
                    tentative_is_better = false;
                }
                if (tentative_is_better)
                {
                    neighbour._cameFrom = thisNode;
                    neighbour._g = tentative_g_score;
                    neighbour._h = GetDistance(thisNode, end);
                    neighbour._f = neighbour._g + neighbour._h;
                }
            }
        }
        return false;
    }

    public void ReconstructPath(Node startID, Node endID)
    {
        _pathList.Clear();
        _pathList.Add(endID);

        var p = endID._cameFrom;
        while (p != startID && p != null)
        {
            _pathList.Insert(0, p);
            p = p._cameFrom;
        }

        _pathList.Insert(0, startID);
    }

    private float GetDistance(Node a, Node b)
    {
        return Vector2.Distance(a.GetId().GetComponent<RectTransform>().anchoredPosition, b.GetId().GetComponent<RectTransform>().anchoredPosition);
    }

    private int GetLowestF(List<Node> l)
    {
        float lowestF = 0;
        int count = 0;
        int iteratorCount = 0;

        lowestF = l[0]._f;

        for (int i = 1; i < l.Count; i++)
        {
            if (l[0]._f < lowestF)
            {
                lowestF = l[i]._f;
                iteratorCount = count;
            }

            count++;
        }

        return iteratorCount;
    }
    #endregion Methods
}
public class Edge
{
    public Node _startNode;
    public Node _endNode;

    public Edge(Node from, Node to)
    {
        _startNode = from;
        _endNode = to;
    }
}
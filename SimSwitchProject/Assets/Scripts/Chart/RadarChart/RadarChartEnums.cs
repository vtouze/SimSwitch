using UnityEngine;

public class RadarChartEnums : MonoBehaviour
{
    public enum RadarChart3Edges
    {
        Aspect1 = 6,
        Aspect2 = 9,
        Aspect3 = 15,
    }

    public enum RadarChart5Edges
    {
        Aspect1 = 8,
        Aspect2 = 2,
        Aspect3 = 18,
        Aspect4 = 14,
        Aspect5 = 20,
    }

    public enum RadarChart9Edges
    {
        Aspect1 = 4,
        Aspect2 = 10,
        Aspect3 = 7,
        Aspect4 = 16,
        Aspect5 = 9,
        Aspect6 = 11,
        Aspect7 = 7,
        Aspect8 = 19,
        Aspect9 = 3
    }

    public enum NumberOfEdges
    {
        Three = 3,
        Five = 5,
        Nine = 9
    }
}
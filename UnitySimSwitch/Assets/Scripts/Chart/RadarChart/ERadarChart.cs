using UnityEngine;

/// <summary>
/// Enum definitions for radar chart configurations.
/// </summary>
public class ERadarChart : MonoBehaviour
{
    public enum RadarChart3Edges { Aspect1 = 5, Aspect2 = 10, Aspect3 = 15 }
    public enum RadarChart5Edges { Aspect1 = 4, Aspect2 = 8, Aspect3 = 12, Aspect4 = 16, Aspect5 = 20 }
    public enum RadarChart9Edges { Aspect1 = 2, Aspect2 = 4, Aspect3 = 6, Aspect4 = 8, Aspect5 = 10, Aspect6 = 12, Aspect7 = 14, Aspect8 = 16, Aspect9 = 18 }
    public enum NumberOfEdges { Three = 3, Five = 5, Nine = 9 }
}
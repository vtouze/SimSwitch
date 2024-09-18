using UnityEngine;
using UnityEngine.UI;

public class UIRadarChart : MonoBehaviour
{
    [SerializeField] private Material radarMaterial;
    [SerializeField] private Sprite radarChartSprite3Edges;
    [SerializeField] private Sprite radarChartSprite5Edges;
    [SerializeField] private Sprite radarChartSprite9Edges;

    [SerializeField] private RadarChartEnums.NumberOfEdges numberOfEdges; // Dropdown for number of edges

    private RadarStats stats;
    private CanvasRenderer radarMeshCanvasRenderer;
    private Image radarImageComponent;

    private void Awake()
    {
        radarMeshCanvasRenderer = transform.Find("radarMesh").GetComponent<CanvasRenderer>();
        radarImageComponent = transform.Find("radarImage").GetComponent<Image>();
    }

    public void SetStats(RadarStats stats)
    {
        this.stats = stats;
        stats.OnStatsChanged += Stats_OnStatsChanged;
        UpdateStatsVisual();
    }

    private void Stats_OnStatsChanged(object sender, System.EventArgs e)
    {
        UpdateStatsVisual();
    }

    private void UpdateStatsVisual()
    {
        // Set the sprite based on the number of edges
        switch (numberOfEdges)
        {
            case RadarChartEnums.NumberOfEdges.Three:
                radarImageComponent.sprite = radarChartSprite3Edges;
                break;
            case RadarChartEnums.NumberOfEdges.Five:
                radarImageComponent.sprite = radarChartSprite5Edges;
                break;
            case RadarChartEnums.NumberOfEdges.Nine:
                radarImageComponent.sprite = radarChartSprite9Edges;
                break;
            default:
                radarImageComponent.sprite = null;
                Debug.LogWarning("Unsupported number of edges.");
                break;
        }

        Mesh mesh = new Mesh();
        int vertexCount = (int)numberOfEdges + 1; // +1 for the center
        Vector3[] vertices = new Vector3[vertexCount];
        Vector2[] uv = new Vector2[vertexCount];
        int[] triangles = new int[3 * (int)numberOfEdges];

        float angleIncrement = 360f / (int)numberOfEdges;
        float radarChartSize = 145f;

        // Create vertices
        vertices[0] = Vector3.zero; // Center point
        for (int i = 0; i < (int)numberOfEdges; i++)
        {
            Vector3 vertex = Quaternion.Euler(0, 0, -angleIncrement * i) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(i);
            vertices[i + 1] = vertex;
        }

        // Set UV
        for (int i = 0; i < vertexCount; i++)
        {
            uv[i] = new Vector2(vertices[i].x, vertices[i].y);
        }

        // Create triangles
        for (int i = 0; i < (int)numberOfEdges; i++)
        {
            int nextIndex = (i + 1) % (int)numberOfEdges;
            triangles[i * 3 + 0] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = nextIndex + 1;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        radarMeshCanvasRenderer.SetMesh(mesh);
        radarMeshCanvasRenderer.SetMaterial(radarMaterial, null);
    }
}

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the visual representation of a radar chart in the UI.
/// </summary>
public class UIRadarChart : MonoBehaviour
{
    [Tooltip("Material used to render the radar mesh.")]
    [SerializeField] private Material radarMaterial;

    [Tooltip("Sprite for radar chart with 3 edges.")]
    [SerializeField] private Sprite radarChartSprite3Edges;

    [Tooltip("Sprite for radar chart with 5 edges.")]
    [SerializeField] private Sprite radarChartSprite5Edges;

    [Tooltip("Sprite for radar chart with 9 edges.")]
    [SerializeField] private Sprite radarChartSprite9Edges;

    [Tooltip("Number of edges for the radar chart.")]
    public ERadarChart.NumberOfEdges numberOfEdges;

    private RadarStats stats; // Holds radar statistics.
    private CanvasRenderer radarMeshCanvasRenderer; // Renders the radar mesh.
    private RectTransform radarMeshRectTransform; // RectTransform to position radarMesh.
    private Image radarImageComponent; // Image to display radar chart sprite.

    private void Awake()
    {
        // Initialize components.
        radarMeshCanvasRenderer = transform.Find("radarMesh").GetComponent<CanvasRenderer>();
        radarMeshRectTransform = radarMeshCanvasRenderer.GetComponent<RectTransform>();
        radarImageComponent = transform.Find("radarImage").GetComponent<Image>();
    }

    /// <summary>
    /// Assigns the radar stats and subscribes to stat change events.
    /// </summary>
    /// <param name="stats">Radar statistics.</param>
    public void SetStats(RadarStats stats)
    {
        this.stats = stats;
        stats.OnStatsChanged += Stats_OnStatsChanged;
        UpdateStatsVisual();
    }

    /// <summary>
    /// Callback when radar stats are updated.
    /// </summary>
    private void Stats_OnStatsChanged(object sender, System.EventArgs e)
    {
        UpdateStatsVisual();
    }

    private void Update()
    {
        // Continuously update the radar chart visuals.
        UpdateStatsVisual();
    }

    /// <summary>
    /// Updates the visual representation of the radar chart.
    /// </summary>
    private void UpdateStatsVisual()
    {
        // Set radar chart sprite based on number of edges.
        switch (numberOfEdges)
        {
            case ERadarChart.NumberOfEdges.Three:
                radarImageComponent.sprite = radarChartSprite3Edges;
                radarImageComponent.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                radarMeshRectTransform.anchoredPosition = new Vector2(radarMeshRectTransform.anchoredPosition.x, -30f);
                break;

            case ERadarChart.NumberOfEdges.Five:
                radarImageComponent.sprite = radarChartSprite5Edges;
                radarImageComponent.rectTransform.localScale = Vector3.one;
                radarMeshRectTransform.anchoredPosition = new Vector2(0, -5);
                break;

            case ERadarChart.NumberOfEdges.Nine:
                radarImageComponent.sprite = radarChartSprite9Edges;
                radarImageComponent.rectTransform.localScale = Vector3.one;
                radarMeshRectTransform.anchoredPosition = new Vector2(0, -5);
                break;

            default:
                radarImageComponent.sprite = null;
                radarImageComponent.rectTransform.localScale = Vector3.one;
                radarMeshRectTransform.anchoredPosition = new Vector2(0, -5);
                Debug.LogWarning("Unsupported number of edges.");
                break;
        }

        // Generate radar mesh.
        Mesh mesh = new Mesh();
        int vertexCount = (int)numberOfEdges + 1; // +1 for center vertex.
        Vector3[] vertices = new Vector3[vertexCount];
        Vector2[] uv = new Vector2[vertexCount];
        int[] triangles = new int[3 * (int)numberOfEdges];

        float angleIncrement = 360f / (int)numberOfEdges;
        float radarChartSize = 145f;

        // Define vertices: center and outer points.
        vertices[0] = Vector3.zero; // Center vertex.
        for (int i = 0; i < (int)numberOfEdges; i++)
        {
            // Calculate position for each vertex based on stat values.
            Vector3 vertex = Quaternion.Euler(0, 0, -angleIncrement * i) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(i);
            vertices[i + 1] = vertex;
        }

        // Set UV coordinates for mesh (not used in this context but required).
        for (int i = 0; i < vertexCount; i++)
        {
            uv[i] = new Vector2(vertices[i].x, vertices[i].y);
        }

        // Create triangles for the mesh.
        for (int i = 0; i < (int)numberOfEdges; i++)
        {
            int nextIndex = (i + 1) % (int)numberOfEdges;
            triangles[i * 3] = 0; // Center vertex.
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = nextIndex + 1;
        }

        // Assign vertices, UVs, and triangles to mesh.
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        // Set the mesh and material to the CanvasRenderer.
        radarMeshCanvasRenderer.SetMesh(mesh);
        radarMeshCanvasRenderer.SetMaterial(radarMaterial, null);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class UIRadarChart : MonoBehaviour
{
    [SerializeField] private Material radarMaterial;
    [SerializeField] private Sprite radarChartSprite3Edges;
    [SerializeField] private Sprite radarChartSprite5Edges;
    [SerializeField] private Sprite radarChartSprite9Edges;

    public ERadarChart.NumberOfEdges numberOfEdges; // Dropdown for number of edges

    private RadarStats stats;
    private CanvasRenderer radarMeshCanvasRenderer;
    private RectTransform radarMeshRectTransform; // RectTransform for moving the radarMesh
    private Image radarImageComponent;

    private void Awake()
    {
        radarMeshCanvasRenderer = transform.Find("radarMesh").GetComponent<CanvasRenderer>();
        radarMeshRectTransform = radarMeshCanvasRenderer.GetComponent<RectTransform>(); // Get the RectTransform
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

    private void Update()
    {
        UpdateStatsVisual();
    }

    private void UpdateStatsVisual()
    {
        // Set the sprite based on the number of edges
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

        // Create the radar mesh and update its visual representation
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
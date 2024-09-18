using UnityEngine;

public class UIRadarChart : MonoBehaviour {

    [SerializeField] private Material _radarMaterial;
    [SerializeField] private Texture2D _radarTexture2D;

    private RadarStats stats;
    private CanvasRenderer radarMeshCanvasRenderer;

    private void Awake() {
        radarMeshCanvasRenderer = transform.Find("radarMesh").GetComponent<CanvasRenderer>();
    }

    public void SetStats(RadarStats stats) {
        this.stats = stats;
        stats.OnStatsChanged += Stats_OnStatsChanged;
        UpdateStatsVisual();
    }

    private void Stats_OnStatsChanged(object sender, System.EventArgs e) {
        UpdateStatsVisual();
    }

    private void UpdateStatsVisual() {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[10];
        Vector2[] uv = new Vector2[10];
        int[] triangles = new int[3 * 9];

        float angleIncrement = 360f / 9;
        float radarChartSize = 145f;

        Vector3 attackVertex = Quaternion.Euler(0, 0, -angleIncrement * 0) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(RadarStats.Type.Attack);
        int attackVertexIndex = 1;
        Vector3 defenceVertex = Quaternion.Euler(0, 0, -angleIncrement * 1) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(RadarStats.Type.Defence);
        int defenceVertexIndex = 2;
        Vector3 speedVertex = Quaternion.Euler(0, 0, -angleIncrement * 2) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(RadarStats.Type.Speed);
        int speedVertexIndex = 3;
        Vector3 manaVertex = Quaternion.Euler(0, 0, -angleIncrement * 3) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(RadarStats.Type.Mana);
        int manaVertexIndex = 4;
        Vector3 healthVertex = Quaternion.Euler(0, 0, -angleIncrement * 4) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(RadarStats.Type.Health);
        int healthVertexIndex = 5;
        Vector3 aVertex = Quaternion.Euler(0, 0, -angleIncrement * 5) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(RadarStats.Type.A);
        int aVertexIndex = 6;
        Vector3 bVertex = Quaternion.Euler(0, 0, -angleIncrement * 6) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(RadarStats.Type.B);
        int bVertexIndex = 7;
        Vector3 cVertex = Quaternion.Euler(0, 0, -angleIncrement * 7) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(RadarStats.Type.C);
        int cVertexIndex = 8;
        Vector3 dVertex = Quaternion.Euler(0, 0, -angleIncrement * 8) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(RadarStats.Type.D);
        int dVertexIndex = 9;

        vertices[0] = Vector3.zero;
        vertices[attackVertexIndex]  = attackVertex;
        vertices[defenceVertexIndex] = defenceVertex;
        vertices[speedVertexIndex]   = speedVertex;
        vertices[manaVertexIndex]    = manaVertex;
        vertices[healthVertexIndex]  = healthVertex;
        vertices[aVertexIndex]       = aVertex;
        vertices[bVertexIndex]       = bVertex;
        vertices[cVertexIndex]       = cVertex;
        vertices[dVertexIndex]       = dVertex;

        uv[0]                   = Vector2.zero;
        uv[attackVertexIndex]   = Vector2.one;
        uv[defenceVertexIndex]  = Vector2.one;
        uv[speedVertexIndex]    = Vector2.one;
        uv[manaVertexIndex]     = Vector2.one;
        uv[healthVertexIndex]   = Vector2.one;
        uv[aVertexIndex]        = Vector2.one;
        uv[bVertexIndex]        = Vector2.one;
        uv[cVertexIndex]        = Vector2.one;
        uv[dVertexIndex]        = Vector2.one; 

        triangles[0] = 0;
        triangles[1] = attackVertexIndex;
        triangles[2] = defenceVertexIndex;

        triangles[3] = 0;
        triangles[4] = defenceVertexIndex;
        triangles[5] = speedVertexIndex;

        triangles[6] = 0;
        triangles[7] = speedVertexIndex;
        triangles[8] = manaVertexIndex;

        triangles[9]  = 0;
        triangles[10] = manaVertexIndex;
        triangles[11] = healthVertexIndex;

        triangles[12] = 0;
        triangles[13] = healthVertexIndex;
        triangles[14] = aVertexIndex;

        triangles[15] = 0;
        triangles[16] = aVertexIndex;
        triangles[17] = bVertexIndex;

        triangles[18] = 0;
        triangles[19] = bVertexIndex;
        triangles[20] = cVertexIndex;

        triangles[21] = 0;
        triangles[22] = cVertexIndex;
        triangles[23] = dVertexIndex;

        triangles[24] = 0;
        triangles[25] = dVertexIndex;
        triangles[26] = attackVertexIndex;



        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        radarMeshCanvasRenderer.SetMesh(mesh);
        radarMeshCanvasRenderer.SetMaterial(_radarMaterial, _radarTexture2D);
    }
}
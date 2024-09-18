using UnityEngine;
using CodeMonkey;

public class RadarChartController : MonoBehaviour {

    [SerializeField] private UIRadarChart _uiRadarChart;
    [SerializeField] private UIRadarChart _uiTestRadarChart;
    public RadarStats _stats = new RadarStats(20,10,20,5,12,3,20,18,13);

    private void Start() {
        _uiRadarChart.SetStats(_stats);
        _uiTestRadarChart.SetStats(_stats);
        /* 
        CMDebug.ButtonUI(new Vector2(100, +20), "ATK++", () => stats.IncreaseStatAmount(RadarStats.Type.Attack));
        CMDebug.ButtonUI(new Vector2(100, -20), "ATK--", () => stats.DecreaseStatAmount(RadarStats.Type.Attack));
        
        CMDebug.ButtonUI(new Vector2(180, +20), "DEF++", () => stats.IncreaseStatAmount(RadarStats.Type.Defence));
        CMDebug.ButtonUI(new Vector2(180, -20), "DEF--", () => stats.DecreaseStatAmount(RadarStats.Type.Defence));
        
        CMDebug.ButtonUI(new Vector2(260, +20), "SPD++", () => stats.IncreaseStatAmount(RadarStats.Type.Speed));
        CMDebug.ButtonUI(new Vector2(260, -20), "SPD--", () => stats.DecreaseStatAmount(RadarStats.Type.Speed));
        
        CMDebug.ButtonUI(new Vector2(340, +20), "MAN++", () => stats.IncreaseStatAmount(RadarStats.Type.Mana));
        CMDebug.ButtonUI(new Vector2(340, -20), "MAN--", () => stats.DecreaseStatAmount(RadarStats.Type.Mana));
        
        CMDebug.ButtonUI(new Vector2(420, +20), "HEL++", () => stats.IncreaseStatAmount(RadarStats.Type.Health));
        CMDebug.ButtonUI(new Vector2(420, -20), "HEL--", () => stats.DecreaseStatAmount(RadarStats.Type.Health));
        */
    }
}
using System;
using UnityEngine;

[System.Serializable]
public class RadarStat {
    public string Name;
    public int Value;
}

public class RadarStats : MonoBehaviour {

    public RadarStat[] radarStats;

    public event EventHandler OnStatsChanged;

    private void Awake() {
        // Initialize or handle any setup if necessary
    }

    public int GetEdgesCount() {
        return radarStats.Length;
    }

    public void SetStatAmount(int index, int statAmount) {
        if (index >= 0 && index < radarStats.Length) {
            radarStats[index].Value = Mathf.Clamp(statAmount, 0, 20);
            OnStatsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public int GetStatAmount(int index) {
        if (index >= 0 && index < radarStats.Length) {
            return radarStats[index].Value;
        }
        return 0;
    }

    public float GetStatAmountNormalized(int index) {
        if (index >= 0 && index < radarStats.Length) {
            return (float)radarStats[index].Value / 20; // Assuming 20 is the max value
        }
        return 0f;
    }
}
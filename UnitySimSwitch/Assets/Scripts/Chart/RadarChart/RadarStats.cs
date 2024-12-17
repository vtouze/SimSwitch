using System;
using UnityEngine;

/// <summary>
/// Represents a single stat for a radar chart.
/// </summary>
[Serializable]
public class RadarStat
{
    public string Name; // Stat name.
    public int Value;   // Stat value.
}

/// <summary>
/// Holds and manages radar chart statistics.
/// </summary>
public class RadarStats : MonoBehaviour
{
    public RadarStat[] radarStats;

    /// <summary>
    /// Event triggered when radar stats change.
    /// </summary>
    public event EventHandler OnStatsChanged;

    /// <summary>
    /// Returns the number of stats (edges).
    /// </summary>
    public int GetEdgesCount()
    {
        return radarStats.Length;
    }

    /// <summary>
    /// Sets the stat amount at a specific index.
    /// </summary>
    public void SetStatAmount(int index, int statAmount)
    {
        if (index >= 0 && index < radarStats.Length)
        {
            radarStats[index].Value = Mathf.Clamp(statAmount, 0, 20);
            OnStatsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Retrieves the stat amount for a specific index.
    /// </summary>
    public int GetStatAmount(int index)
    {
        return index >= 0 && index < radarStats.Length ? radarStats[index].Value : 0;
    }

    /// <summary>
    /// Retrieves the normalized stat amount for a specific index.
    /// </summary>
    public float GetStatAmountNormalized(int index)
    {
        return index >= 0 && index < radarStats.Length ? (float)radarStats[index].Value / 20 : 0f;
    }
}
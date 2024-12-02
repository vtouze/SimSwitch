using UnityEngine;

/// <summary>
/// Manages the game state, including pausing and resuming the game. 
/// It provides a singleton instance for easy access and ensures that the game manager persists across scenes.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Fields
    /// <summary>
    /// Singleton instance of the GameManager. Allows easy access to the GameManager from any other script.
    /// </summary>
    public static GameManager Instance { get; private set; }
    private bool _isPaused = true;
    #endregion Fields

    #region Methods
    /// <summary>
    /// Initializes the singleton instance and ensures the GameManager persists across scene changes.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Gets or sets the current pause state of the game.
    /// </summary>
    public bool IsPaused
    {
        get { return _isPaused; }
        set { _isPaused = value; }
    }

    /// <summary>
    /// Toggles the game's pause state. If the game is paused, it sends a "pause" message to the ConnectionManager.
    /// If the game is resumed, it sends a "play" message.
    /// </summary>
    public void TogglePause()
    {
        IsPaused = !IsPaused;

        if (IsPaused)
        {
            ConnectionManager.Instance.SendStatusMessage("pause");
        }
        else
        {
            ConnectionManager.Instance.SendStatusMessage("play");
        }
    }
    #endregion Methods
}
using UnityEngine;
using TMPro;

public class PlayerNameManager : MonoBehaviour
{
    #region Fields
    [Tooltip("The input field where the player will enter their name.")]
    [SerializeField] private TMP_InputField _playerNameInputField; // Input field for player name

    private string _playerName; // Store the player's name
    #endregion Fields

    #region Methods

    /// <summary>
    /// Saves the player's name from the input field to PlayerPrefs.
    /// Also logs the saved name to the console for debugging purposes.
    /// </summary>
    public void SavePlayerName()
    {
        _playerName = _playerNameInputField.text; // Get the text from the input field
        PlayerPrefs.SetString("PlayerName", _playerName); // Save the player name in PlayerPrefs
        Debug.Log("Player Name Saved: " + _playerName); // Log the saved player name
    }

    /// <summary>
    /// Loads the player's name from PlayerPrefs (if available) and sets it in the input field.
    /// Also logs the loaded name to the console for debugging purposes.
    /// </summary>
    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName")) // Check if the player name exists in PlayerPrefs
        {
            _playerName = PlayerPrefs.GetString("PlayerName"); // Load the player name from PlayerPrefs
            _playerNameInputField.text = _playerName; // Set the loaded name in the input field
            Debug.Log("Player Name Loaded: " + _playerName); // Log the loaded player name
        }
    }

    #endregion Methods
}
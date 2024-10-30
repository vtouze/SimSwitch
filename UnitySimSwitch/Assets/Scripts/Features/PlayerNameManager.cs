using UnityEngine;
using TMPro;

public class PlayerNameManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private TMP_InputField _playerNameInputField;
    private string _playerName;
    #endregion Fields

    #region Methods
    public void SavePlayerName()
    {
        _playerName = _playerNameInputField.text;
        PlayerPrefs.SetString("PlayerName", _playerName);
        Debug.Log("Player Name Saved: " + _playerName);
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            _playerName = PlayerPrefs.GetString("PlayerName");
            _playerNameInputField.text = _playerName;
            Debug.Log("Player Name Loaded: " + _playerName);
        }
    }
    #endregion Methods
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EncyclopediaManager : MonoBehaviour
{
    #region Fields
    [Header("UI Elements")]
    public Transform _buttonListParent;
    public GameObject _buttonPrefab;
    public Image _coverImage;
    public TMP_Text _entryTitle;
    public TMP_Text _entryDescription;
    public TMP_Text _entryArticle;

    [SerializeField] private Scrollbar _scrollbar = null;

    [Header("Data")]
    public List<EncyclopediaEntry> _entries;
    #endregion Fields

    #region Methods
    private void Awake()
    {
        DisplayEntry(_entries[0]);
        PopulateButtons();
    }

    private void Start()
    {
        Canvas.ForceUpdateCanvases();
        _scrollbar.value = 1;
    }

    void PopulateButtons()
    {
        foreach (EncyclopediaEntry entry in _entries)
        {
            GameObject newButton = Instantiate(_buttonPrefab, _buttonListParent);
            newButton.GetComponentInChildren<TMP_Text>().text = entry._entryName;

            newButton.GetComponent<Button>().onClick.AddListener(() => DisplayEntry(entry));
        }
    }

    void DisplayEntry(EncyclopediaEntry entry)
    {
        _coverImage.sprite = entry._coverImage;
        _entryTitle.text = entry._entryName;
        _entryDescription.text = entry._description;
        _entryArticle.text = entry._articles;
    }
    #endregion Methods
}
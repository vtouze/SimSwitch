using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Image))]
public class RoadSelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    #region Fields
    private Image roadImage;
    private bool isSelected;
    [HideInInspector] public bool isUnderConstruction, hasBeenUnderConstruction;
    private float constructionTime = 10f, currentConstructionTime;

    [Header("Construction")]
    [SerializeField] private Sprite constructionRoadSprite;
    [SerializeField] private Sprite defaultRoadSprite;
    [SerializeField] private GameObject constructionOverlay;
    [SerializeField] private TMP_Text constructionText;
    private Image _lineRoad = null;
    private Image _lineRoad2 = null;
    private Image progressBar;
    private GameObject transportGameObject;

    [Header("Transports Icons")]
    [SerializeField] private Sprite bikeIcon;
    [SerializeField] private Sprite carIcon;
    [SerializeField] private Sprite busIcon;

    [Header("Overlay Buttons")]
    [SerializeField] private Button validateButton;
    [SerializeField] private Button abortButton;

    [Header("Road Entries")]
    [SerializeField] private RoadsEntry bikeEntries;
    [SerializeField] private RoadsEntry carEntries;
    [SerializeField] private RoadsEntry busEntries;

    private GameObject progressBarParent;
    private Animator progressBarAnimator, progressBarParentAnimator;
    private Color originalColor;
    private Outline roadOutline;
    private PublicWorksType? selectedType, currentTransportType;
    private ParticleSystem confettiParticles;
    private Image _transportsIcon;
    private Sprite defaultLineSprite;
    private Color defaultLineColor;
    private float defaultPixelsPerUnitMultiplier;

    [Header("Colors")]
    private static readonly Color GreenColor = ParseHexColor("#01A800");
    private static readonly Color OrangeColor = ParseHexColor("#E8B01B");
    private static readonly Color RedColor = ParseHexColor("#DE3F2B");
    #endregion Fields

    #region Unity Lifecycle
    
    private void Start()
    {
        CacheReferences();
        InitializeUI();
        AddButtonListeners();
    }

    private void CacheReferences()
    {
        roadImage = GetComponent<Image>();
        originalColor = roadImage.color;
        roadOutline = GetComponent<Outline>();
        confettiParticles = transform.GetChild(3).GetComponent<ParticleSystem>();
        _lineRoad = transform.GetChild(0).GetComponent<Image>();

        if (_lineRoad.transform.childCount > 0)
        {
            _lineRoad2 = _lineRoad.transform.GetChild(0).GetComponent<Image>();
        }
        else
        {
            _lineRoad2 = null;
        }

        _transportsIcon = transform.GetChild(1).GetComponent<Image>();

        if (_lineRoad != null)
        {
            defaultLineSprite = _lineRoad.sprite;
            defaultLineColor = _lineRoad.color;
            defaultPixelsPerUnitMultiplier = _lineRoad.pixelsPerUnitMultiplier;
        }

        Transform constructionBar = transform.GetChild(2);
        if (constructionBar != null)
        {
            progressBarParent = constructionBar.gameObject;
            progressBarAnimator = constructionBar.GetComponent<Animator>();
            progressBarParentAnimator = progressBarAnimator;
            progressBar = constructionBar.GetChild(2).GetComponent<Image>();
            transportGameObject = constructionBar.GetChild(1).gameObject;
        }
    }

    private void InitializeUI()
    {
        constructionOverlay.SetActive(false);
        _transportsIcon.gameObject.SetActive(false);
        defaultRoadSprite = roadImage.sprite;
        isUnderConstruction = isSelected = hasBeenUnderConstruction = false;
        if (roadOutline != null)
        {
            roadOutline.enabled = false;
            roadOutline.effectDistance = new Vector2(5, 5);
        }
        progressBarParent?.SetActive(false);
        selectedType = currentTransportType = null;
    }
    private void AddButtonListeners()
    {
        validateButton.onClick.AddListener(ValidateConstruction);
        abortButton.onClick.AddListener(AbortConstruction);
    }

    #endregion Unity Lifecycle

    #region Pointer Handlers
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected && !isUnderConstruction) roadOutline.enabled = true;
    }   

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected && !isUnderConstruction) roadOutline.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isUnderConstruction) CompleteConstruction();
        else HandleNewConstruction();
    }

    #endregion Pointer Handlers

    #region Overlay Actions

    private void ValidateConstruction()
    {
        CloseOverlay();
        if (selectedType.HasValue && hasBeenUnderConstruction)
        {
            currentTransportType = selectedType.Value;
            StartConstruction();
        }
        PublicWorksButton.StopFollowing();
        PublicWorksButton.lastSelectedButton?.HideFeedback();
    }

    private void AbortConstruction()
    {
        CloseOverlay();
        PublicWorksButton.StopFollowing();
        PublicWorksButton.lastSelectedButton?.HideFeedback();

        if (selectedType.HasValue)
        {
            ConstructionManager.Instance.RemoveTransportType(this);
            selectedType = null;
        }

        progressBarParent?.SetActive(false);
        transportGameObject.GetComponent<Image>().sprite = null;
        progressBar.fillAmount = 0;

        roadImage.sprite = defaultRoadSprite;
        roadOutline.enabled = false;
        isSelected = false;
        hasBeenUnderConstruction = false;
    }


    private void CloseOverlay()
    {
        constructionOverlay.GetComponent<Animator>()?.SetBool("isOpeningMenu", false);
    }

    #endregion Overlay Actions

    #region Construction

    private void HandleNewConstruction()
    {
        if (!hasBeenUnderConstruction && !isUnderConstruction && PublicWorksButton.selectedPublicWorksType != null)
        {
            selectedType = PublicWorksButton.selectedPublicWorksType;
            ConstructionManager.Instance.RemoveTransportType(this);
            ConstructionManager.Instance.AddTransportType(this, selectedType.Value);
            ShowPreConstructionUI(selectedType.Value);
            isSelected = true;
            DeselectOtherRoads();
            hasBeenUnderConstruction = true;
        }
    }

    private void ShowPreConstructionUI(PublicWorksType type)
    {
        transportGameObject.GetComponent<Image>().sprite = GetTransportIcon(type);
        progressBarParent?.SetActive(true);
        progressBar.fillAmount = 0;
        constructionOverlay.SetActive(true);
        constructionOverlay.GetComponent<Animator>()?.SetBool("isOpeningMenu", true);
        UpdateConstructionText();
    }

    private void StartConstruction()
    {
        isUnderConstruction = true;
        roadImage.sprite = constructionRoadSprite;
        roadOutline.enabled = false;
        constructionTime = GetConstructionTimeForType(selectedType.Value);
        TriggerIdleAnimation(true);

        if (_lineRoad != null)
        {
            _lineRoad.sprite = defaultLineSprite;
            _lineRoad.color = defaultLineColor;
            _lineRoad.pixelsPerUnitMultiplier = defaultPixelsPerUnitMultiplier;
        }

        StartCoroutine(ConstructionTimer());
    }

    private IEnumerator ConstructionTimer()
    {
        currentConstructionTime = 0f;
        while (currentConstructionTime < constructionTime)
        {
            currentConstructionTime += Time.deltaTime;
            progressBar.fillAmount = currentConstructionTime / constructionTime;
            yield return null;
        }
        CompleteConstruction();
    }

    private void CompleteConstruction()
    {
        if (!isUnderConstruction) return; 
        roadImage.sprite = defaultRoadSprite;
        _lineRoad = GetRoadLine(selectedType.Value);
        confettiParticles.Play();
        _transportsIcon.sprite = GetTransportIcon(selectedType.Value);
        _transportsIcon.gameObject.SetActive(true);
        ResetConstructionStatus();
    }

    private void ResetConstructionStatus()
    {
        isUnderConstruction = hasBeenUnderConstruction = isSelected = false;
        progressBarParent.SetActive(false);
        ConstructionManager.Instance.RemoveTransportType(this);
    }

    #endregion Construction

    #region Utility

    private void DeselectOtherRoads()
    {
        foreach (var road in FindObjectsOfType<RoadSelection>())
        {
            if (road != this) road.Deselect();
        }
    }

    private void Deselect()
    {
        isSelected = false;
        roadImage.color = originalColor;
        roadOutline.enabled = false;
        if (isUnderConstruction) CompleteConstruction();
    }

    private void UpdateConstructionText()
    {
        var (totalCost, totalDuration) = ConstructionManager.Instance.GetTotalCostAndDuration(new Dictionary<PublicWorksType, RoadsEntry>
        {
            { PublicWorksType.BIKE, bikeEntries },
            { PublicWorksType.CAR, carEntries },
            { PublicWorksType.BUS, busEntries }
        });

        var constructionCounts = ConstructionManager.Instance.GetConstructionCountsByType();

        string constructionTypesText = string.Join(", ", constructionCounts
            .Select(kv => kv.Value > 1 ? $"{kv.Key} ({kv.Value})" : kv.Key.ToString()));

        constructionText.text = $"{(totalDuration > 1 ? "Constructions" : "Construction")} for: " +
                                $"{constructionTypesText}\n" +
                                $"Total Cost: {FormatCost(totalCost)}\n" +
                                $"Total Duration: {totalDuration} {(totalDuration > 1 ? "years" : "year")}";
    }

    private Sprite GetTransportIcon(PublicWorksType type) => type switch
    {
        PublicWorksType.BIKE => bikeIcon,
        PublicWorksType.CAR => carIcon,
        PublicWorksType.BUS => busIcon,
        _ => defaultRoadSprite
    };

    private Image GetRoadLine(PublicWorksType type)
    {
        switch (type)
        {
            case PublicWorksType.BIKE:
                SetRoadLineProperties(2, GreenColor);
                break;
            case PublicWorksType.BUS:
                SetRoadLineProperties(1, OrangeColor);
                break;
            case PublicWorksType.CAR:
                SetRoadLineProperties(0.5f, RedColor);
                break;
        }

        if (_lineRoad2 != null)
        {
            _lineRoad2.pixelsPerUnitMultiplier = _lineRoad.pixelsPerUnitMultiplier;
            _lineRoad2.color = _lineRoad.color;
            _lineRoad2.sprite = _lineRoad.sprite;
            _lineRoad2.transform.SetParent(_lineRoad.transform);
        }   

        return _lineRoad;
    }

    private void SetRoadLineProperties(float pixelsPerUnitMultiplier, Color color)
    {
        _lineRoad.pixelsPerUnitMultiplier = pixelsPerUnitMultiplier;
        _lineRoad.color = color;
    }

    private float GetConstructionTimeForType(PublicWorksType type) => type switch
    {
        PublicWorksType.BIKE => bikeEntries._duration * 15,
        PublicWorksType.CAR => carEntries._duration * 15,
        PublicWorksType.BUS => busEntries._duration * 15,
        _ => 10f
    };

    private void TriggerIdleAnimation(bool state) => progressBarParentAnimator?.SetBool("isIdle", state);

    private string FormatCost(float cost) =>
        cost >= 1_000_000 ? $"{cost / 1_000_000:0.##}M" :
        cost >= 1_000 ? $"{cost / 1_000:0.##}k" :
        cost.ToString("0.##");

    private static Color ParseHexColor(string hexColor)
    {
        if (ColorUtility.TryParseHtmlString(hexColor, out Color color))
        {
            return color;
        }
        return Color.white;
    }
    #endregion Utility
}
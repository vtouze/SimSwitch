using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Image))]
public class RoadSelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    #region Fields

    private Image roadImage;
    private bool isSelected = false;
    [HideInInspector] public bool isUnderConstruction = false;
    [HideInInspector] public bool hasBeenUnderConstruction = false;
    private float constructionTime = 10f;
    private float currentConstructionTime = 0f;

    [Header("Construction")]
    [SerializeField] private Sprite constructionRoadSprite;
    [SerializeField] private Sprite defaultRoadSprite;
    [SerializeField] private GameObject _constructionOverlay = null;
    [SerializeField] private TMP_Text constructionText;
    private Image _lineRoad = null;
    private Image progressBar;
    private GameObject transportGameObject;

    [Header("Transports Icons")]
    [SerializeField] private Sprite bikeIcon;
    [SerializeField] private Sprite carIcon;
    [SerializeField] private Sprite busIcon;

    [Header("Overlay Buttons")]
    [SerializeField] private Button validateButton;
    [SerializeField] private Button abortButton;

    private GameObject progressBarParent;
    private Animator progressBarAnimator;
    private Animator progressBarParentAnimator;

    private Color originalColor;
    private Sprite currentTransportSprite;
    private Outline roadOutline;
    private PublicWorksType? selectedType = null;
    private PublicWorksType? currentTransportType = null;
    
    private ParticleSystem _confettiParticles = null;

    [Header("Road Entries")]
    [SerializeField] private RoadsEntries bikeEntries;
    [SerializeField] private RoadsEntries carEntries;
    [SerializeField] private RoadsEntries busEntries;

    #endregion Fields

    private void Start()
    {
        Transform constructionBar = gameObject.transform.GetChild(1).transform;
        if (constructionBar != null)
        {
            progressBarParent = constructionBar.gameObject;
            progressBarAnimator = constructionBar.GetComponent<Animator>();
            progressBarParentAnimator = constructionBar.GetComponent<Animator>();

            Transform pbFill = gameObject.transform.GetChild(1).GetChild(2).transform;
            if (pbFill != null)
            {
                progressBar = pbFill.GetComponent<Image>();
            }

            Transform transportsIcon = gameObject.transform.GetChild(1).GetChild(1).transform;
            if (transportsIcon != null)
            {
                transportGameObject = transportsIcon.gameObject;
            }
        }

        _confettiParticles = gameObject.transform.GetChild(2).GetComponent<ParticleSystem>();
        _lineRoad = gameObject.transform.GetChild(0).GetComponent<Image>();

        _constructionOverlay.SetActive(false);
        roadImage = GetComponent<Image>();
        originalColor = roadImage.color;
        roadOutline = GetComponent<Outline>();

        if (roadOutline != null)
        {
            roadOutline.enabled = false;
            roadOutline.effectDistance = new Vector2(5, 5);
        }

        if (progressBarParent != null)
        {
            progressBarParent.SetActive(false);
        }

        defaultRoadSprite = GetComponent<Image>().sprite;
        isUnderConstruction = false;
        selectedType = null;
        currentTransportType = null;

        validateButton.onClick.AddListener(ValidateConstruction);
        abortButton.onClick.AddListener(AbortConstruction);
    }

    #region PointerHandler

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected && !isUnderConstruction)
        {
            if (roadOutline != null)
            {
                roadOutline.enabled = true;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected && !isUnderConstruction)
        {
            if (roadOutline != null)
            {
                roadOutline.enabled = false;
            }
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (isUnderConstruction)
        {
            CompleteConstruction();
        }

        if (!hasBeenUnderConstruction && !isUnderConstruction && PublicWorksButton.selectedPublicWorksType != null)
        {
            selectedType = (PublicWorksType)PublicWorksButton.selectedPublicWorksType;

            if (!isUnderConstruction)
            {
                ConstructionManager.Instance.RemoveTransportType(this);
                ConstructionManager.Instance.AddTransportType(this, selectedType.Value);

                _constructionOverlay.SetActive(true);
                ShowPreConstructionUI(selectedType.Value);

                Animator anim = _constructionOverlay.GetComponent<Animator>();
                if (anim != null) anim.SetBool("isOpeningMenu", true);

                UpdateConstructionText();
            }

            isSelected = true;
            DeselectAllOtherRoads();

            hasBeenUnderConstruction = true;
        }
    }   

 
    #endregion PointerHandler

    #region OverlayActions

    private void ValidateConstruction()
    {
        CloseConstructionOverlay();

        if (selectedType.HasValue && hasBeenUnderConstruction)
        {
            currentTransportType = selectedType.Value;
            StartConstruction(selectedType.Value);
        }
        else
        {
            Debug.LogWarning("No transport type selected for construction.");
        }

        PublicWorksButton.StopFollowing();
        PublicWorksButton.lastSelectedButton?.HideFeedback();
    }
    private void AbortConstruction()
    {
        CloseConstructionOverlay();

        PublicWorksButton.StopFollowing();
        PublicWorksButton.lastSelectedButton?.HideFeedback();

        isSelected = false;
    }

    private void CloseConstructionOverlay()
    {
        Animator anim = _constructionOverlay.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetBool("isOpeningMenu", false);
        }
    }

    #endregion OverlayActions

    #region Construction

    private void ShowPreConstructionUI(PublicWorksType type)
    {
        transportGameObject.GetComponent<Image>().sprite = currentTransportSprite;
        SetTransportIcon(type);

        if (progressBarParent != null)
        {
            progressBarParent.SetActive(true);
            progressBar.fillAmount = 0;
        }
    }

    private void StartConstruction(PublicWorksType type)
    {
        if (isUnderConstruction) return;

        isUnderConstruction = true;
        roadImage.sprite = constructionRoadSprite;

        if (roadOutline != null)
        {
            roadOutline.enabled = false;
        }

        constructionTime = GetConstructionTimeForType(type);

        TriggerIdleAnimation(true);
        StartCoroutine(ConstructionTimer());
    }


    private float GetConstructionTimeForType(PublicWorksType type)
    {
        switch (type)
        {
            case PublicWorksType.BIKE:
                return bikeEntries._duration * 15;
            case PublicWorksType.CAR:
                return carEntries._duration * 15;
            case PublicWorksType.BUS:
                return busEntries._duration * 15;
            default:
                return 10f;
        }
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

        if (selectedType.HasValue)
        {
            roadImage.sprite = currentTransportSprite ?? defaultRoadSprite;
        }

        isUnderConstruction = false;
        isSelected = false;
        progressBarParent.SetActive(false);
        _confettiParticles.Play();

        ConstructionManager.Instance.RemoveTransportType(this);

        if (selectedType.HasValue)
        {
            _lineRoad = GetRoadLine(selectedType.Value);
        }
        
        ResetConstructionStatus();
    }


    private void ResetConstructionStatus()
    {
        hasBeenUnderConstruction = false;
        isUnderConstruction = false;
        isSelected = false;
    }

    #endregion Construction

    private void TriggerIdleAnimation(bool state)
    {
        if (progressBarParentAnimator != null && progressBarParentAnimator.isActiveAndEnabled)
        {
            progressBarParentAnimator.SetBool("isIdle", state);
        }
    }

    private void DeselectAllOtherRoads()
    {
        RoadSelection[] allRoads = FindObjectsOfType<RoadSelection>();
        foreach (RoadSelection road in allRoads)
        {
            if (road != this)
            {
                road.isSelected = false;
                road.roadImage.color = road.originalColor;
                road.roadOutline.enabled = false;

                if (road.isUnderConstruction)
                {
                    road.CompleteConstruction();
                }
            }
        }
    }

    private void UpdateConstructionText()
    {
        Dictionary<PublicWorksType, RoadsEntries> transportEntries = new Dictionary<PublicWorksType, RoadsEntries>
        {   
            { PublicWorksType.BIKE, bikeEntries },
            { PublicWorksType.CAR, carEntries },
            { PublicWorksType.BUS, busEntries }
        };

        var (totalCost, totalDuration) = ConstructionManager.Instance.GetTotalCostAndDuration(transportEntries);

        string transportNames = ConstructionManager.Instance.GetSelectedTransportTypesAsString();
        string formattedCost = FormatCost(totalCost);

        string constructionTextFormatted = (ConstructionManager.Instance.GetSelectedTransportCount() > 1) ? "Constructions" : "Construction";

        string durationText = totalDuration > 1 ? "years" : "year";

        constructionText.text = $"{constructionTextFormatted} for: {transportNames}\n" +
                                $"Total Cost: {formattedCost}\n" +
                                $"Total Duration: {totalDuration} {durationText}";
    }


    #region Misc
    private string FormatCost(float cost)
    {
        if (cost >= 1000000)
        {
            return (cost / 1000000).ToString("0.##") + "M";
        }
        else if (cost >= 1000)
        {
            return (cost / 1000).ToString("0.##") + "k";
        }
        else
        {
            return cost.ToString("0.##");
        }
    } 


    private void SetTransportIcon(PublicWorksType type)
    {
        switch (type)
        {
            case PublicWorksType.BIKE:
                transportGameObject.GetComponent<Image>().sprite = bikeIcon;
                break;
            case PublicWorksType.CAR:
                transportGameObject.GetComponent<Image>().sprite = carIcon;
                break;
            case PublicWorksType.BUS:
                transportGameObject.GetComponent<Image>().sprite = busIcon;
                break;
        }
    }

    private Image GetRoadLine(PublicWorksType type)
    {
        switch (type)
        {
            case PublicWorksType.BIKE:
                SetRoadLineProperties(2, Color.green);
                break;
            case PublicWorksType.BUS:
                SetRoadLineProperties(1, Color.yellow);
                break;
            case PublicWorksType.CAR:
                SetRoadLineProperties(0.5f, Color.red);
                break;
            default:
                Debug.LogWarning("Unsupported PublicWorksType");
                break;
        }

        return _lineRoad;
    }

    private void SetRoadLineProperties(float pixelsPerUnitMultiplier, Color color)
    {
        _lineRoad.pixelsPerUnitMultiplier = pixelsPerUnitMultiplier;
        _lineRoad.color = color;
    }
    #endregion Misc
}
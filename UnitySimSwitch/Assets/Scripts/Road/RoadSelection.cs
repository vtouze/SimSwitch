using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Image))]
public class RoadSelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    #region Fields

    private Image roadImage;
    private bool isSelected = false;
    private bool isUnderConstruction = false;
    private float constructionTime = 10f;
    private float currentConstructionTime = 0f;

    [Header("Construction")]
    [SerializeField] private Sprite constructionRoadSprite;
    [SerializeField] private GameObject _constructionOverlay = null;
    [SerializeField] private TMP_Text constructionText;
    private Image progressBar;

    [Header("Road Sprites")]
    [SerializeField] private Sprite bikeSprite;
    [SerializeField] private Sprite carSprite;
    [SerializeField] private Sprite busSprite;

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
    private PublicWorksType selectedType;
    private PublicWorksType? currentTransportType = null;
    
    private ParticleSystem _confettiParticles = null;

    [Header("Road Entries")]
    [SerializeField] private RoadsEntries bikeEntries;
    [SerializeField] private RoadsEntries carEntries;
    [SerializeField] private RoadsEntries busEntries;

    #endregion Fields

    private void Start()
    {
        Transform constructionBar = gameObject.transform.GetChild(0).transform;
        if (constructionBar != null)
        {
            progressBarParent = constructionBar.gameObject;
            progressBarAnimator = constructionBar.GetComponent<Animator>();
            progressBarParentAnimator = constructionBar.GetComponent<Animator>();

            Transform pbFill = gameObject.transform.GetChild(0).GetChild(2).transform;
            if (pbFill != null)
            {
                progressBar = pbFill.GetComponent<Image>();
            }

            Transform transportsIcon = gameObject.transform.GetChild(0).GetChild(1).transform;
            if (transportsIcon != null)
            {
                transportGameObject = transportsIcon.gameObject;
            }
        }

        _confettiParticles = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();

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
        if (!isUnderConstruction && PublicWorksButton.selectedPublicWorksType != null)
        {
            if (roadOutline != null)
            {
                roadOutline.enabled = false;
            }
            
            selectedType = (PublicWorksType)PublicWorksButton.selectedPublicWorksType;
        
            if (currentTransportType.HasValue)
            {
                ConstructionManager.Instance.RemoveTransportType(currentTransportType.Value);
            }

            currentTransportType = selectedType;
            ConstructionManager.Instance.AddTransportType(selectedType);

            _constructionOverlay.SetActive(true);
            ShowPreConstructionUI(selectedType);
        
            Animator anim = _constructionOverlay.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetBool("isOpeningMenu", true);
            }

            UpdateConstructionText();
        }

        DeselectAllRoads();
        isSelected = true;
    }

    #endregion PointerHandler

    #region OverlayActions

    private void ValidateConstruction()
    {
        CloseConstructionOverlay();
        currentTransportType = null;
        ConstructionManager.Instance.ClearTransportTypes();
        StartConstruction(selectedType);

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
        currentTransportSprite = GetTransportSprite(type);
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
        roadImage.sprite = currentTransportSprite;
        isUnderConstruction = false;

        if (progressBarParent != null)
        {
            progressBarParent.SetActive(false);
            TriggerIdleAnimation(false);
        }

        _confettiParticles.Play();
    }

    #endregion Construction

    private void TriggerIdleAnimation(bool state)
    {
        if (progressBarParentAnimator != null && progressBarParentAnimator.isActiveAndEnabled)
        {
            progressBarParentAnimator.SetBool("isIdle", state);
        }
    }

    private void DeselectAllRoads()
    {
        RoadSelection[] allRoads = FindObjectsOfType<RoadSelection>();
        foreach (RoadSelection road in allRoads)
        {
            road.isSelected = false;
            road.roadImage.color = road.originalColor;
            road.roadOutline.enabled = false;
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

        string constructionTextFormatted = (ConstructionManager.Instance.SelectedTransportTypes.Count > 1) ? "Construction" + "s" : "Construction";

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

    private Sprite GetTransportSprite(PublicWorksType type)
    {
        switch (type)
        {
            case PublicWorksType.BIKE:
                return bikeSprite;
            case PublicWorksType.CAR:
                return carSprite;
            case PublicWorksType.BUS:
                return busSprite;
            default:
                return null;
        }
    }
    #endregion Misc
}
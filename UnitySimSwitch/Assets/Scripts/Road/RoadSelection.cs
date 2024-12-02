using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RoadSelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image roadImage;
    private bool isSelected = false;
    private bool isUnderConstruction = false;
    private float constructionTime = 10f; // Time in seconds for the construction
    private float currentConstructionTime = 0f;

    [Header("UI Elements")]
    public Image progressBar; // Circular progress bar
    public Sprite constructionRoadSprite; // Construction sprite

    [Header("Transport Sprites")]
    public Sprite bikeSprite;
    public Sprite carSprite;
    public Sprite busSprite;

    [Header("Transport GameObject")]
    public GameObject transportGameObject; // Single transport GameObject for all types

    [Header("Icons")]
    public Sprite bikeIcon;
    public Sprite carIcon;
    public Sprite busIcon;

    [Header("Progress Bar Parent")]
    [SerializeField] private GameObject progressBarParent; // Parent GameObject of the progress bar

    [Header("Animator")]
    public Animator progressBarAnimator; // Animator component for the progress bar animation

    private Color originalColor;
    private Sprite currentTransportSprite;

    // Outline component reference
    private Outline roadOutline;

    private void Start()
    {
        roadImage = GetComponent<Image>();
        originalColor = roadImage.color;
        roadOutline = GetComponent<Outline>(); // Get the Outline component

        if (roadOutline != null)
        {
            roadOutline.enabled = false; // Disable the outline at the start
            roadOutline.effectDistance = new Vector2(5, 5); // Set outline width (optional)
        }

        Debug.Log($"RoadSelection initialized on {gameObject.name}. Original color set to {originalColor}");

        // Hide the progress bar parent at the start
        if (progressBarParent != null)
        {
            progressBarParent.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected && !isUnderConstruction)
        {
            // Enable the outline on hover with a white color
            if (roadOutline != null)
            {
                roadOutline.enabled = true;
                roadOutline.effectColor = Color.white; // Set outline color to white
            }

            Debug.Log($"Mouse entered {gameObject.name}. Outline enabled.");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected && !isUnderConstruction)
        {
            // Disable the outline when the mouse exits
            if (roadOutline != null)
            {
                roadOutline.enabled = false;
            }

            Debug.Log($"Mouse exited {gameObject.name}. Outline disabled.");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked on {gameObject.name}.");

        if (!isUnderConstruction && PublicWorksButton.selectedPublicWorksType != null)
        {
            StartConstruction((PublicWorksType)PublicWorksButton.selectedPublicWorksType);
            Debug.Log($"Started construction for {PublicWorksButton.selectedPublicWorksType} on {gameObject.name}.");
        }
        else
        {
            Debug.LogWarning("No Public Works type selected or road is under construction.");
        }

        DeselectAllRoads();
        isSelected = true;
        Debug.Log($"{gameObject.name} is now selected.");
    }

    private void StartConstruction(PublicWorksType type)
    {
        isUnderConstruction = true;
        roadImage.sprite = constructionRoadSprite; // Set the road to construction sprite

        // Disable the outline when construction starts
        if (roadOutline != null)
        {
            roadOutline.enabled = false; // Hide the outline during construction
        }

        // Set the corresponding transport sprite
        currentTransportSprite = GetTransportSprite(type);
        transportGameObject.GetComponent<Image>().sprite = currentTransportSprite; // Update transport sprite directly

        // Set the corresponding transport icon
        SetTransportIcon(type);

        // Show the progress bar parent and start the construction timer (coroutine)
        if (progressBarParent != null)
        {
            progressBarParent.SetActive(true);
            TriggerProgressBarAnimation(); // Trigger animation when progress bar is visible
        }

        StartCoroutine(ConstructionTimer());
    }

    private IEnumerator ConstructionTimer()
    {
        currentConstructionTime = 0f;

        // Update the progress bar until the construction is complete
        while (currentConstructionTime < constructionTime)
        {
            currentConstructionTime += Time.deltaTime;
            float progress = currentConstructionTime / constructionTime;
            progressBar.fillAmount = progress;

            yield return null;
        }

        // When construction is done
        CompleteConstruction();
    }

    private void CompleteConstruction()
    {
        // Set the road to the selected transport type sprite
        roadImage.sprite = currentTransportSprite;
        isUnderConstruction = false;
        Debug.Log($"{gameObject.name} construction completed.");

        // Hide the progress bar parent when construction is finished
        if (progressBarParent != null)
        {
            progressBarParent.SetActive(false);
        }
    }

    private void SetTransportIcon(PublicWorksType type)
    {
        switch (type)
        {
            case PublicWorksType.BIKE:
                transportGameObject.GetComponent<Image>().sprite = bikeIcon; // Set the transport icon to bike
                break;
            case PublicWorksType.CAR:
                transportGameObject.GetComponent<Image>().sprite = carIcon; // Set the transport icon to car
                break;
            case PublicWorksType.BUS:
                transportGameObject.GetComponent<Image>().sprite = busIcon; // Set the transport icon to bus
                break;
            default:
                Debug.LogWarning("Unknown Public Works type.");
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

    private void TriggerProgressBarAnimation()
    {
        if (progressBarAnimator != null)
        {
            progressBarAnimator.SetTrigger("StartProgress"); // Trigger the "StartProgress" animation (assuming it's set in the Animator)
        }
    }

    private void DeselectAllRoads()
    {
        RoadSelection[] allRoads = FindObjectsOfType<RoadSelection>();
        foreach (RoadSelection road in allRoads)
        {
            road.isSelected = false;
            road.roadImage.color = road.originalColor;
            road.roadOutline.enabled = false; // Disable outline when deselected
            Debug.Log($"Deselected {road.gameObject.name}. Reverted to original color and outline disabled.");
        }
    }
}
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RoadSelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image roadImage;

    public Color highlightColor = Color.yellow;
    public Sprite bikeSprite;
    public Sprite carSprite;
    public Sprite busSprite;
    private Color originalColor;
    private bool isSelected = false;

    private void Start()
    {
        roadImage = GetComponent<Image>();
        originalColor = roadImage.color;
        Debug.Log($"RoadSelection initialized on {gameObject.name}. Original color set to {originalColor}");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected)
        {
            roadImage.color = highlightColor;
            Debug.Log($"Mouse entered {gameObject.name}. Highlight color applied.");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected)
        {
            roadImage.color = originalColor;
            Debug.Log($"Mouse exited {gameObject.name}. Reverted to original color.");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked on {gameObject.name}.");

        if (PublicWorksButton.selectedPublicWorksType != null)
        {
            ApplyPublicWorks((PublicWorksType)PublicWorksButton.selectedPublicWorksType);
            Debug.Log($"Applied {PublicWorksButton.selectedPublicWorksType} to {gameObject.name}.");
        }
        else
        {
            Debug.LogWarning("No Public Works type selected.");
        }

        DeselectAllRoads();
        isSelected = true;
        Debug.Log($"{gameObject.name} is now selected.");
    }

    private void ApplyPublicWorks(PublicWorksType type)
    {
        switch (type)
        {
            case PublicWorksType.BIKE:
                roadImage.sprite = bikeSprite;
                Debug.Log($"{gameObject.name} sprite set to BIKE.");
                break;
            case PublicWorksType.CAR:
                roadImage.sprite = carSprite;
                Debug.Log($"{gameObject.name} sprite set to CAR.");
                break;
            case PublicWorksType.BUS:
                roadImage.sprite = busSprite;
                Debug.Log($"{gameObject.name} sprite set to BUS.");
                break;
            default:
                Debug.LogWarning("Unknown Public Works type.");
                break;
        }
    }

    private void DeselectAllRoads()
    {
        RoadSelection[] allRoads = FindObjectsOfType<RoadSelection>();
        foreach (RoadSelection road in allRoads)
        {
            road.isSelected = false;
            road.roadImage.color = road.originalColor;
            Debug.Log($"Deselected {road.gameObject.name}. Reverted to original color.");
        }
    }
}
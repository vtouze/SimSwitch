using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class RoadClickHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private SpriteRenderer spriteRenderer;

    [Header("Highlight Colors")]
    public Color highlightColor = Color.yellow;
    public Color selectedColor = Color.red;
    
    private Color originalColor;
    private bool isSelected = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected)
        {
            spriteRenderer.color = highlightColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected)
        {
            spriteRenderer.color = originalColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DeselectAllRoads();

        isSelected = true;
        spriteRenderer.color = selectedColor;

        PublicWorksButton publicWorksButton = FindObjectOfType<PublicWorksButton>();
        RoadSegment roadSegment = GetComponent<RoadSegment>();
        if (publicWorksButton != null && roadSegment != null)
        {
            publicWorksButton.ApplyPublicWorksToRoad(roadSegment);
        }
    }

    private void DeselectAllRoads()
    {
        RoadClickHandler[] allRoads = FindObjectsOfType<RoadClickHandler>();
        foreach (RoadClickHandler road in allRoads)
        {
            road.isSelected = false;
            road.spriteRenderer.color = road.originalColor;
        }
    }
}
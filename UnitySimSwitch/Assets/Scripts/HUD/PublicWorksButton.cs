using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PublicWorksButton : MonoBehaviour, IPointerClickHandler
{
    public Vector2 offset = new Vector2(20, 20);

    private static Image globalImageToMove;
    private static bool isAnyImageFollowing = false;

    private RoadSegment selectedRoadSegment;

    private void Update()
    {
        if (isAnyImageFollowing && globalImageToMove != null)
        {
            Vector3 pos = Input.mousePosition;
            pos += new Vector3(offset.x, offset.y, 0);
            pos.z = 0f;
            globalImageToMove.transform.position = pos;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Image buttonImage = GetComponent<Image>();
        if (buttonImage == null)
        {
            Debug.LogWarning("PublicWorksButton: No Image component found on this GameObject.");
            return;
        }

        if (isAnyImageFollowing && globalImageToMove != null)
        {
            if (globalImageToMove.sprite == buttonImage.sprite)
            {
                Debug.Log("PublicWorksButton: Same sprite is already being followed. No action taken.");
                return;
            }

            StopFollowing();
        }

        globalImageToMove = Instantiate(buttonImage.gameObject).GetComponent<Image>();
        globalImageToMove.raycastTarget = false;

        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("PublicWorksButton: Canvas not found in parent hierarchy.");
            return;
        }

        globalImageToMove.transform.SetParent(canvas.transform, false);
        globalImageToMove.sprite = buttonImage.sprite;
        globalImageToMove.transform.localScale = Vector3.one;
        globalImageToMove.transform.SetAsLastSibling();

        isAnyImageFollowing = true;
    }

    public static void StopFollowing()
    {
        if (globalImageToMove != null)
        {
            Destroy(globalImageToMove.gameObject);
            globalImageToMove = null;
        }
        isAnyImageFollowing = false;
    }

    public void ApplyPublicWorksToRoad(RoadSegment roadSegment)
    {
        if (roadSegment == null)
        {
            Debug.LogWarning("PublicWorksButton: No road segment provided.");
            return;
        }

        if (selectedRoadSegment != roadSegment)
        {
            selectedRoadSegment = roadSegment;
            selectedRoadSegment.ApplyPublicWorks();
        }
        else
        {
            Debug.Log("PublicWorksButton: The selected road is already under public works.");
        }
    }
}
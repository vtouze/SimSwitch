using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PublicWorksButton : MonoBehaviour, IPointerClickHandler
{
    public Vector2 offset = new Vector2(20, 20);
    public PublicWorksType publicWorksType;

    private static Image globalImageToMove;
    private static bool isAnyImageFollowing = false;
    public static PublicWorksType? selectedPublicWorksType = null;

    public GameObject cancelImage;

    private static PublicWorksButton lastSelectedButton = null;

    private void Start()
    {
        if (cancelImage != null)
        {
            cancelImage.SetActive(false);
        }
    }

    private void Update()
    {
        if (isAnyImageFollowing && globalImageToMove != null)
        {
            Vector3 pos = Input.mousePosition + new Vector3(offset.x, offset.y, 0);
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

        if (selectedPublicWorksType == publicWorksType)
        {
            Debug.Log($"PublicWorksButton: Deselecting {publicWorksType}");
            StopFollowing();
            HideFeedback();
            return;
        }

        if (isAnyImageFollowing)
        {
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
        selectedPublicWorksType = publicWorksType;

        ShowFeedback();

        if (lastSelectedButton != null && lastSelectedButton != this)
        {
            lastSelectedButton.HideFeedback();
        }

        lastSelectedButton = this;

        Debug.Log($"PublicWorksButton: Selected {publicWorksType}");
    }

    public static void StopFollowing()
    {
        if (globalImageToMove != null)
        {
            Destroy(globalImageToMove.gameObject);
            globalImageToMove = null;
        }
        isAnyImageFollowing = false;
        selectedPublicWorksType = null;

        Debug.Log("PublicWorksButton: Stopped following. Public works type deselected.");
    }

    private void ShowFeedback()
    {
        if (cancelImage != null)
        {
            cancelImage.SetActive(true);
        }
    }

    private void HideFeedback()
    {
        if (cancelImage != null)
        {
            cancelImage.SetActive(false); 
        }
    }
}

public enum PublicWorksType
{
    BIKE,
    CAR,
    BUS
}
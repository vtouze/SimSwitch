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

    public static PublicWorksButton lastSelectedButton = null;

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

        if (selectedPublicWorksType == publicWorksType)
        {
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
    }

    public void ShowFeedback()
    {
        if (cancelImage != null)
        {
            cancelImage.SetActive(true);
        }
    }

    public void HideFeedback()
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
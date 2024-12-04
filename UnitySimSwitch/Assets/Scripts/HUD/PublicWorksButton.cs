using UnityEngine;
using UnityEngine.UI;

public class PublicWorksButton : MonoBehaviour
{
    public Vector2 offset = new Vector2(20, 20);
    public PublicWorksType publicWorksType;

    private static Image globalImageToMove;
    private static bool isAnyImageFollowing = false;
    public static PublicWorksType? selectedPublicWorksType = null;

    public GameObject cancelImage;

    public static PublicWorksButton lastSelectedButton = null;

    private Canvas canvas;
    private Camera canvasCamera;

    void Start()
    {
        if (cancelImage != null)
        {
            cancelImage.SetActive(false);
        }

        canvas = GetComponentInParent<Canvas>();
        canvasCamera = canvas.worldCamera;
    }

    void Update()
    {
        if (isAnyImageFollowing && globalImageToMove != null)
        {
            Vector3 worldPosition = Input.mousePosition + new Vector3(offset.x, offset.y, 0);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), worldPosition, canvasCamera, out Vector2 localPosition);
            globalImageToMove.transform.localPosition = localPosition;
        }
    }

    public void OnButtonClick()
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
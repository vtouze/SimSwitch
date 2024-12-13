using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the behavior of a public works button, including visual feedback and dragging functionality.
/// </summary>
public class PublicWorksButton : MonoBehaviour
{
    [Tooltip("Offset for the dragging image from the mouse cursor.")]
    public Vector2 offset = new Vector2(20, 20);

    [Tooltip("The type of public works associated with this button.")]
    public PublicWorksType publicWorksType;

    [Tooltip("Image displayed when this button is selected.")]
    public GameObject cancelImage;

    /// <summary>
    /// The image currently being dragged.
    /// </summary>
    private static Image globalImageToMove;

    /// <summary>
    /// Indicates if any button's image is currently being dragged.
    /// </summary>
    private static bool isAnyImageFollowing = false;

    /// <summary>
    /// The currently selected type of public works.
    /// </summary>
    public static PublicWorksType? selectedPublicWorksType = null;

    /// <summary>
    /// The last selected button for visual feedback purposes.
    /// </summary>
    public static PublicWorksButton lastSelectedButton = null;

    private Canvas canvas;
    private Camera canvasCamera;

    /// <summary>
    /// Initializes the button and sets up necessary references.
    /// </summary>
    void Start()
    {
        // Disable the cancel image by default if it exists.
        if (cancelImage != null)
        {
            cancelImage.SetActive(false);
        }

        // Get the canvas and its associated camera.
        canvas = GetComponentInParent<Canvas>();
        canvasCamera = canvas.worldCamera;
    }

    /// <summary>
    /// Updates the position of the dragged image to follow the mouse cursor.
    /// </summary>
    void Update()
    {
        // If an image is being dragged, update its position.
        if (isAnyImageFollowing && globalImageToMove != null)
        {
            // Calculate the desired world position based on the mouse cursor and offset.
            Vector3 worldPosition = Input.mousePosition + new Vector3(offset.x, offset.y, 0);

            // Convert the screen position to a local position relative to the canvas.
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(), 
                worldPosition, 
                canvasCamera, 
                out Vector2 localPosition
            );

            // Apply the local position to the dragged image.
            globalImageToMove.transform.localPosition = localPosition;
        }
    }

    /// <summary>
    /// Handles the button click logic, including initiating or stopping the dragging behavior.
    /// </summary>
    public void OnButtonClick()
    {
        // Get the button's Image component.
        Image buttonImage = GetComponent<Image>();

        // If this button is already selected, stop following the image and hide feedback.
        if (selectedPublicWorksType == publicWorksType)
        {
            StopFollowing();
            HideFeedback();
            return;
        }

        // If another image is currently being dragged, stop its dragging behavior.
        if (isAnyImageFollowing)
        {
            StopFollowing();
        }

        // Create a copy of the button's image for dragging.
        globalImageToMove = Instantiate(buttonImage.gameObject).GetComponent<Image>();
        globalImageToMove.raycastTarget = false; // Disable raycast to prevent interference.

        // Set the dragged image's parent and configure its appearance.
        globalImageToMove.transform.SetParent(canvas.transform, false);
        globalImageToMove.sprite = buttonImage.sprite;
        globalImageToMove.transform.localScale = Vector3.one;
        globalImageToMove.transform.SetAsLastSibling(); // Ensure it renders above other elements.

        // Update the dragging state and set this button's public works type as selected.
        isAnyImageFollowing = true;
        selectedPublicWorksType = publicWorksType;

        // Show feedback for the current button.
        ShowFeedback();

        // Hide feedback for the previously selected button, if any.
        if (lastSelectedButton != null && lastSelectedButton != this)
        {
            lastSelectedButton.HideFeedback();
        }

        // Update the reference to the last selected button.
        lastSelectedButton = this;
    }

    /// <summary>
    /// Stops the dragging behavior and resets related variables.
    /// </summary>
    public static void StopFollowing()
    {
        // Destroy the dragged image if it exists.
        if (globalImageToMove != null)
        {
            Destroy(globalImageToMove.gameObject);
            globalImageToMove = null;
        }

        // Reset the dragging state and selected public works type.
        isAnyImageFollowing = false;
        selectedPublicWorksType = null;
    }

    /// <summary>
    /// Displays feedback when this button is selected.
    /// </summary>
    public void ShowFeedback()
    {
        // Activate the cancel image if it exists.
        if (cancelImage != null)
        {
            cancelImage.SetActive(true);
        }
    }

    /// <summary>
    /// Hides feedback when this button is deselected.
    /// </summary>
    public void HideFeedback()
    {
        // Deactivate the cancel image if it exists.
        if (cancelImage != null)
        {
            cancelImage.SetActive(false);
        }
    }
}

/// <summary>
/// Enum representing the different types of public works that can be selected.
/// </summary>
public enum PublicWorksType
{
    BIKE,
    CAR,
    BUS
}
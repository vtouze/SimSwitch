using UnityEngine;
using UnityEngine.EventSystems;

public class InputFieldCursorChanger : MonoBehaviour
{
    #region Fields
    [Tooltip("Custom cursor texture to be used when the pointer hovers over the input field.")]
    [SerializeField] private Texture2D _customCursor; // Custom cursor when hovering over the input field

    private Vector2 _hotSpot = Vector2.zero; // Hotspot for the custom cursor (usually the center of the cursor)

    [Tooltip("Default system cursor texture.")]
    [SerializeField] private Texture2D _defaultCursor; // Default cursor for when not hovering over the input field
    #endregion Fields

    #region Methods

    /// <summary>
    /// Sets up the event triggers for the input field to change the cursor when the pointer enters and exits.
    /// </summary>
    void Start()
    {
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>(); // Get the EventTrigger component from the GameObject

        if (trigger == null) // If the EventTrigger component doesn't exist, add it
        {
            trigger = gameObject.AddComponent<EventTrigger>();
        }

        // Create event for when the pointer enters the input field
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
        pointerEnterEntry.eventID = EventTriggerType.PointerEnter; // Define the event as PointerEnter
        pointerEnterEntry.callback.AddListener((data) => { OnPointerEnter(); }); // Add listener to call OnPointerEnter method
        trigger.triggers.Add(pointerEnterEntry); // Add the pointer enter event to the trigger

        // Create event for when the pointer exits the input field
        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
        pointerExitEntry.eventID = EventTriggerType.PointerExit; // Define the event as PointerExit
        pointerExitEntry.callback.AddListener((data) => { OnPointerExit(); }); // Add listener to call OnPointerExit method
        trigger.triggers.Add(pointerExitEntry); // Add the pointer exit event to the trigger
    }

    /// <summary>
    /// Changes the cursor to the custom one when the pointer enters the input field.
    /// </summary>
    public void OnPointerEnter()
    {
        Cursor.SetCursor(_customCursor, _hotSpot, CursorMode.Auto); // Set the custom cursor
    }

    /// <summary>
    /// Restores the cursor to the default one when the pointer exits the input field.
    /// </summary>
    public void OnPointerExit()
    {
        Cursor.SetCursor(_defaultCursor, Vector2.zero, CursorMode.Auto); // Restore the default cursor
    }

    #endregion Methods
}
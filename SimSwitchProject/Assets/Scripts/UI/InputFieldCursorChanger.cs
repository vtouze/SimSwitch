using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class InputFieldCursorChanger : MonoBehaviour
{
    public Texture2D customCursor; // Custom cursor texture
    public Vector2 hotSpot = Vector2.zero; // Hotspot of the cursor

    public Texture2D defaultCursor; // To store the default cursor

    void Start()
    {
        // Ensure the EventTrigger component is added to the InputField
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();

        if (trigger == null)
        {
            trigger = gameObject.AddComponent<EventTrigger>();
        }

        // Add PointerEnter event
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
        pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
        pointerEnterEntry.callback.AddListener((data) => { OnPointerEnter(); });
        trigger.triggers.Add(pointerEnterEntry);

        // Add PointerExit event
        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
        pointerExitEntry.eventID = EventTriggerType.PointerExit;
        pointerExitEntry.callback.AddListener((data) => { OnPointerExit(); });
        trigger.triggers.Add(pointerExitEntry);
    }

    public void OnPointerEnter()
    {
        // Change the cursor to the custom cursor
        Cursor.SetCursor(customCursor, hotSpot, CursorMode.Auto);
    }

    public void OnPointerExit()
    {
        // Revert back to the default cursor
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }
}

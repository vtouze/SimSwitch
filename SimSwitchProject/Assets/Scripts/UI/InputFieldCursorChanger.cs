using UnityEngine;
using UnityEngine.EventSystems;
public class InputFieldCursorChanger : MonoBehaviour
{
    #region Fields
    [SerializeField] private Texture2D _customCursor;
    private Vector2 _hotSpot = Vector2.zero;
    [SerializeField] private Texture2D _defaultCursor;
    #endregion Fields

    #region Methods
    void Start()
    {
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();

        if (trigger == null)
        {
            trigger = gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
        pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
        pointerEnterEntry.callback.AddListener((data) => { OnPointerEnter(); });
        trigger.triggers.Add(pointerEnterEntry);

        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
        pointerExitEntry.eventID = EventTriggerType.PointerExit;
        pointerExitEntry.callback.AddListener((data) => { OnPointerExit(); });
        trigger.triggers.Add(pointerExitEntry);
    }

    public void OnPointerEnter()
    {
        Cursor.SetCursor(_customCursor, _hotSpot, CursorMode.Auto);
    }

    public void OnPointerExit()
    {
        Cursor.SetCursor(_defaultCursor, Vector2.zero, CursorMode.Auto);
    }
    #endregion Methods
}
using UnityEngine.EventSystems;
using UnityEngine;

public class PublicWorksMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("Reference to the Animator component for animating the public works menu.")]
    [SerializeField] private Animator _publicWorksAnim = null;

    /// <summary>
    /// Method called when the pointer enters the UI element.
    /// Triggers the animation to open the public works menu.
    /// </summary>
    /// <param name="eventData">Contains data related to the pointer event.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Set the "isOpeningMenu" parameter of the animator to true to start the menu opening animation
        _publicWorksAnim.SetBool("isOpeningMenu", true);
    }

    /// <summary>
    /// Method called when the pointer exits the UI element.
    /// Triggers the animation to close the public works menu.
    /// </summary>
    /// <param name="eventData">Contains data related to the pointer event.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        // Set the "isOpeningMenu" parameter of the animator to false to start the menu closing animation
        _publicWorksAnim.SetBool("isOpeningMenu", false);
    }
}
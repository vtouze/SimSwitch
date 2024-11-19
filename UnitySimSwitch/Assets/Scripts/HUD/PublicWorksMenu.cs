using UnityEngine.EventSystems;
using UnityEngine;

public class PublicWorksMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Animator _publicWorksAnim = null;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _publicWorksAnim.SetBool("isOpeningMenu", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _publicWorksAnim.SetBool("isOpeningMenu", false);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class LawsMenu : MonoBehaviour
{
    #region Fields
    [SerializeField] private Sprite _nullBackground = null;
    [SerializeField] private Sprite _lowBackground = null;
    [SerializeField] private Sprite _mediumBackground = null;
    [SerializeField] private Sprite _highBackground = null;
    [SerializeField] private Sprite _lowIcon = null;
    [SerializeField] private Sprite _mediumIcon = null;
    [SerializeField] private Sprite _highIcon = null;
    [SerializeField] private Button[] _horizontalBoxButtons = null;
    #endregion Fields

    #region Methods
    public void LowButton()
    {
        foreach (Button button in _horizontalBoxButtons)
        {
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.sprite = _nullBackground;
            }
        }
        gameObject.GetComponent<Image>().sprite = _lowBackground;
        gameObject.GetComponentInChildren<Image>().sprite = _lowIcon;
    }
    #endregion Methods
}
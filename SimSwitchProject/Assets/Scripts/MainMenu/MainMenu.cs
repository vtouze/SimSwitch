using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;

public class MainMenu : MonoBehaviour
{
    #region Fields
    [Header("Game Object")]
    [SerializeField] private GameObject _mainMenu = null;
    [SerializeField] private GameObject _settingsMenu = null;
    [SerializeField] private GameObject _quitCheck = null;
    [Header("Animation")]
    private float _fadeAnimationTime = 1.95f;
    [SerializeField] private GameObject _fadeInCircle = null;
    private bool _hasFinishedQuitAnimation = false;
    private bool _hasFinishedPlayAnimation = false;
    [SerializeField] private Animator _quitAnim = null;
    [SerializeField] private Animator _openSettings = null;

    #endregion Fields

    #region Methods
    void Start()
    {
        _mainMenu.SetActive(true);
        _settingsMenu.SetActive(false);
        _quitCheck.SetActive(false);
    }

    public void Play()
    {
        if(_openSettings.GetBool("isOpening"))
        {
            _openSettings.SetBool("isOpening", false);
        }
        _hasFinishedPlayAnimation = true;
    }

    public void OpenSettings()
    {
        _settingsMenu.SetActive(true);
        _openSettings.SetBool("isOpening", true);
    }

    #region Quit Methods
    public void QuitChecking()
    {
        _quitCheck.SetActive(true);
        _quitAnim.SetBool("isQuiting", true);
    }
    private void Update()
    {
        if (_hasFinishedQuitAnimation == true && _fadeAnimationTime > 0)
        {
            _fadeInCircle.SetActive(true);
            _fadeAnimationTime -= Time.deltaTime;
        }

        if (_fadeAnimationTime <= 0 && _hasFinishedQuitAnimation == true)
        {
            Application.Quit();
        }

        if (_hasFinishedPlayAnimation == true && _fadeAnimationTime > 0)
        {
            _fadeInCircle.SetActive(true);
            _fadeAnimationTime -= Time.deltaTime;
        }

        if (_fadeAnimationTime <= 0 && _hasFinishedPlayAnimation == true)
        {
            SceneManager.LoadScene("Overview");
        }
    }

    public void QuitY()
    {
        _hasFinishedQuitAnimation = true;
        //Application.OpenURL("https://teez21.itch.io/testwebgl2022");
        //Application.Quit();
    }

    public void QuitN()
    {
        _quitAnim.SetBool("isQuiting", false);
    }
    #endregion Quit Methods
    #endregion Methods
}
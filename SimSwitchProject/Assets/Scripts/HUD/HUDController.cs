using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HUDController : MonoBehaviour
{
    #region Fields
    [Header("Game Objects")]
    [SerializeField] private GameObject _hud = null;
    [SerializeField] private GameObject _pauseMenu = null;
    [SerializeField] private CameraController _cameraController = null;

    [Header("Animations")]
    [SerializeField] private Animator _openPauseMenu = null;
    [SerializeField] private GameObject _fadeInCircle = null;
    private float _fadeAnimationTime = 1.95f;
    private bool _hasFinishedQuitMenuAnimation = false;
    private bool _hasFinishedQuitDesktopAnimation = false;
    #endregion Fields

    #region Methods
    private void Start()
    {
        _hud.SetActive(true);
        _pauseMenu.SetActive(false);
    }

    private void Update()
    {
        QuickEscape();
        QuitToDesktopAnimation();
        QuitToMainMenuAnimation();
    }


    private void QuickEscape()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_openPauseMenu.GetBool("isOpeningMenu"))
            {
                _openPauseMenu.SetBool("isOpeningMenu", false);
            }
        }
    }
    
    #region Menu
    public void OpenMenuListener(GameObject gameObject)
    {
        OpenMenu(_cameraController, gameObject);
    }

    public void CloseMenuListener(GameObject gameobject)
    {
        CloseMenu(_cameraController, gameobject);
    }

    public void OpenMenu(CameraController cameraController, GameObject gameObject)
    {
        cameraController._isMenuing = true;
        gameObject.SetActive(true);
        gameObject.GetComponent<Animator>().SetBool("isOpeningMenu", true);
        gameObject.GetComponent<CanvasGroup>().interactable = true;
        gameObject.GetComponent<CanvasGroup>().interactable = true;
    }

    public void CloseMenu(CameraController cameraController, GameObject gameObject)
    {
        StartCoroutine(AnimationExit(gameObject));
        cameraController._isMenuing = false;
        gameObject.GetComponent<Animator>().SetBool("isOpeningMenu", false);
        gameObject.GetComponent<CanvasGroup>().interactable = true;
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    IEnumerator AnimationExit(GameObject gameObject)
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
    #endregion Menu
    #region Quit Methods
    public void QuitToMainMenu()
    {
        _hasFinishedQuitMenuAnimation = true;
    }

    public void QuitToDesktop()
    {
        _hasFinishedQuitDesktopAnimation = true;
    }

    private void QuitToDesktopAnimation()
    {
        if (_hasFinishedQuitDesktopAnimation == true && _fadeAnimationTime > 0)
        {
            _fadeInCircle.SetActive(true);
            _fadeAnimationTime -= Time.deltaTime;
        }

        if (_fadeAnimationTime <= 0 && _hasFinishedQuitDesktopAnimation == true)
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }

    private void QuitToMainMenuAnimation()
    {
        if (_hasFinishedQuitMenuAnimation == true && _fadeAnimationTime > 0)
        {
            _fadeInCircle.SetActive(true);
            _fadeAnimationTime -= Time.deltaTime;
        }

        if (_fadeAnimationTime <= 0 && _hasFinishedQuitMenuAnimation == true)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    #endregion Quit Methods
    #endregion Methods
}
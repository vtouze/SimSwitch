using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    #region Fields
    [Header("Game Objects")]
    [SerializeField] private GameObject _hud = null;
    [SerializeField] private CameraController _cameraController = null;
    [SerializeField] private List<GameObject> _menus = new List<GameObject>();

    [Header("Animations")]
    [SerializeField] private GameObject _fadeInCircle = null;
    private float _fadeAnimationTime = 1.95f;
    private bool _hasFinishedQuitMenuAnimation = false;
    private bool _hasFinishedQuitDesktopAnimation = false;
    #endregion Fields

    #region Methods
    private void Start()
    {
        _hud.SetActive(true);

        foreach (var menu in _menus)
        {
            menu.SetActive(false);
        }
    }

    private void Update()
    {
        QuickEscape();
        QuitToDesktopAnimation();
        QuitToMainMenuAnimation();
    }
    
    #region Menu
    public void OpenMenuListener(GameObject menu)
    {
        OpenMenu(_cameraController, menu);
    }

    public void CloseMenuListener(GameObject menu)
    {
        CloseMenu(_cameraController, menu);
    }

    public void OpenMenu(CameraController cameraController, GameObject menu)
    {
        cameraController._isMenuing = true;
        menu.SetActive(true);
        Animator animator = menu.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isOpeningMenu", true);
        }
        CanvasGroup canvasGroup = menu.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void CloseMenu(CameraController cameraController, GameObject menu)
    {
        StartCoroutine(AnimationExit(menu));
        cameraController._isMenuing = false;
        Animator animator = menu.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isOpeningMenu", false);
        }
        CanvasGroup canvasGroup = menu.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    IEnumerator AnimationExit(GameObject menu)
    {
        yield return new WaitForSeconds(1);
        menu.SetActive(false);
    }

    private void QuickEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            foreach (GameObject menu in _menus)
            {
                Animator animator = menu.GetComponent<Animator>();

                if (animator != null && animator.GetBool("isOpeningMenu"))
                {
                    CloseMenu(_cameraController, menu);
                    return;
                }
            }
        }
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
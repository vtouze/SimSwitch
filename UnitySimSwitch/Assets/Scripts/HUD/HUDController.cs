using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    #region Fields
    [Header("Game Objects")]
    [Tooltip("The main HUD GameObject.")]
    [SerializeField] private GameObject _hud = null;
    
    [Tooltip("Reference to the CameraController for handling camera-related logic.")]
    [SerializeField] private CameraController _cameraController = null;
    
    [Tooltip("List of menu GameObjects controlled by this HUD.")]
    [SerializeField] private List<GameObject> _menus = new List<GameObject>();

    [Header("Animations")]
    [Tooltip("The GameObject representing the fade-in circle animation.")]
    [SerializeField] private GameObject _fadeInCircle = null;
    
    // Duration of the fade-in animation in seconds.
    private float _fadeAnimationTime = 1.95f;
    
    // Tracks whether the quit-to-menu animation has finished.
    private bool _hasFinishedQuitMenuAnimation = false;
    
    // Tracks whether the quit-to-desktop animation has finished.
    private bool _hasFinishedQuitDesktopAnimation = false;
    #endregion Fields

    #region Methods
    private void Start()
    {
        _hud.SetActive(true);

        // Disables all menu GameObjects at the start.
        foreach (var menu in _menus)
        {
            menu.SetActive(false);
        }
    }

    private void Update()
    {
        // Listens for escape key and manages quit animations.
        QuickEscape();
        QuitToDesktopAnimation();
        QuitToMainMenuAnimation();
    }
    
    #region Menu
    /// <summary>
    /// Opens the specified menu by using a listener method.
    /// </summary>
    public void OpenMenuListener(GameObject menu)
    {
        OpenMenu(_cameraController, menu);
    }

    /// <summary>
    /// Closes the specified menu by using a listener method.
    /// </summary>
    public void CloseMenuListener(GameObject menu)
    {
        CloseMenu(_cameraController, menu);
    }

    /// <summary>
    /// Opens a menu and makes necessary UI and animator adjustments.
    /// </summary>
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

    /// <summary>
    /// Closes a menu and triggers its exit animation.
    /// </summary>
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

    /// <summary>
    /// Coroutine for waiting before disabling the menu after the exit animation.
    /// </summary>
    IEnumerator AnimationExit(GameObject menu)
    {
        yield return new WaitForSeconds(1);
        menu.SetActive(false);
    }

    /// <summary>
    /// Closes the currently opened menu when the Escape key is pressed.
    /// </summary>
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
    /// <summary>
    /// Triggers the quit-to-main-menu process.
    /// </summary>
    public void QuitToMainMenu()
    {
        _hasFinishedQuitMenuAnimation = true;
    }

    /// <summary>
    /// Triggers the quit-to-desktop process.
    /// </summary>
    public void QuitToDesktop()
    {
        _hasFinishedQuitDesktopAnimation = true;
    }

    /// <summary>
    /// Handles the fade animation and exits the application.
    /// </summary>
    private void QuitToDesktopAnimation()
    {
        if (_hasFinishedQuitDesktopAnimation && _fadeAnimationTime > 0)
        {
            _fadeInCircle.SetActive(true);
            _fadeAnimationTime -= Time.deltaTime;
        }

        if (_fadeAnimationTime <= 0 && _hasFinishedQuitDesktopAnimation)
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }

    /// <summary>
    /// Handles the fade animation and loads the main menu scene.
    /// </summary>
    private void QuitToMainMenuAnimation()
    {
        if (_hasFinishedQuitMenuAnimation && _fadeAnimationTime > 0)
        {
            _fadeInCircle.SetActive(true);
            _fadeAnimationTime -= Time.deltaTime;
        }

        if (_fadeAnimationTime <= 0 && _hasFinishedQuitMenuAnimation)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    #endregion Quit Methods
    #endregion Methods
}
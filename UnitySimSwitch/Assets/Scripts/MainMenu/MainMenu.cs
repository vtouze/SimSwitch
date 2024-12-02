using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// Manages the main menu interactions, including switching between menus,
/// playing animations, and handling quit actions.
/// </summary>
public class MainMenu : MonoBehaviour
{
    #region Fields
    [Header("Game Objects")]
    [Tooltip("Main menu panel that is displayed at the start.")]
    [SerializeField] private GameObject _mainMenu = null;

    [Tooltip("Settings menu panel that can be opened from the main menu.")]
    [SerializeField] private GameObject _settingsMenu = null;

    [Tooltip("Quit confirmation panel that appears when quitting.")]
    [SerializeField] private GameObject _quitCheck = null;

    [Tooltip("Encyclopedia panel that can be opened from the main menu.")]
    [SerializeField] private GameObject _encyclopedia = null;

    [Tooltip("Fade circle effect used during scene transitions.")]
    [SerializeField] private GameObject _fadeInCircle = null;

    [Tooltip("Camera controller that is used to manage camera behaviors during menus.")]
    [SerializeField] private CameraController _cameraController = null;

    [Header("Animation")]
    [Tooltip("Animator for the quit confirmation panel.")]
    [SerializeField] private Animator _quitAnim = null;

    [Tooltip("Animator for the settings menu.")]
    [SerializeField] private Animator _openSettings = null;

    [Tooltip("Animator for the encyclopedia panel.")]
    [SerializeField] private Animator _openEncyclopedia = null;

    private float _fadeAnimationTime = 1.95f; // Duration of the fade animation
    private bool _hasFinishedQuitAnimation = false; // Indicates if quit animation is finished
    private bool _hasFinishedPlayAnimation = false; // Indicates if play animation is finished
    #endregion Fields

    #region Methods
    /// <summary>
    /// Called at the start to set up the initial state of the menus and camera.
    /// </summary>
    void Start()
    {
        // Initially, only the main menu is visible, and all other menus are hidden.
        _mainMenu.SetActive(true);
        _settingsMenu.SetActive(false);
        _quitCheck.SetActive(false);
        _encyclopedia.SetActive(false);

        // Set the camera controller to be in the menu state.
        _cameraController._isMenuing = true;
    }

    /// <summary>
    /// Called every frame to handle the quit and scene switch animations.
    /// </summary>
    private void Update()
    {
        QuitToDesktopAnimation();
        SwitchSceneAnimation();
    }

    #region Buttons
    /// <summary>
    /// Called when the "Play" button is pressed. Triggers animations and prepares for scene transition.
    /// </summary>
    public void Play()
    {
        // Close other menus and ensure animations are reset before starting the play animation.
        CheckAnimations(_openSettings);
        CheckAnimations(_openEncyclopedia);
        CheckAnimations(_quitAnim);
        _hasFinishedPlayAnimation = true;
    }

    /// <summary>
    /// Checks if any menu animation is in progress and resets it.
    /// </summary>
    private void CheckAnimations(Animator anim)
    {
        if (anim.GetBool("isOpening"))
        {
            anim.SetBool("isOpening", false);
        }
    }

    /// <summary>
    /// Starts the opening animation for the given menu and sets the menu active.
    /// </summary>
    private void PlayAnimations(Animator anim, GameObject gameObject)
    {
        gameObject.SetActive(true);
        anim.SetBool("isOpening", true);
    }

    /// <summary>
    /// Opens the settings menu when the respective button is pressed.
    /// </summary>
    public void OpenSettings()
    {
        // Close other menus before opening settings.
        CheckAnimations(_openEncyclopedia);
        CheckAnimations(_quitAnim);
        PlayAnimations(_openSettings, _settingsMenu);
    }

    /// <summary>
    /// Opens the encyclopedia menu when the respective button is pressed.
    /// </summary>
    public void OpenEncyclopedia()
    {
        // Close other menus before opening the encyclopedia.
        CheckAnimations(_openSettings);
        CheckAnimations(_quitAnim);
        PlayAnimations(_openEncyclopedia, _encyclopedia);
    }

    /// <summary>
    /// Opens the quit confirmation panel when the respective button is pressed.
    /// </summary>
    public void QuitChecking()
    {
        // Close other menus before opening the quit check.
        CheckAnimations(_openEncyclopedia);
        CheckAnimations(_openSettings);
        PlayAnimations(_quitAnim, _quitCheck);
    }

    #endregion Buttons

    #region Quit Methods
    /// <summary>
    /// Confirms the quit action, setting necessary flags.
    /// </summary>
    public void QuitY()
    {
        _hasFinishedQuitAnimation = true;
    }

    /// <summary>
    /// Cancels the quit action and resets the quit animation.
    /// </summary>
    public void QuitN()
    {
        CheckAnimations(_quitAnim);
    }

    /// <summary>
    /// Handles the scene switch animation after the play button is pressed.
    /// </summary>
    private void SwitchSceneAnimation()
    {
        if (_hasFinishedPlayAnimation == true && _fadeAnimationTime > 0)
        {
            _fadeInCircle.SetActive(true);
            _fadeAnimationTime -= Time.deltaTime;
        }

        // Load the "OverviewV2" scene after the fade animation completes.
        if (_fadeAnimationTime <= 0 && _hasFinishedPlayAnimation == true)
        {
            SceneManager.LoadScene("OverviewV2");
        }
    }

    /// <summary>
    /// Handles the quit to desktop animation and exits the application when done.
    /// </summary>
    private void QuitToDesktopAnimation()
    {
        if (_hasFinishedQuitAnimation == true && _fadeAnimationTime > 0)
        {
            _fadeInCircle.SetActive(true);
            _fadeAnimationTime -= Time.deltaTime;
        }

        if (_fadeAnimationTime <= 0 && _hasFinishedQuitAnimation == true)
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // For Unity Editor.
            #else
                Application.Quit(); // For build.
            #endif
        }
    }
    #endregion Quit Methods
    #endregion Methods
}
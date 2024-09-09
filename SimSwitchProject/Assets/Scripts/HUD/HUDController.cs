using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    #region Fields
    [Header("Game Objects")]
    [SerializeField] private GameObject _hud = null;
    [SerializeField] private GameObject _pauseMenu = null;

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
        if (_hasFinishedQuitDesktopAnimation == true && _fadeAnimationTime > 0)
        {
            _fadeInCircle.SetActive(true);
            _fadeAnimationTime -= Time.deltaTime;
        }

        if (_fadeAnimationTime <= 0 && _hasFinishedQuitDesktopAnimation == true)
        {
            Application.Quit();
        }

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

    public void OpenPauseMenu()
    {
        _pauseMenu.SetActive(true);
        _openPauseMenu.SetBool("isOpeningPauseMenu", true);
        _pauseMenu.gameObject.GetComponent<CanvasGroup>().interactable = true;
        _pauseMenu.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void ClosePauseMenu()
    {
        _openPauseMenu.SetBool("isOpeningPauseMenu", false);
        _pauseMenu.gameObject.GetComponent<CanvasGroup>().interactable = false;
        _pauseMenu.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void QuitToMainMenu()
    {
        _hasFinishedQuitMenuAnimation = true;
    }

    public void QuitToDesktop()
    {
        _hasFinishedQuitDesktopAnimation = true;
    }
    #endregion Methods
}

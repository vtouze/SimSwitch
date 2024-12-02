using UnityEngine;

/// <summary>
/// Controls the fade-out animation at the start of the scene by activating 
/// a fade-out circle and disabling it after a set duration.
/// </summary>
public class InitScene : MonoBehaviour
{
    [Tooltip("Duration of the fade-out animation in seconds.")]
    private float _fadeAnimationTime = 1.95f;

    [Tooltip("GameObject representing the fade-out circle, which is enabled at the start and disabled after the animation.")]
    [SerializeField] private GameObject _fadeOutCircle = null;

    /// <summary>
    /// Called when the scene starts. Activates the fade-out circle GameObject.
    /// </summary>
    private void Start()
    {
        _fadeOutCircle.SetActive(true);
    }

    /// <summary>
    /// Called every frame. Handles the countdown for the fade-out animation and disables the fade-out circle when complete.
    /// </summary>
    private void Update()
    {
        if (_fadeAnimationTime > 0)
        {
            _fadeAnimationTime -= Time.deltaTime;
        }

        if (_fadeAnimationTime <= 0)
        {
            _fadeOutCircle.SetActive(false);
        }
    }
}

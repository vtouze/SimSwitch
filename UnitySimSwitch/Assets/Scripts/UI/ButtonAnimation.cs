using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script handles the animation of a button when it is clicked. The button scales up and then returns
/// to its original size, providing a visual effect to indicate interaction.
/// </summary>
public class ButtonAnimation : MonoBehaviour
{ 
    [Tooltip("Scale factor applied to the button when it is animated (scales the button up).")]
    [SerializeField] private Vector3 _sizeUp = new Vector3(1.4f, 1.4f, 1);

    [Tooltip("Duration of the animation in seconds.")]
    [SerializeField] private float animationDuration = 0.2f;

    private Button _button;

    /// <summary>
    /// Initializes the button and adds a listener for the button click event to trigger the animation.
    /// </summary>
    private void Awake()
    {
        _button = gameObject.GetComponent<Button>();
        _button.onClick.AddListener(DoAnimation);
    }

    /// <summary>
    /// Triggers the animation when the button is clicked. It starts the scaling animation coroutine.
    /// </summary>
    private void DoAnimation()
    {
        StartCoroutine(ScaleAnimation());
    }

    /// <summary>
    /// Handles the scaling animation.
    /// </summary>
    /// <returns>IEnumerator for coroutine handling.</returns>
    private IEnumerator ScaleAnimation()
    {
        // Scale the button up
        yield return ScaleTo(_sizeUp, animationDuration);

        // Scale the button back to its original size
        yield return ScaleTo(Vector3.one, animationDuration);
    }

    /// <summary>
    /// Smoothly scales the button to the target scale over the specified duration.
    /// </summary>
    /// <param name="targetScale">The target scale to reach.</param>
    /// <param name="duration">The duration of the scaling animation.</param>
    /// <returns>IEnumerator for coroutine handling.</returns>
    private IEnumerator ScaleTo(Vector3 targetScale, float duration)
    {
        Vector3 initialScale = transform.localScale;
        float elapsedTime = 0f;

        // Perform the scaling over time using lerp
        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Ensure the final scale matches the target scale
        transform.localScale = targetScale;
    }
}
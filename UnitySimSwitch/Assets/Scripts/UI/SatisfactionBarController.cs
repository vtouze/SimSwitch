using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// This script controls the satisfaction bar's behavior in the UI, including updating the value smoothly 
/// and changing the fill color based on the satisfaction level.
/// </summary>
public class SatisfactionBarController : MonoBehaviour
{
    #region Fields

    [Tooltip("Slider representing the satisfaction level.")]
    [SerializeField] private Slider _satisfactionSlider;

    [Tooltip("Image used to visually represent the satisfaction level (changes color).")]
    [SerializeField] private Image _fillImage;

    private Color _redColor = new Color(245f / 255f, 73f / 255f, 73f / 255f);
    private Color _orangeColor = new Color(255f / 255f, 166f / 255f, 0f);
    private Color _yellowColor = new Color(231f / 255f, 245f / 255f, 35f / 255f);
    private Color _lightgreenColor = new Color(120f / 255f, 243f / 255f, 132f / 255f);
    private Color _greenColor = new Color(80f / 255f, 205f / 255f, 93f / 255f);

    private Coroutine _smoothUpdateCoroutine;
    private float _satisfaction = 50f;  // Default satisfaction level

    #endregion Fields

    #region Methods

    /// <summary>
    /// Returns the current satisfaction level.
    /// </summary>
    public float GetSatisfaction()
    {
        return _satisfaction;
    }

    /// <summary>
    /// Changes the satisfaction level by a given amount. Updates the satisfaction bar accordingly.
    /// </summary>
    /// <param name="amount">The amount to change the satisfaction level by.</param>
    public void ChangeSatisfaction(float amount)
    {
        _satisfaction = Mathf.Clamp(_satisfaction + amount, 0, 100);
        UpdateSatisfactionBar(_satisfaction);
    }

    /// <summary>
    /// Starts a smooth update of the satisfaction bar towards the target satisfaction value.
    /// </summary>
    /// <param name="targetSatisfaction">The target satisfaction value to update the bar to.</param>
    private void UpdateSatisfactionBar(float targetSatisfaction)
    {
        if (_smoothUpdateCoroutine != null)
        {
            StopCoroutine(_smoothUpdateCoroutine);
        }
        _smoothUpdateCoroutine = StartCoroutine(SmoothUpdateSatisfactionBar(targetSatisfaction));
    }

    /// <summary>
    /// Smoothly updates the satisfaction bar's value over a given duration, and updates the fill color based on the value.
    /// </summary>
    /// <param name="targetSatisfaction">The target satisfaction value to animate to.</param>
    private IEnumerator SmoothUpdateSatisfactionBar(float targetSatisfaction)
    {
        float initialSatisfaction = _satisfactionSlider.value;
        float duration = 0.5f;
        float elapsed = 0f;

        // Perform the smooth update of the satisfaction slider value
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newSatisfaction = Mathf.Lerp(initialSatisfaction, targetSatisfaction, elapsed / duration);
            _satisfactionSlider.value = newSatisfaction;
            UpdateFillColor(newSatisfaction);
            yield return null;
        }

        _satisfactionSlider.value = targetSatisfaction;
        UpdateFillColor(targetSatisfaction);  // Final color update after the animation
    }

    /// <summary>
    /// Updates the fill color of the satisfaction bar based on the current satisfaction value.
    /// </summary>
    /// <param name="satisfaction">The current satisfaction level to determine the color.</param>
    private void UpdateFillColor(float satisfaction)
    {
        if (satisfaction <= 20f)
        {
            _fillImage.color = _redColor;
        }
        else if (satisfaction <= 40f)
        {
            _fillImage.color = _orangeColor;
        }
        else if (satisfaction <= 60f)
        {
            _fillImage.color = _yellowColor;
        }
        else if (satisfaction <= 80f)
        {
            _fillImage.color = _lightgreenColor;
        }
        else
        {
            _fillImage.color = _greenColor;
        }

        // Make the fill color slightly transparent for better visual appearance
        _fillImage.color = new Color(_fillImage.color.r, _fillImage.color.g, _fillImage.color.b, 0.9f);
    }

    #endregion Methods
}
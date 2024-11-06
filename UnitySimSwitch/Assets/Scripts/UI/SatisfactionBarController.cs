using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SatisfactionBarController : MonoBehaviour
{
    #region Fields
    [SerializeField] private Slider _satisfactionSlider;
    [SerializeField] private Image _fillImage;

    private Color _redColor = new Color(245f / 255f, 73f / 255f, 73f / 255f);
    private Color _orangeColor = new Color(255f / 255f, 166f / 255f, 0f);
    private Color _yellowColor = new Color(231f / 255f, 245f / 255f, 35f / 255f);
    private Color _lightgreenColor = new Color(120f / 255f, 243f / 255f, 132f / 255f);
    private Color _greenColor = new Color(80f / 255f, 205f / 255f, 93f / 255f);

    private Coroutine _smoothUpdateCoroutine;
    private float _satisfaction = 50f;
    #endregion Fields

    #region Methods
    public float GetSatisfaction()
    {
        return _satisfaction;
    }

    public void ChangeSatisfaction(float amount)
    {
        _satisfaction = Mathf.Clamp(_satisfaction + amount, 0, 100);
        UpdateSatisfactionBar(_satisfaction);
    }

    private void UpdateSatisfactionBar(float targetSatisfaction)
    {
        if (_smoothUpdateCoroutine != null)
        {
            StopCoroutine(_smoothUpdateCoroutine);
        }
        _smoothUpdateCoroutine = StartCoroutine(SmoothUpdateSatisfactionBar(targetSatisfaction));
    }

    private IEnumerator SmoothUpdateSatisfactionBar(float targetSatisfaction)
    {
        float initialSatisfaction = _satisfactionSlider.value;
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newSatisfaction = Mathf.Lerp(initialSatisfaction, targetSatisfaction, elapsed / duration);
            _satisfactionSlider.value = newSatisfaction;
            UpdateFillColor(newSatisfaction);
            yield return null;
        }

        _satisfactionSlider.value = targetSatisfaction;
        UpdateFillColor(targetSatisfaction);
    }

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

        _fillImage.color = new Color(_fillImage.color.r, _fillImage.color.g, _fillImage.color.b, 0.9f);
    }
    #endregion Methods
}
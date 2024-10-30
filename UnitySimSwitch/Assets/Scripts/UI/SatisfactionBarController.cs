using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SatisfactionBarController : MonoBehaviour
{
    #region Fields
    [SerializeField] private Slider satisfactionSlider;
    [SerializeField] private Image fillImage;

    private Color redColor = new Color(245f / 255f, 73f / 255f, 73f / 255f);
    private Color orangeColor = new Color(255f / 255f, 166f / 255f, 0f);
    private Color yellowColor = new Color(231f / 255f, 245f / 255f, 35f / 255f);
    private Color lightGreenColor = new Color(120f / 255f, 243f / 255f, 132f / 255f);
    private Color greenColor = new Color(80f / 255f, 205f / 255f, 93f / 255f);

    private Coroutine smoothUpdateCoroutine;
    #endregion Fields

    #region Methods
    public void UpdateSatisfactionBar(float targetSatisfaction)
    {
        if (smoothUpdateCoroutine != null)
        {
            StopCoroutine(smoothUpdateCoroutine);
        }
        smoothUpdateCoroutine = StartCoroutine(SmoothUpdateSatisfactionBar(targetSatisfaction));
    }

    private IEnumerator SmoothUpdateSatisfactionBar(float targetSatisfaction)
    {
        float initialSatisfaction = satisfactionSlider.value;
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newSatisfaction = Mathf.Lerp(initialSatisfaction, targetSatisfaction, elapsed / duration);
            satisfactionSlider.value = newSatisfaction;
            UpdateFillColor(newSatisfaction);
            yield return null;
        }

        satisfactionSlider.value = targetSatisfaction;
        UpdateFillColor(targetSatisfaction);
    }

    private void UpdateFillColor(float satisfaction)
    {
        if (satisfaction <= 20f)
        {
            fillImage.color = redColor;
        }
        else if (satisfaction <= 40f)
        {
            fillImage.color = orangeColor;
        }
        else if (satisfaction <= 60f)
        {
            fillImage.color = yellowColor;
        }
        else if (satisfaction <= 80f)
        {
            fillImage.color = lightGreenColor;
        }
        else
        {
            fillImage.color = greenColor;
        }

        fillImage.color = new Color(fillImage.color.r, fillImage.color.g, fillImage.color.b, 0.9f);
    }
    #endregion Methods
}
/*using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{ 
    private Button _button;
    [SerializeField] private Vector3 _sizeUp = new Vector3(1.2f, 1.2f, 1);

    private void Awake()
    {
        _button = gameObject.GetComponent<Button>();
        _button.onClick.AddListener(DoAnimation);
    }

    private void DoAnimation()
    {
        LeanTween.scale(gameObject, _sizeUp, .1f);
        LeanTween.scale(gameObject, Vector3.one, .1f).setDelay(.1f);
    }
}*/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{ 
    private Button _button;
    [SerializeField] private Vector3 _sizeUp = new Vector3(1.4f, 1.4f, 1);
    [SerializeField] private float animationDuration = 0.2f;

    private void Awake()
    {
        _button = gameObject.GetComponent<Button>();
        _button.onClick.AddListener(DoAnimation);
    }

    private void DoAnimation()
    {
        StartCoroutine(ScaleAnimation());
    }

    private IEnumerator ScaleAnimation()
    {
        yield return ScaleTo(_sizeUp, animationDuration);

        yield return ScaleTo(Vector3.one, animationDuration);
    }

    private IEnumerator ScaleTo(Vector3 targetScale, float duration)
    {
        Vector3 initialScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
    }
}
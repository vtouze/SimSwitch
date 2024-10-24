using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    public TMP_Text _fpsText;
    private float _deltaTime = 0.0f;

    void Update()
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        float fps = 1.0f / _deltaTime;

        if (_fpsText != null)
        {
            _fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();
        }
    }
}
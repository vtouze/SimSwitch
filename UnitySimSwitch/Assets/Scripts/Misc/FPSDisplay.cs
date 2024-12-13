using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    [Tooltip("The TMP_Text component used to display the FPS on screen.")]
    public TMP_Text _fpsText;

    private float _deltaTime = 0.0f; // Variable to store the time between frames

    /// <summary>
    /// Called once per frame to update the FPS display.
    /// </summary>
    void Update()
    {
        // Accumulate the delta time, smoothing out the values for a more stable FPS reading
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;

        // Calculate the frames per second (FPS) based on the delta time
        float fps = 1.0f / _deltaTime;

        // If the TMP_Text reference is assigned, update the text to display the FPS
        if (_fpsText != null)
        {
            // Set the text of the TMP_Text to display the FPS value, rounded up
            _fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();
        }
    }
}
using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{
    [Tooltip("The minimum time interval between random events (in seconds).")]
    public float minEventInterval = 11f;

    [Tooltip("The maximum time interval between random events (in seconds).")]
    public float maxEventInterval = 12f;

    [Tooltip("Reference to the NewspaperController that will handle event animations.")]
    public NewspaperController newspaperController;

    private Coroutine randomEventCoroutine;

    // Start the random event timer when the game starts
    private void Start()
    {
        StartRandomEventTimer();
    }

    // Method to start the random event timer
    private void StartRandomEventTimer()
    {
        if (randomEventCoroutine != null)
        {
            StopCoroutine(randomEventCoroutine); // Stop any existing coroutine
        }
        randomEventCoroutine = StartCoroutine(RandomEventTimerCoroutine());
    }

    // Coroutine to trigger AddRandomEventWithAnimation at random intervals
    private IEnumerator RandomEventTimerCoroutine()
    {
        while (true) // Run indefinitely (you can add a condition if needed)
        {
            float randomInterval = Random.Range(minEventInterval, maxEventInterval); // Randomize the interval
            yield return new WaitForSeconds(randomInterval); // Wait for the random interval
            newspaperController.AddRandomEventWithAnimation(); // Trigger the random event
        }
    }
}
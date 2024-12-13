/*Unity Analytics is a service provided by Unity that allows developers to track player behavior, game performance, and other data to help improve their games.
For more information, visit the Unity Analytics documentation: 
https://docs.unity.com/ugs/manual/analytics/manual/overview*/

using System;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;

public class UGS_Analytics : MonoBehaviour
{
    #region Methods

    /// <summary>
    /// Initializes Unity Services asynchronously and triggers custom events upon successful initialization.
    /// If any error occurs during initialization, it is logged to the console.
    /// </summary>
    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync(); // Initialize Unity Services asynchronously
            GiveConsent(); // Give consent for data collection
            LevelCompletedCustomEvent(); // Trigger a custom event for level completion
        }
        catch (Exception e)
        {
            Debug.LogError("Error initializing Unity Services: " + e.Message); // Log any errors that occur during initialization
        }
    }

    /// <summary>
    /// Triggers a custom "LevelCompleted" event with a randomly selected level number.
    /// Records the event using Unity Analytics.
    /// </summary>
    private void LevelCompletedCustomEvent()
    {
        int currentLevel = UnityEngine.Random.Range(1, 4); // Randomly select a level number between 1 and 3

        // Create a new custom event with a parameter indicating the level completed
        TestCustomEvent myEvent = new TestCustomEvent
        {
            TestCustomParameter = $"level{currentLevel}" // Set the level parameter
        };

        AnalyticsService.Instance.RecordEvent(myEvent); // Record the custom event using the Analytics Service

        // Log the event trigger for debugging purposes
        Debug.Log($"LevelCompletedCustomEvent() triggered for level {currentLevel} with TestCustomParameter.");
    }

    /// <summary>
    /// Starts the data collection by giving consent to the Unity Analytics SDK.
    /// Logs a message indicating that data collection has started.
    /// </summary>
    public void GiveConsent()
    {
        AnalyticsService.Instance.StartDataCollection(); // Start data collection for Unity Analytics
        Debug.Log("Consent has been provided. The SDK is now collecting data!"); // Log the consent confirmation
    }

    #endregion Methods
}
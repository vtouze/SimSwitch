using System;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;

public class UGS_Analytics : MonoBehaviour
{
    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            GiveConsent();
            LevelCompletedCustomEvent();
        }
        catch (Exception e)
        {
            Debug.LogError("Error initializing Unity Services: " + e.Message);
        }
    }

    private void LevelCompletedCustomEvent()
    {
        int currentLevel = UnityEngine.Random.Range(1, 4); 
        TestCustomEvent myEvent = new TestCustomEvent
        {
            TestCustomParameter = $"level{currentLevel}"
        };

        AnalyticsService.Instance.RecordEvent(myEvent);

        Debug.Log($"LevelCompletedCustomEvent() triggered for level {currentLevel} with TestCustomParameter.");
    }

    public void GiveConsent()
    {
        AnalyticsService.Instance.StartDataCollection();
        Debug.Log("Consent has been provided. The SDK is now collecting data!");
    }
}
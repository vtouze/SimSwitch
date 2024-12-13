using Unity.Services.Analytics;

public class TestCustomEvent : Event
{
    /// <summary>
    /// Constructor that sets the event name for the custom event.
    /// </summary>
    public TestCustomEvent() : base("TestCustomEvent") 
    {
    }

    /// <summary>
    /// Custom parameter for the event. This is used to send specific data with the event.
    /// </summary>
    public string TestCustomParameter 
    { 
        set { SetParameter("TestCustomParameter", value); } // Set the custom parameter for the event
    }
}

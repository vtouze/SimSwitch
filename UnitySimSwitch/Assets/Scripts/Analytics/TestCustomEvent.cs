using Unity.Services.Analytics;

public class TestCustomEvent : Event
{
    public TestCustomEvent() : base("TestCustomEvent") 
    {
    }

    public string TestCustomParameter 
    { 
        set { SetParameter("TestCustomParameter", value); } 
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SendReceiveDaily : SimulationManager
{
    #region Fields
    DailyMessage _dailyMessage = null;

    [SerializeField] private TMP_Text _dayText = null;
    [SerializeField] private TMP_Text _dayOfWeekText = null;
    [SerializeField] private TMP_Text _monthText = null;
    [SerializeField] private TMP_Text _yearText = null;

    private string[] _daysOfWeek = new string[]
    {
        "Sun.", "Mon.", "Tue.", "Wed.", "Thu.", "Fri.", "Sat."
    };

    private string[] _months = new string[]
    {
        "Jan.", "Feb.", "Mar.", "Apr.", "May", "Jun.",
        "Jul.", "Aug.", "Sept.", "Oct.", "Nov.", "Dec."
    };

    private Dictionary<string, string> _emptyString = new Dictionary<string, string>{};
    #endregion Fields

    #region Methods
    protected override void ManageOtherMessages(string content)
    {
        _dailyMessage = DailyMessage.CreateFromJSON(content);
    }

    protected override void OtherUpdate()
    {
        if(IsGameState(GameState.GAME))
        {
            if(_dailyMessage != null && GameManager.Instance.IsPaused == false)
            {
                _dayText.text = _dailyMessage._day.ToString();

                int dayOfWeekIndex = (_dailyMessage._dayOfWeek % 7);
                if(dayOfWeekIndex >= 0 && dayOfWeekIndex < _daysOfWeek.Length)
                {
                    _dayOfWeekText.text = _daysOfWeek[dayOfWeekIndex];
                }

                if(_dailyMessage._month >= 1 && _dailyMessage._month <= 12)
                {
                    _monthText.text = _months[_dailyMessage._month - 1];
                }
                _yearText.text = _dailyMessage._year.ToString();

                _dailyMessage = null;
            }
        }
    }

    public void SetPause()
    {
        GameManager.Instance.TogglePause();
    }

    public void Test()
    {
        if(IsGameState(GameState.GAME))
        ConnectionManager.Instance.SendExecutableAction("test");
    }

    public void IncreaseSpeedStep()
    {
        if(IsGameState(GameState.GAME))
        ConnectionManager.Instance.SendExecutableAsk("slow_down_cycle_speed", _emptyString);
    }

    public void SlowDownSpeedStep()
    {
        if(IsGameState(GameState.GAME))
        ConnectionManager.Instance.SendExecutableAsk("increase_cycle_speed", _emptyString);
    }

    #endregion Methods
}

#region DailyMessage
[System.Serializable]
public class DailyMessage
{
    public int _day;
    public int _dayOfWeek;
    public int _month;
    public int _year;

    public static DailyMessage CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<DailyMessage>(jsonString);
    }
}
#endregion DailyMessage
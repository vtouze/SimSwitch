using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SendReceiveDaily : SimulationManager
{
    #region Fields
    DailyMessage _dailyMessage = null;

    [Header("Daily Messages")]
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

    [Header("Speed Steps")]
    private int _increaseSpeedCount = 0;
    private int _decreaseSpeedCount = 0;
    private const int maxConsecutiveChanges = 4;

    #endregion Fields

    #region Methods
    protected override void ManageOtherMessages(string content)
    {
        _dailyMessage = DailyMessage.CreateFromJSON(content);
    }

    protected override void OtherUpdate()
    {
        if (IsGameState(GameState.GAME))
        {
            if (_dailyMessage != null && !GameManager.Instance.IsPaused)
            {
                _dayText.text = _dailyMessage._day.ToString();

                int dayOfWeekIndex = (_dailyMessage._dayOfWeek % 7);
                if (dayOfWeekIndex >= 0 && dayOfWeekIndex < _daysOfWeek.Length)
                {
                    _dayOfWeekText.text = _daysOfWeek[dayOfWeekIndex];
                }

                if (_dailyMessage._month >= 1 && _dailyMessage._month <= 12)
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

    public void IncreaseSpeedStep()
    {
        if (IsGameState(GameState.GAME))
        {
            if (_increaseSpeedCount < maxConsecutiveChanges)
            {
                float speedMultiplier = 1.6f;
                VehicleManager.Instance.ChangeSpeedForAllVehicles(speedMultiplier);
                ConnectionManager.Instance.SendExecutableAsk("slow_down_cycle_speed", _emptyString);

                _increaseSpeedCount++;
                _decreaseSpeedCount = 0;
            }
        }
    }

    public void SlowDownSpeedStep()
    {
        if (IsGameState(GameState.GAME))
        {
            if (_decreaseSpeedCount < maxConsecutiveChanges)
            {
                float speedMultiplier = 0.7f;
                VehicleManager.Instance.ChangeSpeedForAllVehicles(speedMultiplier);
                ConnectionManager.Instance.SendExecutableAsk("increase_cycle_speed", _emptyString);

                _decreaseSpeedCount++;
                _increaseSpeedCount = 0;
            }
        }
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
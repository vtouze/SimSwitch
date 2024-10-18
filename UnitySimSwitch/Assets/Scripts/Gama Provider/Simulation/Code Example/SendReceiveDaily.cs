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

    private bool _isPaused = false;

    private string[] daysOfWeek = new string[]
    {
        "Sun.", "Mon.", "Tue.", "Wed.", "Thu.", "Fri.", "Sat."
    };

    private string[] months = new string[]
    {
        "Jan.", "Feb.", "Mar.", "Apr.", "May", "Jun.",
        "Jul.", "Aug.", "Sept.", "Oct.", "Nov.", "Dec."
    };
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
            if (_dailyMessage != null)
            {
                ConnectionManager.Instance.SendExecutableAction("daily");
                _dayText.text = _dailyMessage._day.ToString();

                int dayOfWeekIndex = (_dailyMessage._dayOfWeek % 7);
                if (dayOfWeekIndex >= 0 && dayOfWeekIndex < daysOfWeek.Length)
                {
                    _dayOfWeekText.text = daysOfWeek[dayOfWeekIndex];
                }

                if (_dailyMessage._month >= 1 && _dailyMessage._month <= 12)
                {
                    _monthText.text = months[_dailyMessage._month - 1];
                }
                _yearText.text = _dailyMessage._year.ToString();

                _dailyMessage = null;
            }
        }
    }

    public void SetPause()
    {
        if (_isPaused)
        {
            _isPaused = false;
            ConnectionManager.Instance.SendStatusMessage("play");
        }
        else
        {
            _isPaused = true;
            ConnectionManager.Instance.SendStatusMessage("pause");
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
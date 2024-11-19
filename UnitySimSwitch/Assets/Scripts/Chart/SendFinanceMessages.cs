using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SendFinanceMessages : SimulationManager
{
    #region Fields
    FinanceMessage _financeMessage = null;

    [Header("Finance Messages")]
    [SerializeField] private TMP_Text _budgetText = null;

    private Dictionary<string, string> _emptyString = new Dictionary<string, string>{};
    #endregion Fields

    #region Methods
    protected override void ManageOtherMessages(string content)
    {
        _financeMessage = FinanceMessage.CreateFromJSON(content);
    }

    protected override void OtherUpdate()
    {

        if (IsGameState(GameState.GAME))
        {
            if(_financeMessage != null && !GameManager.Instance.IsPaused)
            {
                _budgetText.text = _financeMessage._budget.ToString();
            }
        }
    }
    #endregion Methods
}

#region FinanceMessage
[System.Serializable]
public class FinanceMessage
{
    public float _budget;

    public static FinanceMessage CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<FinanceMessage>(jsonString);
    }
}
#endregion FinanceMessage
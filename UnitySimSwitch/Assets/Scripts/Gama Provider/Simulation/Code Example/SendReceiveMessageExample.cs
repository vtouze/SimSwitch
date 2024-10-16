using System.Collections.Generic;
using UnityEngine;
public class SendReceiveMessageExample : SimulationManager
{
    GAMAMessage message = null;
    string _expID = "0";

    protected override void ManageOtherMessages(string content)
    {
        message = GAMAMessage.CreateFromJSON(content);
    }

    protected override void OtherUpdate()
    {

        if (IsGameState(GameState.GAME))
        {
           
            if (UnityEngine.Random.Range(0.0f, 1.0f) < 0.002f)
            {
                string mes = "A message from Unity at time: " + Time.time;
                //call the action "receive_message" from the unity_linker agent with two arguments: the id of the player and a message
                Dictionary<string, string> args = new Dictionary<string, string> {
         {"id",ConnectionManager.Instance.GetConnectionId()  },
         {"mes",  mes }};

                Debug.Log("sent to GAMA: " + mes);
                ConnectionManager.Instance.SendExecutableAsk("receive_message", args);
            }
            if (message != null)
            {
                Debug.Log("received from GAMA: cycle " + message.cycle);
                Debug.Log("received from GAMA: test " + message.test);
                message = null;
            }
        }
    }

    public void SetSimulationStatus(string status)
    {
        if(IsGameState(GameState.GAME))
        {
            Dictionary<string, string> args = new Dictionary<string, string> {
         {"type", status},
         {"exp_id", _expID}};

            Debug.Log("sent to GAMA: " + status + ": " + _expID);
            //ConnectionManager.Instance.SendExecutableAsk("receive_experiment", args);
            ConnectionManager.Instance.SendSimulationStatus("receive_experiment", args);
            /*Debug.Log("sent to GAMA: " + "pause" + ": " + "0");
            ConnectionManager.Instance.PauseSimulation("receive_experiment");*/
        }
    }

    public void LoadSimulationButton()
    {
        if(IsGameState(GameState.GAME))
        {
            Debug.Log("sent to GAMA: " + "load"/* + ": " + "0"*/);
            //ConnectionManager.Instance.LoadSimulation("receive_experiment");
        }
    }

    public void PauseSimulationButton()
    {
        if(IsGameState(GameState.GAME))
        {
            Debug.Log("sent to GAMA: " + "pause");
            ConnectionManager.Instance.SendPauseMessage();
        }
    }

    public void StopSimulationButton()
    {
        if(IsGameState(GameState.GAME))
        {
            Debug.Log("sent to GAMA: " + "pause");
            ConnectionManager.Instance.SendStopMessage();
        }
    }

    public void StartSimulationButton()
    {
        if(IsGameState(GameState.GAME))
        {
            Debug.Log("sent to GAMA: " + "pause");
            ConnectionManager.Instance.SendStartMessage();
        }
    }
}

[System.Serializable]
public class GAMAMessage
{  
    public int cycle;
    public int test;

    public static GAMAMessage CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GAMAMessage>(jsonString);
    }

}
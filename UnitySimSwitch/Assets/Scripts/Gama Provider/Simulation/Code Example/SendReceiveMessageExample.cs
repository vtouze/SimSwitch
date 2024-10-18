using System.Collections.Generic;
using UnityEngine;
public class SendReceiveMessageExample : SimulationManager
{
    GAMAMessage message = null;
       
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
                Debug.Log("received from GAMA: cycle " + message.city);
                Debug.Log("received from GAMA: test " + message.district);
                Debug.Log("received from GAMA: test " + message.households);
                message = null;
            }
        }
    }

    public void LoadSimulationButton()
    {
        if(IsGameState(GameState.GAME))
        {
            ConnectionManager.Instance.SendLoadMessage();
        }
    }

    public void SendStatusButton(string status)
    {
        if(IsGameState(GameState.GAME))
        {
            ConnectionManager.Instance.SendStatusMessage(status);
        }
    }

    public void GenerateButton()
    {
        if(IsGameState(GameState.GAME))
        {
            ConnectionManager.Instance.SendExecutableAction("cc");
        }
    }
}

[System.Serializable]
public class GAMAMessage
{  
    public string city;
    public string district;
    public string households;

    public static GAMAMessage CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GAMAMessage>(jsonString);
    }

}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SendReceiveMessageExample : SimulationManager
{

 GAMAMessage message = null;
      
    protected override void ManageOtherMessages(string content)
    {
        message = GAMAMessage.CreateFromJSON(content);
    }

    //action activated at the end of the update phase (every frame)
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

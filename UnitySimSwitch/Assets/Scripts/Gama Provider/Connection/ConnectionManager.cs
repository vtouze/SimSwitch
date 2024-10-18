using System;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;

public class ConnectionManager : WebSocketConnector
{
    private ConnectionState currentState;

    // called when the connection state is manually changed
    public event Action<ConnectionState> OnConnectionStateChanged;

    // called when a "json_simulation" message is received
    public event Action<String, String> OnServerMessageReceived;

    // called when a "json_state" message is received 
    public event Action<JObject> OnConnectionStateReceived;

    // called when a connection request fails
    public event Action<bool> OnConnectionAttempted;

    public static ConnectionManager Instance = null;

    public String AgentToSendInfo = "simulation[0].unity_linker[0]";
    public String MessageSeparator = "|||";      
    public String _modelPath = "/home/stage/Documents/SimSwitch/GamaSimSwitch/models/UnityLinker/SendReceiveMessages.gaml";
    public String _experimentName = "unity_xp";

    // ############################################# UNITY FUNCTIONS #############################################
    void Awake() {
     
        Instance = this;

        Debug.Log("START");
        UpdateConnectionState(ConnectionState.DISCONNECTED);
    }

    
    // ############################################# CONNECTION HANDLER #############################################
    public void UpdateConnectionState(ConnectionState newState) {
        
        switch (newState) {
            case ConnectionState.PENDING:
                Debug.Log("ConnectionManager: UpdateConnectionState -> PENDING");
                break;
            case ConnectionState.CONNECTED:
                Debug.Log("ConnectionManager: UpdateConnectionState -> CONNECTED");
                break;
            case ConnectionState.AUTHENTICATED:
                Debug.Log("ConnectionManager: UpdateConnectionState -> AUTHENTICATED");
                break;
            case ConnectionState.DISCONNECTED:
                Debug.Log("ConnectionManager: UpdateConnectionState -> DISCONNECTED");
                //TryConnectionToServer();
                break;
            default:
                break;
        }

        currentState = newState;
        OnConnectionStateChanged?.Invoke(newState);
    }

    // ############################################# HANDLERS #############################################

    protected override void ManageMessage(string message)
    {
        if (message != null )
        {
           
            JObject jsonObj = JObject.Parse(message);
            string type = (string)jsonObj["type"];
           
        
           
                switch (type)
                {
                    case "ping":
                        var jsonId = new Dictionary<string, string> {{"type", "pong"}};
                        string jsonStringId = JsonConvert.SerializeObject(jsonId);
                        SendMessageToServer(jsonStringId);
                        break;
                    case "json_state":
                        OnConnectionStateReceived?.Invoke(jsonObj);
                        bool authenticated = (bool)jsonObj["in_game"];
                        bool connected = (bool)jsonObj["connected"];

                        if (authenticated && connected)
                        {
                            if (!IsConnectionState(ConnectionState.AUTHENTICATED))
                            {
                               // Debug.Log("ConnectionManager: Player successfully authenticated");
                                UpdateConnectionState(ConnectionState.AUTHENTICATED);
                            }

                        }
                        else if (connected && !authenticated)
                        {
                            if (!IsConnectionState(ConnectionState.CONNECTED))
                            {
                                UpdateConnectionState(ConnectionState.CONNECTED);
                                OnConnectionAttempted?.Invoke(true);
                            }
                            else
                            {
                                //Debug.LogWarning("ConnectionManager: Already connected, waiting for authentication...");
                            }

                        } 
                        break;  
                     
                    case "json_output":
                        JObject content = (JObject)jsonObj["contents"];
                        String firstKey = content.Properties().Select(pp => pp.Name).FirstOrDefault();
                        OnServerMessageReceived?.Invoke(firstKey, content.ToString());
                        break;

                    default:
                        break;
                }
            
        }
    }

    protected override void HandleConnectionOpen()
    {
            var jsonId = new Dictionary<string, string> {
                {"type", "connection"},
                { "id", PlayerName }, 
                { "set_heartbeat", UseHeartbeat ? "true": "false" }
            };
            string jsonStringId = JsonConvert.SerializeObject(jsonId);
         
            SendMessageToServer(jsonStringId);
            Debug.Log("ConnectionManager: Connection opened");
        

    }


    public bool IsConnectionState(ConnectionState currentState) {
        return this.currentState == currentState;
    }

    public void SendExecutableExpression(string expression) {
        Dictionary<string, string> jsonExpression = null;
        jsonExpression = new Dictionary<string, string> {
            {"type", "expression"},
            {"expr", expression}
        };

        string jsonStringExpression = JsonConvert.SerializeObject(jsonExpression);
        SendMessageToServer(jsonStringExpression);
    }

    public void SendExecutableAsk(string action, Dictionary<string,string> arguments)
    {
        string argsJSON = JsonConvert.SerializeObject(arguments);
        Dictionary<string, string> jsonExpression = new Dictionary<string, string> {
            {"type", "ask"},
            {"action", action},
            {"args", argsJSON},
            {"agent", AgentToSendInfo}
        };

        string jsonStringExpression = JsonConvert.SerializeObject(jsonExpression);

        SendMessageToServer(jsonStringExpression);
    }

    public void SendExecutableAction(string action)
    {
        Dictionary<string, string> jsonExpression = new Dictionary<string, string>{
            {"type", "do"},
            {"action", action}
        };
        string jsonStringExpression = JsonConvert.SerializeObject(jsonExpression);
        SendMessageToServer(jsonStringExpression);
        Debug.Log(jsonStringExpression);
    }

    public void SendStatusMessage(string type)
    {
        Dictionary<string, string> jsonExpression = new Dictionary<string, string>{
            {"type", type},
            {"exp_id", "0"}
        };
        string jsonStringExpression = JsonConvert.SerializeObject(jsonExpression);
        SendMessageToServer(jsonStringExpression);
        Debug.Log(jsonStringExpression);
    }
    public void SendLoadMessage()
    {
        Dictionary<string, string> jsonExpression = new Dictionary<string, string>{
            {"type", "load"},
            {"model", _modelPath},
            {"experiment", _experimentName}
        };
        string jsonStringExpression = JsonConvert.SerializeObject(jsonExpression);
        SendMessageToServer(jsonStringExpression);
        Debug.Log(jsonStringExpression);
    }

    public void DisconnectProperly() {
        Dictionary<string,string> jsonExpression = new Dictionary<string,string> {
            {"type", "disconnect_properly"}
        };
        string jsonStringExpression = JsonConvert.SerializeObject(jsonExpression);
        SendMessageToServer(jsonStringExpression);
    }

    public string GetConnectionId() {
        return PlayerName;
    }


    public void Reconnect()
    {
        Debug.Log("Reconnect");
        currentState = ConnectionState.DISCONNECTED;
    }


}


public enum ConnectionState {
    DISCONNECTED,
    // waiting for connection to be established
    PENDING, 
    // connection established, waiting for authentication
    CONNECTED,
    // connection established and authenticated
    AUTHENTICATED
}

using UnityEngine;

using NativeWebSocket;
using System.Text;  

public abstract class WebSocketConnector : MonoBehaviour
{



    private WebSocket websocket;

    public string DefaultIP = "localhost";
    public string DefaultPort = "8080";

    public bool UseHeartbeat = true; //only for middleware mode

    public static string PlayerName = "Web Client";

    public int numErrorsBeforeDeconnection = 10;
    protected int numErrors = 0;



    // Start is called before the first frame update
    async void  Start() 
    {
        websocket = new WebSocket("ws://" + DefaultIP + ":" + DefaultPort);
        // Add OnOpen event listener
        websocket.OnOpen += () => 
        {
            Debug.Log("WS connected!");
            HandleConnectionOpen();
           };

        // Add OnMessage event listener
        websocket.OnMessage += (byte[] msg) =>
        {
            string mes = Encoding.UTF8.GetString(msg);
            Debug.Log("WS received message: " + mes);
            ManageMessage(mes);
        };

        // Add OnError event listener
        websocket.OnError += (string errMsg) =>
        {
            Debug.Log("WS error: " + errMsg);
        };

        // Add OnClose event listener
        websocket.OnClose += (WebSocketCloseCode code) =>
        { 
            Debug.Log("WS closed with code: " + code.ToString());
        };

        // Connect to the server 
        await websocket.Connect();
        
    }


    protected virtual void ManageMessage(string message)
    {

    }

    protected virtual void HandleConnectionOpen()
    {

    }


    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }


    async protected void SendMessageToServer(string message) {
//        Debug.Log("SEND MESSAGE: " + message);
        await websocket.SendText(message);
    }



    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

}

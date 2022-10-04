using UnityEngine;
using WebSocketSharp;

public class NetworkManager : MonoBehaviour
{
    public Loaf loaf;
    public Log log;

    public static bool DEBUG = false;

    public static bool emphasizeScheduled = false;

    WebSocket ws;
    private void Start()
    {
        loaf.Emphasize();

        if (Application.isEditor)
        {
            DEBUG = true;
            log.Add("Running in DEBUG");
        }else{
            log.Add("for real this time");
        }

        string host = "ws://54.147.44.11:42742";
        if(DEBUG){
            host = "ws://localhost:42742";
        }

        ws = new WebSocket(host);
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {
            string str = "Message Received from "+((WebSocket)sender).Url+", Data : "+e.Data;
            log.Add(str);
            NetworkManager.emphasizeScheduled = true;
        };

        log.Add("about to send setAsActiveSky");
        ws.Send("sky:setAsActiveSky");
        log.Add("sent setAsActiveSky");
    }
    private void Update()
    {
        if(ws == null)
        {
            log.Add("ws is null :(");
            return;
        }
    if (Input.GetKeyDown(KeyCode.Space))
        {
            ws.Send("sky:sorry, pants are required");
        }  

        if(emphasizeScheduled){
            emphasizeScheduled = false;
            loaf.Emphasize();
        }
    }
}

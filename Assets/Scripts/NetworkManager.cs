using UnityEngine;
using WebSocketSharp;
using System.Collections;
using System.Collections.Generic;
using System;

public class NetworkManager : MonoBehaviour
{
    public bool isPlayerConnectionScene;
    public bool isGameScene;

    public Log log;
    public GameManager gameManager;
    public PlayerConnectionManager playerConnectionManager;
    public NoiseGameManager noiseGameManager;

    public static bool DEBUG = false;
    
    string host = "ws://3.85.104.149:42742";
    WebSocket ws;

    List<Action<NetworkMessage>> handlers;
    List<NetworkMessage> messages;

    public void EstablishConnection(){
        ws = new WebSocket(host);
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {
            string str = "Message Received from "+((WebSocket)sender).Url+", Data : "+e.Data;
            Debug.Log("Received message: " + str);
            NetworkMessage msgObj = JsonUtility.FromJson<NetworkMessage>(e.Data);
            if(!msgObj.source.Equals("Server")){
                Debug.Log("ERROR - Unknown source");
            }else{
                switch(msgObj.command){
                    case "DeliverFirework":
                        handlers.Add(HandleDeliverFirework);
                        messages.Add(msgObj);
                        break;
                    case "PlayerInputData":
                        handlers.Add(HandlePlayerInputData);
                        messages.Add(msgObj);
                        break;
                    default:
                        Debug.Log("ERROR - Unknown command");
                        break;
                }
            }
        };
    }

    public void SendMessage(NetworkMessage msg){
        string str = msg.Serialized();
        Debug.Log("Sending Serialized obj on Network: " + str);
        ws.Send(str);
    }
    
    // ---------------------------------- HANDLERS --------------------------------

    public void HandleDeliverFirework(NetworkMessage msgObj){
        Debug.Log("Deliver Firework handler");
        gameManager.NetworkLaunch(msgObj.ToFwkData());
    }

    public void HandlePlayerInputData(NetworkMessage msgObj){
        Debug.Log("Player Input Data handler");
        if(isPlayerConnectionScene){
            playerConnectionManager.HandlePlayerInputData(msgObj);
        }else if(isGameScene){

        }
        
    }

    // ----------------------------------- UNITY STUFF -----------------------------

    private void Start()
    {
        if (Application.isEditor)
        {
            DEBUG = true;
            Debug.Log("Running in DEBUG");
        }else{
            Debug.Log("for real this time");
        }

        if(DEBUG){
            host = "ws://127.0.0.1:42742";
        }

        handlers = new List<Action<NetworkMessage>>();
        messages = new List<NetworkMessage>();
    }

    private void Update(){
        while(handlers.Count > 0){
            handlers[0](messages[0]);
            handlers.RemoveAt(0);
            messages.RemoveAt(0);
        }
    }
}

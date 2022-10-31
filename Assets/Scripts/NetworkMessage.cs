using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NetworkMessage
{
    public string source;
    // VALID SOURCES:
    // Sky
    // Designer
    // Server

    public string command;
    // ALL VALID COMMANDS
    //
    // Sky:
    // OpenNewSky -> Called upon establishing a new sky room
    //
    // Designer:
    // RequestSkyAspect
    // SendFirework -> From Designer to Server
    //
    // Server:
    // RequestSkyAspectResponse
    // DeliverFirework -> From Server to Sky
    //

    // Used with Sky:OpenNewSky
    public float skyAspect;

    // Used with Server:DeliverFirework
    public int particleShape;
    public Color particleColor;

    public string Serialized(){
        return JsonUtility.ToJson(this);
    }
}

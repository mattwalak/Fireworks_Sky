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

    // FWK DATA
    // Used with Server:DeliverFirework
    public int type; // [0, 1, 2]
    public int shape; // [0-9]
    public float hue; // [0, 1]
    public float scale; // [0, 1]
    public float normPosX; // [0, 1]
    public float normPosY; // [0, 1]

    public string Serialized(){
        return JsonUtility.ToJson(this);
    }

    public FwkData ToFwkData(){
        FwkData data = new FwkData();
        data.type = type;
        data.shape = shape;
        data.hue = hue;
        data.scale = scale;
        data.normPosX = normPosX;
        data.normPosY = normPosY;

        return data;
    }
}

public class FwkData{
    public int type; // [1, 2, 3]
    public int shape; // [0-9]
    public float hue; // [0, 1]
    public float scale; // [0, 1]
    public float normPosX; // [0, 1]
    public float normPosY; // [0, 1]

    public string ToString(){
        string result = "";
        result += "type = " + type + "; ";
        result += "shape = " + shape + "; ";
        result += "hue = " + hue + "; ";
        result += "scale = " + scale + "; ";
        result += "normPosX = " + normPosX + "; ";
        result += "normPosY = " + normPosY + "; ";
        return result;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidFirework : MonoBehaviour
{
    private GameManager gameManager;
    private OSC osc;

    void Start(){
        gameManager = (GameManager) FindObjectOfType<GameManager>();
        osc = (OSC) FindObjectOfType<OSC>();

        gameManager.OnLaunchReceived();
        PlaySound();

        Destroy(gameObject);
    }

    private void PlaySound(){
        OscMessage msg = new OscMessage();
        msg.address = "/melodyNote";
        msg.values.Add(Random.Range(200f, 1000f));
        osc.Send(msg);
    }
}

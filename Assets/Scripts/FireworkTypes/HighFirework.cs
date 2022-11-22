using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighFirework : MonoBehaviour
{
    public GameObject highFireworkBurst;

    private OSC osc;
    private float screenHeight;
    private float screenWidth;

    private bool hasStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        osc = (OSC) FindObjectOfType<OSC>();
        screenHeight = Camera.main.orthographicSize;
        screenWidth = screenHeight * Camera.main.aspect;
        transform.position = new Vector2(
            Random.Range(-screenWidth, screenWidth),
            Random.Range(screenHeight * 1f/3f, screenHeight) // Can explode in top 3rd
        );

        Burst();
        StartCoroutine(StaticData.RunWithDelay(0.2f, Burst));
        StartCoroutine(StaticData.RunWithDelay(0.4f, Burst));
    }

    private void Burst(){
        GameObject obj = Instantiate(highFireworkBurst, transform);
        if(!hasStarted){
            hasStarted = true;
        }
        
        obj.transform.localPosition = new Vector2(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        );
        BurstSound();
    }

    private void BurstSound(){
        OscMessage msg = new OscMessage();
        msg.address = "/highNote";
        msg.values.Add(Random.Range(1000f, 5000f));
        osc.Send(msg);
    }

    // Update is called once per frame
    void Update()
    {
        if(hasStarted){
            if(gameObject.transform.childCount == 0){
                Destroy(gameObject);
            }
        }   
    }
}

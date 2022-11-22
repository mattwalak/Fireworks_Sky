using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public NetworkManager netManager;
    public GameObject fireworkPrefab;
    public OSC osc;
    public GameObject demoHighExplosionPrefab;
    public GameObject demoMidExplosionPrefab;
    public GameObject demoLowExplosionPrefab;

    private float screenHeight;
    private float screenWidth;
    private const float SCREEN_EXPLODE_FRAC = 0.7f;

    private void Start(){
        screenHeight = Camera.main.orthographicSize;
        screenWidth = screenHeight * Camera.main.aspect;
    }

    public void OnOpenNewSkyClicked(){
        netManager.EstablishConnection();

        float aspect = Camera.main.aspect;

        NetworkMessage msg = new NetworkMessage();
        msg.source = "Sky";
        msg.command = "OpenNewSky";
        msg.skyAspect = aspect;

        netManager.SendMessage(msg);
    }

    public void OnSendPantsOptionalClicked(){
        NetworkMessage msg = new NetworkMessage();
        msg.source = "Sky";
        msg.command = "PantsOptional";

        netManager.SendMessage(msg);
    }

    public void OnLaunchReceived(){
        FireworkBody fwk = Instantiate(fireworkPrefab).GetComponent<FireworkBody>();

        float random_x = Random.Range(
            -SCREEN_EXPLODE_FRAC * screenWidth, 
            SCREEN_EXPLODE_FRAC * screenWidth);
        float random_y = Random.Range(
            -SCREEN_EXPLODE_FRAC * screenHeight, 
            SCREEN_EXPLODE_FRAC * screenHeight);

        fwk.numParticles = Random.Range(5, 10)*2 + 1;
        fwk.particleShape = (ParticleShape)Random.Range(1, 3);
        fwk.particleColor = Color.HSVToRGB(Random.Range(0f, 1f), 1f, 0.8f);

        fwk.Launch(new Vector2(random_x, random_y));
    }

    public void NetworkLaunch(int shape, Color color){
        FireworkBody fwk = Instantiate(fireworkPrefab).GetComponent<FireworkBody>();

        float random_x = Random.Range(
            -SCREEN_EXPLODE_FRAC * screenWidth, 
            SCREEN_EXPLODE_FRAC * screenWidth);
        float random_y = Random.Range(
            -SCREEN_EXPLODE_FRAC * screenHeight, 
            SCREEN_EXPLODE_FRAC * screenHeight);

        fwk.numParticles = Random.Range(5, 10)*2 + 1;
        fwk.particleShape = (ParticleShape)shape;
        fwk.particleColor = color; /*Color.HSVToRGB(hue, 1f, 0.8f);*/

        fwk.Launch(new Vector2(random_x, random_y));

        Debug.Log("End of network launch");
    }

    public void DemoHigh(){
        Instantiate(demoHighExplosionPrefab);
    }

    public void DemoMid(){
        Instantiate(demoMidExplosionPrefab);
    }

    public void DemoLow(){
        Instantiate(demoLowExplosionPrefab);
    }
}

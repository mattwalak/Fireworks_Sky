using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoiseGameManager : MonoBehaviour
{
    public float scaleAdditionValue = 0.01f;

    public TMP_Text circlesCounter;
    private int circlesLeft = 20;

    public GameObject laserCollection;
    public StonePlacer stonePlacer;
    public CircleBounds circleBounds;

    public GameObject airParticlePrefab;
    public GameObject windTouchGameObject;

    private NetworkManager netManager;

    private GameObject activeWindTouchObject;
    private Camera mainCamera;

    // 0 = impulse mode
    // [1, 2) = rhythmic impulse mode
    // 2 = laser mode

    private int gameState = 0;

    private const int NUM_PARTICLES_PER_CLICK = 10;
    private const float TIME_BETWEEN_BLASTS = 0.15f;
    private const float ROTATION_PERIOD = 500.0f;

    private float blastCounter_t = 0f;
    private float rotationTimer_t = 0f;

    private float ABSOLUTE_STONE_SIZE_MAX = 1.0f;
    private float ABSOLUTE_STONE_SIZE_MIN = 0.1f;
    private float currentStoneMin = 0.1f;
    private float currentStoneMax = 1.0f;
    
    private List<Stone> stones;

    void Start(){
        mainCamera = Camera.main;
        activeWindTouchObject = Instantiate(windTouchGameObject, transform);
        activeWindTouchObject.SetActive(false);

        netManager = (NetworkManager) FindObjectOfType(typeof(NetworkManager));
        if(netManager != null){
            netManager.LoadNoiseGameScene();
        }
    }

    public void AddStone(Stone stone){
        if(stones == null){
            stones = new List<Stone>();
        }

        stones.Add(stone);
    }

    void blastParticlesAtLocation(Vector2 pos){
        float rotationFrac = rotationTimer_t / ROTATION_PERIOD;
        for(int i = 0; i < NUM_PARTICLES_PER_CLICK; i++){
            float rad = i * (2f * Mathf.PI / NUM_PARTICLES_PER_CLICK) /*+ (2f * Mathf.PI * rotationFrac)*/;
            Vector2 dir = new Vector2(
                Mathf.Cos(rad /*+ Random.Range(0f, 0.1f)*/), 
                Mathf.Sin(rad /*+ Random.Range(0f, 0.1f)*/)
            );
                
            GameObject p = Instantiate(airParticlePrefab, transform);
            p.transform.position = pos;
            Rigidbody2D p2d = p.GetComponent<Rigidbody2D>();
            p2d.velocity = dir * 4;
        }    
    }

    void blastParticlesAtMouseLocation(){
        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        blastParticlesAtLocation(worldPos);
    }

    void Update(){
        
        blastCounter_t += Time.deltaTime;
        rotationTimer_t += Time.deltaTime;

        while(rotationTimer_t > ROTATION_PERIOD){
            rotationTimer_t -= ROTATION_PERIOD;
        }

        while(blastCounter_t > TIME_BETWEEN_BLASTS){
            // blastParticlesAtMouseLocation();
            blastCounter_t -= TIME_BETWEEN_BLASTS;
        }

        if(Input.GetMouseButtonDown(0)){
            blastParticlesAtMouseLocation();
        }

        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        laserCollection.transform.position = worldPos;


        /*
        // Wind mechanics
        if(Input.GetMouseButtonDown(0)){
            Debug.Log("ACTIVE");
            activeWindTouchObject.SetActive(true);
        }else if(Input.GetMouseButtonUp(0)){
            Debug.Log("INACTIVE");
            activeWindTouchObject.SetActive(false);
        }else{
            Vector2 mousePos = Input.mousePosition;
            Vector2 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            activeWindTouchObject.transform.position = worldPos;
        }*/

        ManageStones();
    }

    public void RegisterNewActivatedStone(){
        circlesLeft--;
        circlesCounter.text = circlesLeft.ToString();
        if(circlesLeft == 0){
            // Activate next stage
        }
    }

    private void ManageStones(){
        if(stones == null || stones.Count == 0){
            return;
        }

        float maxHitScore = stones[0].GetHitScore();
        float minHitScore = maxHitScore;

        foreach(Stone s in stones){
            float hitScore = s.GetHitScore();
            if(hitScore > maxHitScore){
                maxHitScore = hitScore;
            }

            if(hitScore < minHitScore){
                minHitScore = hitScore;
            }
        }

        /*
        Debug.Log("Max hit score");
        Debug.Log("Min hit score");

        Debug.Log("Max score = " + maxHitScore);
        Debug.Log("Min score = " + minHitScore);*/
    }

    public void ReceivePlayerInput(NetworkMessage msg){
        
    }

    public void HandleCircleButtonClick(NetworkMessage msg){
        Debug.Log("Toggling glow mode for circle = " + msg.circleButtonID);
        if(msg.circleButtonState == -1){
            stonePlacer.stoneScripts[msg.circleButtonID].DisableGlowState();
        }else{
            stonePlacer.stoneScripts[msg.circleButtonID].EnableGlowState();
        }
    }

    public void HandleSendTouchPositionData(NetworkMessage msg){
        if(gameState == 0){
            if(msg.touchState == 1){
                Vector2 pos = new Vector2(msg.touchPosX, msg.touchPosY);
                blastParticlesAtLocation(pos * circleBounds.radius);
            }
        }
    }
}

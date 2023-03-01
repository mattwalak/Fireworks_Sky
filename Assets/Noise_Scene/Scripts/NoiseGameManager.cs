using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGameManager : MonoBehaviour
{
    public float scaleAdditionValue = 0.01f;

    public GameObject laserCollection;

    public GameObject airParticlePrefab;
    public GameObject windTouchGameObject;

    private GameObject activeWindTouchObject;
    private Camera mainCamera;

    private const int NUM_PARTICLES_PER_CLICK = 25;
    private const float TIME_BETWEEN_BLASTS = 0.5f;
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
    }

    public void AddStone(Stone stone){
        if(stones == null){
            stones = new List<Stone>();
        }

        stones.Add(stone);
    }

    void blastParticles(){
        float rotationFrac = rotationTimer_t / ROTATION_PERIOD;
        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = mainCamera.ScreenToWorldPoint(mousePos);

        for(int i = 0; i < NUM_PARTICLES_PER_CLICK; i++){
            float rad = i * (2f * Mathf.PI / NUM_PARTICLES_PER_CLICK) /*+ (2f * Mathf.PI * rotationFrac)*/;
            Vector2 dir = new Vector2(
                Mathf.Cos(rad /*+ Random.Range(0f, 0.1f)*/), 
                Mathf.Sin(rad /*+ Random.Range(0f, 0.1f)*/)
            );
                
            GameObject p = Instantiate(airParticlePrefab, transform);
            p.transform.position = worldPos;
            Rigidbody2D p2d = p.GetComponent<Rigidbody2D>();
            p2d.velocity = dir * 4;
        }    
    }

    void Update(){
        
        blastCounter_t += Time.deltaTime;
        rotationTimer_t += Time.deltaTime;

        while(rotationTimer_t > ROTATION_PERIOD){
            rotationTimer_t -= ROTATION_PERIOD;
        }

        while(blastCounter_t > TIME_BETWEEN_BLASTS){
            blastParticles();
            blastCounter_t -= TIME_BETWEEN_BLASTS;
        }

        if(Input.GetMouseButtonDown(0)){
            // blastParticles();
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
}

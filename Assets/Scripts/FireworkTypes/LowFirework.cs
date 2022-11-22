using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowFirework : MonoBehaviour
{
    public GameObject initialExplosion;
    public GameObject secondaryExplosion;

    public Color color = new Color(1, 1, 0, 1);
    public float explosionSize = 5;
    
    private const float EXPLOSION_TIME = 2f;
    private const float ALPHA_PEAK_PERCENT = 0.7f;
    private const float INITIAL_PERCENT_SCALE = 0.2f;
    private const float INITIAL_PERCENT_TIME = 0.2f;
    private const float MAX_ALPHA = 0.3f;
    private SpriteRenderer secondaryRenderer;
    public OSC osc;


    // Start is called before the first frame update
    void Start()
    {
        osc = (OSC) FindObjectOfType<OSC>();

        float screenHeight = Camera.main.orthographicSize;
        float screenWidth = screenHeight * Camera.main.aspect;
        transform.position = new Vector2(
            Random.Range(-screenWidth, screenWidth),
            Random.Range(-screenHeight, -screenHeight * 2f/3f) // Can explode in bottom 3rd
        );

        secondaryRenderer = secondaryExplosion.GetComponent<SpriteRenderer>();
        secondaryRenderer.color = new Color(color.r, color.g, color.b, 0);

        // Initial Explosion
        initialExplosion.transform.localScale = new Vector2(0, 0);
        float initialSize = explosionSize * INITIAL_PERCENT_SCALE;
        float initialTime = EXPLOSION_TIME * INITIAL_PERCENT_TIME;
        LeanTween.scale(initialExplosion, new Vector3(initialSize, initialSize, initialSize), initialTime)
            .setEase(LeanTweenType.easeOutExpo);
        LeanTween.alpha(initialExplosion, MAX_ALPHA, initialTime * ALPHA_PEAK_PERCENT)
            .setEase(LeanTweenType.easeOutSine)
            .setOnComplete(InitialAlphaDown);

        // Secondary Explosion
        secondaryExplosion.transform.localScale = new Vector2(0, 0);
        LeanTween.scale(secondaryExplosion, new Vector3(explosionSize, explosionSize, explosionSize), EXPLOSION_TIME)
            .setEase(LeanTweenType.easeOutExpo);
        LeanTween.alpha(secondaryExplosion, MAX_ALPHA, EXPLOSION_TIME * ALPHA_PEAK_PERCENT)
            .setEase(LeanTweenType.easeOutSine)
            .setOnComplete(SecondaryAlphaDown);

        PlaySound();
    }

    private void PlaySound(){
        OscMessage msg = new OscMessage();
        msg.address = "/bassNote";
        msg.values.Add(Random.Range(50f, 200f));
        osc.Send(msg);
    }

    private void InitialAlphaDown(){
        float initialTime = EXPLOSION_TIME * INITIAL_PERCENT_TIME; 
        LeanTween.alpha(initialExplosion, 0.0f, initialTime * (1f - ALPHA_PEAK_PERCENT))
            .setEase(LeanTweenType.easeInSine);
    }

    private void SecondaryAlphaDown(){
        LeanTween.alpha(secondaryExplosion, 0.0f, EXPLOSION_TIME * (1f - ALPHA_PEAK_PERCENT))
            .setEase(LeanTweenType.easeInSine)
            .setOnComplete(DestroySelf);
    }

    private void DestroySelf(){
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public int stoneNum;
    public OSC osc;

    private NoiseGameManager gameManager;
    private float hitScore;
    private const float HIT_DECAY_PER_SEC = 1;

    private const float MIN_HIT_SCORE = 0;
    private const float MAX_HIT_SCORE = 10;

    public float GetHitScore(){
        return hitScore;
    }

    void Start(){
        gameManager = (NoiseGameManager)FindObjectOfType<NoiseGameManager>();
        gameManager.AddStone(this);
    }

    void Update(){
        AdjustHitScore(-HIT_DECAY_PER_SEC * Time.deltaTime);
    }

    private void AdjustHitScore(float ammount){
        hitScore += ammount;
        hitScore = Mathf.Clamp(hitScore, MIN_HIT_SCORE, MAX_HIT_SCORE);
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        OscMessage msg = new OscMessage();
        msg.address = "/playStoneNote";
        msg.values.Add(stoneNum);
        msg.values.Add(collisionInfo.gameObject.GetComponent<AirParticle>().GetGain());
        osc.Send(msg);

        /*
        transform.localScale = new Vector2(
            transform.localScale.x + gameManager.scaleAdditionValue,
            transform.localScale.y + gameManager.scaleAdditionValue
        );*/

        AdjustHitScore(1.0f);
    }
}

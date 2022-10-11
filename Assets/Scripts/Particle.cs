using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public Sprite CircleSprite;
    public Sprite TriangleSprite;
    public Sprite SquareSprite;

    private ParticleShape shape;
    private Color color;

    private SpriteRenderer renderer;
    private Rigidbody2D body;


    public void Init(ParticleShape shape_in, Color color_in){
        renderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        renderer.enabled = true;

        shape = shape_in;
        color = color_in;

        switch(shape){
            case ParticleShape.CIRCLE:
                renderer.sprite = CircleSprite;
                break;
            case ParticleShape.SQUARE:
                renderer.sprite = SquareSprite;
                break;
            case ParticleShape.TRIANGLE:
                renderer.sprite = TriangleSprite;
                break;
        }

        renderer.color = color;
    }

    public void SetPosition(Vector2 pos){
        transform.position = pos;
    }

    public void SetLocalPosition(Vector2 localPos){
        transform.localPosition = localPos;
    }

    public void SetScale(Vector2 scale){
        transform.localScale = scale;
    }

    public void SetLinearDrag(float drag){
        body.drag = drag;
    }

    public void SetAngularDrag(float angularDrag){
        body.angularDrag = angularDrag;
    }

    public void AddForce(Vector2 force){
        body.AddForce(force, ForceMode2D.Impulse);
    }

    public void SetLookAt(Vector2 look){
        transform.right = look;
    }

    public void SetAngularVelocity(float vel){
        body.angularVelocity = vel;
    }

    public void SetFadeout(float untilFadeout, float duringFadeout){
        StartCoroutine(FadeoutWithDelay(untilFadeout, duringFadeout));
    }

    private IEnumerator FadeoutWithDelay(float untilFadeout, float duringFadeout){
        yield return new WaitForSeconds(untilFadeout);
        LeanTween.alpha(gameObject, 0f, duringFadeout).setOnComplete(DestroySelf);
    }

    public void DestroySelf(){
        Destroy(gameObject);
    }
}

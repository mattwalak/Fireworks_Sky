using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonePlacer : MonoBehaviour
{
    public List<GameObject> stones;
    public float radius = 3f;

    void Start()
    {
        for(int i = 0; i < stones.Count; i++){
            float rad = i * (2f * Mathf.PI / stones.Count);
            Vector2 pos = new Vector2(
                Mathf.Cos(rad) * radius,
                Mathf.Sin(rad) * radius
            );

            stones[i].transform.position = pos;
        }        
    }

}

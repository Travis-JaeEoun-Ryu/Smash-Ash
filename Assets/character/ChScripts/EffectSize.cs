using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSize : MonoBehaviour
{
    private Transform tr;
    // Start is called before the first frame update
    void Start()
    {
        RandomScale();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RandomScale()
    {
        int number = Random.Range(1, 5);
        float scale = (0.1f * number); 
        tr.position += new Vector3(0,scale,scale);
    }
}

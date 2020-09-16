using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endExplosion : MonoBehaviour
{
    public float timer = 0.0f;
    public float endTime;

    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= endTime) {
            Destroy(gameObject);
        }
    }
}

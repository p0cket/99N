using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explode : MonoBehaviour {

    public GameObject explosion;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            Instantiate(explosion, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
}
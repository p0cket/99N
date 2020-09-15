using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)){
            Debug.Log('E');
            Attack();
        }
    }

    void Attack() {
        // Play attack animation
        // detect whats in range
        // do
    }
}

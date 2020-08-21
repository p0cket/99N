using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class triggerFungusBlock : MonoBehaviour
{
    [Header("Important: Player needs the 'Player' tag")]
    //[SerializeField]
    public Fungus.Flowchart flowChart;
    //[SerializeField]
    public string blockName;

    // Start is called before the first frame update
    void Start()
    {
        //flowChart.ExecuteBlock("test");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Player"){
            if(blockName != null) {
                Debug.Log("Block Triggered");
                flowChart.ExecuteBlock(blockName);
            }
            else {
                Debug.Log("Please enter the block name");
            }
        }
    }

}

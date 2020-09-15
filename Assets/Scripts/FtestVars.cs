using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class FtestVars : MonoBehaviour
{
    public Flowchart flowchart;
    // Start is called before the first frame update
    void Start()
    {
        flowchart.SetIntegerVariable("myNum", 11);
    }

    // Update is called once per frame
    void Update()
    {
        // Uncomment Below if you need it
        // int firstNum = flowchart.GetIntegerVariable("myNum");
        // Debug.Log(firstNum);
    }
}


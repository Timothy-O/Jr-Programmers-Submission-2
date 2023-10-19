using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenAnt : Ant
{
    private GameObject[] antChildren;
    // Start is called before the first frame update
    void Start()
    {
        antChildren = GameObject.FindGameObjectsWithTag("Ant");
        ResourceTracking();
        isSafe = true;
        isIdle = true;
        withResource = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (antChildren.Length < 2)
        {
            GoGather();
        }
        if (isControlled)
        {
            ControlledMovement();
        }
    }
}

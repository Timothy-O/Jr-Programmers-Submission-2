using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerAnt : Ant
{
    // Start is called before the first frame update
    void Start()
    {
        antBase = GameObject.Find("Base");
        antView = GetComponent<SphereCollider>();
        basePerimeter = antBase.GetComponent<SphereCollider>();
        antView.radius = range;
        ResourceTracking();
        isSafe = true;
        isIdle = true;
        isAttackType = false;
        withResource = false;
        isControlled = false;
    }

    // Update is called once per frame
    void Update()
    {
        GoGather(activePile);
        if (isControlled)
        {
            ControlledState();
        }
    }
}

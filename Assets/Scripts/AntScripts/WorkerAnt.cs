using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerAnt : Ant
{
    // Start is called before the first frame update
    void Start()
    {
        antBase = GameObject.Find("Base");
        basePerimeter = antBase.GetComponent<SphereCollider>();
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
        GoGather();
        ControlledState();
    }
}

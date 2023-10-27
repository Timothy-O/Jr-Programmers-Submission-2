using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reproductive : Ant
{
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

    void Update()
    {
        if (isControlled)
        {
            ControlledState();
        }  
    }
}

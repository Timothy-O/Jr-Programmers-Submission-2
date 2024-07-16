using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSoldier : Ant
{
    void Start()
    {
        antBase = GameObject.Find("Base");
        basePerimeter = antBase.GetComponent<SphereCollider>();
        antView = GetComponent<SphereCollider>();
        antView.radius = range;
        isSafe = true;
        isIdle = true;
        isAttackType = true;
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

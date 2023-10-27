using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenAnt : Ant
{
    private GameObject[] antChildren;
    // Start is called before the first frame update
    void Start()
    {
        antBase = GameObject.Find("Base");
        basePerimeter = antBase.GetComponent<SphereCollider>();
        antView = GetComponent<SphereCollider>();
        antView.radius = range;
        antChildren = GameObject.FindGameObjectsWithTag("Ant");
        isSafe = true;
        isIdle = true;
        isAttackType = false;
        isControlled = false;
        withResource = false;
        ResourceTracking();
    }

    // Update is called once per frame
    void Update()
    {
        antChildren = GameObject.FindGameObjectsWithTag("Ant");
        if (antChildren.Length < 2)
        {
            GoGather(activePile);
            if (isControlled)
            {
                ControlledState();
            }
        }
        else
        {
            BackToNest();
        }
    }
    private void BackToNest()
    {
        if (Vector3.Distance(transform.position, antBase.transform.position) > 0.05f && isSafe)
        {
            MoveTo(antBase);
        }
    }
}

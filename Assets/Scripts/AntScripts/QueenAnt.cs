using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenAnt : Ant
{
    private GameObject[] antChildren;// should be centrallized info, should be a list.
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
        antChildren = GameObject.FindGameObjectsWithTag("Ant");//Shold'nt be called every frame
        if (antChildren.Length < 2)
        {
            Gather(activePile);
            if (isControlled)
            {
                ControlledState();
            }
        }
        else
        {
            BackToNest();//This is called every frame, repetition is taken care of by method.
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

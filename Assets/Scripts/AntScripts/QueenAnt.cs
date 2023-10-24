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
        antChildren = GameObject.FindGameObjectsWithTag("Ant");
        ResourceTracking();
        isSafe = true;
        isIdle = true;
        withResource = false;
    }

    // Update is called once per frame
    void Update()
    {
        antChildren = GameObject.FindGameObjectsWithTag("Ant");
        if (antChildren.Length < 2)
        {
            GoGather();
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
    private void OnMouseUp()
    {
        
    }
}

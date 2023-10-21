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
            if (isControlled)
            {
                ControlledMovement();
            }
        }
        else
        {
            BackToNest();
        }
    }
    private void BackToNest()
    {
        Vector3 baseLocation = GameObject.Find("Base").transform.position;
        Vector3 baseDirection = baseLocation - transform.position;
        if (Vector3.Distance(transform.position, baseLocation) > 0.05f)
        {
            transform.Translate(baseDirection.normalized * speed * Time.deltaTime);
        }
    }
    private void OnMouseUp()
    {
        
    }
}

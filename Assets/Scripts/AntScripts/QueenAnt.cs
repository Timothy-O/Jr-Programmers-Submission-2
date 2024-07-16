using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenAnt : Ant
{
    // Start is called before the first frame update
    void Start()
    {
        antBase = GameObject.Find("Base");
        basePerimeter = antBase.GetComponent<SphereCollider>();
        antView = GetComponent<SphereCollider>();
        antView.radius = range;
        isSafe = true;
        isIdle = true;
        isAttackType = false;
        isControlled = false;
        withResource = false;
        ResourceSelection();
    }
    // Update is called once per frame
    void Update()
    {
        if (gameManager.ants.Count < 2)
        {
            if (isControlled)
            {
                ControlledState();
            }
            else
            {
                Gather(activePile);
            }
        }
        else
        {
            BackToNest();
        }
    }
}

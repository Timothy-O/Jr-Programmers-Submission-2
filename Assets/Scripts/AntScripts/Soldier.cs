using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Ant
{
    public float idleTime = 10;
    public float movementTime = 5;
    private Vector3 randomPos;
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
        isAttackType = true;
        withResource = false;
        isControlled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isControlled)
        {
            IdleMovement();
        }
        else if (isControlled)
        {
            ControlledState();
        }
    }
    public Vector3 RandomPos()
    {
        float xPos = Random.Range(-10, 10);
        float zPos = Random.Range(-10, 10);
        randomPos = new Vector3(xPos, 0.5f, zPos);
        return randomPos;
    }

    public void IdleMovement()
    {
        if (isSafe && isIdle)
        {
            idleTime -= Time.deltaTime;
            movementTime = 5;
        }
        if (idleTime < 0 && isSafe)
        {
            isIdle = false;
            MoveTo(randomPos);
            movementTime -= Time.deltaTime;
        }
        if (movementTime < 0 && isSafe)
        {
            idleTime = 10;
            isIdle = true;
            RandomPos();
        }
    }
}

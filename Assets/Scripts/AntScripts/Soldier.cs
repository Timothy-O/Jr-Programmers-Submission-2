using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Ant
{
    public float idleTime;
    //Idletime is the amount of time in which ant can be idle
    private Vector3 randomPos;
    //Random pos is a random loction within a specific range.

    // Start is called before the first frame update
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
        RandomPos();
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

    //Generates a vector3 position within a certain range
    public Vector3 RandomPos()
    {
        float xPos = Random.Range(-10, 10);
        float zPos = Random.Range(-10, 10);
        randomPos = new Vector3(xPos, transform.position.y, zPos);
        return randomPos;
    }
    //Generates a rondom float and assigns as the idle time
    public float RandomTime()
    {
        idleTime = Random.Range(10, 20);
        return idleTime;
    }

    //Conrols the soldiers idle movement in a random direction
    public void IdleMovement()
    {
        if (isSafe && isIdle)
        {
            idleTime -= Time.deltaTime;
        }
        if (idleTime < 0 && isSafe)
        {
            isIdle = false;
            MoveTo(randomPos);
        }
        if (Vector3.Distance(transform.position, randomPos)<0.05 && isSafe)
        {
            RandomTime();
            isIdle = true;
            RandomPos();
        }
    }
}

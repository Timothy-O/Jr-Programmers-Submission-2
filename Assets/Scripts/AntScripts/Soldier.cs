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
        isIdle = true;
        isSafe = true;
        isAttackType = true;
        RandomPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isControlled)
        {
            IdleMovement();
        }
    }
    public Vector3 RandomPos()
    {
        float xPos = Random.Range(-10, 10);
        float zPos = Random.Range(-10, 10);
        randomPos = new Vector3(xPos, transform.position.y, zPos);
        return randomPos;
    }

    public void IdleMovement()
    {
        if (isSafe && isIdle)
        {
            idleTime -= Time.deltaTime;
            if (idleTime < 0)
            {
                //isIdle = false;
                MoveTo(randomPos);
                movementTime -= Time.deltaTime;
                if (movementTime < 0)
                {
                    isIdle = false;
                }
            }
        }
        else if (!isIdle && isSafe)
        {
            idleTime = 10;
            isIdle = true;
            movementTime = 5;
            RandomPos();
        }
    }

}

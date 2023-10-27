using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Range is the distance of detection, idleTime is how long it can be idle, movementTime is for how long it should move
    //speed is how fast it moves
    //targetAnt is the ant target when it is in range, enemyView is the sphere collider that detects the ant
    //isAttacking is true when it is chasing an ant, isActive is true when it is moving but not attacking
    public int range;
    public float idleTime;
    public float movementTime;
    public int speed;

    private Vector3 randomPos;

    private GameObject targetAnt;
    private GameObject[] allAnts;
    private SphereCollider enemyView;

    public bool isAttacking;
    public bool isActive;

    void Start()
    {
        //all references are made and all values assigned and the RandomPos is called to generate a random direction
        targetAnt = gameObject;
        enemyView = GetComponent<SphereCollider>();
        enemyView.radius = range;
        isActive = false;
        RandomPos();
    }

    void Update()
    {
        //only moves the object when idle time is exceded
        IdleMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        //checks for when an ant is in range and sets isAttacking to true
        if (other.tag == "Ant")
        {
            isAttacking = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //chases ant that is in range
        if (other.tag == "Ant")
        {
            targetAnt = other.gameObject;
            Chase(targetAnt);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //checks if the ant escpaed and deactivates isAttacking bool
        if (other.tag == "Ant")
        {
            targetAnt = gameObject;
            isAttacking = false;
        }
    }
    private void OnDestroy()
    {
        //Activates when object is about to be destroyed
        DeathAlert();
    }

    public void Chase(GameObject target)
    {
        //Method to chase target
        MoveTo(target);
    }
    public Vector3 RandomPos()
    {
        //generates random position whenever called
        float xPos = Random.Range(-10, 10);
        float zPos = Random.Range(-10, 10);
        randomPos =new Vector3(xPos, 0.5f, zPos);
        return randomPos;
    }
    public void IdleMovement()
    {
        //checks if idleness time is exceded and moves object in random direction for movementTime duration
        if (!isAttacking && !isActive)
        {
            idleTime -= Time.deltaTime;
            movementTime = 10;
        }
        if (idleTime < 0 && !isAttacking)
        {
            isActive = true;
            MoveTo(randomPos);
            movementTime -= Time.deltaTime;
        }
        if (movementTime < 0 && !isAttacking)
        {
            idleTime = 20;
            isActive = false;
            RandomPos();
        }
    }
    private void DeathAlert()
    {
        //returns the ants to a state before the chase began
        allAnts = GameObject.FindGameObjectsWithTag("Ant");
        for (int i = 0; i < allAnts.Length; i++)
        {
            allAnts[i].GetComponentInParent<Ant>().isSafe = true;
            allAnts[i].GetComponentInParent<Ant>().ResourceTracking();
        }
    }

    public void MoveTo(GameObject target)
    {
        //moves enemy towards gameobject.
        Vector3 directionVector = target.transform.position - transform.position;
        transform.Translate(directionVector.normalized * speed * Time.deltaTime);
    }
    public void MoveTo(Vector3 target)
    {
        // moves enemy towards a position
        Vector3 directionVector = target - transform.position;
        transform.Translate(directionVector.normalized * speed * Time.deltaTime);
    }

}

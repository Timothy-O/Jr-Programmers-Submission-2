using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Range is the distance of detection, idleTime is how long it can be idle, movementTime is for how long it should move
    //speed is how fast it moves, xRange is the distance from the center to the farthest x boundary, same explanation for zRange
    //tragetPosition is the targetAnt's position, initialPosition is the reference object position used to determine change in distance
    //targetAnt is the ant target when it is in range, ground is the ground object, enemyView is the sphere collider that detects the ant
    //isAttacking is true when it is chasing an ant, isActive is true when it is moving but not attacking
    private int range = 4;
    public float idleTime = 10;
    public float movementTime = 5;
    public float speed;
    private float xRange;
    private float zRange;
    private float yPos;

    private Vector3 antPosition;
    private Vector3 initialPos;
    private Vector3 randomDirection;

    private GameObject targetAnt;
    private GameObject[] allAnts;
    private GameObject ground;
    private SphereCollider enemyView;

    public bool isAttacking;
    public bool isActive;

    //all references are made and all values assigned and the RandomDirection is called to generate a random direction
    void Start()
    {
        yPos = transform.position.y;
        targetAnt = gameObject;
        enemyView = GetComponent<SphereCollider>();
        enemyView.radius = range;
        ground = GameObject.Find("Ground");
        xRange = ground.transform.localScale.x * 5;
        zRange = ground.transform.localScale.z * 5;
        initialPos = transform.position;
        isActive = true;
        RandomDirection();
    }

    //only used to check for idleness and moves the object when idle time is exceded
    void Update()
    {
        IdleMovement();
        //yPos = 0.5f;
    }

    //check for ants that are in range
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ant")
        {

        }
    }
    //chases ant that is in range and actives the isAttacking bool
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ant")
        {
            targetAnt = other.gameObject;
            antPosition = targetAnt.transform.position;
            Chase();
            IsAttackingCheck();
        }
    }
    //checks if the ant escpaed and deactivates isAttacking bool
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ant")
        {
            targetAnt = gameObject;
            IsAttackingCheck();
        }
    }
    //Activates when object is about to be destroyed
    private void OnDestroy()
    {
        DeathAlert();
    }

    //Method to chase target
    public void Chase()
    {    
        Vector3 directionVector = antPosition - gameObject.transform.position;
        transform.Translate(directionVector.normalized*speed*Time.deltaTime);
    }
    //Checking for movement while ant is in range
    public void IsAttackingCheck()
    {   
        Vector3 finalPos = transform.position;
        float positionChange = Vector3.Distance(finalPos, initialPos);
        if (positionChange != 0)
        {
            isAttacking = true;
            idleTime = 10;
        }
        else if (positionChange == 0)
        {
            isAttacking = false;
            RandomDirection();
        }
        initialPos = finalPos;
    }
    //generates random direction whenever called
    public Vector3 RandomDirection()
    {
        float xPos = Random.Range(-xRange, +xRange);
        float zPos = Random.Range(-zRange, +zRange);
        randomDirection =new Vector3(xPos, yPos, zPos) - transform.position;
        return randomDirection;
    }
    //checks if idleness time is exceded and moves object in random direction for movementTime duration
    public void IdleMovement()
    {
        if (!isAttacking && isActive)
        {
            idleTime -= Time.deltaTime;
            if (idleTime < 0)
            {
                transform.Translate(randomDirection.normalized * speed * Time.deltaTime);
                movementTime -= Time.deltaTime;
                if (movementTime < 0)
                {
                    isActive = false;
                }
            }
        }
        else if (!isActive)
        {
            idleTime = 10;
            isActive = true;
            movementTime = 5;
            RandomDirection();
        }
    }
    //returns the ants to a state before the chase began
    private void DeathAlert()
    {
        allAnts = GameObject.FindGameObjectsWithTag("Ant");
        for (int i = 0; i < allAnts.Length; i++)
        {
            allAnts[i].GetComponentInParent<Ant>().isSafe = true;
            allAnts[i].GetComponentInParent<Ant>().ResourceTracking();
        }
    }
}

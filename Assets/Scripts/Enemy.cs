using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int range;
    //Range is the distance of detection,
    public float idleTime;
    //idleTime is how long it can be idle,
    public float movementTime;
    //movementTime is for how long it should move
    public int speed;
    //speed is how fast it moves

    private Vector3 randomPos;
    // randomPos is a random position on the scene

    private GameObject targetAnt;
    //targetAnt is the ant target when it is in range,
    private GameObject[] allAnts;
    // an array of all active ants, should be central info
    private SphereCollider enemyView;
    //enemyView is the sphere collider that detects the ant

    public bool isAttacking;
    //isAttacking is true when it is chasing an ant,
    public bool isIdle;
    //isIdle is true when not moving at all.

    //all references are made and all values assigned and the RandomPos is called to generate a random direction
    void Start()
    {
        targetAnt = gameObject;
        enemyView = GetComponent<SphereCollider>();//not quite sure if its necessary to assign the collider
        enemyView.radius = range;
        isIdle = true;
        RandomPos();
    }
    //only moves the object when idle time is exceded
    void Update()
    {
        IdleMovement();
    }

    //checks for when an ant is in range and sets isAttacking to true
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ant")
        {
            isAttacking = true;
        }
    }
    //chases ant that is in range
    private void OnTriggerStay(Collider other)
    {//doesnt account for multiple ants in renge but shouldnt matter
        if (other.tag == "Ant")// not neceesary for ant to be assigned every frame
        {
            targetAnt = other.gameObject;
            Chase(targetAnt);
        }
    }
    //checks if the ant escpaed and deactivates isAttacking bool
    private void OnTriggerExit(Collider other)
    {//should probably check if other ants are in range
        if (other.tag == "Ant")
        {
            targetAnt = gameObject;
            isAttacking = false;
        }
    }
    //Activates when object is about to be destroyed
    private void OnDestroy()
    {
        DeathAlert();
    }

    //Method to chase target
    public void Chase(GameObject target)
    {
        MoveTo(target);
    }
    //generates random position whenever called
    public Vector3 RandomPos()
    {// range should'nt be hard coded
        float xPos = Random.Range(-10, 10);
        float zPos = Random.Range(-10, 10);
        randomPos =new Vector3(xPos, 0.5f, zPos);
        return randomPos;
    }
    //checks if idleness time is exceded and moves object in random direction for movementTime duration
    public void IdleMovement()
    {
        if (!isAttacking && isIdle)
        {
            idleTime -= Time.deltaTime;
            movementTime = 10;
        }
        if (idleTime < 0 && !isAttacking)
        {
            isIdle = false;
            MoveTo(randomPos);
            movementTime -= Time.deltaTime;
        }
        if (movementTime < 0 && !isAttacking)//moveemntTime doesnt seem necessry with the translate method
        {
            idleTime = 20;
            isIdle =true;
            RandomPos();
        }
    }
    //returns the ants to a state before the chase began
    private void DeathAlert()
    {
        allAnts = GameObject.FindGameObjectsWithTag("Ant");//should be assigned centrally
        for (int i = 0; i < allAnts.Length; i++)
        {
            allAnts[i].GetComponentInParent<Ant>().isSafe = true;
            allAnts[i].GetComponentInParent<Ant>().ResourceTracking();
        }
    }

    //moves enemy towards gameobject.
    public void MoveTo(GameObject target)
    {
        Vector3 directionVector = target.transform.position - transform.position;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(directionVector);
    }
    // moves enemy towards a position
    public void MoveTo(Vector3 target)
    {
        Vector3 directionVector = target - transform.position;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(directionVector);
    }
}

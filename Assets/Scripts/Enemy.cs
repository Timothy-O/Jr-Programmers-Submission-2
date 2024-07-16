using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int range;
    //Range is the distance of detection,
    public float idleTime;
    //idleTime is how long it can be idle,
    public int speed;
    //speed is how fast it moves

    private Vector3 randomPos;
    // randomPos is a random position on the scene

    private GameObject targetAnt;
    //targetAnt is the ant target when it is in range,
    private SphereCollider enemyView;
    //enemyView is the sphere collider that detects the ant
    private GameManager gameManager;

    public bool isAttacking;
    //isAttacking is true when it is chasing an ant,
    public bool isIdle;
    //isIdle is true when not moving at all.

    //Relationships between scripts are defined and initiated
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    //all references are made and all values assigned and the RandomPos is called to generate a random direction
    void Start()
    {
        targetAnt = gameObject;
        enemyView = GetComponent<SphereCollider>();
        enemyView.radius = range;
        isIdle = true;
        RandomPos();
    }
    //only moves the object when idle time is exceded
    void Update()
    {
        if (!isAttacking)
        {
            IdleMovement();
        }
    }

    //checks for when an ant is in range and sets isAttacking to true
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ant")
        {
            targetAnt = other.gameObject;
            isAttacking = true;
        }
    }
    //chases ant that is in range
    private void OnTriggerStay(Collider other)
    {//doesnt account for multiple ants in renge but shouldnt matter
        if (other.tag == "Ant")
        {
            MoveTo(other.gameObject);
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
    //Adds enemy to gamemanager enemies list when instantiated
    private void OnEnable()
    {//still needs corection as it duplicates objects
        if (gameManager.enemies.Contains(transform.GetChild(0).gameObject))
        {
            Debug.Log(true);
            //gameManager.enemies.Add(gameObject.transform.GetChild(0).gameObject);
        }
    }

    //generates random position whenever called
    public Vector3 RandomPos()
    {// range should'nt be hard coded, should reflect total map and ant base location
        float pox = Random.Range(-20, -10);
        float nex = Random.Range(10, 20);
        float poz = Random.Range(20, -10);
        float nez = Random.Range(10, 20);
        float xPos = Random.Range(pox, nex);
        float zPos = Random.Range(poz, nez);
        randomPos = new Vector3(xPos, transform.position.y, zPos);
        return randomPos;
    }
    //Generates a rondom float and assigns as the idle time
    public float RandomTime()
    {
        idleTime = Random.Range(10, 20);
        return idleTime;
    }

    //checks if idleness time is exceded and moves object to random position
    public void IdleMovement()
    {
        if (!isAttacking && isIdle)
        {
            idleTime -= Time.deltaTime;
        }
        if (idleTime < 0 && !isAttacking)
        {
            isIdle = false;
            MoveTo(randomPos);
        }
        if (Vector3.Distance(transform.position, randomPos) < 0.05 && !isAttacking)
        {
            RandomTime();
            isIdle =true;
            RandomPos();
        }
    }
    //returns the ants to a state before the chase began
    private void DeathAlert()
    {
        for (int i = 0; i < gameManager.ants.Count; i++)
        {
            gameManager.ants[i].GetComponentInParent<Ant>().isSafe = true;
            gameManager.ants[i].GetComponentInParent<Ant>().ResourceSelection();
        }
        gameManager.enemies.Remove(transform.GetChild(0).gameObject);
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

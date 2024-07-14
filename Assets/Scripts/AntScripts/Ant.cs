using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    public float speed;
    //speed is the objects speend during translation;
    public int range;
    //range is the radius of the sphere colider that detects enemys
    private int pileIndex;
    //pileIndex is the random index for the resourceObject;

    private GameObject enemyobject;
    //enemyObject is the detected enemy oject;
    private GameObject[] allEnemys;
    //allEnemys is an array of all enemies in scene, info should be central, should be a list instead.
    public GameObject[] resourceObject;
    //resourceobjects is an array of all resources in the scene, info should be central, should be a list instead.
    public GameObject activePile;
    //activePile is the random resource pile selected from the resourceObjects array
    protected GameObject antBase;
    //antBase is the base gameobject where resources are taken;
    protected SphereCollider antView;
    // antView is the sphere collider that detects enemys;
    protected SphereCollider basePerimeter;
    //base perimeter is the sphere colider attached to the base

    public bool isSafe;
    //isSafe activitaes while ant is not being chased;
    public bool isIdle;
    //isIdle is active when an ant isn't doing work
    public bool isAttackType;
    //isAttackType distinguishes between ant that should attack and those that should'nt
    public bool isGathering;
    //isGathering is active when an ant is gathering,
    public bool withResource;
    //withResources activates after ant collide with a resource pile,
    public bool isControlled;
    //isControlled is activated when ant is clicked and deactivates when command is completed

    //Assigns all values and runs the resource tracking method
    void Start()
    {
        antBase = GameObject.Find("Base");
        basePerimeter = antBase.GetComponent<SphereCollider>();
        antView = GetComponent<SphereCollider>();
        antView.radius = range;
        ResourceTracking();
        isSafe = true;
        isIdle = true;
        isAttackType = false;
        withResource = false;
        isControlled = false;
    }
    // runs the Gather method and checks if the ant is controlled
    void Update()
    {
        Gather(activePile);
        if (isControlled)
        {
            ControlledState();
        }
    }

    // Checks if an enemy is within range and deactivates the isSafe bool
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            isSafe = false;
        }
    }
    //detects the enemy position every frame and moves accordingly
    private void OnTriggerStay(Collider other)
    {//doesnt acount for multiple enemies within range.
        // is there really a need to assign the enemy every frame?
        if (other.tag == "Enemy")
        {
            enemyobject = other.gameObject;
            ConflictState(enemyobject);
        }
    }
    // checks if enemy is no longer in range
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Enemy")
        {
            enemyobject = gameObject;
            isSafe = true;
            ResourceTracking();// should look for another way to constantly track resource changes
        }
    }
    //Checks when object is clicked to be controlled
    private void OnMouseDown()
    {
        if (isControlled)
        {
            isControlled=false;
        }
        else
        {
            isControlled = true;
        }
    }
    //runs when object is about to be destroyed
    private void OnDestroy()
    {
        DeathAlert();
    }

    //determines what action to take when ant is controlled and gets mouse's info when clicked
    public void ControlledState()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit);
            if(hit.collider.tag == "EnemyParent")
            {
               StartCoroutine(ControlledAttack(hit.collider.gameObject));
            }
            else if (hit.collider.tag == "Resource")
            {
                StartCoroutine(ControlledGathering(hit.collider.gameObject));
            }
            else
            {
                Vector3 mousePos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                StartCoroutine(ControlledMovement(mousePos));
            }
        }
    }
    //moves ant to clickposition when called
    IEnumerator ControlledMovement(Vector3 clickPosition)
    {
        while (Vector3.Distance(transform.position, clickPosition) > 0.05f)
        {
            MoveTo(clickPosition);
            yield return null;
        }
        isControlled = false;
        ResourceTracking();
        // Resource data should be centralized and ants should have access to info, method should be called after an event.
        yield return null;
    }
    //attcks the selected enemy when called
    IEnumerator ControlledAttack(GameObject enemy)
    {
        while(enemy!=null)
        {
            isAttackType = true;
            MoveTo(enemy);
            yield return null;
        }
        isControlled = false;
        ResourceTracking();
        // Resource data should be centralized and ants should have access to info, method should be called after an event.
        yield return null;
    }
    // goes to gather selected resource pile when called
    IEnumerator ControlledGathering(GameObject resourcePile)
    {
        while(resourcePile != null || withResource)//not quite sure about the or
        {
            GoGather(resourcePile);
            yield return null;
        }
        isControlled = false;
        ResourceTracking();
        // Resource data should be centralized and ants should have access to info, method should be called after an event.
        isGathering = false;
        isAttackType = true;//This shouldnt be changed at all from start probably
        yield return null;
    }

    //determines what happens when enemy is in range
    public virtual void ConflictState(GameObject enemy)
    {
        if (isAttackType)
        {
            MoveTo(enemy);
        }
        else
        {
            MoveAway(enemy);
        }
    }

    //checks for objects taged as resources and selects a random one
    public void ResourceTracking()
    {//tracking of reources should be central while selection of pile should be done by ant
        resourceObject = GameObject.FindGameObjectsWithTag("Resource");
        if (resourceObject.Length != 0)
        {
            pileIndex = Random.Range(0, resourceObject.Length);
            activePile = resourceObject[pileIndex];
        }
    }
    //uncontrolled gathering of random reource piles
    public void Gather(GameObject resourcePile)
    {
        isGathering = true;
        isAttackType = false;
        //Attack types should'nt gather
        if (isSafe && resourceObject.Length != 0 && isIdle && !isControlled)
        {
            if (resourcePile != null)
            {
                MoveTo(resourcePile);
            }
            else
            {
                ResourceTracking();
            }
        }
        else if (withResource && isSafe && !isIdle && !isControlled)
        {
            MoveTo(antBase);
        }
        else if (isSafe && isIdle && resourceObject.Length == 0 && !isControlled)
        {
            if( Vector3.Distance(transform.position, antBase.transform.position) > basePerimeter.radius)
            {
                MoveTo(antBase);
            }
        }
    }
    // controlled gathering of selected resource pile
    public void GoGather(GameObject resourcePile)
    {
        isGathering = true;
        isAttackType = false;
        //Attack types should'nt gather
        if (isSafe && resourceObject.Length != 0 && isIdle)
        {
            if (resourcePile != null)
            {
                MoveTo(resourcePile);
            }
            else
            {
                ResourceTracking();
            }
        }
        else if (withResource && isSafe && !isIdle)
        {
            MoveTo(antBase);
        }
    }

    //should run when ant is idle
    public virtual void Idle()
    {
        //will run things like idle animation
    }
    //should attack a selected target
    public void AttackTarget(GameObject enemy)
    {// not quite sure what this is about
        if(Vector3.Distance(transform.position, enemy.transform.position) > 0.05)
        {
            MoveTo(enemy);
        }
    }
    // use to move ant towards and objects and used to reduce redundancy of translate method
    public void MoveTo(GameObject target)
    {//still called during every frame, needs to only be called once per movement
        Vector3 directionVector = target.transform.position - transform.position;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(directionVector);
    }
    //moves ant to a position and reduces the redundancy of translate method
    public void MoveTo(Vector3 target)
    {//still called during every frame, needs to only be called once per movement
        Vector3 directionVector = target - transform.position;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(directionVector);
    }
    // moves ant away from an object
    public void MoveAway(GameObject target)
    {//still called during every frame, needs to only be called once per movement
        Vector3 directionVector = transform.position- target.transform.position;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(directionVector);
    }
    //returns all enemys to a state before attcking and runs when ant is about to die
    private void DeathAlert()
    {
        allEnemys = GameObject.FindGameObjectsWithTag("Enemy");// should be able to get this info from central
        for (int i = 0; i < allEnemys.Length; i++)
        {
            allEnemys[i].GetComponentInParent<Enemy>().isAttacking = false;
            allEnemys[i].GetComponentInParent<Enemy>().RandomPos();
        }
    }
    //need to learn how to stop translate method
}

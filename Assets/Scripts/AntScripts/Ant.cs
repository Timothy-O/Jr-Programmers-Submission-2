using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    //speed is the objects speend during translation; range is the radius of the sphere colider that detects enemys
    //pileIndex is the random index for the resourceObject; allEnemys is an array of all enemies in scene
    //enemyObject is the detected enemy oject; resourceobjects is an array of all resources in the scene
    //mouseWorldPos is the position over which the mouse is clicked at the gameObjects heigth
    //activePile is the random resource pile selected from the resourceObjects array
    //antBase is the base gameobject where resources are taken; base perimeter is the sphere colider attached to the base
    // antView is the sphere collider that detects enemys; isSafe activitaes while ant is not being chased;
    //withResources activates after ant collide with a resource pile, isIdle is active when an ant isn't doing work
    //isGathering is active when an ant is gathering, isControlled is activated when ant is clicked and deactivates when command is completed
    //isAttackType distinguishes between ant that should attack and those that should'nt
    public float speed;
    public int range;
    private int pileIndex;

    private GameObject enemyobject;
    private GameObject[] allEnemys;
    public GameObject[] resourceObject;
    public GameObject activePile;
    protected GameObject antBase;
    protected SphereCollider antView;
    protected SphereCollider basePerimeter;

    public bool isSafe;
    public bool isIdle;
    public bool isAttackType;
    public bool isGathering;
    public bool withResource;
    public bool isControlled;
    
    void Start()
    {
        //Assigns all values and runs the resource tracking method
        antBase = GameObject.Find("Base");
        antView = GetComponent<SphereCollider>();
        basePerimeter = antBase.GetComponent<SphereCollider>();
        antView.radius = range;
        ResourceTracking();
        isSafe = true;
        isIdle = true;
        isAttackType = false;
        withResource = false;
        isControlled = false;
    }
    void Update()
    {
        // runs the GoGather method and checks if the ant is controlled
        GoGather(activePile);
        if (isControlled)
        {
            ControlledState();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Checks if an enemy is within range and deactivates the isSafe bool
        if(other.tag == "Enemy")
        {
            isSafe = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //detects the enemy position every frame and moves accordingly
        if (other.tag == "Enemy")
        {
            enemyobject = other.gameObject;
            ConflictState(enemyobject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // checks if enemy is no longer in range
        if(other.tag == "Enemy")
        {
            enemyobject = gameObject;
            isSafe = true;
            ResourceTracking();
        }
    }
    private void OnMouseDown()
    {
        //Checks when object is clicked to be controlled
        if (isControlled)
        {
            isControlled=false;
        }
        else
        {
            isControlled = true;
        }
    }
    private void OnDestroy()
    {
        //runs when object is about to be destroyed
        DeathAlert();
    }

    public void ControlledState()
    {
        //determines what action to take when mouse is controlled and gets mouse's info when clicked
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
    IEnumerator ControlledMovement(Vector3 clickPosition)
    {
        //moves ant to clickposition when called
        while (Vector3.Distance(transform.position, clickPosition) > 0.05f)
        {
            MoveTo(clickPosition);
            yield return null;
        }
        isControlled = false;
        ResourceTracking();
        yield return null;
    }
    IEnumerator ControlledAttack(GameObject enemy)
    {
        //attcks the selected enemy when called
        while(enemy!=null)
        {
            isAttackType = true;
            MoveTo(enemy);
            yield return null;
        }
        isControlled = false;
        ResourceTracking();
        yield return null;
    }
    IEnumerator ControlledGathering(GameObject resourcePile)
    {
        // goes to gather selected resource pile when called
        while(resourcePile != null || withResource)
        {
            Gather(resourcePile);
            yield return null;
        }
        isControlled = false;
        ResourceTracking();
        isGathering = false;
        isAttackType = true;
        yield return null;
    }

    public virtual void ConflictState(GameObject enemy)
    {
        //determines what happens when enemy is in range
        if (isAttackType)
        {
            MoveTo(enemy);
        }
        else
        {
            MoveAway(enemy);
        }
    }

    public void ResourceTracking()
    {
        //checks for objects taged as resources and selects a random one
        resourceObject = GameObject.FindGameObjectsWithTag("Resource");
        if (resourceObject.Length != 0)
        {
            pileIndex = Random.Range(0, resourceObject.Length);
            activePile = resourceObject[pileIndex];
        }
    }
    public void GoGather(GameObject resourcePile)
    {
        //uncontrolled gathering of random reource piles
        isGathering = true;
        isAttackType = false;
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
    public void Gather(GameObject resourcePile)
    {
        // controlled gathering of selected resource pile
        isGathering = true;
        isAttackType = false;
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

    public virtual void Idle()
    {
        //should run when ant is idle
    }
    public void AttackTarget(GameObject enemy)
    {//should attack a selected target
        if(Vector3.Distance(transform.position, enemy.transform.position) > 0.05)
        {
            MoveTo(enemy);
        }
    }
    public void MoveTo(GameObject target)
    {
        // use to move ant towards and objects and used to reduce redundancy of translate method
        Vector3 directionVector = target.transform.position - transform.position;
        transform.Translate(directionVector.normalized * speed * Time.deltaTime);
    }
    public void MoveTo(Vector3 target)
    {
        //moves ant to a position and reduces the redundancy of translate method
        Vector3 directionVector = target - transform.position;
        transform.Translate(directionVector.normalized * speed * Time.deltaTime);
    }
    public void MoveAway(GameObject target)
    {
        // moves ant away from an object
        Vector3 directionVector = transform.position- target.transform.position;
        transform.Translate(directionVector.normalized * speed * Time.deltaTime);
    }
    private void DeathAlert()
    {
        //returns all enemys to a state before attcking and runs when ant is about to die
        allEnemys = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < allEnemys.Length; i++)
        {
            allEnemys[i].GetComponentInParent<Enemy>().isAttacking = false;
            allEnemys[i].GetComponentInParent<Enemy>().RandomPos();
        }
    }

}

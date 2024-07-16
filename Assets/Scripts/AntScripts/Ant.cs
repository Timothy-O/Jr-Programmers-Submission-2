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
    public GameObject activePile;
    //activePile is the random resource pile selected from the resources list
    protected GameObject antBase;
    //antBase is the base gameobject where resources are taken;
    protected SphereCollider antView;
    // antView is the sphere collider that detects enemys;
    protected SphereCollider basePerimeter;
    //base perimeter is the sphere colider attached to the base
    public GameManager gameManager;
    //GameManager script reference

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

    //Relationships between scripts are defined and initiated
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    //Assigns all values and runs the ResourceSelection method
    void Start()
    {
        antBase = GameObject.Find("Base");
        basePerimeter = antBase.GetComponent<SphereCollider>();
        antView = GetComponent<SphereCollider>();
        antView.radius = range;
        ResourceSelection();
        isSafe = true;
        isIdle = true;
        isAttackType = false;
        withResource = false;
        isControlled = false;
    }
    // runs the Gather method and checks if the ant is controlled
    void Update()
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

    // Checks if an enemy is within range and deactivates the isSafe bool
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            isSafe = false;
            enemyobject = other.gameObject;
        }
    }
    //detects the enemy position every frame and moves accordingly
    private void OnTriggerStay(Collider other)
    {//doesnt acount for multiple enemies within range.
        if (other.tag == "Enemy")
        {
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
            ResourceSelection();
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
    //Adds ant to gamemanager ants list when instantiated
    private void OnEnable()
    {//still needs corection as it duplicates objects
        if (gameManager.ants.Contains(transform.GetChild(0).gameObject))
        {
            Debug.Log(true);
            //gameManager.ants.Add(gameObject.transform.GetChild(0).gameObject);
        }
    }

    //determines what action to take when ant is controlled and gets mouse's info when clicked
    public void ControlledState()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit);
            if(hit.collider.tag == "EnemyParent" && isAttackType)//might need re-writting
            {
               StartCoroutine(ControlledAttack(hit.collider.gameObject));
            }
            else if (hit.collider.tag == "Resource" && !isAttackType)
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
        yield return null;
    }
    //attcks the selected enemy when called, only works for soldiers
    IEnumerator ControlledAttack(GameObject enemy)
    {
        while(enemy!= null)
        {
            MoveTo(enemy);
            yield return null;
        }
        isControlled = false;
        yield return null;
    }
    // goes to gather selected resource pile when called, only works for workers
    IEnumerator ControlledGathering(GameObject resourcePile)
    {
        while(resourcePile != null || withResource)//not quite sure about the or
        {
            GoGather(resourcePile);
            yield return null;
        }
        isControlled = false;
        ResourceSelection();
        isGathering = false;
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
    public void ResourceSelection()
    {
        if (gameManager.resources.Count != 0)
        {
            pileIndex = Random.Range(0, gameManager.resources.Count);
            activePile = gameManager.resources[pileIndex];
        }
    }
    //uncontrolled gathering of random reource piles
    public void Gather(GameObject resourcePile)
    {
        if (isSafe && gameManager.resources.Count != 0 && isIdle && !isControlled)
        {
            isGathering = true;
            if (resourcePile != null)
            {
                MoveTo(resourcePile);
            }
            else
            {
                ResourceSelection();
            }
        }
        else if (withResource && isSafe && !isIdle && !isControlled)
        {
            MoveTo(antBase);
        }
        else if (isSafe && isIdle && gameManager.resources.Count == 0 && !isControlled)
        {
            isGathering = false;
            BackToNest();
        }
    }
    // controlled gathering of selected resource pile
    public void GoGather(GameObject resourcePile)
    {
        if (isSafe && gameManager.resources.Count != 0 && isIdle)
        {
            isGathering = true;
            if (resourcePile != null)
            {
                MoveTo(resourcePile);
            }
            else
            {
                ResourceSelection();
            }
        }
        else if (withResource && isSafe && !isIdle)
        {
            MoveTo(antBase);
        }
    }
    public void BackToNest()
    {
        if (Vector3.Distance(transform.position, antBase.transform.position) > 0.05f)
        {
            MoveTo(antBase);
        }
        else
        {
            Idle();
        }
    }


    //should run when ant is idle
    public virtual void Idle()
    {
        //will run things like idle animation
    }
    // use to move ant towards an objects and used to reduce redundancy of translate method
    public void MoveTo(GameObject target)
    {
        Vector3 directionVector = target.transform.position - transform.position;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(directionVector);
    }
    //moves ant to a position and reduces the redundancy of translate method
    public void MoveTo(Vector3 target)
    {
        Vector3 directionVector = target - transform.position;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(directionVector);
    }
    // moves ant away from an object
    public void MoveAway(GameObject target)
    {
        Vector3 directionVector = transform.position- target.transform.position;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(directionVector);
    }
    //returns all enemys to a state before attcking and runs when ant is about to die
    private void DeathAlert()
    {
        for (int i = 0; i < gameManager.enemies.Count; i++)
        {
            gameManager.enemies[i].GetComponentInParent<Enemy>().isAttacking = false;
            gameManager.enemies[i].GetComponentInParent<Enemy>().RandomPos();
        }
        gameManager.ants.Remove(transform.GetChild(0).gameObject);
    }
    //need to learn how to stop translate method
}
